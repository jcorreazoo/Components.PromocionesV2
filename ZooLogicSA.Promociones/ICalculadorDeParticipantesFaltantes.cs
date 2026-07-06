using System.Collections.Generic;
namespace ZooLogicSA.Promociones
{
    public interface ICalculadorDeParticipantesFaltantes
    {
        List<ParticipanteFaltante> Obtener( List<ResultadoReglas> resultadoreglas );
    }
}
