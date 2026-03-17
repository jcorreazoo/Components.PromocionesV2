using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
	public class CalculadorDeUsabilidadSegunTipoCantidad
	{
		private ConfiguracionComportamiento configuracionComportamiento;

		public CalculadorDeUsabilidadSegunTipoCantidad( ConfiguracionComportamiento comportamiento )
		{
			this.configuracionComportamiento = comportamiento;
		}

		public List<ItemUsabilidad> ObtenerUsabilidad( List<ResultadoReglas> resultadoreglas, out List<ResultadoReglas> resultadosCantidad )
		{
			List<ItemUsabilidad> usabilidadLoca = new List<ItemUsabilidad>();

			resultadosCantidad = resultadoreglas.Where(
				x => x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
					||
				x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto
				).ToList();

			var todosLosCandidatosSegunCantidad = resultadosCantidad.SelectMany( parti => parti.Participantes ).Select( 
				o => new { id = o.Id,
						   clave= o.Clave,
						   nombre = o.Clave + o.Id, 
						   cantidad = Convert.ToDecimal( o.Cantidad, new CultureInfo( "en-US" ) ), 
						   precio = o.ObtenerPrecioUnitario() }
					).Distinct().ToList();
			todosLosCandidatosSegunCantidad = todosLosCandidatosSegunCantidad.Where( x => x.cantidad > 0 ).ToList();

			var todosLosParticipantes = resultadosCantidad.Select( resul => new { codigo = resul.PartPromo.Codigo, participante = resul.PartPromo.Id, regla = resul.Regla.Id, requerido = resul.Requerido, beneficiario = resul.PartPromo.Beneficiario } ).Distinct().ToList();

			ItemUsabilidad itemUsabilidad;
			decimal montoRequeridoValor;
			foreach ( var item in todosLosParticipantes )
			{
				montoRequeridoValor = 0;
				foreach ( var candidato in todosLosCandidatosSegunCantidad )
				{
					ParticipanteRegla participanteEnResultado = resultadoreglas.Where( x => x.PartPromo.Id == item.participante ).First().PartPromo;
					Regla reglaEnCandidato = resultadoreglas.Where( x => x.PartPromo.Id == item.participante && x.Regla.Id == item.regla ).First().Regla;

					bool esLaRegla = resultadoreglas.Where( x => x.PartPromo.Id == item.participante && x.Regla.Id == item.regla ).ToList().First().Participantes.Find( x => x.Clave + x.Id == candidato.nombre ) != null;

					itemUsabilidad = new ItemUsabilidad();
					itemUsabilidad.IdParticipante = item.participante;
					itemUsabilidad.IdRegla = item.regla;
					itemUsabilidad.EsBeneficiario = item.beneficiario;
					itemUsabilidad.Clave = candidato.clave;
					itemUsabilidad.Candidato = candidato.nombre;
					itemUsabilidad.CantidadEnItem = Convert.ToDecimal( candidato.cantidad, new CultureInfo( "en-US" ) );

					if ( reglaEnCandidato.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnResultado.Codigo].CantidadMonto )
					{
						itemUsabilidad.RequeridoPorRegla = 1;
					}
					else
					{
						itemUsabilidad.RequeridoPorRegla = (esLaRegla) ? item.requerido : 0;
					}
					itemUsabilidad.Precio = candidato.precio;

					if (candidato.clave == "COMPROBANTE.FACTURADETALLE.ITEM")
						montoRequeridoValor += itemUsabilidad.Precio * itemUsabilidad.RequeridoPorRegla;

					if (candidato.clave == "COMPROBANTE.VALORESDETALLE.ITEM")
						itemUsabilidad.RequeridoPorRegla = montoRequeridoValor;

					usabilidadLoca.Add( itemUsabilidad );
				}
			}

			//pongo dificultad
			foreach ( ItemUsabilidad item in usabilidadLoca )
			{
				if ( item.RequeridoPorRegla > 0 )
				{
					//papota! si le alcanza para suplir todos los participantes que lo podrian consumir, dificultad es cero!!!
					item.Dificultad = usabilidadLoca
										.Where( x => !(x.IdParticipante == item.IdParticipante) && x.Candidato == item.Candidato )
										.Sum( x => x.AporteRelativoDelCandidato );

					ParticipanteRegla participanteEnResultado = resultadoreglas.Where( x => x.PartPromo.Id == item.IdParticipante ).First().PartPromo;
					Regla reglaEnResultado = resultadoreglas.Where( x => x.PartPromo.Id == item.IdParticipante && x.Regla.Id == item.IdRegla ).First().Regla;

                    if ( item.EsBeneficiario )
                    {
                        item.Dificultad += 1;
                    }

                    if ( reglaEnResultado.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[ participanteEnResultado.Codigo ].CantidadMonto )
					{
						item.Dificultad = (item.Dificultad + 1 )* 1000;
					}
				}
			}

			return usabilidadLoca;
		}
	}
}
