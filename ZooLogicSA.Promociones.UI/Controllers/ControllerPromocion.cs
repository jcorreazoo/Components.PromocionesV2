using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.UI.Clases;
using System.Drawing;
using DevExpress.Utils.Drawing;
using System.Collections.Generic;
using DevExpress.XtraScheduler.UI;
using DevExpress.XtraEditors.Filtering;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.UI.Clases.Managers;
using ZooLogicSA.Promociones.UI.Clases.Adaptadores;
using ZooLogicSA.Promociones.UI.Clases.Interpretes;
using DevExpress.Data.Filtering.Helpers;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Linq;

namespace ZooLogicSA.Promociones.UI.Controllers
{
    public class ControllerPromocion : IControllerPromocion
    {
        private Control controlPromocion;
        private ListBox lisTiposPromocion;
        private MaskPromocion mskBeneficio;
        private MaskPromocion mskTopeDescuento;
        private MaskedTextBox mskCondicion;
        private Label lblBeneficio;
        private Label lblCondicion;
        private ZooFilterControl filCondicion;
        private ZooFilterControl filBeneficio;
        private System.Windows.Forms.ComboBox cmbTipoPrecio;
        private ControlBotonMaximizar botonMaximizarFiltroCondicion;
        private ControlBotonMaximizar botonMaximizarFiltroBeneficio;
        private TreeList treeParticipantes;
        private TreeList treeFiltroParticipantes;
        private TimeEdit timDesde;
        private TimeEdit timHasta;
        private WeekDaysCheckEdit weekDias;
        private DateEdit dtDesde;
        private DateEdit dtHasta;
        private ExpressionConditionsEditor expCondicion;
        private ExpressionConditionsEditor expBeneficio;
        private EventosPromocion _eventosPromocion;
        private InterpreteEstructuraPromocion _interprete;
        private ManagerParticipantes _participantes;
        private System.Windows.Forms.ComboBox cmbComportamiento;
        private System.Windows.Forms.ComboBox comboListaDePrecios;
        private Label lblListaDePrecios;
        private MaskPromocion mskCuotasSinRecargo;
        private CheckEdit aplicaAutomaticamente;
        private LabelControl lblAplicaAutomaticamente;

        /// <summary>
        /// Interprete
        /// </summary>
        public InterpreteEstructuraPromocion Interprete
        {
            get { return _interprete; }
        }

        internal ManagerParticipantes Participantes
        {
            get
            {
                return _participantes;
            }
        }

        public EventosPromocion EventosPromocion
        {
            get { return _eventosPromocion; }
            set { _eventosPromocion = value; }
        }
        private List<Control> listaControlesCanFocused;

        public ControllerPromocion()
        {
            BackgroundPaintHelper.DefaultDisabledColor = Color.Empty;
            this.controlPromocion = new Control();
            this.lisTiposPromocion = new ListBox();
            this.mskBeneficio = new MaskPromocion();
            this.mskTopeDescuento = new MaskPromocion();
            this.mskCondicion = new MaskedTextBox();
            this.lblBeneficio = new Label();
            this.lblCondicion = new Label();
            this.filBeneficio = new ZooFilterControl();
            this.filCondicion = new ZooFilterControl();
            this.cmbTipoPrecio = new System.Windows.Forms.ComboBox();
            this.treeFiltroParticipantes = new TreeList();
            this.treeParticipantes = new TreeList();
            this.timDesde = new TimeEdit();
            this.timHasta = new TimeEdit();
            this.weekDias = new WeekDaysCheckEdit();
            this.dtDesde = new DateEdit();
            this.dtHasta = new DateEdit();
            this.expBeneficio = new ExpressionConditionsEditor();
            this.expCondicion = new ExpressionConditionsEditor();
            this._eventosPromocion = new EventosPromocion();
            this.listaControlesCanFocused = new List<Control>();
            this._interprete = new InterpreteEstructuraPromocion( this );
            this.cmbComportamiento = new System.Windows.Forms.ComboBox();
            this.comboListaDePrecios = new System.Windows.Forms.ComboBox();
            this.lblListaDePrecios = new Label();
            this.mskCuotasSinRecargo = new MaskPromocion();
            this.aplicaAutomaticamente = new CheckEdit();
            this.lblAplicaAutomaticamente = new LabelControl();
        }

        public void Inicializar( ListBox listaTipoPromocion, MaskPromocion maskBeneficio, MaskPromocion maskTopeDescuento,
                                MaskedTextBox maskCondicion, Label labelBeneficio, 
                                Label labelCondicion, ZooFilterControl filterCondicion, 
                                ZooFilterControl filterBeneficio, Button btnMaximizarFiltroCondicion,
                                Button btnMaximizarFiltroBeneficio, Control ctlPromocion,
                                TreeList treeListParticipantes, TreeList treeListFiltroParticipantes,
                                TimeEdit timeDesde, TimeEdit timeHasta, List<Control> lisControlesCanFocused,
                                WeekDaysCheckEdit weekDays, DateEdit dtDesdeFecha,
                                DateEdit dtHastaFecha, System.Windows.Forms.ComboBox comboTipoPrecio,                                
                                ExpressionConditionsEditor exprCondicion, ExpressionConditionsEditor exprBeneficio,
                                System.Windows.Forms.ComboBox comboComportamiento, FlowLayoutPanel panelBeneficio,
                                System.Windows.Forms.ComboBox comboListaDePrecios, Label labelListaDePrecios, 
                                MaskPromocion maskCuotasSinRecargo, CheckEdit aplicaAutomaticamente, LabelControl lblAplicaAutomaticamente )
        {
            this.controlPromocion = ctlPromocion;
            this.lisTiposPromocion = listaTipoPromocion;
            this.mskBeneficio = maskBeneficio;
            this.mskTopeDescuento = maskTopeDescuento;
            this.mskCondicion = maskCondicion;
            this.lblBeneficio = labelBeneficio;
            this.lblCondicion = labelCondicion;
            this.filBeneficio = filterBeneficio;
            this.filCondicion = filterCondicion;
            this.cmbTipoPrecio = comboTipoPrecio;
            this.treeParticipantes = treeListParticipantes;
            this.treeFiltroParticipantes = treeListFiltroParticipantes;
            this.timHasta = timeHasta;
            this.timDesde = timeDesde;
            this.weekDias = weekDays;
            this.dtDesde = dtDesdeFecha;
            this.dtHasta = dtHastaFecha;
            this.expCondicion = exprCondicion;
            this.expBeneficio = exprBeneficio;
            this.expCondicion.filtro = this.filCondicion;
            this.expBeneficio.filtro = this.filBeneficio;
            this.listaControlesCanFocused = lisControlesCanFocused;
			this.panelBeneficio = panelBeneficio;
            this.OcultaControlesSegunTipoPromocion();
            this.botonMaximizarFiltroBeneficio = new ControlBotonMaximizar( btnMaximizarFiltroBeneficio, this.filBeneficio, this.controlPromocion );
            this.botonMaximizarFiltroCondicion = new ControlBotonMaximizar( btnMaximizarFiltroCondicion, this.filCondicion, this.controlPromocion );
            this.cmbTipoPrecio.SelectedIndex = 0;
            this.cmbComportamiento = comboComportamiento;
            this.comboListaDePrecios = comboListaDePrecios;
            this.comboListaDePrecios.SelectedIndex = -1;
            this.lblListaDePrecios = labelListaDePrecios;
            this.mskCuotasSinRecargo = maskCuotasSinRecargo;
            this.aplicaAutomaticamente = aplicaAutomaticamente;
            this.lblAplicaAutomaticamente = lblAplicaAutomaticamente;

            this.InicializarListaTipoPromocion( this.lisTiposPromocion );
            this.LlenarComboTipoPrecio( this.cmbTipoPrecio );
            this.LlenarComboComportamientoAsistente( this.cmbComportamiento );            

            this.LimpiarControles();

            this.Interprete.Inicializar( this.lisTiposPromocion, this.mskCondicion, this.mskBeneficio, this.mskTopeDescuento,
                                        this.expCondicion, this.expBeneficio, this.dtDesde,
                                        this.dtHasta, this.timDesde, this.timHasta, this.weekDias, this.cmbTipoPrecio,
                                        this.filCondicion, this.filBeneficio, this.cmbComportamiento, this.comboListaDePrecios, this.mskCuotasSinRecargo, this.aplicaAutomaticamente );
            this.InicializarEventos();
        }

