using System;
using ZooLogicSA.Promociones.Comprobante;
namespace ZooLogicSA.Promociones.Comparadores
{
    public interface IComparador
    {
        bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante );
    }
}
