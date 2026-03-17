using System.Collections.Generic;
using System.Linq;

namespace ZooLogicSA.Promociones
{
    public class ExtractorDeCoincidenciasDesdeConsumo : IExtractorDeCoincidenciasDesdeConsumo
    {
        public List<CoincidenciaEvaluacion> Obtener( List<ConsumoParticipanteEvaluado> consumo )
        {
            List<CoincidenciaEvaluacion> retorno = (from x in consumo
                                                          where x.Requerido == x.Satisfecho
                                                          select new CoincidenciaEvaluacion()
                                                          {
                                                              Atributos = x.Atributos,
                                                              CodigoParticipanteEnComprobante = x.CodigoParticipanteEnComprobante,
                                                              Consume = x.Requerido,
                                                              IdParticipanteEnComprobante = x.ParticipantesEnComprobante,
                                                              IdParticipanteRestante = x.ParticipantesRestantes,
                                                              IdParticipanteEnRegla = x.IdParticipanteEnRegla
                                                          }).ToList();

            return retorno;
        }
    }
}
