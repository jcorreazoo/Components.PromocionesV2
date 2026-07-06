using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public enum ValidacionPromoType
    {
        None = 0,
        ValidarCantidadParticipantes = 1,
        ValidarDescuento = 2,
        ValidarDescuentoYCuotas = 3
    }
}
