using System.Collections.Generic;
using System;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarCantidadParticipantes : IValidacionPromocion
    {
        private int _cantidad;
        private List<string> _participantesRegla;
        private string _nombreAtributoCantidad;
        private string _cadenaBuscada;
        private const string cDelimitadorValorCantidad = "'";
        private string _mensajeError;
        private string _tipoPromocion;

        public ValidarCantidadParticipantes(int cantidad, List<string> participantesRegla, string nombreAtributoCantidad, string tipoPromocion)
        {
            this._cantidad = cantidad;
            this._participantesRegla = participantesRegla;
            this._nombreAtributoCantidad = nombreAtributoCantidad;
            this._cadenaBuscada = "[" + this._nombreAtributoCantidad + "] ";
            this._mensajeError = "";
            this._tipoPromocion = tipoPromocion;
        }

        public string ObtenerMensajeError()
        {
            return this._mensajeError;
        }

        public bool Validar( params object[] parametros )
        {

            if ( !this.RecorrerParticipantesReglaYValidarQueNoTengaMasDeUnAtributoCantidad() )
            {
                this._mensajeError = "Un regla de participante tiene mas de un atributo Cantidad.";
                return false;
            }

            if ( this._participantesRegla.Count == 0 )
            {
                return true;
            }

            if ( this._participantesRegla.Count == 1 && !this.DetectarAtributoCantidadEnRegla( this._participantesRegla[0] ) )
            {
                this._mensajeError = "No coincide la cantidad de la promoción con la sumatoria de cantidades del participante, debe agregar una regla de cantidad.";
                return false;
            }

            if ( !this.RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad() )
            {
                this._mensajeError = "No coincide la cantidad de la promoción con la sumatoria de cantidades de las reglas de participantes.";
                return false;
            }

            if ( this._tipoPromocion == "LLevaXPagaY" && this.UsaCantidadMayorOIgualEnPagaXLlevaY())
            {
                this._mensajeError = "No puede utilizar una condición distinta a 'igual' para la cantidad en las promociones del tipo 'Lleva una cantidad, paga otra cantidad'.";
                return false;
            }

            return true;
        }

        private int DetectarCantidadParticipantesTipoDetalle()
        {
            int cant = 0;

            return cant;
        }

        private bool DetectarAtributoCantidadEnRegla( string regla )
        {
            string cadenaBuscada = "[" + this._nombreAtributoCantidad + "] ";

            return regla.Contains( cadenaBuscada );
        }

        private bool RecorrerParticipantesReglaYValidarQueNoTengaMasDeUnAtributoCantidad()
        {
            bool valida = true;
            string[] arr;
            string[] delimitador = new string[1];
            delimitador[0] = this._cadenaBuscada;

            foreach ( string participanteRegla in this._participantesRegla )
            {
                if ( DetectarAtributoCantidadEnRegla( participanteRegla ) )
                {
                    // agrego caracteres al ppio y final para el caso que la regla empiece o termine con el atributo
                    string buscar = "  " + participanteRegla + "   ";
                    arr = buscar.Split( delimitador, StringSplitOptions.RemoveEmptyEntries );
                    if ( arr.Length > 2 )
                    {
                        valida = false;
                        break;
                    }
                }
            }

            return valida;
        }

        protected bool RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad()
        {
            int startIndex = 0;
            int cantidad = 0;

            foreach ( string participanteRegla in this._participantesRegla )
            {
                if ( DetectarAtributoCantidadEnRegla( participanteRegla ) )
                {
                    startIndex = participanteRegla.IndexOf( this._cadenaBuscada );
                    cantidad = cantidad + ObtenerValorDeCantidadEnParticipanteRegla( participanteRegla, cDelimitadorValorCantidad, startIndex );
                } else { cantidad += 1; }
            }
            return (this._cantidad == cantidad);
        }

        private int ObtenerValorDeCantidadEnParticipanteRegla( string participanteRegla, string delimitador, int startIndex )
        {
            int valor = 0;
            string valorCantidad = "";

            if ( participanteRegla.Contains( delimitador ) )
            {
                valorCantidad = participanteRegla.Substring( participanteRegla.IndexOf( "'", startIndex ) + 1, participanteRegla.IndexOf( "'", participanteRegla.IndexOf( "'", startIndex ) + 1 ) - 1 - participanteRegla.IndexOf( "'", startIndex ) );
                int.TryParse( valorCantidad, out valor );
            }

            return valor;
        }

        private bool UsaCantidadMayorOIgualEnPagaXLlevaY()
        {
            bool retorno = false;
            string cadena;
            int index;

            foreach(string participanteRegla in this._participantesRegla)
            {
                if (DetectarAtributoCantidadEnRegla(participanteRegla))
                {
                    index = participanteRegla.IndexOf("[" + this._nombreAtributoCantidad + "] ");
                    cadena = participanteRegla.Substring(participanteRegla.IndexOf("[" + this._nombreAtributoCantidad + "] ") + 11, 1);
                    if (cadena != "=")
                    {
                        retorno = true;
                    }
                    else
                    {
                        retorno = false;
                    }
                }
            }

            return retorno;
        }
    }
}
