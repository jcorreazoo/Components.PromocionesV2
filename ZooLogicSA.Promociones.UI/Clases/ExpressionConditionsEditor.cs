using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Design;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.DXperience.Demos;
using DevExpress.XtraTreeList.StyleFormatConditions;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using System.Linq;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.UI.Mensajeria;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public partial class ExpressionConditionsEditor : XtraUserControl
	{
        public event EventHandler<CustomEventArgs> EventoAceptoRegla;
        public event EventHandler<CustomEventArgs> EventoEditaRegla;
        public event EventHandler<CustomEventArgs> EventoAgregaRegla;
        public event EventHandler<CustomEventArgs> EventoBloqueaFiltro;
        public event EventHandler<CustomEventArgs> EventoHabilitarListaTipoPromocion;
        public ZooFilterControl filtro = null;
        private bool editando = false;
        private int indice = 0;
        private bool bloqueado;
        //private Dictionary<string,string> nomenclador;
        //private int contadorItems = 0;

        public ExpressionConditionsEditor() {
            InitializeComponent();
            this.bloqueado = false;
            ResetBarItems();
            //this.contadorItems = 0;
            //this.nomenclador = new Dictionary<string,string>();
        }

#region AccionesSobreRegla
        
        private void EditarRegla()
        {
            try
            {
                if (FormatItemList.SelectedIndex > -1)
                {
                    editando = true;
                    indice = FormatItemList.SelectedIndex;
                    this.filtro.FilterString = (string)FormatItemList.SelectedValue;
                    txtRegla.Caption = (string)FormatItemList.SelectedValue;
                    if (this.filtro.FilterString.Trim() != "")
                        this.MetodoEventoEditaRegla(this.ObtenerNombreDetalleEnRegla(this.filtro.FilterString));
                }
            }
            catch ( Exception )
            {
                string condicion = this.ObtenerNombreDetalleEnRegla((string)FormatItemList.SelectedValue);
                this.filtro.FilterString = condicion + " = ''" ;
                txtRegla.Caption = (string)FormatItemList.SelectedValue;
            }
            finally
            {
                ShowEditor();
            }
        }
        
        private void EliminarRegla()
        {
            if ( FormatItemList.SelectedIndex > -1 )
            {
                FormatItemList.Items.RemoveAt( FormatItemList.SelectedIndex );
                FormatItemList.Refresh();
                HabilitaBotones();
                //this.VerificarContenidoListaRegla();
            }
        }
        
        private void AgregarRegla()
        {
            editando = false;
            txtRegla.Caption = "Nuevo";
            this.filtro.FilterString = "";
            if (this.Name == "expBeneficio")
            {
                string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType(CodigoDetalleType.DetalleItem);
                this.MetodoEventoAgregaRegla(detalle);
            }           
            ShowEditor();
        }

        public void MostrarTextoRegla()
        {
            barButtonItem1.Visibility = BarItemVisibility.Never;
            barButtonItem2.Visibility = BarItemVisibility.Never;
            barButtonItem3.Visibility = BarItemVisibility.Never;
            txtRegla.Visibility = BarItemVisibility.Always;
            btnAceptar.Visibility = BarItemVisibility.Always;
            btnCancelar.Visibility = BarItemVisibility.Always;
        }

        private void ActualizarListaReglas( string definicion, bool Editando )
        {
            //this.VerificarContenidoListaRegla();
            if ( Editando )
                //this.nomenclador[FormatItemList.Items[indice].ToString()] = definicion;
                FormatItemList.Items[ indice ] = definicion;
            else
            {
                //this.contadorItems++;
                //string regla = "Regla participante " + this.contadorItems.ToString();
                //this.nomenclador.Add( regla, definicion );
                //FormatItemList.Items.Add( regla );
                FormatItemList.Items.Add( definicion );
            }
        }

        public void VerificarContenidoListaRegla()
        {
            if ( FormatItemList.ItemCount == 0 )
            {
                //this.contadorItems = 0;
                //this.nomenclador.Clear();
            }
        }

        private string ObtenerNombreDetalleEnRegla( string textoFiltro )
        {
            string detalle = "";
            int desde = 0;
            int hasta = 0;

            desde = textoFiltro.IndexOf( "[" );
            hasta = textoFiltro.IndexOf( "]" );
            detalle = textoFiltro.Substring( desde + 1, hasta - desde - 1 );
            return detalle;
        }

#endregion
#region AccionesTipoClick

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            EditarRegla();
        }

        private void FormatItemList_MouseDoubleClick(object sender, MouseEventArgs e) 
        {
            EditarRegla();
        }

        private void barButtonItem2_ItemClick( object sender, ItemClickEventArgs e )
        {
            EliminarRegla();
        }

        private void barButtonItem1_ItemClick( object sender, ItemClickEventArgs e )
        {
            AgregarRegla();
        }

        private void btnCancelar_ItemClick( object sender, ItemClickEventArgs e )
        {
            this.Select( true, true );
            CerrarFiltro();
        }

        private void btnAceptar_ItemClick( object sender, ItemClickEventArgs e )
        {
            this.Select( true , true );
            this.Aceptar();
        }

        private void Aceptar()
        {
            string mensajeError = "";

            if ( this.Validar( ref mensajeError ) )
            {
                this.ActualizarListaReglas( this.filtro.FilterString, editando );
                CerrarFiltro();
            }
            else
            {
                ServicioDeMensajeria mensajeria = new ServicioDeMensajeria();
                mensajeria.Advertir( mensajeError, Parametros.ObtenerNombreProducto() );
            }
        }

        public void ForzarAceptar()
        {
            this.Aceptar();
            this.filtro.FilterString = "";
        }

        public bool Validar( ref string mensajeError )
        {            
            ValidarListaCondicion validador = new ValidarListaCondicion();
            bool validar = validador.Validar( this.filtro.FilterString );
            mensajeError = validador.ObtenerMensajeError();

            return validar;
        }

