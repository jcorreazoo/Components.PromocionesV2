using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraLayout.Customization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZooLogicSA.Core.Visual;
using ZooLogicSA.Promociones;
using ZooLogicSA.Promociones.Informantes;
using DevExpress.XtraGrid.Views.Layout.ViewInfo;
using DevExpress.Utils;

namespace ZooLogicSA.Promociones.Asistente
{
    public partial class FrmAsistente : Form
    {
        delegate void CallbackSetearInformacionPromocion( List<InformacionPromocion> info );
        delegate void CallbackSetearInformacionPromocionParcial( List<InformacionPromocionIncumplida> info );
        delegate void CallbackSetearResultadoReglas( List<ResultadoReglas> info );
        delegate void CallbackSetearTexto( string mensaje );
        delegate void CallbackEjecutarMetodo();
        delegate void CallbackMostrarErrorEnPromocion( InformacionPromocionIncumplida infoPromo, string error );

        public KontrolerAsistente kontroler;
        public BindingSource source;

        private ParametrosAsistente parametros;

        public FrmAsistente( ParametrosAsistente parametros )
        {
            this.parametros = parametros;

            SetearAspecto creadorAspecto = new SetearAspecto();
            Aspecto aspecto = creadorAspecto.CrearAspecto(((ParametrosAsistente)parametros).InformacionAplicacion );
            this.MinimumSize = new Size( 240, 150 );
            InitializeComponent();
            this.zooFormExtender1.NoTieneMemoria += zooFormExtender1_NoTieneMemoria;
            this.kontroler = new KontrolerAsistente( aspecto );
            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            this.source = new BindingSource( bindinglist, null );
            this.grillaPromo.DataSource = source;
            this.kontroler.SeteosVisuales( this );
		}
        
		public FrmAsistente()
        {
           
        }

        void zooFormExtender1_NoTieneMemoria( object sender, EventArgs e )
        {
            this.AjustarUbicacion( this.parametros, Screen.PrimaryScreen.Bounds );
        }

        public void AjustarUbicacion( ParametrosAsistente paramAsistente, Rectangle rec )
        {
            this.Top = paramAsistente.TopComprobante;
            this.Height = paramAsistente.HeightComprobante;
            this.Width = 315;
            this.Left = paramAsistente.LeftComprobante + paramAsistente.WidthComprobante;
            if ( rec.Width <= paramAsistente.WidthComprobante + 8 )
                this.Left = this.Left - this.Width;
        }

        public void ArmarGrilla()
        {
           this.grillaPromo.DataSource = source;
        }

        public void AgregarInformacionPromociones( List<InformacionPromocion> info )
        {
            if ( this.grillaPromo.InvokeRequired )
            {
                CallbackSetearInformacionPromocion c = new CallbackSetearInformacionPromocion( this.AgregarInformacionPromociones );
                this.BeginInvoke( c, new object[] { info } );
            }
            else
            {
                if (info != null)
                {
                    this.grillaPromo.SuspendLayout();

                    List<InformacionPromocion> verdes = info.Where(x => x.Afectaciones > 0).ToList();
                    List<InformacionPromocionIncumplida> amarillas = info.Where(x => x.Afectaciones == 0).Select(x => x.infoIncumplida).ToList();

                    if (verdes != null)
                    {
                        this.kontroler.AgregarPromociones(verdes, this.source);
                    }

                    if (amarillas != null)
                    {
                        this.kontroler.AgregarPromociones(amarillas, this.source);
                    }
                    
                    this.kontroler.LimpiarSobrante(info, this.source);
                    this.kontroler.AgregarListaPromocionesASource(this.source);
                    this.grillaPromo.ResumeLayout();
                }

				
			}
        }


        public void MostrarError( string p )
        {
            //MessageBox.Show( p );
        }


        private void FrmAsistente_Load( object sender, EventArgs e )
        {
            this.zooFormExtender1.FormatearFormularioSegunMemorizado( "Promociones.Asistente" );
        }

        private void FrmAsistente_Deactivate( object sender, EventArgs e )
        {
            this.MemorizarOpciones();
        }

        private void MemorizarOpciones()
        {
            this.zooFormExtender1.Memorizar( "Promociones.Asistente" );
        }

  
        internal void LimpiarGrilla()
        {
            if ( this.grillaPromo.InvokeRequired )
            {
                CallbackEjecutarMetodo c = new CallbackEjecutarMetodo( this.LimpiarGrilla );
                this.Invoke( c );
            }
            else
            {
                if ( this.source.Count > 0 )
                {
                    this.source.Clear();
                }
            }
        }

  
        private void SetearImagenCabecera(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewCardCaptionImageEventArgs e)
        {

            LayoutView view = sender as LayoutView;
            if (((PromocionesEstado)view.GetRow(e.RowHandle)).Destacada)
            {
                e.Image = Properties.Resources.favoritos;
            }

        }
        

        private void layoutView1_CustomCardStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewCardStyleEventArgs e)
        {
            LayoutView view = sender as LayoutView;
            int lnI = e.RowHandle;
            PromocionesEstado ficha = (PromocionesEstado)view.GetRow(lnI);
            
            switch (ficha.Estado)
            {
                case estado.Cumplida:
                    e.Appearance.BackColor = Color.FromArgb(102, 204, 51);
                    e.Appearance.BackColor2 = Color.FromArgb(102, 204, 51);

                    break;
                case estado.Incumplida:
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.BackColor2 = Color.LightGray;
                    break;
                case estado.Parcial:
                    e.Appearance.BackColor = Color.FromArgb(255, 230, 102);
                    e.Appearance.BackColor2 = Color.FromArgb(255, 230, 102);

                    break;
                default:
                    e.Appearance.BackColor = Color.LightGray;
                    e.Appearance.BackColor2 = Color.LightGray;
                    break;
            }
            

        }

        private void toolTipController1_GetActiveObjectInfo( object sender, ToolTipControllerGetActiveObjectInfoEventArgs e )
        {
            LayoutViewHitInfo hitInfo = this.layoutView1.CalcHitInfo( e.ControlMousePosition );
            if ( hitInfo.InFieldValue && hitInfo.Column.Caption == "Faltante" )
            {
                object fila = this.layoutView1.GetRow( hitInfo.RowHandle );
                e.Info = new ToolTipControlInfo( hitInfo.Column, ((PromocionesEstado)fila).FaltanteCompleto );
            }
        }

        public void MostrarErrorEnPromocion( InformacionPromocionIncumplida infoPromo, string error )
        {
			//if ( this.grillaPromo.InvokeRequired )
			//{
			//	CallbackMostrarErrorEnPromocion c = new CallbackMostrarErrorEnPromocion( this.MostrarErrorEnPromocion );
			//	this.BeginInvoke( c, new object[] { infoPromo, error } );
			//}
			//else
			//{
			//	this.kontroler.AgregarPromocionConError( infoPromo, error, this.source );
			//}
        }

		public delegate void EventoPromocionSeleccionadaParaAplicarEventHandler( string id );

		public event EventoPromocionSeleccionadaParaAplicarEventHandler EventoPromocionSeleccionadaParaAplicar;

		private void layoutView1_DoubleClick( object sender, EventArgs e )
		{
            //activar para aplicar con doble click
            LayoutView vista = (LayoutView)sender;
            
            if (vista.RowCount > 0)
            {
                PromocionesEstado infoPromocion = ((PromocionesEstado)vista.GetFocusedRow());

                if (infoPromocion.Estado.Equals(estado.Cumplida))
                {
                    EventoPromocionSeleccionadaParaAplicar(infoPromocion.Id);
                }
            }
        }
    }
}