        private void LlenarComboComportamientoAsistente( System.Windows.Forms.ComboBox combo )
        {
            
            AdaptadorEleccionVisualizacionPromocionAsistenteType adaptador = new AdaptadorEleccionVisualizacionPromocionAsistenteType();
            combo.Items.Clear();

            foreach ( string valor in adaptador.listaDescripciones )
            {
                combo.Items.Add( valor );
            }
            combo.SelectedIndex = 0;
            
        }
        
        private void LlenarComboTipoPrecio( System.Windows.Forms.ComboBox combo )
        {
            AdaptadorEleccionParticipanteType adaptadorEleccionParticipante = new AdaptadorEleccionParticipanteType();
            combo.Items.Clear();
            foreach ( string valor in adaptadorEleccionParticipante.listaDescripciones )
            {
                combo.Items.Add( valor );
            }
            if ( combo.Items.Count > 0 )
            {
                combo.SelectedIndex = 0;
            }
        }

        private void LlenarComboListaDePrecios(System.Windows.Forms.ComboBox comboListas)
        {
            DatosParticipanteEventArgs datosParticipante = new DatosParticipanteEventArgs("LISTADEPRECIOS");
            datosParticipante.AgregarAtributo("CODIGO");
            datosParticipante.AgregarAtributo("NOMBRE");
            this.EventosPromocion.MetodoEventoLlenaComboDeListaDePrecios(datosParticipante);

            InterpreteDatosParticipanteXML interpreteDatosParticipante = new InterpreteDatosParticipanteXML(datosParticipante.Datos);
            InterpreteNodoDatosParticipanteXML interpreNodoDatosParticipante = interpreteDatosParticipante.NodoDatosParticipante;
            comboListas.Items.Clear();
            foreach (InterpreteNodoDatosParticipanteRegistroXML nodoRegistro in interpreNodoDatosParticipante.ListaRegistros)
            {
                comboListas.Items.Add(nodoRegistro.ListaAtributos[0].Valor);
            }
            if (comboListas.Items.Count > 0)
            {
                comboListas.SelectedIndex = 0;
            }
            comboListas.Sorted = true;
        }

        public void InicializarManagerParticipantes( string xml )
        {
            this._participantes = new ManagerParticipantes( xml, this.treeParticipantes,
                                                            this.treeFiltroParticipantes,
                                                            this.filCondicion, this.filBeneficio, this.ObtenerTipoPromocionSeleccionada() );
        }

        private void SeleccionaItemParticipante( object sender, EventArgs e )
        {
            TreeList listaArbol = (TreeList)sender;
            string NodoPadre = "";
            string NodoSeleccionado = (string)listaArbol.FocusedNode.GetValue( 1 );
            if (listaArbol.FocusedNode.ParentNode != null)
            {
                NodoPadre = (string)listaArbol.FocusedNode.ParentNode.GetValue( 1 );
            }

            if ( NodoSeleccionado == "CUPON" )
            {
                this.treeFiltroParticipantes.ClearNodes();
                this.treeFiltroParticipantes.Columns.Clear();
            }
            else
            {

                string atributosParticipante = "";
                if ( treeParticipantes.State == TreeListState.NodePressed )
                {
                    DatosParticipanteEventArgs datosParticipante;
                    atributosParticipante = this._participantes.ArmarColumnasAtributosParticipante( ((TreeList)sender).FocusedNode.Id );
                    this.OrdenarColumnaFiltroParticipante( this.treeFiltroParticipantes );
                    datosParticipante = this.CrearDatosParticipante( this.ObtenerNombreParticipanteSeleccionado( (TreeList)sender ), atributosParticipante, NodoPadre);
                    this.EventosPromocion.MetodoEventoSeleccionParticipante( datosParticipante );
                    this.AgregarFilasFiltroParticipante( datosParticipante.Datos );
                }
            }
        }

        private void OrdenarColumnaFiltroParticipante( TreeList filtro )
        {
            if ( filtro.Columns.Count > 0 )
            {
                filtro.Columns[0].SortOrder = SortOrder.Ascending;
            }
        }

        private DatosParticipanteEventArgs CrearDatosParticipante( string nombreParticipante, string listaAtributos, string nodoPadre )
        {
            DatosParticipanteEventArgs datos = new DatosParticipanteEventArgs( nombreParticipante );
            string[] atributos;

            TipoPromocion tp = (TipoPromocion)this.lisTiposPromocion.SelectedItem;
            atributos = listaAtributos.Split( InterpreteParticipanteFiltroXML.cDelimitadorColumnas.ToCharArray() );
            foreach ( string atributo in atributos )
            {
                if ( atributo != string.Empty )
                {
                    string[] valores;
                    valores = atributo.Split( InterpreteParticipanteFiltroXML.cDelimitadorValorColumna.ToCharArray() );
                    datos.AgregarAtributo(valores[0]);                      
                }
            }
            
            datos.WhereAdicional = tp.ObtenerWhereAdicional(nombreParticipante, nodoPadre);                                          
            
            return datos;
        }

        private string ObtenerNombreParticipanteSeleccionado( TreeList listaParticipantes )
        {
            string nombre = "";
            // ESTE ESTA BIEN
            if ( listaParticipantes.FocusedNode.GetValue( 1 ) != null )
                nombre = listaParticipantes.FocusedNode.GetValue( 1 ).ToString();

            return nombre;
        }

        private void MouseEnterFiltroCondicion( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroCondicion.SetearControl( Control.MousePosition );
        }

        private void MouseLeaveFiltroCondicion( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroCondicion.SetearControl( Control.MousePosition );
        }

        private void DragEnterFiltroCondicion( object sender, DragEventArgs e )
        {
            e.Effect = DragDropEffects.Move;
        }

        private void MouseEnterFiltroBeneficio( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroBeneficio.SetearControl( Control.MousePosition );
        }

        private void MouseLeaveFiltroBeneficio( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroBeneficio.SetearControl( Control.MousePosition );
        }

        private void DragEnterFiltroBeneficio( object sender, DragEventArgs e )
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DragDropFiltroBeneficio( object sender, DragEventArgs e )
        {
            DragAndDrop.Autocompletar = false;
            DragAndDrop.DropearControl( this.controlPromocion, e );
            if ( DragAndDrop.DragueadoDesde() == TreeListState.NodePressed )
            {
                DragAndDrop.FilterEditorReemplazarLugarVacio( this.filBeneficio, DragAndDrop.informacionDropeada, DragAndDrop.informacionColumna );
                if ( DragAndDrop.Autocompletar )
                {
                    this._participantes.FiltrarParticipantesSegunGrupo(DragAndDrop.informacionParticipante);
                    this._participantes.FiltrarAtributoFiltroBeneficio("MONTO");
                }
            }
            else
            {
                DragAndDrop.FilterEditorReemplazarColumnaVacia( this.filBeneficio, DragAndDrop.informacionDropeada );
                this._participantes.FiltrarParticipantesSegunGrupo( DragAndDrop.informacionParticipante );
                this._participantes.FiltrarAtributoFiltroBeneficio("MONTO");
            }
        }

        private void DragDropFiltroCondicion( object sender, DragEventArgs e )
        {
            DragAndDrop.Autocompletar = false;
            DragAndDrop.DropearControl( this.controlPromocion, e );
            if ( DragAndDrop.DragueadoDesde() == TreeListState.NodePressed )
            {
                DragAndDrop.FilterEditorReemplazarLugarVacio( this.filCondicion, DragAndDrop.informacionDropeada, DragAndDrop.informacionColumna );
                if ( DragAndDrop.Autocompletar )
                    this._participantes.FiltrarParticipantesSegunGrupo( DragAndDrop.informacionParticipante );
            }
            else
            {
                DragAndDrop.FilterEditorReemplazarColumnaVacia( this.filCondicion, DragAndDrop.informacionDropeada );
                this._participantes.FiltrarParticipantesSegunGrupo( DragAndDrop.informacionParticipante );
            }
        }

        private void ClickBotonMaximizarFiltroCondicion( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroCondicion.AccionarBoton();
        }

