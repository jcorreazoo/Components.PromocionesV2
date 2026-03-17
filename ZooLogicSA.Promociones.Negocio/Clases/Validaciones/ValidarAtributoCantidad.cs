using System;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarAtributoCantidad : IValidacionPromocion
    {
        private List<string> _participantesRegla;
        private string _nombreAtributoCantidad;
        private string _cadenaCantidadBuscada;
        private string _mensajeError;

        public ValidarAtributoCantidad(List<string> participantesRegla, string nombreAtributoCantidad)
        {
            this._participantesRegla = participantesRegla;
            this._nombreAtributoCantidad = nombreAtributoCantidad;
            this._cadenaCantidadBuscada = "[" + this._nombreAtributoCantidad + "] ";
        }

        #region IValidacionPromocion Members

        public bool Validar(params object[] parametros)
        {
            if (!this.RecorrerParticipantesReglaYValidarQueElAtributoCantidadNoEsteSolo())
            {
                this._mensajeError = "El atributo Cantidad no puede estar solo en una regla de participantes.";
                return false;
            }

            return true;
        }

        public string ObtenerMensajeError()
        {
            return this._mensajeError;
        }

        #endregion

        private bool DetectarAtributoCantidadEnRegla(string regla)
        {
            return regla.Contains(this._cadenaCantidadBuscada);
        }

        private bool RecorrerParticipantesReglaYValidarQueElAtributoCantidadNoEsteSolo()
        {
            bool valida = true;
            string[] arr;
            string[] delimitador = new string[1];
            delimitador[0] = "[";

            foreach (string participanteRegla in this._participantesRegla)
            {
                if (DetectarAtributoCantidadEnRegla(participanteRegla))
                {
                    // agrego caracteres al ppio y final para el caso que la regla empiece o termine con el atributo
                    string buscar = "  " + participanteRegla + "   ";
                    arr = buscar.Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length <= 2)
                    {
                        valida = false;
                        break;
                    }
                }
            }
            return valida;
        }
    }
}
