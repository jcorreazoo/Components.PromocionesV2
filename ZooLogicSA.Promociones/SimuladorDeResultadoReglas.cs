using System.Collections.Generic;
using ZooLogicSA.Promociones.EvaluacionReglas;

namespace ZooLogicSA.Promociones
{
    public class SimuladorDeResultadoReglas : ISimuladorDeResultadoReglas
    {
        private ConfiguracionComportamiento configuracionComportamiento;

        public SimuladorDeResultadoReglas( ConfiguracionComportamiento configuracionComportamiento )
        {
            this.configuracionComportamiento = configuracionComportamiento;
        }

        public void AgregarDummies( List<ResultadoReglas> resultadoreglas, List<ParticipanteFaltante> imposibles, CombinacionParticipanteFaltantes combinacion )
        {
            foreach ( ParticipanteFaltante participante in imposibles )
            {
                ResultadoReglas resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad );
                resultadoCantidad.Satisfecho = resultadoCantidad.Satisfecho + participante.Cantidad;

                ParticipanteDummy p = new ParticipanteDummy( participante.Participante, participante.Cantidad );
                resultadoCantidad.Participantes.Add( p );

                resultadoCantidad.Cumple = resultadoCantidad.Satisfecho >= resultadoCantidad.Requerido;
            }

            foreach ( ParticipanteFaltante participante in combinacion.Faltantes )
            {
				ResultadoReglas resultadoCantidad;

				resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad );

				if ( resultadoCantidad == null )
				{
					resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto );
				}
				
				resultadoCantidad.Satisfecho = resultadoCantidad.Satisfecho + participante.Cantidad;

                ParticipanteDummy p = new ParticipanteDummy( participante.Participante, participante.Cantidad );
                resultadoCantidad.Participantes.Add( p );

                resultadoCantidad.Cumple = resultadoCantidad.Satisfecho >= resultadoCantidad.Requerido;
            }
        }

        public void QuitarDummies( List<ResultadoReglas> resultadoreglas, List<ParticipanteFaltante> imposibles, CombinacionParticipanteFaltantes combinacion )
        {

            foreach ( ParticipanteFaltante participante in imposibles )
            {
                ResultadoReglas resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad );

				if ( resultadoCantidad == null )
				{
					resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto );
				}
				
				resultadoCantidad.Satisfecho = resultadoCantidad.Satisfecho - participante.Cantidad;

                //ParticipanteDummy p = new ParticipanteDummy( participante.Participante, 1 );
                resultadoCantidad.Participantes.RemoveAll( x => x.GetType() == typeof( ParticipanteDummy ) );

                resultadoCantidad.Cumple = resultadoCantidad.Satisfecho >= resultadoCantidad.Requerido;
            }

            foreach ( ParticipanteFaltante participante in combinacion.Faltantes )
            {
                ResultadoReglas resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad );

				if ( resultadoCantidad == null )
				{
					resultadoCantidad = resultadoreglas.Find( x => x.PartPromo == participante.Participante && x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto );
				}

				resultadoCantidad.Satisfecho = resultadoCantidad.Satisfecho - participante.Cantidad;

                //ParticipanteDummy p = new ParticipanteDummy( resultadoCantidad.Participantes[0], 1 );
                resultadoCantidad.Participantes.RemoveAll( x => x.GetType() == typeof( ParticipanteDummy ) );

                resultadoCantidad.Cumple = resultadoCantidad.Satisfecho >= resultadoCantidad.Requerido;
            }
        }
    }
}