        private void ClickBotonMaximizarFiltroBeneficio( object sender, EventArgs e )
        {
            this.botonMaximizarFiltroBeneficio.AccionarBoton();
        }

        private void MouseEnterFiltroParticipantes( object sender, EventArgs e )
        {
            DragAndDrop.MostrarPunteroPuedeDraguear( this.controlPromocion );
        }

        private void MouseLeaveFiltroParticipantes( object sender, EventArgs e )
        {
            DragAndDrop.OcultarPunteroPuedeDraguear( this.controlPromocion );
        }

        private void SeleccionaItemOColumnaFiltroParticipantes( object sender, EventArgs e )
        {     
            if ( this.treeFiltroParticipantes.Nodes.Count == 0 )
                return;
            if ( this.treeFiltroParticipantes.State == TreeListState.NodePressed )
            {
                if ( this.treeFiltroParticipantes.FocusedNode != null )
                    //Name o Tag?
                    DragAndDrop.DraguearControl( this.controlPromocion, new DragObject( (string)this.treeFiltroParticipantes.FocusedNode.GetValue( this.treeFiltroParticipantes.FocusedColumn.VisibleIndex ), this.treeFiltroParticipantes.VisibleColumns[this.ObtenerColumna( this.treeFiltroParticipantes )].FieldName, (string)this.treeFiltroParticipantes.VisibleColumns[this.ObtenerColumna( this.treeFiltroParticipantes )].Tag ), TreeListState.NodePressed );
            }
        }

        private void CambiarColorBackground( object sender, EventArgs e )
        {
            Color colorEnabled = Color.White;
            Color colorDisabled = Color.FromArgb( 245, 245, 245 );
            if ( ((Control)sender).Enabled )
            {
                this.lisTiposPromocion.BackColor = colorEnabled;
                this.mskBeneficio.BackColor = colorEnabled;
                this.mskTopeDescuento.BackColor = colorEnabled;
                this.mskCondicion.BackColor = colorEnabled;
                this.filBeneficio.BackColor = colorEnabled;
                this.filCondicion.BackColor = colorEnabled;
                this.cmbTipoPrecio.BackColor = colorEnabled;
                this.timDesde.BackColor = colorEnabled;
                this.timHasta.BackColor = colorEnabled;
                this.treeParticipantes.Appearance.Row.BackColor = colorEnabled;
                this.treeParticipantes.Appearance.FocusedCell.BackColor = colorEnabled;
                this.treeParticipantes.Appearance.FocusedRow.BackColor = colorEnabled;
                this.expBeneficio.BackColor = colorEnabled;
                this.expCondicion.BackColor = colorEnabled;
                this.expBeneficio.FormatItemList.BackColor = colorEnabled;
                this.expCondicion.FormatItemList.BackColor = colorEnabled;
                this.cmbComportamiento.BackColor = colorEnabled;
                this.comboListaDePrecios.BackColor = colorEnabled;
            }
            else
            {
                this.cmbTipoPrecio.BackColor = colorDisabled;
                this.lisTiposPromocion.BackColor = colorDisabled;
                this.mskBeneficio.BackColor = colorDisabled;
                this.mskTopeDescuento.BackColor = colorDisabled;
                this.mskCondicion.BackColor = colorDisabled;
                this.filBeneficio.BackColor = colorDisabled;
                this.filCondicion.BackColor = colorDisabled;
                this.timDesde.BackColor = colorDisabled;
                this.timHasta.BackColor = colorDisabled;
                this.treeParticipantes.Appearance.Row.BackColor = colorDisabled;
                this.treeParticipantes.Appearance.FocusedCell.BackColor = colorDisabled;
                this.treeParticipantes.Appearance.FocusedRow.BackColor = colorDisabled;
                this.expBeneficio.BackColor = colorDisabled;
                this.expCondicion.BackColor = colorDisabled;
                this.expBeneficio.FormatItemList.BackColor = colorDisabled;
                this.expCondicion.FormatItemList.BackColor = colorDisabled;
                this.cmbComportamiento.BackColor = colorDisabled;
                this.comboListaDePrecios.BackColor = colorDisabled;
            }
        }
        private void SeleccionaItemListaPromocionMouse( object sender, MouseEventArgs e )
        {
            this.SeleccionaItemListaPromocion();
        }

        private void SeleccionaItemListaPromocionTeclado( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == 9 || e.KeyChar == 13 )
            {
                if ( Control.ModifierKeys.Equals( Keys.Shift ) )
                    this.PasarControlAnterior( listaControlesCanFocused, (XtraUserControl)this.controlPromocion, "SHIFT+TAB" );
                else
                    this.PasarControlSiguiente( listaControlesCanFocused, (XtraUserControl)this.controlPromocion, "TAB" );
            }
        }

