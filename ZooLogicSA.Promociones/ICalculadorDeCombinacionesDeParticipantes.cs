using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
namespace ZooLogicSA.Promociones
{
    public interface ICalculadorDeCombinacionesDeParticipantes
    {
        List<CombinacionParticipanteFaltantes> ObtenerCombinaciones( List<ParticipanteRegla> participantes, decimal cantidadDeElementosEnCombinacion );
    }
}
