using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ZooLogicSA.Promociones.UI.Clases;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraScheduler.UI;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Controllers
{
    public interface IControllerPromocion
    {
        EventosPromocion EventosPromocion { get; set; }
        void Inicializar( ListBox listaTipoPromocion, MaskPromocion maskBeneficio, MaskPromocion maskTopeDescuento,
                        MaskedTextBox maskCondicion, Label labelBeneficio, Label labelCondicion,
                        ZooFilterControl filterCondicion, ZooFilterControl filterBeneficio,
                        Button btnMaximizarFiltroCondicion, Button btnMaximizarFiltroBeneficio,
                        Control ctlPromocion, TreeList treeListParticipantes, TreeList treeListFiltroParticipantes,
                        TimeEdit timeDesde, TimeEdit timeHasta, List<Control> lisControlesCanFocused,
                        WeekDaysCheckEdit weekDays, DateEdit dtDesdeFecha, DateEdit dtHastaFecha,
                        System.Windows.Forms.ComboBox comboTipoPrecio, ExpressionConditionsEditor exprCondicion,
                        ExpressionConditionsEditor exprBeneficio, System.Windows.Forms.ComboBox comboComportamiento, FlowLayoutPanel panelBeneficio,
                        System.Windows.Forms.ComboBox comboListaDePrecios, Label labelListaDePrecios, MaskPromocion maskCuotasSinRecargo, CheckEdit aplicaAutomaticamente, LabelControl lblAplicaAutomaticamente );
        void InicializarManagerParticipantes( string xml );
        InterpreteEstructuraPromocion Interprete { get; }
        void LimpiarControles();
        List<Control> ObtenerControlesVisiblesMask();
        string ObtenerValorMaskBeneficio();
        string ObtenerValorMaskTopeDescuento();
        string ObtenerValorMaskCondicion();
        void SeleccionaItemListaPromocion();
        void ActualizarInformacionInterprete();
        string ObtenerValorEstructuraInterprete();
        List<string> ObtenerListaCondicionesSegunTipoPromocion( TipoPromocion tipoPromo );
        string ObtenerTipoDetalleDelParticipante( string fullDescripcion );
        TipoPromocion ObtenerTipoPromocionSeleccionada();
        List<string> ObtenerListaReglaParticipantesBeneficio();
        List<string> ObtenerListaReglaParticipantesCondicion();
        string ObtenerDescripcionComboTipoPrecio();
        DateTime ObtenerVigenciaFechaDesde();
        DateTime ObtenerVigenciaFechaHasta();
        DateTime ObtenerVigenciaHoraDesde();
        DateTime ObtenerVigenciaHoraHasta();
        string[] ObtenerVigenciaDiasSemana();
        void QuitarFocoControlesReglas();
        string ObtenerTopeCargado();
        VisualizacionPromocionAsistenteType ObtenerVisualizacion();
        string ObtenerListaDePreciosCargada();
        string ObtenerCuotasSinRecargo();
        bool ObtenerAplicaAutomaticamente();
        bool ObtenerAplicaConMedioDePago();
    }
}