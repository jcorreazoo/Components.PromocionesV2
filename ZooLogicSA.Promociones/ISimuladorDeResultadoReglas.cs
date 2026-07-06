using System.Collections.Generic;
namespace ZooLogicSA.Promociones
{
    public interface ISimuladorDeResultadoReglas
    {
        void AgregarDummies( List<ResultadoReglas> resultadoreglas, List<ParticipanteFaltante> imposibles, CombinacionParticipanteFaltantes combinacion );
        void QuitarDummies( List<ResultadoReglas> resultadoreglas, List<ParticipanteFaltante> imposibles, CombinacionParticipanteFaltantes combinacion );
    }
}
