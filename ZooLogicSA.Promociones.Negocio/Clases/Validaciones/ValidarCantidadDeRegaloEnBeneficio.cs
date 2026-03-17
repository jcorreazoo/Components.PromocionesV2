using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarCantidadDeRegaloEnBeneficio: IValidacionPromocion
    {
        private string _mensajeError = "";

        public bool Validar( params object[] parametros )
        {
            bool valida = true;
            int cantidadLleva = 0;
            int cantidadPaga = 0;

            if ( parametros == null )
            {
                this._mensajeError = "No se recibieron correctamente la cantidad de parámetros.";
                return false;
            }

            if ( parametros.Length < 2 )
            {
                valida = false;
                this._mensajeError = "No se recibieron correctamente la cantidad de parámetros.";
            }

            if ( !int.TryParse( (string)parametros[0].ToString(), out cantidadLleva ) ||
                 !int.TryParse( (string)parametros[1].ToString(), out cantidadPaga ) )
            {
                valida = false;
                this._mensajeError = "No se recibieron correctamente los parámetros.";
            }
            else
            {
                if ( cantidadPaga >= cantidadLleva )
                {
                    valida = false;
                    this._mensajeError = "No se puede crear una promoción donde la cantidad que paga es igual o mayor a la que lleva.";
                }
            }

            return valida;
        }

        public string ObtenerMensajeError()
        {
            return this._mensajeError;
        }
    }
}
