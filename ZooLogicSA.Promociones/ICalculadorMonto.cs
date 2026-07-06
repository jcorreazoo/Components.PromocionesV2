using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones
{
    public interface ICalculadorMonto
    {
        decimal ObtenerPrecio( IParticipante participante, decimal cantidad );
    }
}
