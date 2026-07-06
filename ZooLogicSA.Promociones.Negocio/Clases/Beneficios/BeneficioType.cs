using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Negocio.Clases.Beneficios
{
    public enum BeneficioType
    {
        None = 0,
        LlevaUnoPagaConDescuentoOtro = 1,
        LLevaXPagaY = 2,
        MontoFijoDeDescuento = 3,
        PorcentajeFijoDeDescuento = 4,
        PorcentajeFijoDeDescuentoBancario = 5,
        ValorDeOtraListaDePrecios = 6
    }
}
