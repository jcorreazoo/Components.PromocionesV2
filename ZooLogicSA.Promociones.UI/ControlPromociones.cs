using DevExpress.XtraEditors;
using ZooLogicSA.Promociones.UI.Clases;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using ZooLogicSA.Promociones.UI.Clases.Managers;
using ZooLogicSA.Promociones.UI.Clases.Interpretes;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;
using DevExpress.XtraEditors.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Mask;
using System.Threading;
using System.Globalization;

namespace ZooLogicSA.Promociones.UI
{
    public partial class ControlPromociones : XtraUserControl
    {
        public event EventHandler<CustomEventArgs> EventoPerdioFoco;
        public event EventHandler<CustomEventArgs> EventoControlFin;
        public event EventHandler<DatosParticipanteEventArgs> EventoSeleccionParticipante;
        public event EventHandler<DatosDeParticipantesReglasEventsArgs> EventoAntesDeSerializar;
        public event EventHandler<DatosParticipanteEventArgs> EventoLlenaComboDeListaDePrecios;

        string valueAnterior = "";
        private Control ultimoControl;
        private string idPromocion;

        public string IDPromocion
        {
            get { return this.idPromocion; }
            set { this.idPromocion = value; }
        }

        private ControllerPromocion _kontrolerPromocion;
        private PromocionUI _promocion;

        public string Value
        {
            get
            {
                InterpreteXMLPromocion interpreteXML = new InterpreteXMLPromocion() ;
                interpreteXML.EventoAntesDeSerializarPromocion+=interpreteXML_EventoAntesDeSerializarPromocion;
                interpreteXML.Ejecutarproceso( this._kontrolerPromocion, this.idPromocion );
                return interpreteXML.XML;
            }

            set
            {
                if ( value != valueAnterior )
                {
                    valueAnterior = value;
					this.SuspendLayout();
					this._kontrolerPromocion.LimpiarControles();
                    this._kontrolerPromocion.Interprete.RestaurarInformacion( InterpreteXMLPromocion.ObtenerValorNodoInformacionControl( value ) );
                    this._kontrolerPromocion.ActualizarParticipantes();
					this.ResumeLayout();
                }
            }
        }
        #region Evento EventoAntesDeSerializarPromocion
        void interpreteXML_EventoAntesDeSerializarPromocion( object sender, DatosDeParticipantesReglasEventsArgs e )
        {
            this.EventoAntesDeSerializar( sender, e );
        }
         #endregion

         #region evento EventoSeleccionParticipante
         void EventosPromocion_EventoSeleccionParticipante( object sender, DatosParticipanteEventArgs e )
         {
             OnRaiseEventoSeleccionarParticipante( e );
         }

         protected void OnRaiseEventoSeleccionarParticipante( DatosParticipanteEventArgs e )
         {
             EventHandler<DatosParticipanteEventArgs> handler = EventoSeleccionParticipante;

             if ( handler != null )
             {
                 handler( this, e );
             }
         }
        #endregion

         #region evento EventoLlenaComboDeListaDePrecios
         void EventosPromocion_EventoLlenaComboDeListaDePrecios( object sender, DatosParticipanteEventArgs e )
         {
             OnRaiseEventoLlenaComboDeListaDePrecios( e );
         }

         protected void OnRaiseEventoLlenaComboDeListaDePrecios( DatosParticipanteEventArgs e )
         {
             EventHandler<DatosParticipanteEventArgs> handler = EventoLlenaComboDeListaDePrecios;

             if ( handler != null )
             {
                 handler( this, e );
             }
         }
        #endregion

         public void MostrarVistaPredeterminada( bool tlEsEdicion, string valorAnterior )
        {
            this._kontrolerPromocion.MostrarVistaPredeterminada( tlEsEdicion, valorAnterior );
        }

