using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public interface IArmadorDeCoincidencias
    {
        List<ConsumoParticipanteEvaluado> ObtenerCoincidencias( Promocion promocion, List<ResultadoReglas> resultadoreglas );
    }
}
