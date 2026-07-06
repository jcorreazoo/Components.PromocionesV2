using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarCuotasSinRecargo : IValidacionPromocion
    {
        private string mensajeError = "";

        #region IValidacionPromocion Members

        public bool Validar(params object[] parametros)
        {
            bool retorno = true;
            
            string parametro = (string)parametros[0];

            if (String.IsNullOrEmpty(parametro))
            {
                retorno = false;
                this.mensajeError = "Debe cargar la cantidad de cuotas";
            }
            //Entero cuotas = Convert.ToInt16( parametro );
            Int16 cuotas = Convert.ToInt16(parametro);

            if (cuotas > 150)
            {
                retorno = false;
                this.mensajeError = "No se puede asignar una cantidad de cuotas mayor a 150.";
            }
            if (cuotas == 0)
            {
                retorno = false;
                this.mensajeError = "Debe asignar una cantidad de cuotas mayor a cero en el beneficio.";
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
