using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Negocio.Clases;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.XtraScheduler.UI;
using ZooLogicSA.Promociones.UI.Controllers;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteEstructuraPromocion
    {
        private EstructuraPromocion estructura;
        private ControllerPromocion control;
        private ListBox lisTipoPromocion;
        private MaskedTextBox mskCondicion;
        private MaskPromocion mskBeneficio;
        private MaskPromocion mskTopeDescuento;
        private ExpressionConditionsEditor expCondicion;
        private ExpressionConditionsEditor expBeneficio;
        private DateEdit datFechaDesde;
        private DateEdit datFechaHasta;
        private TimeEdit timDesde;
        private TimeEdit timHasta;
        private WeekDaysCheckEdit weeDias;
        private System.Windows.Forms.ComboBox cmbTipoPrecio;
        private FilterControl filtroCondicion;
        private FilterControl filtroBeneficio;
        private System.Windows.Forms.ComboBox comboComportamiento;
        private System.Windows.Forms.ComboBox comboListaDePrecios;
        private MaskPromocion mskCuotasSinRecargo;
        private CheckEdit aplicaAutomaticamente;

        public InterpreteEstructuraPromocion( ControllerPromocion controlPromocion )
        { 
            this.estructura = new EstructuraPromocion();
            this.control = controlPromocion;
            this.lisTipoPromocion = new ListBox();
            this.mskCondicion = new MaskedTextBox();
            this.mskBeneficio = new MaskPromocion();
            this.mskTopeDescuento = new MaskPromocion();
            this.expCondicion = new ExpressionConditionsEditor();
            this.expBeneficio = new ExpressionConditionsEditor();
            this.datFechaDesde = new DateEdit();
            this.datFechaHasta = new DateEdit();
            this.timDesde = new TimeEdit();
            this.timHasta = new TimeEdit();
            this.weeDias = new WeekDaysCheckEdit();
            this.cmbTipoPrecio = new System.Windows.Forms.ComboBox();
            this.filtroBeneficio = new FilterControl();
            this.filtroCondicion = new FilterControl();
            this.comboComportamiento = new System.Windows.Forms.ComboBox();
            this.comboListaDePrecios = new System.Windows.Forms.ComboBox();
            this.mskCuotasSinRecargo = new MaskPromocion();
            this.aplicaAutomaticamente = new CheckEdit();

        }

        public void Inicializar( ListBox listaTipoPromocion, MaskedTextBox maskCondicion,
                                MaskPromocion maskBeneficio, MaskPromocion maskTopeDescuento, ExpressionConditionsEditor exprCondicion,
                                ExpressionConditionsEditor exprBeneficio, DateEdit dateFechaDesde,
                                DateEdit dateFechaHasta, TimeEdit timeDesde,
                                TimeEdit timeHasta, WeekDaysCheckEdit weekDias,
                                System.Windows.Forms.ComboBox comboTipoPrecio,
                                FilterControl filCondicion, FilterControl filBeneficio, System.Windows.Forms.ComboBox comboComportamiento,
                                System.Windows.Forms.ComboBox comboListaDePrecios, MaskPromocion maskCuotasSinRecargo, CheckEdit aplicaAutomaticamente)
        {
            this.lisTipoPromocion = listaTipoPromocion;
            this.mskCondicion = maskCondicion;
            this.mskBeneficio = maskBeneficio;
            this.mskTopeDescuento = maskTopeDescuento;
            this.expCondicion = exprCondicion;
            this.expBeneficio = exprBeneficio;
            this.datFechaDesde = dateFechaDesde;
            this.datFechaHasta = dateFechaHasta;
            this.timDesde = timeDesde;
            this.timHasta = timeHasta;
            this.weeDias = weekDias;
            this.cmbTipoPrecio = comboTipoPrecio;
            this.filtroCondicion = filCondicion;
            this.filtroBeneficio = filBeneficio;
            this.comboComportamiento = comboComportamiento;
            this.comboListaDePrecios = comboListaDePrecios;
            this.mskCuotasSinRecargo = maskCuotasSinRecargo;
            this.aplicaAutomaticamente = aplicaAutomaticamente;
        }

        public void ActualizarInformacion()
        {
            this.estructura.Inicializar();
            if ( this.mskCondicion.Visible == false && this.mskBeneficio.Visible == false && this.comboListaDePrecios.Visible == false &&  this.mskCuotasSinRecargo.Visible == false )
                this.estructura.tipoPromocion = "-1";
            else
                this.estructura.tipoPromocion = this.lisTipoPromocion.SelectedIndex.ToString();
                this.estructura.condicionMask = this.mskCondicion.Text;
                this.estructura.beneficioMask = this.mskBeneficio.ObtenerIngreso();
                this.estructura.topeDescuentoMask = this.mskTopeDescuento.ObtenerIngreso();
                this.estructura.vigenciaFechaDesde = this.datFechaDesde.DateTime;
                this.estructura.vigenciaFechaHasta = this.datFechaHasta.DateTime;
                this.estructura.vigenciaHoraDesde = this.timDesde.Time;
                this.estructura.vigenciaHoraHasta = this.timHasta.Time;
                this.estructura.cuotasSinRecargoMask = Convert.ToUInt16(this.mskCuotasSinRecargo.ObtenerIngreso());
            if ( this.cmbTipoPrecio.SelectedIndex == -1 )
                this.estructura.tipoPrecio = "";
            else
                this.estructura.tipoPrecio = (string)this.cmbTipoPrecio.SelectedItem.ToString();
            if (this.comboListaDePrecios.SelectedIndex == -1)
                this.estructura.listaDePrecios = "";
            else
                this.estructura.listaDePrecios = (string)this.comboListaDePrecios.SelectedItem.ToString();
            string[] filtros = new string[ this.expCondicion.FormatItemList.ItemCount ];
            int contador = 0;
            foreach( string valor in this.expCondicion.FormatItemList.Items )
            {
                filtros[contador] = valor;
                contador = contador + 1;
            }
            this.estructura.condicionFiltro = filtros;
            filtros = new string[this.expBeneficio.FormatItemList.ItemCount];
            contador = 0;
            foreach ( string valor in this.expBeneficio.FormatItemList.Items )
            {
                filtros[contador] = valor;
                contador = contador + 1;
            }
            this.estructura.beneficioFiltro = filtros;
            filtros = new string[ 7 ];
            contador = 0;
            foreach ( CheckEdit dia in this.weeDias.Controls )
            {
                filtros[contador] = dia.Checked.ToString();
                contador = contador + 1;
            }
            this.estructura.diasSemana = filtros;
            if ( expBeneficio.FiltroVisible() )
                this.estructura.reglaBeneficio = this.filtroBeneficio.FilterString;
            else
                this.estructura.reglaBeneficio = "";
            if ( expCondicion.FiltroVisible() )
                this.estructura.reglaCondicion = this.filtroCondicion.FilterString;
            else
                this.estructura.reglaCondicion = "";
            this.estructura.comportamiento = this.comboComportamiento.Text;
            this.estructura.aplicaAutomaticamente = this.aplicaAutomaticamente.Checked;
        }

        public string ValorEstructura
        {
            get
            {
                return this.estructura.Resultado;
            }
        }

        public void RestaurarInformacion( string result )
        {
            if ( result.Trim() != "" )
            {
                this.estructura.Inicializar();
                this.estructura.Resultado = result;
                SetearValorAControles();
            }
        }

        private void SetearValorAControles()
        {
            int index;
            index = int.Parse( this.estructura.tipoPromocion );
            if ( index != -1 )
                this.SetearValorListTipoPromocion( index );

            this.SetearValorMaskCondicion( this.estructura.condicionMask );
            this.SetearValorMaskBeneficio( this.estructura.beneficioMask );
            this.SetearValorTopeDescuento( this.estructura.topeDescuentoMask );
            this.SetearValorFechaDesde( this.estructura.vigenciaFechaDesde );
            this.SetearValorFechaHasta( this.estructura.vigenciaFechaHasta );
            this.SetearValorHoraDesde( this.estructura.vigenciaHoraDesde );
            this.SetearValorHoraHasta( this.estructura.vigenciaHoraHasta );
            this.SetearValorFiltroCondicion( this.estructura.condicionFiltro );
            this.SetearValorFiltroBeneficio( this.estructura.beneficioFiltro );            
            bool[] checks = new bool[7];
            bool tildado;
            int contador = 0;
            foreach ( string dia in this.estructura.diasSemana )
            { 
                if ( dia.ToLower() == "true" )
                    tildado = true;
                else
                    tildado = false;
                checks[contador] = tildado;
                contador = contador + 1;
            }
            this.SetearValorDiasSemana( checks );
            this.SetearValorCmbTipoPrecio( this.estructura.tipoPrecio );
            this.SetearValorReglaCondicion( this.estructura.reglaCondicion );
            this.SetearValorReglaBeneficio( this.estructura.reglaBeneficio );
            this.SetearValorConfiguracionAsistente( this.estructura);
            this.SetearValorComboListaDePrecios(this.estructura.listaDePrecios);
            this.SetearValorCuotasSinRecargo(this.estructura.cuotasSinRecargoMask.ToString());
            this.SetearValorAplicaAutomaticamente(this.estructura.aplicaAutomaticamente.ToString());
        }

        private void SetearValorConfiguracionAsistente(EstructuraPromocion promo)
        {
            this.comboComportamiento.Text = promo.comportamiento;
            this.aplicaAutomaticamente.Checked = promo.aplicaAutomaticamente;
        }

        private void SetearValorCmbTipoPrecio( string valor )
        {
            if ( valor == null )
                return;
            if ( valor == "" )
                this.cmbTipoPrecio.SelectedIndex = -1;
            else
                this.cmbTipoPrecio.SelectedItem = valor;
        }

        private void SetearValorComboListaDePrecios(string valor)
        {
            if (valor == null)
                return;
            if (valor == "")
                this.comboListaDePrecios.SelectedIndex = -1;
            else
                this.comboListaDePrecios.SelectedItem = valor;
        }

        private void SetearValorListTipoPromocion( int indice )
        {
            this.lisTipoPromocion.SelectedIndex = indice;
            this.control.SeleccionaItemListaPromocion();
        }

        private void SetearValorMaskCondicion( string valor )
        {
            if ( valor == null )
                return;
            this.mskCondicion.Text = valor;
            //this.mskCondicion.SetearValor( valor );
        }

        private void SetearValorMaskBeneficio( string valor )
        {
            if ( valor == null )
                return;
            this.mskBeneficio.SetearValor( valor );
        }

        private void SetearValorTopeDescuento( string valor )
        {
            if ( valor == null )
                return;
            this.mskTopeDescuento.SetearValor( valor );
        }

        private void SetearValorFiltroCondicion( string[] valores )
        {
            if ( valores == null )
                return;
            this.expCondicion.FormatItemList.Items.Clear();
            if ( valores.Length > 0 )
            {
                foreach ( string filtro in valores )
                {
                    if ( filtro != string.Empty )
                        this.expCondicion.AgregarValorEnDictionary( filtro );
                    //this.expCondicion.FormatItemList.Items.Add( filtro );
                }
                this.expCondicion.HabilitaBotones();
            }
            this.expCondicion.VerificarContenidoListaRegla();
        }

        private void SetearValorFiltroBeneficio( string[] valores )
        {
            if ( valores == null )
                return;
            this.expBeneficio.FormatItemList.Items.Clear();
            if ( valores.Length > 0 )
            {
                foreach ( string filtro in valores )
                {
                    if ( filtro != string.Empty )
                        this.expBeneficio.AgregarValorEnDictionary( filtro );
                    //this.expBeneficio.FormatItemList.Items.Add( filtro );
                }
                this.expBeneficio.HabilitaBotones();
            }
            this.expBeneficio.VerificarContenidoListaRegla();
        }

        private void SetearValorFechaDesde( DateTime fecha )
        {
            if ( fecha == null )
                return;
            this.datFechaDesde.DateTime = fecha;
        }

        private void SetearValorFechaHasta( DateTime fecha )
        {
            if ( fecha == null )
                return;
            this.datFechaHasta.DateTime = fecha;
        }

        private void SetearValorHoraDesde( DateTime hora )
        {
            if ( hora == null )
                return;
            this.timDesde.Time = hora;
        }

        private void SetearValorHoraHasta( DateTime hora )
        {
            if ( hora == null )
                return;
            this.timHasta.Time = hora;
        }

        private void SetearValorDiasSemana( bool[] dias )
        {
            if ( dias == null )
                return;
            int Cont = 0;
            foreach ( CheckEdit dia in this.weeDias.Controls )
            {
                dia.Checked = dias[ Cont ];
                Cont = Cont + 1;
            }
        }

        public string[] ObtenerColeccionDiasSemana( WeekDaysCheckEdit weekDays )
        {
            string[] retorno;
            int contador = 0;

            if ( weekDays != null )
            {
                retorno = new string[weekDays.Controls.Count];
                foreach ( CheckEdit check in weekDays.Controls )
                {
                    retorno[contador] = check.Checked.ToString();
                    contador = contador + 1;
                }
            }
            else
                retorno = new string[0];

            return retorno;
        }

        private void SetearValorReglaCondicion( string valor )
        {
            if ( valor == null )
                return;
            this.filtroCondicion.FilterString = valor;
            if ( valor.Trim() != "" )
                this.expCondicion.ShowEditor();
        }

        private void SetearValorReglaBeneficio( string valor )
        {
            if ( valor == null )
                return;
            this.filtroBeneficio.FilterString = valor;
            if ( valor.Trim() != "" )
                this.expBeneficio.ShowEditor();
        }

        private void SetearValorCuotasSinRecargo(string valor)
        {
            if (valor == null)
                return;
//            this.mskCondicion.Text = valor;
//            this.mskCondicion.SetearValor( valor );
            this.mskCuotasSinRecargo.SetearValor(valor);
        }

        private void SetearValorAplicaAutomaticamente(string valor)
        {
            this.aplicaAutomaticamente.Checked = Convert.ToBoolean(valor);
        }
    }
}
