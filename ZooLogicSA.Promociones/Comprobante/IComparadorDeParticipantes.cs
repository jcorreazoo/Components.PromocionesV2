using System;
using ZooLogicSA.Promociones.Utils;
namespace ZooLogicSA.Promociones.Comprobante
{
    public interface IComparadorDeParticipantes
    {
        decimal ObtenerDiferenciaDeImporte( IEvaluadorMatematico evaluador, ConfiguracionComportamiento comportamiento, IParticipante participanteOriginal, IParticipante participantePromocionado, float cantidad );
    }
}