        private void TeclaPresionada( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == 13 )
            {
                e.Handled = true;
                this.PasarControlSiguiente( listaControlesCanFocused, (XtraUserControl)this.controlPromocion, "ENTER" );
            }
            if ( e.KeyChar == 9 )
            {                
                e.Handled = true;
                if ( Control.ModifierKeys.Equals( Keys.Shift ) )
                    this.PasarControlAnterior( listaControlesCanFocused, (XtraUserControl)this.controlPromocion, "SHIFT+TAB" );
                else
                    this.PasarControlSiguiente( listaControlesCanFocused, (XtraUserControl)this.controlPromocion, "TAB" );
            }            
        }

        private void TeclaPresionadaEnWeekDays( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.Left )
            {
                MoverEnWeekDays( false, this.weekDias );
            }
            if ( e.KeyCode == Keys.Right )
            {
                MoverEnWeekDays( true, this.weekDias );
            }
        }

        private void TeclaPresionadaEnExpFiltros( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.F2 )
            {
                ((ExpressionConditionsEditor)((Control)sender).Parent).EditarItem();
            }
            if ( e.KeyCode == Keys.Add )
            {
                ((ExpressionConditionsEditor)((Control)sender).Parent).AgregarItem();
            }
            if ( e.KeyCode == Keys.Subtract )
            {
                ((ExpressionConditionsEditor)((Control)sender).Parent).QuitarItem();
            }
        }

        public void SeleccionaItemListaPromocion()
        {
            object a;
            a = this.lisTiposPromocion.SelectedItem;
            if ( a != null )
                this.ActualizarControlesSegunTipoPromocion( (TipoPromocion)a );
        }

        private void InicializarEventos()
        {
            this.lisTiposPromocion.GotFocus += new EventHandler( lisTiposPromocion_GotFocus );
            this.lisTiposPromocion.MouseDoubleClick += new MouseEventHandler( this.SeleccionaItemListaPromocionMouse );
            this.lisTiposPromocion.KeyPress += new KeyPressEventHandler( this.SeleccionaItemListaPromocionTeclado );
            this.lisTiposPromocion.KeyUp += new KeyEventHandler( this.TeclaPresionadaListaPromocion );
            this.lisTiposPromocion.MouseClick += new MouseEventHandler( this.SeleccionaItemListaPromocionMouse );
            this.timDesde.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.timHasta.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.mskBeneficio.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.mskTopeDescuento.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.mskCondicion.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.filBeneficio.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.filCondicion.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.mskCondicion.Click += new EventHandler( mask_Click );
            this.mskBeneficio.Click += new EventHandler( mask_Click );
            this.mskTopeDescuento.Click += new EventHandler( mask_Click );
            this.expBeneficio.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.expCondicion.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.expBeneficio.FormatItemList.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.expCondicion.FormatItemList.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.expBeneficio.FormatItemList.KeyDown += new KeyEventHandler( this.TeclaPresionadaEnExpFiltros );
            this.expCondicion.FormatItemList.KeyDown += new KeyEventHandler( this.TeclaPresionadaEnExpFiltros );
            this.treeFiltroParticipantes.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.treeParticipantes.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.dtDesde.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.dtHasta.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.cmbTipoPrecio.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.cmbComportamiento.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
            this.comboListaDePrecios.KeyPress += new KeyPressEventHandler(this.TeclaPresionada);
            foreach ( Control control in this.weekDias.Controls )
            {
                control.KeyDown += new KeyEventHandler( this.TeclaPresionadaEnWeekDays );
                control.KeyPress += new KeyPressEventHandler( this.TeclaPresionada );
                control.GotFocus += new EventHandler( control_GotFocus );
            }
            this.controlPromocion.EnabledChanged += new EventHandler( this.CambiarColorBackground );
            this.treeParticipantes.StateChanged += new EventHandler( this.SeleccionaItemParticipante );
            
            // PIJAZO
            this.filCondicion.MouseEnter += new EventHandler( this.MouseEnterFiltroCondicion );
            this.filCondicion.MouseLeave += new EventHandler( this.MouseLeaveFiltroCondicion );
            this.filCondicion.DragDrop += new DragEventHandler( this.DragDropFiltroCondicion );
            this.filCondicion.DragEnter += new DragEventHandler( this.DragEnterFiltroCondicion );
            this.filCondicion.FilterChanged += new FilterChangedEventHandler( FilterChanged );
            this.filCondicion.PopupMenuShowing += new DevExpress.XtraEditors.Filtering.PopupMenuShowingEventHandler( filtroPopupMenuShowing );
            
            this.filBeneficio.MouseEnter += new EventHandler( this.MouseEnterFiltroBeneficio );
            this.filBeneficio.MouseLeave += new EventHandler( this.MouseLeaveFiltroBeneficio );
            this.filBeneficio.DragDrop += new DragEventHandler( this.DragDropFiltroBeneficio );
            this.filBeneficio.DragEnter += new DragEventHandler( this.DragEnterFiltroBeneficio );
            this.filBeneficio.FilterChanged += new FilterChangedEventHandler( FilterChanged );
            this.filBeneficio.PopupMenuShowing += new DevExpress.XtraEditors.Filtering.PopupMenuShowingEventHandler( filtroPopupMenuShowing );
            this.botonMaximizarFiltroCondicion.Boton.MouseEnter += new EventHandler( this.MouseEnterFiltroCondicion );
            this.botonMaximizarFiltroBeneficio.Boton.MouseEnter += new EventHandler( this.MouseEnterFiltroBeneficio );
            this.botonMaximizarFiltroCondicion.Boton.Click += new EventHandler( this.ClickBotonMaximizarFiltroCondicion );
            this.botonMaximizarFiltroBeneficio.Boton.Click += new EventHandler( this.ClickBotonMaximizarFiltroBeneficio );
            this.treeFiltroParticipantes.MouseEnter += new EventHandler( this.MouseEnterFiltroParticipantes );
            this.treeFiltroParticipantes.MouseLeave += new EventHandler( this.MouseLeaveFiltroParticipantes );
            this.treeFiltroParticipantes.StateChanged += new EventHandler( this.SeleccionaItemOColumnaFiltroParticipantes );
            this.treeFiltroParticipantes.MouseMove += new MouseEventHandler( this.treeFiltroParticipantes_MouseMove );
            this.expBeneficio.EventoEditaRegla += new EventHandler<CustomEventArgs>( this.FiltrarListaParticipantes );
            this.expBeneficio.EventoAgregaRegla += new EventHandler<CustomEventArgs>(this.FiltrarListaParticipantesBeneficio);
            this.expBeneficio.EventoAceptoRegla += new EventHandler<CustomEventArgs>( this.ResetearListaParticipantesyBloquearFiltro );
            this.expBeneficio.EventoBloqueaFiltro += new EventHandler<CustomEventArgs>( BloqueaFiltro );
            this.expCondicion.EventoEditaRegla += new EventHandler<CustomEventArgs>( this.FiltrarListaParticipantes );
            this.expCondicion.EventoAceptoRegla += new EventHandler<CustomEventArgs>( this.ResetearListaParticipantesyBloquearFiltro );
            this.expCondicion.EventoBloqueaFiltro += new EventHandler<CustomEventArgs>( BloqueaFiltro );
            this.expCondicion.EventoHabilitarListaTipoPromocion += new EventHandler<CustomEventArgs>( HabilitarListaTipoPromocion );
            this.expBeneficio.EventoHabilitarListaTipoPromocion += new EventHandler<CustomEventArgs>( HabilitarListaTipoPromocion );
            foreach ( Control control in this.listaControlesCanFocused )
            {
                control.GotFocus += new EventHandler( control_GotFocus );
                control.KeyDown += new KeyEventHandler( control_KeyDown );
            }
        }

        void treeFiltroParticipantes_MouseMove( object sender, MouseEventArgs e )
        {
            if ( this.treeFiltroParticipantes.State == TreeListState.ColumnPressed )
            {
                DragAndDrop.DraguearControl( this.controlPromocion, new DragObject( this.treeFiltroParticipantes.VisibleColumns[this.ObtenerColumna( this.treeFiltroParticipantes )].FieldName, "", (string)this.treeFiltroParticipantes.VisibleColumns[this.ObtenerColumna( this.treeFiltroParticipantes )].Tag ), TreeListState.ColumnPressed );
            }
        }

        void filtroPopupMenuShowing( object sender, DevExpress.XtraEditors.Filtering.PopupMenuShowingEventArgs e )
        {
            DevExpress.Utils.Menu.DXMenuItem item;
            if ( e.MenuType == FilterControlMenuType.Clause )
            {
                for (int i = e.Menu.Items.Count - 1; i >= 0; i--) 
                {
                    item = e.Menu.Items[i];
                    string condicion = e.CurrentNode.Text;
                    if (!ManagerFiltros.EsUnOperadorValido( (ClauseType)item.Tag, condicion ) )
                        e.Menu.Items.Remove( item );
                }
            }
        }

        void TeclaPresionadaListaPromocion( object sender, KeyEventArgs e )
        { 
            if ( e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right )
                this.SeleccionaItemListaPromocion();
        }

        void HabilitarListaTipoPromocion( object sender, CustomEventArgs e )
        {
            if ( this.expBeneficio.FormatItemList.ItemCount > 0 ||
                    this.expCondicion.FormatItemList.ItemCount > 0 )
                SetEnabledListaTipoPromocion( false );
            else
                SetEnabledListaTipoPromocion( true );
        }

        void BloqueaFiltro( object sender, CustomEventArgs e )
        {
            if ( ((Control)sender).Name.ToUpper() == this.expBeneficio.Name.ToUpper() )
                this.expCondicion.SetearEstadoBloqueo( true );
            else
                this.expBeneficio.SetearEstadoBloqueo( true );
        }

        void FiltrarListaParticipantes( object sender, CustomEventArgs e )
        {
            string lcNombre = this._participantes.Interprete.ConvertirFullNombreEnDescripcion( e.Message );
            if ( ManagerFiltros.EsUnAtributoBasico( lcNombre) )
            {
                this._participantes.FiltrarParticipantesSegunAtributoBasico();
            }
            else if ( ManagerFiltros.EsUnAtributoGeneral( lcNombre ) )
            {
                this._participantes.FiltrarParticipantesSegunAtributoGeneral();
            }
            else
            {
                this._participantes.FiltrarParticipantesSegunGrupo( lcNombre );
            }
            this._participantes.FiltrarAtributoFiltroBeneficio( "MONTO" );
            this._participantes.LimpiarListaAtributos();
        }

        void FiltrarListaParticipantesBeneficio(object sender, CustomEventArgs e)
        {
            this._participantes.FiltrarParticipantes( "FACTURADETALLE" );
            this._participantes.FiltrarAtributoFiltroBeneficio( "MONTO" );
        }

        void ResetearListaParticipantesyBloquearFiltro( object sender, CustomEventArgs e )
        {
            this.SetEnabledListaTipoPromocion( false );
            this.ResetearListaParticipantes();
            if (((Control)sender).Name.ToUpper() == this.expBeneficio.Name.ToUpper() )
                this.expCondicion.SetearEstadoBloqueo( false );
            else
                this.expBeneficio.SetearEstadoBloqueo( false );
        }

        void SetEnabledListaTipoPromocion( bool bloqueo )
        {
            this.lisTiposPromocion.Enabled = bloqueo;
        }

        private void ResetearListaParticipantes()
        {
            if ( this._participantes != null )
                this._participantes.InicializarControlParticipantes( null );
        }

        void FilterChanged( object sender, FilterChangedEventArgs e )
        {
            if ( e.Action == FilterChangedAction.FieldNameChange )
            {
                string elemento = e.CurrentNode.Elements[0].ToString();
                string nombre = elemento.Substring(elemento.LastIndexOf('[') + 1, elemento.LastIndexOf(']') - 1).ToUpper();

                if ( nombre.Trim() != "" )
                {
                    if ( ManagerFiltros.EsUnAtributoBasico(  nombre ) )
                    {
                        this._participantes.FiltrarParticipantesSegunAtributoBasico();
                    }
                    else
                    {
                        this._participantes.FiltrarParticipantesSegunGrupo( nombre );
                    }

                    this._participantes.FiltrarAtributoFiltroBeneficio( "MONTO" );

                    this.treeFiltroParticipantes.ClearNodes();
                    this.treeFiltroParticipantes.Columns.Clear();
                }

                string propiedad = e.CurrentNode.Elements.Find( x=>x.ElementType == ElementType.Property ).Text;
                string operador = e.CurrentNode.Elements.Find( x => x.ElementType == ElementType.Operation ).Text;
                if ( !this._participantes.EsOperadorAdmitidoParaLaPropiedad( propiedad, operador ) )
                {
                    e.CurrentNode.ChangeElement( ElementType.Operation, this._participantes.ObtenerOperadorPermitidoDefaultParaPropiedad( propiedad ) );
                }
            }
            if (e.Action == FilterChangedAction.AddNode)
            {
                int cuantasReglas = 0;
                var sarasa = this.ObtenerListaReglaParticipantesCondicionConDetalleItem();
                foreach (var item in sarasa)
                {
                    cuantasReglas += item.Split('[').Length - 1;
                }
                try
                {
                    cuantasReglas += ((DevExpress.Data.Filtering.GroupOperator)((DevExpress.XtraEditors.FilterControl)sender).FilterCriteria).Operands.Count;
                }
                catch (Exception)
                {
                    //glup
                }
                
                if ( ( ( cuantasReglas % 100 ) == 0 ) && cuantasReglas > 0 )
                {
                    string mensaje = "Recuerde el limite de 500 reglas por promocion (cantidad actual: " + cuantasReglas.ToString() + ")";
                    MessageBox.Show(mensaje,"Zoo Logic Dragonfish Color y Talle",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }

        }

        void mask_Click( object sender, EventArgs e )
        {
            ((XtraUserControl)this.controlPromocion).ActiveControl = (Control)sender;
            ((Control)sender).Focus();
        }

        void control_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.KeyCode == Keys.End && e.Control )
            {
                e.Handled = true;
                this.EventosPromocion.MetodoEventoControlFin( "" );
            }
        }

        void control_GotFocus( object sender, EventArgs e )
        {
            if ( ((Control)sender).Tag != null )
                this.EventosPromocion.MetodoEventoMostrarAyuda( ((Control)sender).Tag.ToString() );
        }

        void lisTiposPromocion_GotFocus( object sender, EventArgs e )
        {
            //this.lisTiposPromocion.SelectedIndex = 0;
        }

        private void InicializarListaTipoPromocion( ListBox listbox )
        {
            listbox.Items.Clear();
            foreach ( TipoPromocion tp in FactoryPromociones.LlenarListaTipoPromociones() )
            {
                listbox.Items.Add( tp );
            }
            this.lisTiposPromocion.SelectedIndex = 0;
            this.SeleccionaItemListaPromocion();
            listbox.SetSelected( 0, true );
            listbox.Focus();
        }

        private int ObtenerColumna( TreeList lista )
        {
            int retorno = 0;
            int contadorX = 0;
            int contador = 0;

            foreach ( TreeListColumn col in lista.Columns )
            {
                contadorX = contadorX + col.VisibleWidth;
                if ( Control.MousePosition.X <= contadorX + lista.PointToScreen( new Point( 0, 0 ) ).X )
                {
                    retorno = contador;
                    break;
                }
                else
                    contador = contador + 1;
            }
            return retorno;
        }

        private void ActualizarControlesSegunTipoPromocion( TipoPromocion tp )
        {
            this.OcultaControlesSegunTipoPromocion();
			
			if ( tp.tieneTopeDeDescuento() && !this.mskTopeDescuento.Visible)
            {
                this.mskTopeDescuento.Visible = true;
            }
            
			if ( tp.TieneBeneficioLabel() )
            {
                this.lblBeneficio.Text = tp.ObtenerLabelBeneficio();
                //this.lblBeneficio.Visible = true;
            }

            if ( tp.TieneBeneficioFiltro() )
            {
                this.expBeneficio.Height = 6;
                this.expBeneficio.Visible = true;
            }
            if ( tp.TieneBeneficioMask() )
            {
                this.mskBeneficio.Mascara = tp.ObtenerMascaraBeneficio();
                this.mskBeneficio.SetearValor( "" );
                this.mskBeneficio.Visible = true;
                this.mskBeneficio.Tag = tp.ObtenerMensajeDeAyudaMascaraBeneficio();
			}

            if ( tp.TieneCondicionLabel() )
            {
                this.lblCondicion.Text = tp.ObtenerLabelCondicion();
                this.lblCondicion.Visible = true;
            }

            if ( tp.TieneCondicionFiltro() )
            {
                this.expCondicion.Height = 6;
                this.expCondicion.Visible = true;
            }

            if ( tp.TieneCondicionMask() )
            {
                this.mskCondicion.ResetText();
                this.mskCondicion.Mask = tp.ObtenerMascaraCondicion();
                this.mskCondicion.Visible = true;
                this.mskCondicion.SelectionStart = this.ObtenerSelectionStart( this.mskCondicion );
            }

            if ( tp.TieneCondicionFiltro() )
            {
                this.expCondicion.Height = this.expCondicion.Parent.Height - this.expCondicion.Top - 3;
            }

            if ( tp.TieneBeneficioFiltro() )
            {
                this.expBeneficio.Height = this.expBeneficio.Parent.Height - this.expBeneficio.Top - 3;
            }

			if ( this._participantes != null )
			{
				this._participantes.InicializarControlParticipantes( tp );
			}

			if ( tp.TieneBeneficioMask() && tp.tieneTopeDeDescuento() )
			{
				this.mskTopeDescuento.TabIndex = 5;
				this.mskBeneficio.TabIndex = 4;
				this.mskTopeDescuento.Visible = true;
				this.mskBeneficio.Visible = true;

				this.expBeneficio.Height = this.expBeneficio.Parent.Height - this.expBeneficio.Top - 8;
			}

			if ( tp.TieneBeneficioMask() )
			{
				if ( !tp.tieneTopeDeDescuento() )
				{
					this.mskBeneficio.Width = 330;
				}
				else
				{
					this.mskBeneficio.Width = 165;
				}
			}

			if ( tp.TieneTipoPrecio() )
			{
                this.cmbTipoPrecio.TabIndex = 6;
				this.cmbTipoPrecio.Visible = true;
				this.cmbTipoPrecio.TabIndex = 6;

				this.panelBeneficio.Controls.SetChildIndex( this.cmbTipoPrecio, 3 );

			}

            this.comboListaDePrecios.SelectedIndex = -1;
            if (tp.TieneListaDePrecios())
            {
                if (this.comboListaDePrecios.Items.Count == 0 || ( this.comboListaDePrecios.Items.Count == 1 && this.comboListaDePrecios.SelectedText == "" ) )
                {
                    this.LlenarComboListaDePrecios(this.comboListaDePrecios);
                }
                this.lblListaDePrecios.Text = tp.ObtenerLabelListaDePrecios();
                this.lblListaDePrecios.Visible = true;
                this.comboListaDePrecios.Visible = true;
                //this.comboListaDePrecios.SelectedIndex = -1;
            }

            if ( tp.TieneCuotas() )
            {
                this.mskCuotasSinRecargo.Visible = true;
            } else {
                this.mskCuotasSinRecargo.Visible = false;
            }

            if ( tp.TieneCuotas() )
            {
                this.aplicaAutomaticamente.Enabled = false;
                this.aplicaAutomaticamente.Checked = false;                
            } else
            {                               
                this.aplicaAutomaticamente.Enabled = true;
            }
            
        }

        public List<Control> ObtenerControlesVisiblesMask()
        {
            List<Control> controles = new List<Control>();

            if ( this.mskBeneficio.Visible )
                controles.Add( this.mskBeneficio );
            if ( this.mskCondicion.Visible )
                controles.Add( this.mskCondicion );
            if ( this.mskTopeDescuento.Visible )
                controles.Add( this.mskTopeDescuento );

            return controles;
        }

        private void OcultaControlesSegunTipoPromocion()
        {
            this.mskBeneficio.Visible = false;
            this.mskTopeDescuento.Visible = false;
            this.mskCondicion.Visible = false;
            this.filBeneficio.Visible = false;
            this.filCondicion.Visible = false;
            this.lblBeneficio.Visible = false;
            this.lblCondicion.Visible = false;
            this.cmbTipoPrecio.Visible = false;
            this.expBeneficio.Visible = false;
            this.expCondicion.Visible = false;
            this.comboListaDePrecios.Visible = false;
            this.lblListaDePrecios.Visible = false;
            this.mskCuotasSinRecargo.Visible = false;
        }

        private int ObtenerSelectionStart( MaskedTextBox mask )
        {
            char caracter = mask.PromptChar;
            int posicion = 0;
            if ( mask.Text.Contains( caracter.ToString() ) )
                posicion = mask.Text.IndexOf( caracter );
            return posicion;
        }

        private void AgregarFilasFiltroParticipante( string xmlDatosParticipante )
        {
            this.treeFiltroParticipantes.ClearSorting();

            InterpreteDatosParticipanteXML interpreteDatosParticipante = new InterpreteDatosParticipanteXML( xmlDatosParticipante );
            InterpreteNodoDatosParticipanteXML interpreNodoDatosParticipante = interpreteDatosParticipante.NodoDatosParticipante;

            this.treeFiltroParticipantes.BeginUnboundLoad();
            foreach ( InterpreteNodoDatosParticipanteRegistroXML nodoRegistro in interpreNodoDatosParticipante.ListaRegistros )
            {
                string[] datos = new string[nodoRegistro.ListaAtributos.Count];
                int contador = 0;
                foreach ( InterpreteNodoDatosParticipanteAtributoXML nodoAtributo in nodoRegistro.ListaAtributos )
                {
                    datos[contador] = nodoAtributo.Valor;
                    contador = contador + 1;
                }
                this.AgregarFila( this.treeFiltroParticipantes, datos );
            }
            this.treeFiltroParticipantes.EndUnboundLoad();
        }

        private void AgregarFila( TreeList lista, object[] datos )
        {
            lista.AppendNode( datos, null );
        }

        private void PasarControlSiguiente( List<Control> listaControles, XtraUserControl userControl, String tcTecla )
        {
            Control controlActual;
            Control controlProximo = null;
            int tabIndex;
            int tabIndexProximo = 9999;

            controlActual = this.ObtenerControlConFoco( listaControles );
            if ( controlActual == null )
                return;
            tabIndex = controlActual.TabIndex;
            foreach ( Control control in listaControles )
            {
                if ( control.TabIndex < tabIndex || control.Equals( controlActual ) || !control.Visible || !control.Enabled )
                    continue;
                if ( control.TabIndex < tabIndexProximo )
                {
                    tabIndexProximo = control.TabIndex;
                    controlProximo = control;
                }
            }
            if ( controlProximo != null )
            {
                userControl.ActiveControl = controlProximo;
                controlProximo.Focus();
            }
            else
                this.EventosPromocion.MetodoEventoPerdioFoco( tcTecla );
        }

        private void PasarControlAnterior( List<Control> listaControles, XtraUserControl userControl, String tcTecla )
        {
            Control controlActual;
            Control controlProximo = null;
            int tabIndex;
            int tabIndexProximo = -1;

            controlActual = this.ObtenerControlConFoco( listaControles );
            if ( controlActual == null )
                return;
            tabIndex = controlActual.TabIndex;
            foreach ( Control control in listaControles )
            {
                if ( control.TabIndex > tabIndex || control.Equals( controlActual ) || !control.Visible || !control.Enabled )
                    continue;
                if ( control.TabIndex > tabIndexProximo )
                {
                    tabIndexProximo = control.TabIndex;
                    controlProximo = control;
                }
            }
            if ( controlProximo != null )
            {
                userControl.ActiveControl = controlProximo;
                controlProximo.Focus();
            }
            else
                this.EventosPromocion.MetodoEventoPerdioFoco( tcTecla );
        }

        private Control ObtenerControlConFoco( List<Control> listaControles )
        {
            Control retorno = null;

            foreach ( Control control in listaControles )
            {
                if ( control.Focused || control.ContainsFocus )
                {
                    retorno = control;
                    break;
                }
            }

            return retorno;
        }

        private void MoverEnWeekDays( bool forward, WeekDaysCheckEdit week )
        {
            Control nextControl = null;
            foreach ( Control control in week.Controls )
            {
                if ( control.Focused ) 
                {
                    nextControl = week.GetNextControl( control, forward );
                    if ( nextControl != null )
                        nextControl.Focus();
                    break;
                }
            }
        }

        public void MostrarVistaPredeterminada( bool tlEdicion, string valorAnterior )
        {
            this.botonMaximizarFiltroCondicion.MinimizarControl();
            this.botonMaximizarFiltroBeneficio.MinimizarControl();
            this.botonMaximizarFiltroCondicion.SetearVisibilidad( false );
            this.botonMaximizarFiltroBeneficio.SetearVisibilidad( false );
            this.expBeneficio.HideEditor();
            this.expCondicion.HideEditor();
            this.expBeneficio.LimpiarBarItems();
            this.expCondicion.LimpiarBarItems();
            this.expBeneficio.SetearEstadoBloqueo( false );
            this.expCondicion.SetearEstadoBloqueo( false );
            this.LimpiarControles();
            if ( tlEdicion && valorAnterior != string.Empty )
                this.Interprete.RestaurarInformacion( InterpreteXMLPromocion.ObtenerValorNodoInformacionControl( valorAnterior ) );
            this.cmbComportamiento.Text = "";
        }

        public void LimpiarControles()
        {
            this.OcultaControlesSegunTipoPromocion();
            this.mskBeneficio.SetearValor( "" );
            this.mskTopeDescuento.SetearValor( "" );
            this.filCondicion.ResetText();
            this.filBeneficio.ResetText();
            this.expBeneficio.HideEditor();
            this.expCondicion.HideEditor();
            this.expBeneficio.FormatItemList.Items.Clear();
            this.expCondicion.FormatItemList.Items.Clear();
            this.expBeneficio.VerificarContenidoListaRegla();
            this.expCondicion.VerificarContenidoListaRegla();
            this.expBeneficio.HabilitaBotones();
            this.expCondicion.HabilitaBotones();
            this.treeFiltroParticipantes.ClearNodes();
            this.treeFiltroParticipantes.ClearColumnsFilter();
            this.treeParticipantes.Selection.Clear();
            this.ResetearListaParticipantes();
            this.mskCuotasSinRecargo.SetearValor( "" );                        
            this.aplicaAutomaticamente.Checked = false;
            
            this.dtDesde.DateTime = DateTime.Now;
            this.dtHasta.DateTime = new DateTime( DateTime.Now.Year, 12, 31 );

            this.timDesde.Time = new DateTime( 1, 1, 1, 0, 0, 0 );
            this.timHasta.Time = new DateTime( 1, 1, 1, 23, 59, 59 );
            
            foreach( CheckEdit check in this.weekDias.Controls)
            {
                check.Checked = true;
            }
            this.lisTiposPromocion.SelectedIndex = 0;
            this.SeleccionaItemListaPromocion();
            this.lisTiposPromocion.SetSelected( 0, true );

            AdaptadorEleccionVisualizacionPromocionAsistenteType adaptador = new AdaptadorEleccionVisualizacionPromocionAsistenteType();
            this.cmbComportamiento.Text = "Normal";
        }

        public string ObtenerValorMaskCondicion()
        {
            string valor;
            MaskFormat maskAnterior = this.mskCondicion.TextMaskFormat;
            this.mskCondicion.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            valor = this.mskCondicion.Text;
            this.mskCondicion.TextMaskFormat = maskAnterior;
            
            return valor;
        }

        public string ObtenerValorMaskBeneficio()
        {
            return this.mskBeneficio.ObtenerIngreso();
        }

        public string ObtenerValorMaskTopeDescuento()
        {
            return this.mskTopeDescuento.ObtenerIngreso();
        }

        public List<string> ObtenerListaReglaParticipantesCondicion()
        {         
            List<string> listaParticipantes = new List<string>();
            for ( int i = 0; i < this.expCondicion.FormatItemList.ItemCount; i++ )
            {
                string expresion = (string)(this.expCondicion.FormatItemList.Items[i]);
                expresion = this.ReformatearExpresion( expresion );

                if ( !String.IsNullOrEmpty( expresion) )
                {
                    listaParticipantes.Add( expresion );
                }
            }

            return listaParticipantes;
        }

        private string ReformatearExpresion( string expresion )
        {
            string retorno = expresion;

            if ( expresion.Substring( 0, 4 ).Equals( "Not " ) )
            {
                if ( expresion.ToUpper().IndexOf( " OR " ) >= 0 || expresion.ToUpper().IndexOf( " AND " ) >= 0 )
                {
                    retorno = expresion.Replace( "Not ", "not" );
                }
                else
                {
                    retorno = expresion.Replace( "Not ", "not(" );
                    retorno = retorno + ")";
                }
            }

            return retorno;
        }

        public List<string> ObtenerListaReglaParticipantesBeneficio()
        {
            List<string> listaParticipantes = new List<string>();

            for ( int i = 0; i < this.expBeneficio.FormatItemList.ItemCount; i++ )
            {
                string expresion = (string)(this.expBeneficio.FormatItemList.Items[i]);
                expresion = this.ReformatearExpresion( expresion );

                if ( !String.IsNullOrEmpty( expresion) )
                {
                    listaParticipantes.Add( expresion );
                }
            }
            return listaParticipantes;
        }

        public TipoPromocion ObtenerTipoPromocionSeleccionada()
        {
            TipoPromocion tipoPromo = null;
            object a;
            a = this.lisTiposPromocion.SelectedItem;
            if ( a != null )
                tipoPromo = (TipoPromocion)a;

            return tipoPromo;
        }

        internal List<string> ValidarPromocionUI( string idRedondeo )        
        {
            List<string> listaErrores = new List<string>();
            ValidarPromocion validador = new ValidarPromocion();

            if ( !validador.ValidarFechas( this.dtDesde.DateTime, this.dtHasta.DateTime ) )
                listaErrores.Add("Rango de fecha de vigencia incorrecto..");

            if (!validador.ValidarHorario(this.timDesde.Time, this.timHasta.Time))
                listaErrores.Add("Rango de horario de vigencia incorrecto.");

            if (!validador.ValidarDiasDeLaSemana( this.ObtenerVigenciaDiasSemana()))
                listaErrores.Add("Debe seleccionar al menos un día de la semana.");

            TipoPromocion tipoPromo = this.ObtenerTipoPromocionSeleccionada();

            List<string> reglas = this.ObtenerListaReglaParticipantesCondicionConDetalleItem(); ;
            if (!validador.ValidarCantidadDeReglasTotales( reglas ))
                listaErrores.Add("Ha superado la cantidad maxima de condiciones por promoción ( 500 ), debe reducirlas o reorganizarlas para poder guardar la promoción.");

            if ( tipoPromo != null )
            {
                ManagerValidaciones validadorControles = new ManagerValidaciones();
                if ( tipoPromo.TieneCondicionMask() )
                {
                    if ( !validador.ValidarDatoObligatorio( this.mskCondicion.ObtenerValor() ) )
                        listaErrores.Add( tipoPromo.ObtenerMensajeErrorCondicionMask() );
                }

                if ( tipoPromo.TieneBeneficioMask() )
                {
                    if ( !validador.ValidarDatoObligatorio( this.mskBeneficio.ObtenerIngreso() ) )
                        listaErrores.Add( tipoPromo.ObtenerMensajeErrorBeneficioMask() );
                }

                if ( tipoPromo.TieneCondicionFiltro() )
                {
                    if ( this.expCondicion.FiltroVisible() )
                    {
                        string mensError = "";
                        if ( !this.expCondicion.Validar( ref mensError ) )
                            listaErrores.Add( mensError );
                        else
                        {
                            this.expCondicion.ForzarAceptar();
                        }
                    }
                    if ( this.expCondicion.FormatItemList.ItemCount == 0 )
                        listaErrores.Add( "Debe cargar al menos una regla de condición." );
                }

                if ( tipoPromo.TieneBeneficioFiltro() )
                {
                    if ( this.expBeneficio.FiltroVisible() )
                    {
                        string mensError = "";
						if ( !this.expBeneficio.Validar( ref mensError ) )
						{
							listaErrores.Add( mensError );
						}
						else
						{
							this.expBeneficio.ForzarAceptar();
							string idPromo = ((ControlPromociones)this.controlPromocion).IDPromocion;
							InterpreteXMLPromocion interpreteXML = new InterpreteXMLPromocion( this, idPromo );
							((ControlPromociones)this.controlPromocion).Value = interpreteXML.XML;
						}
                    }

					if ( this.expBeneficio.FormatItemList.ItemCount == 0 )
					{
						listaErrores.Add( "Debe cargar al menos una regla de beneficio." );
					}
                    else 
                    {
                        if ( !validadorControles.ValidarCantidadCondicionesBeneficio( this ) )
                            listaErrores.Add( validadorControles.ObtenerMensajeError() );
                    }
                }

                if ( tipoPromo.BeneficioType == BeneficioType.PorcentajeFijoDeDescuentoBancario )
                {
                    if ( ObtenerCantidadCondicionesTipoCupon( tipoPromo ) == 0 )
                        listaErrores.Add( "Debe agregar al menos una condición que esté relacionada a cupón de tarjeta" );
                }
                else
                {
                    if (( tipoPromo.TieneBeneficioMask() && (tipoPromo.TieneBeneficioFiltro() || tipoPromo.TieneCondicionFiltro())) || tipoPromo.TieneListaDePrecios())
                    {
                        if ( !validadorControles.ValidarExistenciaParticipanteTipoDetalle( this ) )
                            listaErrores.Add( "Debe agregar al menos una condición que esté relacionada a los artículos" );

                        if ( !validadorControles.ValidarCantidadCondicionesDeValores( this ) )
                            listaErrores.Add("Solo puede haber una sola condición relacionada a los valores");

                        if (!validadorControles.ValidarMediosDePagoYPromoAutomatica(this))
                            listaErrores.Add("No se puede aplicar automaticamente una promoción con una condición relacionada a los valores");
                    }
                }


                if (!tipoPromo.tieneRedondeo() && idRedondeo.Trim() != "")
                {
                    listaErrores.Add("Este tipo de promoción no permite aplicar redondeos");
                }

                if ( !validadorControles.Validar( this, tipoPromo.TypeValidacion() ) )
                    listaErrores.Add( validadorControles.ObtenerMensajeError() );

                if (tipoPromo.TieneListaDePrecios() && this.comboListaDePrecios.SelectedIndex == -1)
                {
                    listaErrores.Add("Debe seleccionar una lista de precios");
                }

                if ( tipoPromo.BeneficioType == BeneficioType.LLevaXPagaY && this.expCondicion.FormatItemList.ItemCount > 0 )
                {
                    if (!validadorControles.RecorrerParticipantesReglaYValidarQueNoExistanCantidadesConDecimales( this ))
                    {
                        listaErrores.Add("No se pueden cargar condiciones que contengan cantidades con decimales.");
                    }
                }

            }

            return listaErrores;
        }

        private int ObtenerCantidadCondicionesTipoCupon( TipoPromocion tipoPromo )
        {
            int cantidadCondicionesTipoCupon = 0;
            List<string> listaCondiciones = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion( tipoPromo, this );
            foreach ( string condicion in listaCondiciones )
            {
                string condicionFiltrada = ManagerReglas.ObtenerFullDescripcionEnRegla( condicion );
                condicionFiltrada = condicionFiltrada.Replace( "[", "" );
                condicionFiltrada = condicionFiltrada.Remove( Math.Min( 5, condicionFiltrada.Length - 1 ) );
                if ( condicionFiltrada == "CUPON" )
                    cantidadCondicionesTipoCupon++;
            }
            return cantidadCondicionesTipoCupon;
        }

        public string ObtenerDescripcionComboTipoPrecio()
        {
            string descripcion = "";
            if (  this.cmbTipoPrecio.SelectedIndex != -1 && this.cmbTipoPrecio.Visible )
                descripcion = (string)this.cmbTipoPrecio.SelectedItem.ToString();

            return descripcion;
        }

        public void ActualizarInformacionInterprete()
        {
            this._interprete.ActualizarInformacion();
        }

        public string ObtenerValorEstructuraInterprete()
        {
            return this._interprete.ValorEstructura;
        }

        public List<string> ObtenerListaCondicionesSegunTipoPromocion( TipoPromocion tipoPromo )
        {
            List<string> listaCondiciones = new List<string>();

            if ( tipoPromo.TieneCondicionFiltro() )
            {
                foreach ( string regla in this.ObtenerListaReglaParticipantesCondicion() )
                {
                    listaCondiciones.Add( regla );
                }
            }
            if ( tipoPromo.TieneCondicionMask() )
            {
                string valorMask = this.ObtenerValorMaskCondicion();
                string condicionMask = tipoPromo.ObtenerReglaCondicionSegunMask( valorMask );
                if ( tipoPromo.DebeAgregarReglaCondicionMask( listaCondiciones ) )
                {
                    string searchedCondicion = "";
                    if ( this.DebeInsertarReglaCondicion( listaCondiciones, tipoPromo, ref searchedCondicion ) )
                        InsertarCondicionEnRegla( listaCondiciones, searchedCondicion, condicionMask );
                    else
                        listaCondiciones.Add( condicionMask );
                }
            }

            return listaCondiciones;
        }

        private bool DebeInsertarReglaCondicion( List<string> listaCondiciones, TipoPromocion tipoPromocion, ref string searchedCondicion )
        {
            bool debe = false;

            foreach ( string condicion in listaCondiciones )
            {
                string fullDescripcion = ManagerReglas.ObtenerFullDescripcionEnRegla( condicion );
                string nombreDetalle = this._participantes.Interprete.ObtenerTipoDetalle( fullDescripcion );
                string codigoDetalle = ManagerReglas.ObtenerCodigoDetalleSegunNombre( nombreDetalle );

                if ( codigoDetalle.Trim().ToUpper()
                    == ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType ).Trim().ToUpper() )
                {
                    searchedCondicion = condicion;
                    debe = true;
                    break;
                }
            }

            return debe;
        }

        private void InsertarCondicionEnRegla( List<string> listaCondiciones, string searchedCondicion, string insertCondicion )
        {
            if ( listaCondiciones.Exists( x => x == searchedCondicion ) )
            {
                int indice = listaCondiciones.IndexOf( searchedCondicion );
                if ( indice >= 0 )
                {
                    string oldCondicion = listaCondiciones[ indice ];
                    listaCondiciones[indice] = "( " + oldCondicion + " ) And ( " + insertCondicion + " )";
                }
            }
        }

        public string ObtenerTipoDetalleDelParticipante( string fullDescripcion )
        {
            string tipoDetalle = "";
            if ( this.Participantes != null && this.Participantes.Interprete != null )
                tipoDetalle = this.Participantes.Interprete.ObtenerTipoDetalle( fullDescripcion );

            return tipoDetalle;
        }

        public DateTime ObtenerVigenciaFechaDesde()
        {
            return this.dtDesde.DateTime;
        }

        public DateTime ObtenerVigenciaFechaHasta()
        {
            return this.dtHasta.DateTime;
        }

        public DateTime ObtenerVigenciaHoraDesde()
        {
            return this.timDesde.Time;
        }

        public DateTime ObtenerVigenciaHoraHasta()
        {
            return this.timHasta.Time;
        }

        public string[] ObtenerVigenciaDiasSemana()
        {
            string[] dias = new string[7];
            int contador = 0;

            foreach ( CheckEdit check in this.weekDias.Controls )
            {
                dias[contador] = check.Checked.ToString();
                contador++;
            }

            return dias;
        }

        internal List<string> ObtenerListaReglaParticipantesBeneficioConDetalleItem()
        {
            List<string> listaParticipantes = new List<string>();
            for ( int i = 0; i < this.expBeneficio.FormatItemList.ItemCount; i++ )
            {
                if ( (string)this.expBeneficio.FormatItemList.Items[i].ToString() != string.Empty )
                {
                    String lcParticipante = (string)this.expBeneficio.FormatItemList.Items[i].ToString();
                    //String lcParticipante = this.expBeneficio.ObtenerValorEnDictionary( (string)this.expBeneficio.FormatItemList.Items[i].ToString() );
                    String lcDetalle = ManagerReglas.ObtenerNombreDetalleParticipante( lcParticipante, this );
                    if ( ManagerReglas.EsDetalleItem( lcDetalle ) )
                        listaParticipantes.Add( lcParticipante );
                }
            }

            return listaParticipantes;
        }

        internal List<string> ObtenerListaReglaParticipantesCondicionConDetalleItem()
        {
            List<string> listaParticipantes = new List<string>();
            for ( int i = 0; i < this.expCondicion.FormatItemList.ItemCount; i++ )
            {
                if ( (string)this.expCondicion.FormatItemList.Items[i].ToString() != string.Empty )
                {
                    String lcParticipante = (string)this.expCondicion.FormatItemList.Items[i].ToString();
                    //String lcParticipante = this.expCondicion.ObtenerValorEnDictionary( (string)this.expCondicion.FormatItemList.Items[i].ToString() );
                    String lcDetalle = ManagerReglas.ObtenerNombreDetalleParticipante( lcParticipante, this );
                    if ( ManagerReglas.EsDetalleItem( lcDetalle ) )
                        listaParticipantes.Add( lcParticipante );
                }
            }

            return listaParticipantes;
        }
     
        public void QuitarFocoControlesReglas()
        {
            if ( this.expCondicion.Controls.Count > 0 )
            {
                ((Control)(this.expCondicion.Controls[0])).Focus();
            }
        }

        internal void ActualizarParticipantes()
        {
            if ( this.filCondicion.Visible )
            {
                string fullDescripcion = ManagerReglas.ObtenerFullDescripcionEnRegla( this.filCondicion.FilterString );
                string fullNombre = this.Participantes.Interprete.ConvertirFullDescripcionEnNombre( fullDescripcion );
                this._participantes.FiltrarParticipantesSegunGrupo( fullNombre ); 
            }
        }

        public string ObtenerTopeCargado()
        {
            return this.mskTopeDescuento.ObtenerIngreso();
        }

        public string ObtenerListaDePreciosCargada()
        {
            return this.comboListaDePrecios.Text;
        }

        public VisualizacionPromocionAsistenteType ObtenerVisualizacion()
        {
            AdaptadorEleccionVisualizacionPromocionAsistenteType adpatador = new AdaptadorEleccionVisualizacionPromocionAsistenteType();
            return adpatador.ObtenerIdVisualizacionPromocionTypeSegunValorCombo( this.cmbComportamiento.Text );
        }

        public void ExcluirPromocionesPorTipo( BeneficioType tipo )
        {
            foreach ( TipoPromocion item in this.lisTiposPromocion.Items )
            {
                if ( item.BeneficioType == tipo )
                {
                    this.lisTiposPromocion.Items.Remove( item );
                }
            }
        }

		public FlowLayoutPanel panelBeneficio { get; set; }

        public string ObtenerCuotasSinRecargo()
        {
            return this.mskCuotasSinRecargo.ObtenerIngreso();
        }

        public bool ObtenerAplicaAutomaticamente()
        {
            return this.aplicaAutomaticamente.Checked;
        }

        public bool ObtenerAplicaConMedioDePago()
        {
            TipoPromocion tipoPromocion = this.ObtenerTipoPromocionSeleccionada();
            //List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesSegunTipoPromocion(tipoPromocion, this);
            List<string> listaParticipantes = ManagerBeneficios.ObtenerListaParticipantesCondicion(tipoPromocion, this);
            string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType(CodigoDetalleType.DetalleValor);
            int cantidad = ManagerReglas.ObtenerCantidadParticipantesSegunDetalle(listaParticipantes, detalle, this);

            return cantidad > 0;
        }
    }
}