#endregion        

#region Resto

        private void ResetBarItems()
        {
            LimpiarBarItems();
            HabilitaBotones();
        }

        public void LimpiarBarItems()
        {
            barButtonItem1.Visibility = BarItemVisibility.Always;
            barButtonItem2.Visibility = BarItemVisibility.Always;
            barButtonItem3.Visibility = BarItemVisibility.Always;
            txtRegla.Visibility = BarItemVisibility.Never;
            btnAceptar.Visibility = BarItemVisibility.Never;
            btnCancelar.Visibility = BarItemVisibility.Never;
        }

        private void ExpressionConditionsEditor_VisibleChanged( object sender, EventArgs e )
        {
            //this.FormatItemList.Items.Clear();
            //this.VerificarContenidoListaRegla();
            if (!this.filtro.Visible)
            ResetBarItems();
        }

        public void HabilitaBotones()
        { 
            bool habilita = false;
            barButtonItem1.Enabled = true;
            FormatItemList.Enabled = true;
            if ( FormatItemList.ItemCount > 0 )
                habilita = true;
            else
                MetodoEventoHabilitarListaTipoPromocion( "" );
            barButtonItem2.Enabled = habilita;
            barButtonItem3.Enabled = habilita;            
        }

        private void DeshabilitaBotones()
        {
            barButtonItem1.Enabled = false;
            barButtonItem2.Enabled = false;
            barButtonItem3.Enabled = false;
            FormatItemList.Enabled = false;
        }

        private void CerrarFiltro()
        {
            this.HideEditor();
            ResetBarItems();
            MetodoEventoAceptoRegla( "" );
        }

        public void AgregarItem()
        {
            AgregarRegla();
        }

        public void QuitarItem()
        {
            EliminarRegla();
        }

        public void EditarItem()
        {
            EditarRegla();
        }

        protected void MetodoEventoAceptoRegla( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoAceptoRegla( argumentos );
        }

        protected void OnRaiseEventoAceptoRegla( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoAceptoRegla;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void MetodoEventoAgregaRegla(string texto)
        {
            CustomEventArgs argumentos = new CustomEventArgs(texto);
            OnRaiseEventoAgregaRegla(argumentos);
        }

        protected void OnRaiseEventoAgregaRegla(CustomEventArgs e)
        {
            EventHandler<CustomEventArgs> handler = EventoAgregaRegla;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void MetodoEventoEditaRegla( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoEditaRegla( argumentos );
        }

        protected void OnRaiseEventoEditaRegla( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoEditaRegla;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void MetodoEventoBloqueaFiltro( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoBloqueaFiltro( argumentos );
        }

        protected void OnRaiseEventoBloqueaFiltro( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoBloqueaFiltro;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void MetodoEventoHabilitarListaTipoPromocion( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoHabilitarListaTipoPromocion( argumentos );
        }

        protected void OnRaiseEventoHabilitarListaTipoPromocion( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoHabilitarListaTipoPromocion;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        public void ShowEditor()
        {
            this.MetodoEventoBloqueaFiltro( this.Name );
            MostrarTextoRegla();
            this.filtro.Parent = this;
            this.filtro.Dock = DockStyle.Fill;
            this.filtro.Location = new Point( 0, 0 );
            this.filtro.Size = this.Size;
            this.filtro.Visible = true;
            this.filtro.BringToFront();
        }

        public void HideEditor()
        {
            this.filtro.Visible = false;
        }

        public bool FiltroVisible()
        {
            return this.filtro.Visible;
        }

        public void SetearEstadoBloqueo( bool Bloqueado )
        {
            this.bloqueado = Bloqueado;
            if ( Bloqueado )
                this.DeshabilitaBotones();
            else
                this.HabilitaBotones();
        }

        //public string ObtenerValorEnDictionary( string key )
        //{
        //    return this.nomenclador[key];
        //}

        public void AgregarValorEnDictionary( string valor )
        {
            this.ActualizarListaReglas( valor, false );
        }
#endregion
    }
}