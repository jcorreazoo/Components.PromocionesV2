using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Windows.Forms;
using ZooLogicSA.Promociones.Negocio.Clases;

namespace ZooLogicSA.Promociones.UI.Clases.Managers
{
    public class ManagerValidaciones
    {
        private string _mensajeError;

        public string ObtenerMensajeError()
        {
            return _mensajeError;
        }

        public bool Validar(ControllerPromocion control, ValidacionPromoType type)
        {
            bool valida = false;
            _mensajeError = "";

            switch (type)
            {
                case ValidacionPromoType.ValidarCantidadParticipantes:
                    {
                        valida = this.ValidarCantidadParticipantesUI( control );
                        break;
                    }
                case ValidacionPromoType.ValidarDescuento:
                    {
                        valida = this.ValidarDescuentoUI( control );
                        break;
                    }
                case ValidacionPromoType.ValidarDescuentoYCuotas:
                    {
                        valida = this.ValidarDescuentoYCuotasUI( control );
                        break;
                    }
                default:
                    {
                        valida = this.ValidarOtrosTiposUI( control );
                        break;
                    }
            }
            return valida;
        }

        private bool ValidarOtrosTiposUI(ControllerPromocion control)
        {
            bool valida = true;
            List<string> listaParticipantes = control.ObtenerListaReglaParticipantesCondicionConDetalleItem();
            string nombreAtributo = ManagerFiltros.NombreAtributoBasicoCantidad();

            ValidarAtributoCantidad validarMontoCantidad = new ValidarAtributoCantidad(listaParticipantes, nombreAtributo);
            valida = validarMontoCantidad.Validar();
            if (!valida)
            {
                _mensajeError = validarMontoCantidad.ObtenerMensajeError();
            }

            return valida;
        }

        private bool ValidarDescuentoYCuotasUI(ControllerPromocion control)
        {
            bool valida = true;
            int cantCuotas;
            Decimal porceDescuento;

            int.TryParse( control.ObtenerCuotasSinRecargo(), out cantCuotas );
            Decimal.TryParse( control.ObtenerValorMaskBeneficio(), out porceDescuento );

            if ( cantCuotas == 0 )
                valida = this.ValidarDescuentoUI( control );
            else
            {
                string cuotas = control.ObtenerCuotasSinRecargo();
                ValidarCuotasSinRecargo validarCuotas = new ValidarCuotasSinRecargo();
                valida = validarCuotas.Validar( cuotas );
                if ( !valida )
                    _mensajeError = validarCuotas.ObtenerMensajeError();
                else
                {
                    if ( porceDescuento > 0 )
                        valida = this.ValidarDescuentoUI( control ); ;
                }
            }
                
            return valida;
        }

        private bool ValidarDescuentoUI( ControllerPromocion control )
        {
            bool valida;

            string descuento = control.ObtenerValorMaskBeneficio();

            List<string> listaParticipantes = control.ObtenerListaReglaParticipantesCondicionConDetalleItem();
            string nombreAtributo = ManagerFiltros.NombreAtributoBasicoCantidad();

            ValidarDescuento validarCantidad = new ValidarDescuento();
            valida = validarCantidad.Validar( descuento );
            if ( !valida )
            {
                _mensajeError = validarCantidad.ObtenerMensajeError();
            }
            else
            {
                ValidarAtributoCantidad validarMontoCantidad = new ValidarAtributoCantidad(listaParticipantes, nombreAtributo);
                valida = validarMontoCantidad.Validar();
                if (!valida)
                {
                    _mensajeError = validarMontoCantidad.ObtenerMensajeError();
                }
            }

            return valida;
        }

        public List<string> ValidarControles(ControllerPromocion control)
        {
            List<string> mensajes = new List<string>();
            return mensajes;
        }

        public bool ValidarCantidadParticipantesUI( ControllerPromocion control )
        {
            bool valida;
            int cantidad;
            int cantidadLleva;
            List<string> listaParticipantes;
            string nombreAtributo;
            TipoPromocion tipoPromocion;            
            string beneficioType;

            tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
            beneficioType = tipoPromocion.BeneficioType.ToString();

            int.TryParse( control.ObtenerValorMaskCondicion(), out cantidad );
            int.TryParse( control.ObtenerValorMaskBeneficio(), out cantidadLleva );
            listaParticipantes = control.ObtenerListaReglaParticipantesCondicionConDetalleItem();
            nombreAtributo = ManagerFiltros.NombreAtributoBasicoCantidad();
            
            ValidarCantidadParticipantes validarCantidad = new ValidarCantidadParticipantes( cantidad, listaParticipantes, nombreAtributo, beneficioType);
            valida = validarCantidad.Validar();
            if ( !valida )
            {
                _mensajeError = validarCantidad.ObtenerMensajeError();
            }
            else
            {
                ValidarAtributoCantidad validarMontoCantidad = new ValidarAtributoCantidad(listaParticipantes, nombreAtributo);
                valida = validarMontoCantidad.Validar();
                if (!valida)
                {
                    _mensajeError = validarMontoCantidad.ObtenerMensajeError();
                }
                else
                {
                    ValidarCantidadDeRegaloEnBeneficio validarCantidadRegalo = new ValidarCantidadDeRegaloEnBeneficio();
                    object[] parametros = { cantidad, cantidadLleva };
                    valida = validarCantidadRegalo.Validar(parametros);
                    if (!valida)
                    {
                        _mensajeError = validarCantidadRegalo.ObtenerMensajeError();
                    }
                }
            }

            return valida;
        }

