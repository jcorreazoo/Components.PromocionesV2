using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.UI.Controllers;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.UI.Clases.Managers;
using System.Xml;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Globalization;
using ZooLogicSA.Promociones.UI;
using System.IO;


namespace ZooLogicSA.Promociones.UI.Clases.Interpretes
{
    public class InterpreteXMLPromocion
    {
        private string xml;
        private IControllerPromocion control;
        private const string cNombreNodoPromocion = "Promocion";
        private const string cNombreNodoInformacionControl = "InformacionControl";
        private Serializador serializador = new Serializador();
        public event EventHandler<DatosDeParticipantesReglasEventsArgs> EventoAntesDeSerializarPromocion;

        public InterpreteXMLPromocion()
        {
        }

        public InterpreteXMLPromocion( IControllerPromocion control, string idPromocion )
        {
            this.Ejecutarproceso( control, idPromocion );
        }

        public void Ejecutarproceso( IControllerPromocion control, string idPromocion )
        {
            this.control = control;
            this.control.QuitarFocoControlesReglas();
            ManagerReglas.CrearPromo();
            this.AsignarNodoId( idPromocion );
            this.AsignarNodoInformacionControl();
            this.CrearReglasVigencia();
            this.CrearCondiciones();
            this.CrearBeneficios();
            this.SetearTopeBeneficio();
            this.SetearCuotasSinRecargo();
            this.SetearAplicaAutomaticamente();
            this.SetearConMedioDePago();
            this.SetearConfiguracionParaAsistente();
            this.AntesDeSerializarPromocion();
            this.SetearListaDePrecios();
            this.SerializarPromocion();
        }

        private void SetearConfiguracionParaAsistente()
        {
            ManagerReglas.Promo.Visualizacion= this.control.ObtenerVisualizacion();
        }

        private void AntesDeSerializarPromocion()
        {
            DatosDeParticipantesReglasEventsArgs argumentos = new DatosDeParticipantesReglasEventsArgs( ManagerReglas.Promo.Participantes );
            OnRaiseEventoAntesDeSerializar( argumentos );
        }

        public void OnRaiseEventoAntesDeSerializar( DatosDeParticipantesReglasEventsArgs e )
        {
            EventHandler<DatosDeParticipantesReglasEventsArgs> handler = EventoAntesDeSerializarPromocion;
            if ( handler != null )
            {
                handler( this, e );
            }
        }

        private void SetearTopeBeneficio()
        {
            ManagerReglas.Promo.TopeBeneficio = Convert.ToDecimal( this.control.ObtenerTopeCargado(), new CultureInfo("en-US") );
        }

        private void SetearListaDePrecios()
        {
            ManagerReglas.Promo.ListaDePrecios = this.control.ObtenerListaDePreciosCargada();
        }

        private void AsignarNodoInformacionControl()
        {
            this.control.ActualizarInformacionInterprete();
            ManagerReglas.Promo.InformacionControl = this.control.ObtenerValorEstructuraInterprete();
        }

        private void SerializarPromocion()
        {
            this.xml = serializador.Serializar<Promocion>( ManagerReglas.Promo );
        }

        public string XML
        {
            get
            {
                return this.xml;
            }
        }

        private void AsignarNodoId( string idPromo )
        {
            ManagerReglas.Promo.Id = idPromo;
        }

        private void CrearCondiciones()
        {
            TipoPromocion tipoPromo = this.control.ObtenerTipoPromocionSeleccionada();
            if ( tipoPromo != null )
            {
                List<string> listaCondiciones = this.control.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo );
                this.AgregarReglaParticipantes( listaCondiciones, false );

                if ( tipoPromo.TieneBeneficioFiltro() )
                {
                    List<string> listaBeneficios = this.control.ObtenerListaReglaParticipantesBeneficio();

                    this.AgregarReglaParticipantes( listaBeneficios, true );
                }
            }
        }

        private void AgregarReglaParticipantes( List<string> lista, bool EsBeneficiario )
        {
            if ( lista != null )
            {
                foreach ( string regla in lista )
                {
                    string nombreDetalle = ManagerReglas.ObtenerNombreDetalleParticipante( regla, this.control );
                    if ( ManagerReglas.EsDetalleCabecera( nombreDetalle ) )
                        ManagerReglas.AgregarReglaCondicion( regla, this.control, EsBeneficiario );
                    else
                    {
                        ParticipanteRegla participante = ManagerReglas.CrearReglaCondicion( regla, this.control, EsBeneficiario );
                        ManagerReglas.AgregarParticipanteRegla( participante );
                    }
                }
            }
        }

        private void CrearBeneficios()
        {
            List<Beneficio> listaBeneficios = ManagerBeneficios.CrearBeneficios( this.control );
            foreach ( Beneficio beneficio in listaBeneficios )
            {
                ManagerReglas.AgregarBeneficio( beneficio );
            }
        }

        private void CrearReglasVigencia()
        { 
            DateTime desdeFecha = this.control.ObtenerVigenciaFechaDesde();
            DateTime hastaFecha = this.control.ObtenerVigenciaFechaHasta();
            DateTime desdeHora = this.control.ObtenerVigenciaHoraDesde();
            DateTime hastaHora = this.control.ObtenerVigenciaHoraHasta();
            string[] dias = this.control.ObtenerVigenciaDiasSemana();
            ParticipanteRegla participanteReglaVigencia = ManagerReglas.CrearReglaVigencia( desdeFecha, hastaFecha, desdeHora, hastaHora, dias );
            ManagerReglas.Promo.Participantes.Add( participanteReglaVigencia );
        }

        public static string ObtenerValorNodoInformacionControl( string XML )
        {
            string valor = "";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml( XML );

                XmlNode nodoParticipante = xmlDoc.SelectSingleNode( cNombreNodoPromocion );
                XmlNode nodoInformacionControl = nodoParticipante.SelectSingleNode( cNombreNodoInformacionControl );
                valor = nodoInformacionControl.InnerText;
            }
            catch { }

            return valor;
        }

        private void SetearCuotasSinRecargo()
        {
//            ManagerReglas.Promo.CuotasSinRecargo = Convert.ToUInt16(6); //Convert.ToUInt16( this.control.ObtenerTopeCargado(), new CultureInfo("en-US") );
            ManagerReglas.Promo.CuotasSinRecargo = Convert.ToUInt16(this.control.ObtenerCuotasSinRecargo(), new CultureInfo("en-US"));
//            ObtenerCuotasSinRecargo()
        }
        private void SetearAplicaAutomaticamente()
        {
            ManagerReglas.Promo.AplicaAutomaticamente = this.control.ObtenerAplicaAutomaticamente();
        }

        private void SetearConMedioDePago()
        {
            ManagerReglas.Promo.ConMedioDePago = this.control.ObtenerAplicaConMedioDePago();
        }
    }
}
