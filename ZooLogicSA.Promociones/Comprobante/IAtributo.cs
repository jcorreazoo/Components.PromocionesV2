using System;

namespace ZooLogicSA.Promociones.Comprobante
{
    public interface IAtributo
    {
        TipoDato TipoDato { get; set; }
        object Valor { get; set; }
    }
}