        public bool ValidarExistenciaParticipanteTipoDetalle( IControllerPromocion control )
        {
            bool valida = true;
            TipoPromocion tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
            List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion( tipoPromocion, control );
            string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );
            
            if ( ManagerReglas.NoTieneParticipantesSegunDetalle( listaParticipantes, detalle, control ) )
            {
                valida = false;
            }

            return valida;
        }

        public bool ValidarCantidadCondicionesBeneficio( ControllerPromocion control )
        {
            bool valida = true;
            List<string> listaParticipantes = control.ObtenerListaReglaParticipantesBeneficioConDetalleItem();
            string nombreAtributo = ManagerFiltros.NombreAtributoBasicoCantidad();

            ValidarAtributoCantidad validarMontoCantidad = new ValidarAtributoCantidad( listaParticipantes, nombreAtributo );
            valida = validarMontoCantidad.Validar();
            if ( !valida )
            {
                _mensajeError = validarMontoCantidad.ObtenerMensajeError();
            }

            return valida;
        }

        public bool ValidarCantidadCondicionesDeValores( IControllerPromocion control )
        {
            TipoPromocion tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
            List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion(tipoPromocion, control);
            string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType(CodigoDetalleType.DetalleValor);
            return ManagerReglas.ObtenerCantidadParticipantesSegunDetalle(listaParticipantes, detalle, control) <= 1;
        }
        
        public bool ValidarExistenciaParticipanteTipoCupon( ControllerPromocion control )
        {
            bool valida = true;
            TipoPromocion tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
            List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion( tipoPromocion, control );
            string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );

            if ( ManagerReglas.NoTieneParticipantesSegunDetalle( listaParticipantes, detalle, control ) )
            {
                valida = false;
            }

            return valida;
        }

        public bool RecorrerParticipantesReglaYValidarQueNoExistanCantidadesConDecimales(ControllerPromocion control)
        {
            int startIndex = 0;
            string cantidad = "";
            string atributoCantidad = "[CANTIDAD]";
            List<string> listaParticipantes;
            listaParticipantes = control.ObtenerListaReglaParticipantesCondicionConDetalleItem();
            bool lasCantidadesSonEnteras = true;

            foreach (string participanteRegla in listaParticipantes)
            {
                if ( participanteRegla.Contains(atributoCantidad) )
                {
                    startIndex = participanteRegla.IndexOf(atributoCantidad);
                    cantidad = ObtenerStringDeCantidadEnParticipanteRegla(participanteRegla, "'", startIndex);
                    if (cantidad.Contains(".") || cantidad.Contains(","))
                    {
                        lasCantidadesSonEnteras = false;
                        break;
                    }
                }
            }
            return lasCantidadesSonEnteras;
        }

        private string ObtenerStringDeCantidadEnParticipanteRegla(string participanteRegla, string delimitador, int startIndex)
        {
            string valorCantidad = "";
            if (participanteRegla.Contains(delimitador))
            {
                valorCantidad = participanteRegla.Substring(participanteRegla.IndexOf("'", startIndex) + 1, participanteRegla.IndexOf("'", participanteRegla.IndexOf("'", startIndex) + 1) - 1 - participanteRegla.IndexOf("'", startIndex));
            }
            return valorCantidad;
        }

        public bool ValidarMediosDePagoYPromoAutomatica(IControllerPromocion control)
        {
            bool retorno = true;
            if (control.ObtenerAplicaAutomaticamente())
            {
                TipoPromocion tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
                List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion(tipoPromocion, control);
                string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType(CodigoDetalleType.DetalleValor);
                retorno = ManagerReglas.ObtenerCantidadParticipantesSegunDetalle(listaParticipantes, detalle, control) == 0;
            }
            
            return retorno;
        }
    }
}
