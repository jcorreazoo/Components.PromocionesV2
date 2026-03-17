using System.Collections.Generic;

namespace ZooLogicSA.Promociones
{
    public interface IExtractorDeCoincidenciasDesdeConsumo
    {
        List<CoincidenciaEvaluacion> Obtener( List<ConsumoParticipanteEvaluado> consumo );
    }
}
