using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarDescuento : IValidacionPromocion
    {
        private string mensajeError = "";

        #region IValidacionPromocion Members

        public bool Validar( params object[] parametros )
        {
            bool retorno = true;

            string parametro = (string)parametros[0];

            if ( String.IsNullOrEmpty( parametro  ) )
            {
                retorno = false;
                this.mensajeError = "Debe cargar el descuento";
            }
            //Decimal descuento = Convert.ToDecimal( parametro );
            Decimal descuento = Convert.ToDecimal( parametro, new CultureInfo( "en-US" ) );

            if ( descuento>100 )
            {
                retorno = false;
                this.mensajeError = "No se puede asignar un porcentaje de descuento mayor a 100";
            }
            if (descuento == 0)
            {
                retorno = false;
                this.mensajeError = "Debe asignar un porcentaje de descuento mayor a cero en el beneficio";
            }

            return retorno;
        }

        public string ObtenerMensajeError()
        {
            return this.mensajeError;
        }

        #endregion
    }
}
