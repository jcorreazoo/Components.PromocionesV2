using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class EvaluadorDeUsabilidad
    {

        private ConfiguracionComportamiento configuracionComportamiento;

        public EvaluadorDeUsabilidad( ConfiguracionComportamiento comportamiento )
        {
            this.configuracionComportamiento = comportamiento;
        }

        public List<ItemUsabilidad> ObtenerUsabilidad( List<ResultadoReglas> resultadoreglas )
        {
            List<ItemUsabilidad> usabilidadLoca = new List<ItemUsabilidad>();

            List<ResultadoReglas> resultadosCantidad = resultadoreglas.Where( x => x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad ).ToList();

            var todosLosCandidatosSegunCantidad = resultadosCantidad.SelectMany( parti => parti.Participantes )
																		.Select( o => new { 
																								nombre = o.Clave + o.Id, 
																								cantidad = Convert.ToDecimal( o.Cantidad, new CultureInfo( "en-US" ) ), 
																								precio = o.ObtenerPrecioUnitario()
																						} )
																		.Distinct()
																		.ToList();

            todosLosCandidatosSegunCantidad = todosLosCandidatosSegunCantidad.Where( x => x.cantidad > 0 ).ToList();

            var todosLosParticipantes = resultadosCantidad.Select( resul => new { participante = resul.PartPromo.Id, regla = resul.Regla.Id, requerido = resul.Requerido } ).Distinct().ToList();

            ItemUsabilidad itemUsabilidad;
            foreach ( var item in todosLosParticipantes )
            {
                foreach ( var candidato in todosLosCandidatosSegunCantidad )
                {
                    bool esLaRegla = resultadoreglas.Where( x => x.PartPromo.Id == item.participante && x.Regla.Id == item.regla ).ToList().First().Participantes.Find( x => x.Clave + x.Id == candidato.nombre ) != null;

                    itemUsabilidad = new ItemUsabilidad();
                    itemUsabilidad.IdParticipante = item.participante;
                    itemUsabilidad.IdRegla = item.regla;
                    itemUsabilidad.Candidato = candidato.nombre;
                    itemUsabilidad.CantidadEnItem = Convert.ToDecimal( candidato.cantidad, new CultureInfo( "en-US" ) );
                    itemUsabilidad.RequeridoPorRegla = (esLaRegla) ? item.requerido : 0;
                    itemUsabilidad.Precio = candidato.precio;
                    usabilidadLoca.Add( itemUsabilidad );
                }
            }

            // pongo dificultad
            foreach ( ItemUsabilidad item in usabilidadLoca )
            {
                if ( item.RequeridoPorRegla > 0 )
                {
                    // papota! si le alcanza para suplir todos los participantes que lo podrian consumir, dificultad es cero!!!
                    item.Dificultad = usabilidadLoca
                                        .Where( x => !(x.IdParticipante == item.IdParticipante) && x.Candidato == item.Candidato )
                                        .Sum( x => x.AporteRelativoDelCandidato );
                }
            }

            return usabilidadLoca;
        }
    }
}
