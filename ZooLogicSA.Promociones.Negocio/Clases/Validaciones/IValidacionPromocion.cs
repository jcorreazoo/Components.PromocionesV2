using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public interface IValidacionPromocion
    {
        bool Validar( params object[] parametros );
        string ObtenerMensajeError();
    }
}