        public ControlPromociones()
        {
            //CultureInfo ci = CultureInfo.CreateSpecificCulture( CultureInfo.CurrentCulture.Name );
            CultureInfo ci = CultureInfo.CreateSpecificCulture(System.Globalization.CultureInfo.CurrentUICulture.ToString()); 

            DateTimeFormatInfo dtfi = ci.DateTimeFormat;
            dtfi.AbbreviatedDayNames = new String[] { "Do.", "Lu.", "Ma.", "Mi.",
                                                "Ju.", "Vi.", "Sa." };
           
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            InitializeComponent();
            this.idPromocion = "1";
            InicializarClases();
        }

        public void ExcluirPromociones( BeneficioType tipo )
        {
            this._kontrolerPromocion.ExcluirPromocionesPorTipo( tipo );
        }

        private void InicializarClases()
        {
            this.ultimoControl = this.comboComportamiento;
            List<Control> listaControlesCanFocused = new List<Control>();
            listaControlesCanFocused.AddRange( new List<Control>{ lstBeneficios, maskPromocion1, mskCondicion, 
                                                filBeneficios, filCondicion, treeFiltroParticipantes, 
                                                treeParticipantes, timDesde, timHasta, dtFechaDesde, 
                                                dtFechaHasta, weekDias, cmbTipoPrecio, expBeneficio, expCondicion, comboComportamiento,
												comboListaDePrecios, maskCuotasSinRecargo, aplicaAutomaticamente } );
            this._kontrolerPromocion = new ControllerPromocion();
            this._kontrolerPromocion.Inicializar( lstBeneficios, maskPromocion1, maskTopeDescuento, mskCondicion,
                                                lblBeneficio, lblCondicion, filCondicion, filBeneficios,
                                                btnFiltroCondicion, btnFiltroBeneficio, this, treeParticipantes,
                                                treeFiltroParticipantes, timDesde, timHasta, listaControlesCanFocused,
                                                weekDias, dtFechaDesde, dtFechaHasta, cmbTipoPrecio,
                                                expCondicion, expBeneficio, comboComportamiento, this.flowLayoutPanel2,
												comboListaDePrecios, lblListaDePrecios, maskCuotasSinRecargo, aplicaAutomaticamente, lblAplicaAutomaticamente );
            this._promocion = new PromocionUI();
            this._promocion.AsignarKontroler( this._kontrolerPromocion );
            this._kontrolerPromocion.EventosPromocion.EventoControlFin += new System.EventHandler<CustomEventArgs>( EventosPromocion_EventoControlFin );
            this._kontrolerPromocion.EventosPromocion.EventoPerdioFoco += new System.EventHandler<CustomEventArgs>( EventosPromocion_EventoPerdioFoco );
            this._kontrolerPromocion.EventosPromocion.EventoLlenaComboDeListaDePrecios += new System.EventHandler<DatosParticipanteEventArgs>( EventosPromocion_EventoLlenaComboDeListaDePrecios );
            this._kontrolerPromocion.EventosPromocion.EventoSeleccionParticipante += new System.EventHandler<DatosParticipanteEventArgs>( EventosPromocion_EventoSeleccionParticipante );
        }

        void EventosPromocion_EventoPerdioFoco( object sender, CustomEventArgs e )
        {
            OnRaiseEventoPerdioFoco( e );
        }

        void EventosPromocion_EventoControlFin( object sender, CustomEventArgs e )
        {
            OnRaiseEventoControlFin( e );
        }

        protected void OnRaiseEventoPerdioFoco( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoPerdioFoco;

            if ( handler != null )
            {
                handler( this, e );
            }
        }
        
        protected void OnRaiseEventoControlFin( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoControlFin;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        public PromocionUI promocion()
        {
            return this._promocion;
        }

        public void SetearXMLParticipantes( string xml )
        {
            this._kontrolerPromocion.InicializarManagerParticipantes( xml );
        }

        public Control ObtenerUltimoControl()
        {
            return this.ultimoControl;
        }

        public List<string> ValidarControl( string idPromocion, string idRedondeo)
        {
            this.idPromocion = idPromocion;
            return this._kontrolerPromocion.ValidarPromocionUI( idRedondeo );
        }

        private void ControlPromociones_Load( object sender, EventArgs e )
        {

        }
    }
}
