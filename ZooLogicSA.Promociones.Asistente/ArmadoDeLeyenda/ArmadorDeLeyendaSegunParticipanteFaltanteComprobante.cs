using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda
{
	public class ArmadorDeLeyendaSegunParticipanteFaltanteComprobante : IArmadorDeLeyendaSegunParticipanteFaltante
	{
		private IArmadorDeLeyendaSegunParticipanteFaltante armadorGenerico;

		public ArmadorDeLeyendaSegunParticipanteFaltanteComprobante( IArmadorDeLeyendaSegunParticipanteFaltante a )
		{
			this.armadorGenerico = a;
		}

		#region IArmadorDeLeyendaSegunParticipanteFaltanteGenerico Members

		public string ObtenerLeyendaSegunRegla( ParticipanteFaltante participante, InformacionPromocionIncumplida info )
		{
			List<string> condicionesFaltantes = new List<string>();

			// Obtener la fecha del comprobante directamente desde el XML
			// (no usar ObtenerParticipantesSegunClave porque ParticipanteXml requiere
			// que la clave esté en ConfiguracionesPorParticipante, lo cual no aplica a COMPROBANTE)
			DateTime fechaComprobante = DateTime.Now; // valor por defecto
			if (info.Comprobante != null)
			{
				try
				{
					var xml = info.Comprobante.ObtenerXml();
					var nodoFecha = xml.SelectSingleNode("/*/FECHA");
					if (nodoFecha != null && nodoFecha.Attributes["Valor"] != null)
					{
						string valorFecha = nodoFecha.Attributes["Valor"].Value;
						// Formato esperado: dd/MM/yyyy
						fechaComprobante = new DateTime(
							int.Parse(valorFecha.Substring(6, 4)),
							int.Parse(valorFecha.Substring(3, 2)),
							int.Parse(valorFecha.Substring(0, 2)), 0, 0, 0);
					}
				}
				catch
				{
					// Si falla la lectura del XML, queda DateTime.Now como default
				}
			}

			// Evaluar reglas de fecha (atributo = "FECHA" según ManagerReglas)
			var reglasFecha = participante.Participante.Reglas.Where(r => r.Atributo == "FECHA").ToList();
			if (reglasFecha.Count >= 2)
			{
				DateTime fechaDesde = (DateTime)reglasFecha.First(r => r.Comparacion == Factor.DebeSerMayorIgualA).Valor;
				DateTime fechaHasta = (DateTime)reglasFecha.First(r => r.Comparacion == Factor.DebeSerMenorIgualA).Valor;
				if (fechaComprobante.Date < fechaDesde.Date || fechaComprobante.Date > fechaHasta.Date)
				{
					string entreFechas = this.FormatearFecha(fechaDesde) + " y " + this.FormatearFecha(fechaHasta);
					condicionesFaltantes.Add($"fecha entre {entreFechas}");
				}
			}

			// Evaluar reglas de hora (atributo = "HORAALTAFW" según ManagerReglas)
			var reglasHora = participante.Participante.Reglas.Where(r => r.Atributo == "HORAALTAFW").ToList();
			if (reglasHora.Count >= 2)
			{
				TimeSpan horaDesde = TimeSpan.Parse(reglasHora.First(r => r.Comparacion == Factor.DebeSerMayorIgualA).ValorString);
				TimeSpan horaHasta = TimeSpan.Parse(reglasHora.First(r => r.Comparacion == Factor.DebeSerMenorIgualA).ValorString);
				TimeSpan horaComprobante = fechaComprobante.TimeOfDay;
				if (horaComprobante < horaDesde || horaComprobante > horaHasta)
				{
					string entreHoras = reglasHora.First(r => r.Comparacion == Factor.DebeSerMayorIgualA).ValorString + " y " + reglasHora.First(r => r.Comparacion == Factor.DebeSerMenorIgualA).ValorString;
					condicionesFaltantes.Add($"Hora entre {entreHoras}");
				}
			}

			// Evaluar reglas de día de la semana
			var reglasDiaSemana = participante.Participante.Reglas.Where(x => x.Comparacion == Factor.DebeSerIgualADiaDeLaSemana).ToList();
			if (reglasDiaSemana.Any())
			{
				bool cumpleDiaSemana = reglasDiaSemana.Any(regla => (int)fechaComprobante.DayOfWeek == Convert.ToInt32(regla.Valor));
				if (!cumpleDiaSemana)
				{
					IEnumerable<string> diasSemana = reglasDiaSemana.Select(x => this.ObtenerNombreDiaDeLaSemana(x.Valor));
					condicionesFaltantes.Add($"Dia de la semana: {String.Join(", ", diasSemana.ToArray())}");
				}
			}

			// Construir mensaje solo con condiciones faltantes
			string retorno = "";
			if (condicionesFaltantes.Any())
			{
				retorno = String.Join(", ", condicionesFaltantes);
			}

			// Obtener las otras condiciones faltantes del armador genérico
			IEnumerable<string> diasSemanaParaClon = participante.Participante.Reglas.Where(x => x.Comparacion == Factor.DebeSerIgualADiaDeLaSemana).Select(x => this.ObtenerNombreDiaDeLaSemana(x.Valor));
			ParticipanteFaltante participante2 = this.ObtenerClonParticipante(participante, diasSemanaParaClon);

			string viejo = this.armadorGenerico.ObtenerLeyendaSegunRegla(participante2, info).Trim();

			if (!String.IsNullOrEmpty(viejo))
			{
				if (!String.IsNullOrEmpty(retorno))
				{
					retorno = retorno + " y " + viejo;
				}
				else
				{
					retorno = viejo;
				}
			}

			return retorno;
		}

		private ParticipanteFaltante ObtenerClonParticipante( ParticipanteFaltante participante, IEnumerable<string> diasSemana )
		{
			ParticipanteFaltante participante2 = new ParticipanteFaltante();
			participante2.Participante = new ParticipanteRegla();
			participante2.Participante.Id = participante.Participante.Id;
			participante2.Participante.Codigo = participante.Participante.Codigo;
			participante2.Participante.Reglas = new List<Regla>();
			participante2.Participante.RelaReglas = participante.Participante.RelaReglas;
			participante2.Participante.Reglas.AddRange( participante.Participante.Reglas );
			// Quitar reglas de cabecera: FECHA, HORAALTAFW y días de semana
			participante2.Participante.Reglas.RemoveAll( x => x.Atributo == "FECHA" || x.Atributo == "HORAALTAFW" || x.Comparacion == Factor.DebeSerIgualADiaDeLaSemana );
			participante2.Participante.Reglas.RemoveAll( x => x.Atributo == "CANTIDAD" );
			return participante2;
		}

		private string ObtenerNombreDiaDeLaSemana( object valor )
		{
			CultureInfo cultureInfo = new CultureInfo( "es-es" );
			DateTimeFormatInfo formatoDate = cultureInfo.DateTimeFormat;
			TextInfo formatoTexto = cultureInfo.TextInfo;

			return formatoTexto.ToTitleCase( formatoDate.GetDayName( (DayOfWeek)valor ) );
		}

		private string FormatearFecha( object p )
		{
			return ((DateTime)p).ToString("dd/MM/yyyy");
		}

		#endregion
	}
}
