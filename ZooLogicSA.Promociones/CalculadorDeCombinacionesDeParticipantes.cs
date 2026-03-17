using System.Collections.Generic;
using System.Linq;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class CalculadorDeCombinacionesDeParticipantes : ICalculadorDeCombinacionesDeParticipantes
    {
        public List<CombinacionParticipanteFaltantes> ObtenerCombinaciones( List<ParticipanteRegla> participantes, decimal cantidadDeElementosEnCombinacion )
        {
            List<CombinacionParticipanteFaltantes> retorno = new List<CombinacionParticipanteFaltantes>();

            if ( cantidadDeElementosEnCombinacion > 0 && participantes.Count > 0 )
            {
                participantes.ForEach( x => retorno.Add( new CombinacionParticipanteFaltantes()
                                                                        {
                                                                            Faltantes = new List<ParticipanteFaltante>()
                                                                                        { 
                                                                                            new ParticipanteFaltante()
                                                                                            { 
                                                                                                Participante = x, 
                                                                                                Cantidad = 1 
                                                                                                } 
                                                                                            }
                                                                        } ) );
            }

            for ( int i = 1; i < cantidadDeElementosEnCombinacion; i++ )
            {
                if ( retorno.Count > 0 && retorno.Count < 20 )
                {
                    List<CombinacionParticipanteFaltantes> temp = new List<CombinacionParticipanteFaltantes>();
                    retorno.ForEach(x => temp.AddRange(this.Caca(x.Faltantes, participantes)));
                    retorno = temp;
                }
            }

            return retorno;
        }

        private List<CombinacionParticipanteFaltantes> Caca( List<ParticipanteFaltante> uno, List<ParticipanteRegla> p )
        {
            List<CombinacionParticipanteFaltantes> retorno = new List<CombinacionParticipanteFaltantes>();

            foreach ( ParticipanteRegla item in p )
            {
                CombinacionParticipanteFaltantes temp = new CombinacionParticipanteFaltantes();
                temp.Faltantes.AddRange( uno );
                temp.Faltantes.Add( new ParticipanteFaltante() { Participante = item, Cantidad = 1 } );

                temp.Faltantes = temp.Faltantes
                        .GroupBy( x => x.Participante )
                        .Select( x => new ParticipanteFaltante() {
                                                                Participante = x.First().Participante,
                                                                Cantidad = x.Sum( y=>y.Cantidad )
                                                                  } )
                        .ToList();

                retorno.Add( temp );
            }

            return retorno;
        }
    }
}
