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

			// Obtener la fecha del comprobante
			DateTime fechaComprobante = DateTime.Now; // valor por defecto
			if (info.Comprobante != null)
			{
				var participantesComprobante = info.Comprobante.ObtenerParticipantesSegunClave("COMPROBANTE");
				if (participantesComprobante != null && participantesComprobante.Any())
				{
					var atributoFecha = participantesComprobante[0].ObtenerAtributo("FECHA");
					if (atributoFecha != null && atributoFecha.Valor != null)
					{
						fechaComprobante = (DateTime)atributoFecha.Valor;
					}
				}
			}

			// Evaluar reglas de fecha
			var reglasFecha = participante.Participante.Reglas.Where(r => r.Atributo == "Fecha").ToList();
			if (reglasFecha.Count >= 2)
			{
				DateTime fechaDesde = (DateTime)reglasFecha[0].Valor;
				DateTime fechaHasta = (DateTime)reglasFecha[1].Valor;
				if (fechaComprobante.Date < fechaDesde.Date || fechaComprobante.Date > fechaHasta.Date)
				{
					string entreFechas = this.FormatearFecha(fechaDesde) + " y " + this.FormatearFecha(fechaHasta);
					condicionesFaltantes.Add($"fecha entre {entreFechas}");
				}
			}

			// Evaluar reglas de hora
			var reglasHora = participante.Participante.Reglas.Where(r => r.Atributo == "Hora").ToList();
			if (reglasHora.Count >= 2)
			{
				TimeSpan horaDesde = TimeSpan.Parse(reglasHora[0].ValorString);
				TimeSpan horaHasta = TimeSpan.Parse(reglasHora[1].ValorString);
				TimeSpan horaComprobante = fechaComprobante.TimeOfDay;
				if (horaComprobante < horaDesde || horaComprobante > horaHasta)
				{
					string entreHoras = reglasHora[0].ValorString + " y " + reglasHora[1].ValorString;
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
			participante2.Participante.Reglas = new List<Regla>();
			participante2.Participante.RelaReglas = participante.Participante.RelaReglas;
			participante2.Participante.Reglas.AddRange( participante.Participante.Reglas );
			participante2.Participante.Reglas.RemoveRange( 0, 4 + diasSemana.Count() );
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
