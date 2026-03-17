namespace ZooLogicSA.Promociones.UI
{
    partial class ControlPromociones
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button btnFiltroBeneficio;
        private System.Windows.Forms.Button btnFiltroCondicion;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label lblBeneficio;
        private ZooFilterControl filBeneficios;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.MaskedTextBox mskCondicion;
        private System.Windows.Forms.Label lblCondicion;
        private ZooFilterControl filCondicion;
        private System.Windows.Forms.ListBox lstBeneficios;
        public DevExpress.XtraTreeList.TreeList treeParticipantes;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTreeList.TreeList treeFiltroParticipantes;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.DateTimePicker dtHasta;
        private System.Windows.Forms.DateTimePicker dtDesde;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.TimeEdit timHasta;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraScheduler.UI.WeekDaysCheckEdit weekDias;
        private System.Windows.Forms.ComboBox cmbTipoPrecio;
        private Clases.ExpressionConditionsEditor expCondicion;
        private ZooLogicSA.Promociones.UI.Clases.ExpressionConditionsEditor expBeneficio;
        public DevExpress.XtraEditors.TimeEdit timDesde;
        public MaskPromocion maskPromocion1;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.GroupControl groupControl8;
        private DevExpress.XtraEditors.GroupControl groupControl7;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        public MaskPromocion maskTopeDescuento;
        private DevExpress.XtraEditors.GroupControl groupControl9;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.DateEdit dtFechaDesde;
        private DevExpress.XtraEditors.DateEdit dtFechaHasta;
        private System.Windows.Forms.ComboBox comboComportamiento;
        private DevExpress.XtraEditors.GroupControl groupControl10;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private System.Windows.Forms.ComboBox comboListaDePrecios;
        private System.Windows.Forms.Label lblListaDePrecios;
        private MaskPromocion maskCuotasSinRecargo; // System.Windows.Forms.MaskedTextBox
        private DevExpress.XtraEditors.LabelControl lblAplicaAutomaticamente;
        private DevExpress.XtraEditors.CheckEdit aplicaAutomaticamente;
        private DevExpress.XtraEditors.GroupControl groupControl11;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();

			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPromociones));
			this.imageList2 = new System.Windows.Forms.ImageList(this.components);
			this.btnFiltroBeneficio = new System.Windows.Forms.Button();
			this.btnFiltroCondicion = new System.Windows.Forms.Button();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.maskPromocion1 = new ZooLogicSA.Promociones.UI.MaskPromocion();
			this.maskTopeDescuento = new ZooLogicSA.Promociones.UI.MaskPromocion();
			this.cmbTipoPrecio = new System.Windows.Forms.ComboBox();
			this.comboListaDePrecios = new System.Windows.Forms.ComboBox();
			this.lblListaDePrecios = new System.Windows.Forms.Label();
            this.lblAplicaAutomaticamente = new DevExpress.XtraEditors.LabelControl();
            this.aplicaAutomaticamente = new DevExpress.XtraEditors.CheckEdit();
            this.maskCuotasSinRecargo = new ZooLogicSA.Promociones.UI.MaskPromocion(); // new System.Windows.Forms.MaskedTextBox(); // new ZooLogicSA.Promociones.UI.MaskPromocion();

			this.lblBeneficio = new System.Windows.Forms.Label();
			this.expBeneficio = new ZooLogicSA.Promociones.UI.Clases.ExpressionConditionsEditor();
			this.filBeneficios = new ZooLogicSA.Promociones.UI.ZooFilterControl();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.mskCondicion = new System.Windows.Forms.MaskedTextBox();
			this.lblCondicion = new System.Windows.Forms.Label();
			this.expCondicion = new ZooLogicSA.Promociones.UI.Clases.ExpressionConditionsEditor();
			this.filCondicion = new ZooLogicSA.Promociones.UI.ZooFilterControl();
			this.lstBeneficios = new System.Windows.Forms.ListBox();
			this.treeParticipantes = new DevExpress.XtraTreeList.TreeList();
			this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
			this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
			this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
			this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
			this.treeFiltroParticipantes = new DevExpress.XtraTreeList.TreeList();
			this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.dtHasta = new System.Windows.Forms.DateTimePicker();
			this.dtDesde = new System.Windows.Forms.DateTimePicker();
			this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
			this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
			this.dtFechaDesde = new DevExpress.XtraEditors.DateEdit();
			this.dtFechaHasta = new DevExpress.XtraEditors.DateEdit();
			this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
			this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
			this.timHasta = new DevExpress.XtraEditors.TimeEdit();
			this.timDesde = new DevExpress.XtraEditors.TimeEdit();
			this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
			this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
			this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
			this.weekDias = new DevExpress.XtraScheduler.UI.WeekDaysCheckEdit();
			this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
			this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
			this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
			this.groupControl8 = new DevExpress.XtraEditors.GroupControl();
			this.groupControl7 = new DevExpress.XtraEditors.GroupControl();
			this.groupControl9 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl11 = new DevExpress.XtraEditors.GroupControl();
			this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
			this.comboComportamiento = new System.Windows.Forms.ComboBox();
			this.groupControl10 = new DevExpress.XtraEditors.GroupControl();
			this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.AccessibleName = "cntPromociones";

            this.flowLayoutPanel2.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.treeParticipantes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.treeFiltroParticipantes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
			this.groupControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaDesde.Properties.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaDesde.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties.VistaTimeProperties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
			this.groupControl2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timHasta.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timDesde.Properties)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
			this.groupControl3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.weekDias)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl9)).BeginInit();
			this.groupControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).BeginInit();
            this.groupControl11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl10)).BeginInit();
            this.SuspendLayout();
			// 
			// imageList2
			// 
			this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
			this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList2.Images.SetKeyName(0, "ZoomOut.png");
			this.imageList2.Images.SetKeyName(1, "ZoomIn_out.png");
			this.imageList2.Images.SetKeyName(2, "ZoomIn.png");
			this.imageList2.Images.SetKeyName(3, "ZoomOut_out.png");
			this.imageList2.Images.SetKeyName(4, "Add.png");
			this.imageList2.Images.SetKeyName(5, "Add_over.png");
			// 
			// btnFiltroBeneficio
			// 
			this.btnFiltroBeneficio.BackColor = System.Drawing.Color.Transparent;
			this.btnFiltroBeneficio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnFiltroBeneficio.FlatAppearance.BorderSize = 0;
			this.btnFiltroBeneficio.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.btnFiltroBeneficio.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnFiltroBeneficio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFiltroBeneficio.ImageIndex = 0;
			this.btnFiltroBeneficio.ImageList = this.imageList2;
			this.btnFiltroBeneficio.Location = new System.Drawing.Point(959, 3);
			this.btnFiltroBeneficio.Name = "btnFiltroBeneficio";
			this.btnFiltroBeneficio.Size = new System.Drawing.Size(30, 30);
			this.btnFiltroBeneficio.TabIndex = 72;
			this.btnFiltroBeneficio.TabStop = false;
			this.btnFiltroBeneficio.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
			this.btnFiltroBeneficio.UseVisualStyleBackColor = false;
			this.btnFiltroBeneficio.Visible = false;
			// 
			// btnFiltroCondicion
			// 
			this.btnFiltroCondicion.BackColor = System.Drawing.Color.Transparent;
			this.btnFiltroCondicion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnFiltroCondicion.FlatAppearance.BorderSize = 0;
			this.btnFiltroCondicion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
			this.btnFiltroCondicion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
			this.btnFiltroCondicion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnFiltroCondicion.ImageIndex = 0;
			this.btnFiltroCondicion.ImageList = this.imageList2;
			this.btnFiltroCondicion.Location = new System.Drawing.Point(526, 0);
			this.btnFiltroCondicion.Name = "btnFiltroCondicion";
			this.btnFiltroCondicion.Size = new System.Drawing.Size(30, 30);
			this.btnFiltroCondicion.TabIndex = 71;
			this.btnFiltroCondicion.TabStop = false;
			this.btnFiltroCondicion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
			this.btnFiltroCondicion.UseVisualStyleBackColor = false;
			this.btnFiltroCondicion.Visible = false;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel2.Controls.Add(this.maskPromocion1);
			this.flowLayoutPanel2.Controls.Add(this.maskTopeDescuento);
			this.flowLayoutPanel2.Controls.Add(this.cmbTipoPrecio);
			this.flowLayoutPanel2.Controls.Add(this.lblListaDePrecios);
			this.flowLayoutPanel2.Controls.Add(this.comboListaDePrecios);
			this.flowLayoutPanel2.Controls.Add(this.lblBeneficio);
			this.flowLayoutPanel2.Controls.Add(this.expBeneficio);
			this.flowLayoutPanel2.Controls.Add(this.filBeneficios);
			this.flowLayoutPanel2.Controls.Add(this.maskCuotasSinRecargo);
			this.flowLayoutPanel2.Location = new System.Drawing.Point(655, 17);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(340, 167);
			this.flowLayoutPanel2.TabIndex = 70;
			// 
			// maskPromocion1
			// 
			this.maskPromocion1.AccessibleName = "mskBeneficio";
			this.maskPromocion1.Decimal = true;
			this.maskPromocion1.FinEditable = 28;
			this.maskPromocion1.InicioLabel = 0;
			this.maskPromocion1.InicioParteDecimal = 27;
			this.maskPromocion1.InicioParteEntera = 14;
			this.maskPromocion1.largoParteEntera = 12;
			this.maskPromocion1.Location = new System.Drawing.Point(0, 0);
			this.maskPromocion1.Margin = new System.Windows.Forms.Padding(0);
			this.maskPromocion1.Mascara = "Precio prueb\\a 999999999999.99";
			this.maskPromocion1.Name = "maskPromocion1";
			this.maskPromocion1.Size = new System.Drawing.Size(165, 21);
			this.maskPromocion1.TabIndex = 4;
			// 
			// maskTopeDescuento
			// 
			this.maskTopeDescuento.AccessibleName = "mskTopeDescuento";
			this.maskTopeDescuento.Decimal = true;
			this.maskTopeDescuento.FinEditable = 19;
			this.maskTopeDescuento.InicioLabel = 0;
			this.maskTopeDescuento.InicioParteDecimal = 18;
			this.maskTopeDescuento.InicioParteEntera = 5;
			this.maskTopeDescuento.largoParteEntera = 12;
			this.maskTopeDescuento.Location = new System.Drawing.Point(165, 0);
			this.maskTopeDescuento.Margin = new System.Windows.Forms.Padding(0);
			this.maskTopeDescuento.Mascara = "Tope 999999999999.99";
			this.maskTopeDescuento.Name = "maskTopeDescuento";
			this.maskTopeDescuento.Size = new System.Drawing.Size(165, 21);
			this.maskTopeDescuento.TabIndex = 5;
			// 
			// cmbTipoPrecio
			// 
			this.cmbTipoPrecio.AccessibleName = "TipoBeneficio";
			this.cmbTipoPrecio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTipoPrecio.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmbTipoPrecio.FormattingEnabled = true;
			this.cmbTipoPrecio.Items.AddRange(new object[] {
            "Aplicar al de menor precio",
            "Aplicar al de mayor precio"});
			this.cmbTipoPrecio.Location = new System.Drawing.Point(3, 51);
			this.cmbTipoPrecio.Name = "cmbTipoPrecio";
			this.cmbTipoPrecio.Size = new System.Drawing.Size(331, 21);
			this.cmbTipoPrecio.TabIndex = 6;
			this.cmbTipoPrecio.Tag = "Indica si aplica el beneficio al de menor o mayor precio.";
			// 
			// comboListaDePrecios
			// 
			this.comboListaDePrecios.AccessibleName = "";
			this.comboListaDePrecios.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboListaDePrecios.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboListaDePrecios.FormattingEnabled = true;
			this.comboListaDePrecios.Items.AddRange(new object[] {""});
			this.comboListaDePrecios.Location = new System.Drawing.Point(129, 30);
			this.comboListaDePrecios.Name = "comboListaDePrecios";
			//this.comboListaDePrecios.Size = new System.Drawing.Size(60, 21);
			this.comboListaDePrecios.Size = new System.Drawing.Size(65, 21);
			this.comboListaDePrecios.TabIndex = 74;
			this.comboListaDePrecios.Visible = false;
			this.comboListaDePrecios.Tag = "Lista de precios que se utilizará para los participantes que cumplan las condiciones";
			this.comboListaDePrecios.Visible = false;

			// 
			// lblListaDePrecios
			// 
			this.lblListaDePrecios.BackColor = System.Drawing.Color.Transparent;
			this.lblListaDePrecios.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblListaDePrecios.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "ColorRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblListaDePrecios.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.lblListaDePrecios.ForeColor = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.ColorRegular;
			this.lblListaDePrecios.Location = new System.Drawing.Point(3, 27);
			this.lblListaDePrecios.Name = "lblListaDePrecios";
			this.lblListaDePrecios.Size = new System.Drawing.Size(120, 27);
			this.lblListaDePrecios.TabIndex = 73;
			this.lblListaDePrecios.Text = "Usa lista de precios:";
			this.lblListaDePrecios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBeneficio
            // 
            this.lblBeneficio.BackColor = System.Drawing.Color.Transparent;
			this.lblBeneficio.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblBeneficio.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "ColorRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblBeneficio.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.lblBeneficio.ForeColor = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.ColorRegular;
			this.lblBeneficio.Location = new System.Drawing.Point(3, 54);
			this.lblBeneficio.Name = "lblBeneficio";
			this.lblBeneficio.Size = new System.Drawing.Size(337, 13);
			this.lblBeneficio.TabIndex = 7;
			this.lblBeneficio.Text = "2. Condición";
			// 
			// expBeneficio
			// 
			this.expBeneficio.AccessibleName = "expBeneficio";
			this.expBeneficio.Location = new System.Drawing.Point(3, 91);
			this.expBeneficio.Name = "expBeneficio";
			this.expBeneficio.Size = new System.Drawing.Size(337, 79);
			this.expBeneficio.TabIndex = 8;
			// 
			// filBeneficios
			// 
			this.filBeneficios.AllowDrop = true;
			this.filBeneficios.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.filBeneficios.Appearance.Options.UseFont = true;
			this.filBeneficios.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.filBeneficios.Location = new System.Drawing.Point(3, 149);
			this.filBeneficios.Name = "filBeneficios";
			this.filBeneficios.ShowIsNullOperatorsForStrings = true;
			this.filBeneficios.Size = new System.Drawing.Size(304, 91);
			this.filBeneficios.TabIndex = 9;
			this.filBeneficios.Tag = "Beneficiados por la promoción";
			this.filBeneficios.Text = "filterControl1";

            // Cantidad de Cuotas sin Recargo

			this.maskCuotasSinRecargo.AccessibleName = "maskCuotasSinRecargo";
			this.maskCuotasSinRecargo.Decimal = false;
			this.maskCuotasSinRecargo.FinEditable = 29;
			this.maskCuotasSinRecargo.InicioLabel = 0;
			this.maskCuotasSinRecargo.InicioParteDecimal = 10;
			this.maskCuotasSinRecargo.InicioParteEntera = 7;
			this.maskCuotasSinRecargo.largoParteEntera = 3;
			this.maskCuotasSinRecargo.Location = new System.Drawing.Point(165, 29);
			this.maskCuotasSinRecargo.Margin = new System.Windows.Forms.Padding(0);
            this.maskCuotasSinRecargo.Mascara = "H\\ast\\a 999 \\cuot\\as sin re\\c\\argo";
            this.maskCuotasSinRecargo.Name = "maskCuotasSinRecargo";
			this.maskCuotasSinRecargo.Size = new System.Drawing.Size(165, 29);
			this.maskCuotasSinRecargo.TabIndex = 76;

            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AccessibleName = "flpCondicion";
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.flowLayoutPanel1.Controls.Add(this.mskCondicion);
			this.flowLayoutPanel1.Controls.Add(this.lblCondicion);
			this.flowLayoutPanel1.Controls.Add(this.expCondicion);
			this.flowLayoutPanel1.Controls.Add(this.filCondicion);
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(295, 17);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(354, 167);
			this.flowLayoutPanel1.TabIndex = 69;
			// 
			// mskCondicion
			// 
			this.mskCondicion.AccessibleName = "mskCondicion";
			this.mskCondicion.AllowPromptAsInput = false;
			this.mskCondicion.AsciiOnly = true;
			this.mskCondicion.BackColor = System.Drawing.Color.White;
			this.mskCondicion.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.mskCondicion.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "ColorRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.mskCondicion.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.mskCondicion.ForeColor = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.ColorRegular;
			this.mskCondicion.HidePromptOnLeave = true;
			this.mskCondicion.Location = new System.Drawing.Point(3, 3);
			this.mskCondicion.Name = "mskCondicion";
			this.mskCondicion.Size = new System.Drawing.Size(351, 20);
			this.mskCondicion.TabIndex = 2;
			this.mskCondicion.Tag = "Condición para que se cumpla la promoción";
			this.mskCondicion.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			// 
			// lblCondicion
			// 
			this.lblCondicion.BackColor = System.Drawing.Color.Transparent;
			this.lblCondicion.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblCondicion.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "ColorRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lblCondicion.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.lblCondicion.ForeColor = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.ColorRegular;
			this.lblCondicion.Location = new System.Drawing.Point(3, 26);
			this.lblCondicion.Name = "lblCondicion";
			this.lblCondicion.Size = new System.Drawing.Size(351, 13);
			this.lblCondicion.TabIndex = 37;
			this.lblCondicion.Text = "2. Condición";
			// 
			// expCondicion
			// 
			this.expCondicion.AccessibleName = "expCondicion";
			this.expCondicion.Location = new System.Drawing.Point(3, 42);
			this.expCondicion.Name = "expCondicion";
			this.expCondicion.Size = new System.Drawing.Size(351, 91);
			this.expCondicion.TabIndex = 3;
			// 
			// filCondicion
			// 
			this.filCondicion.AllowDrop = true;
			this.filCondicion.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.filCondicion.Appearance.Options.UseFont = true;
			this.filCondicion.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.filCondicion.Location = new System.Drawing.Point(360, 3);
			this.filCondicion.Name = "filCondicion";
			this.filCondicion.ShowIsNullOperatorsForStrings = true;
			this.filCondicion.Size = new System.Drawing.Size(270, 91);
			this.filCondicion.TabIndex = 3;
			this.filCondicion.Tag = "Condición para que se cumpla la promoción";
			this.filCondicion.Text = "filterControl1";
			// 
			// lstBeneficios
			// 
			this.lstBeneficios.AccessibleName = "lstBeneficios";
			this.lstBeneficios.BackColor = System.Drawing.Color.White;
			this.lstBeneficios.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lstBeneficios.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "ColorRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.lstBeneficios.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.lstBeneficios.ForeColor = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.ColorRegular;
			this.lstBeneficios.FormattingEnabled = true;
			this.lstBeneficios.Location = new System.Drawing.Point(10, 21);
			this.lstBeneficios.Name = "lstBeneficios";
			this.lstBeneficios.Size = new System.Drawing.Size(271, 160);
			this.lstBeneficios.TabIndex = 1;
			this.lstBeneficios.Tag = "Tipo de promoción";
			// 
			// treeParticipantes
			// 
			this.treeParticipantes.AccessibleName = "participantes";
			this.treeParticipantes.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4});
			this.treeParticipantes.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.treeParticipantes.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.treeParticipantes.Location = new System.Drawing.Point(11, 210);
			this.treeParticipantes.LookAndFeel.UseDefaultLookAndFeel = false;
			this.treeParticipantes.Name = "treeParticipantes";
			this.treeParticipantes.OptionsBehavior.AllowExpandOnDblClick = false;
			this.treeParticipantes.OptionsBehavior.Editable = false;
			this.treeParticipantes.OptionsBehavior.EnableFiltering = true;
			this.treeParticipantes.OptionsBehavior.ExpandNodeOnDrag = false;
			this.treeParticipantes.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Smart;
			this.treeParticipantes.OptionsMenu.EnableColumnMenu = false;
			this.treeParticipantes.OptionsMenu.EnableFooterMenu = false;
			this.treeParticipantes.OptionsMenu.ShowAutoFilterRowItem = false;
			this.treeParticipantes.OptionsView.ShowFocusedFrame = false;
			this.treeParticipantes.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowOnlyInEditor;
			this.treeParticipantes.Size = new System.Drawing.Size(271, 165);
			this.treeParticipantes.TabIndex = 7;
			this.treeParticipantes.Tag = "Lista de participantes de la promoción";
			// 
			// treeListColumn1
			// 
			this.treeListColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8F);
			this.treeListColumn1.AppearanceHeader.Options.UseFont = true;
			this.treeListColumn1.Caption = "Participantes";
			this.treeListColumn1.FieldName = "treeListColumn1";
			this.treeListColumn1.MinWidth = 52;
			this.treeListColumn1.Name = "treeListColumn1";
			this.treeListColumn1.OptionsColumn.AllowMove = false;
			this.treeListColumn1.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumn1.OptionsColumn.ReadOnly = true;
			this.treeListColumn1.Visible = true;
			this.treeListColumn1.VisibleIndex = 0;
			this.treeListColumn1.Width = 93;
			// 
			// treeListColumn2
			// 
			this.treeListColumn2.Caption = "Nombre";
			this.treeListColumn2.FieldName = "treeListColumn2";
			this.treeListColumn2.MinWidth = 16;
			this.treeListColumn2.Name = "treeListColumn2";
			this.treeListColumn2.OptionsColumn.AllowMove = false;
			this.treeListColumn2.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumn2.OptionsColumn.FixedWidth = true;
			this.treeListColumn2.OptionsColumn.ReadOnly = true;
			this.treeListColumn2.Width = 0;
			// 
			// treeListColumn3
			// 
			this.treeListColumn3.Caption = "Grupo";
			this.treeListColumn3.FieldName = "treeListColumn3";
			this.treeListColumn3.MinWidth = 16;
			this.treeListColumn3.Name = "treeListColumn3";
			this.treeListColumn3.OptionsColumn.AllowMove = false;
			this.treeListColumn3.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumn3.OptionsColumn.FixedWidth = true;
			this.treeListColumn3.OptionsColumn.ReadOnly = true;
			this.treeListColumn3.Width = 0;
			// 
			// treeListColumn4
			// 
			this.treeListColumn4.Caption = "Atrib";
			this.treeListColumn4.FieldName = "Atrib";
			this.treeListColumn4.MinWidth = 16;
			this.treeListColumn4.Name = "treeListColumn4";
			this.treeListColumn4.OptionsColumn.AllowMove = false;
			this.treeListColumn4.OptionsColumn.AllowMoveToCustomizationForm = false;
			this.treeListColumn4.OptionsColumn.FixedWidth = true;
			this.treeListColumn4.OptionsColumn.ReadOnly = true;
			this.treeListColumn4.Width = 16;
			// 
			// treeFiltroParticipantes
			// 
			this.treeFiltroParticipantes.AccessibleName = "filtroParticipantes";
			this.treeFiltroParticipantes.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.treeFiltroParticipantes.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.treeFiltroParticipantes.Location = new System.Drawing.Point(295, 210);
			this.treeFiltroParticipantes.Name = "treeFiltroParticipantes";
			this.treeFiltroParticipantes.OptionsBehavior.AllowExpandOnDblClick = false;
			this.treeFiltroParticipantes.OptionsBehavior.Editable = false;
			this.treeFiltroParticipantes.OptionsBehavior.EnableFiltering = true;
			this.treeFiltroParticipantes.OptionsBehavior.ExpandNodeOnDrag = false;
			this.treeFiltroParticipantes.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Smart;
			this.treeFiltroParticipantes.OptionsMenu.EnableColumnMenu = false;
			this.treeFiltroParticipantes.OptionsMenu.EnableFooterMenu = false;
			this.treeFiltroParticipantes.OptionsMenu.ShowAutoFilterRowItem = false;
			this.treeFiltroParticipantes.OptionsView.ShowAutoFilterRow = true;
			this.treeFiltroParticipantes.OptionsView.ShowButtons = false;
			this.treeFiltroParticipantes.OptionsView.ShowFilterPanelMode = DevExpress.XtraTreeList.ShowFilterPanelMode.Never;
			this.treeFiltroParticipantes.OptionsView.ShowFocusedFrame = false;
			this.treeFiltroParticipantes.Size = new System.Drawing.Size(700, 165);
			this.treeFiltroParticipantes.TabIndex = 8;
			this.treeFiltroParticipantes.Tag = "Valores para armar la promoción";
			// 
			// labelControl3
			// 
			this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl3.Location = new System.Drawing.Point(66, 48);
			this.labelControl3.Name = "labelControl3";
			this.labelControl3.Size = new System.Drawing.Size(6, 13);
			this.labelControl3.TabIndex = 66;
			this.labelControl3.Text = "a";
			// 
			// labelControl4
			// 
			this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl4.Location = new System.Drawing.Point(66, 23);
			this.labelControl4.Name = "labelControl4";
			this.labelControl4.Size = new System.Drawing.Size(13, 13);
			this.labelControl4.TabIndex = 65;
			this.labelControl4.Text = "De";
			// 
			// labelControl2
			// 
			this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl2.Location = new System.Drawing.Point(13, 48);
			this.labelControl2.Name = "labelControl2";
			this.labelControl2.Size = new System.Drawing.Size(28, 13);
			this.labelControl2.TabIndex = 66;
			this.labelControl2.Text = "Hasta";
			// 
			// labelControl1
			// 
			this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl1.Location = new System.Drawing.Point(13, 23);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(30, 13);
			this.labelControl1.TabIndex = 65;
			this.labelControl1.Text = "Desde";
			// 
			// dtHasta
			// 
			this.dtHasta.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.dtHasta.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.dtHasta.Location = new System.Drawing.Point(62, 46);
			this.dtHasta.Name = "dtHasta";
			this.dtHasta.Size = new System.Drawing.Size(200, 20);
			this.dtHasta.TabIndex = 64;
			// 
			// dtDesde
			// 
			this.dtDesde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(169)))), ((int)(((byte)(192)))));
			this.dtDesde.CalendarMonthBackground = System.Drawing.Color.White;
			this.dtDesde.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::ZooLogicSA.Promociones.UI.Properties.Settings.Default, "FontRegular", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.dtDesde.Font = global::ZooLogicSA.Promociones.UI.Properties.Settings.Default.FontRegular;
			this.dtDesde.Location = new System.Drawing.Point(62, 19);
			this.dtDesde.Name = "dtDesde";
			this.dtDesde.Size = new System.Drawing.Size(200, 20);
			this.dtDesde.TabIndex = 63;
			// 
			// labelControl9
			// 
			this.labelControl9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.labelControl9.Location = new System.Drawing.Point(11, 0);
			this.labelControl9.Name = "labelControl9";
			this.labelControl9.Size = new System.Drawing.Size(90, 13);
			this.labelControl9.TabIndex = 80;
			this.labelControl9.Text = "Tipo de promoción ";
			// 
			// labelControl10
			// 
			this.labelControl10.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.labelControl10.Location = new System.Drawing.Point(295, 0);
			this.labelControl10.Name = "labelControl10";
			this.labelControl10.Size = new System.Drawing.Size(46, 13);
			this.labelControl10.TabIndex = 81;
			this.labelControl10.Text = "Condición";
			// 
			// labelControl11
			// 
			this.labelControl11.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.labelControl11.Location = new System.Drawing.Point(655, 0);
			this.labelControl11.Name = "labelControl11";
			this.labelControl11.Size = new System.Drawing.Size(46, 13);
			this.labelControl11.TabIndex = 82;
			this.labelControl11.Text = "Beneficio ";
			// 
			// labelControl12
			// 
			this.labelControl12.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.labelControl12.Location = new System.Drawing.Point(11, 194);
			this.labelControl12.Name = "labelControl12";
			this.labelControl12.Size = new System.Drawing.Size(65, 13);
			this.labelControl12.TabIndex = 83;
			this.labelControl12.Text = "Participantes ";
			// 
			// labelControl13
			// 
			this.labelControl13.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.labelControl13.Location = new System.Drawing.Point(11, 378);
			this.labelControl13.Name = "labelControl13";
			this.labelControl13.Size = new System.Drawing.Size(42, 13);
			this.labelControl13.TabIndex = 84;
			this.labelControl13.Text = "Vigencia ";
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
            this.labelControl17.Location = new System.Drawing.Point(768, 378);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(149, 13);
            this.labelControl17.TabIndex = 94;
            this.labelControl17.Text = "Configuraciones";
            // 
            // groupControl1
            // 
            this.groupControl1.AccessibleName = "cntFecha";
			this.groupControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl1.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl1.Appearance.Options.UseBackColor = true;
			this.groupControl1.Appearance.Options.UseBorderColor = true;
			this.groupControl1.Appearance.Options.UseForeColor = true;
			this.groupControl1.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl1.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl1.Controls.Add(this.dtFechaDesde);
			this.groupControl1.Controls.Add(this.dtFechaHasta);
			this.groupControl1.Controls.Add(this.labelControl7);
			this.groupControl1.Controls.Add(this.labelControl8);
			this.groupControl1.Location = new System.Drawing.Point(11, 392);
			this.groupControl1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl1.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl1.Name = "groupControl1";
			this.groupControl1.Size = new System.Drawing.Size(270, 78);
			this.groupControl1.TabIndex = 85;
			this.groupControl1.Text = "Fecha";
			// 
			// dtFechaDesde
			// 
			this.dtFechaDesde.EditValue = new System.DateTime(2014, 6, 13, 11, 28, 27, 0);
			this.dtFechaDesde.Location = new System.Drawing.Point(56, 23);
			this.dtFechaDesde.Name = "dtFechaDesde";
			this.dtFechaDesde.Properties.Appearance.Options.UseTextOptions = true;
			this.dtFechaDesde.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.dtFechaDesde.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtFechaDesde.Properties.Mask.EditMask = "D";
			this.dtFechaDesde.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.dtFechaDesde.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.dtFechaDesde.Size = new System.Drawing.Size(209, 20);
			this.dtFechaDesde.TabIndex = 9;
			this.dtFechaDesde.Tag = "Fecha de vigencia inicial";
			// 
			// dtFechaHasta
			// 
			this.dtFechaHasta.EditValue = new System.DateTime(2014, 6, 13, 11, 0, 11, 275);
			this.dtFechaHasta.Location = new System.Drawing.Point(56, 47);
			this.dtFechaHasta.Name = "dtFechaHasta";
			this.dtFechaHasta.Properties.Appearance.Options.UseTextOptions = true;
			this.dtFechaHasta.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.dtFechaHasta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
			this.dtFechaHasta.Properties.Mask.EditMask = "D";
			this.dtFechaHasta.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.dtFechaHasta.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.dtFechaHasta.Size = new System.Drawing.Size(209, 20);
			this.dtFechaHasta.TabIndex = 10;
			this.dtFechaHasta.Tag = "Fecha de vigencia final";
			// 
			// labelControl7
			// 
			this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl7.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl7.Location = new System.Drawing.Point(13, 50);
			this.labelControl7.Name = "labelControl7";
			this.labelControl7.Size = new System.Drawing.Size(28, 13);
			this.labelControl7.TabIndex = 70;
			this.labelControl7.Text = "Hasta";
			// 
			// labelControl8
			// 
			this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl8.Location = new System.Drawing.Point(13, 25);
			this.labelControl8.Name = "labelControl8";
			this.labelControl8.Size = new System.Drawing.Size(30, 13);
			this.labelControl8.TabIndex = 69;
			this.labelControl8.Text = "Desde";
			// 
			// groupControl2
			// 
			this.groupControl2.AccessibleName = "cntHora";
			this.groupControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl2.Appearance.Options.UseBackColor = true;
			this.groupControl2.Appearance.Options.UseBorderColor = true;
			this.groupControl2.Appearance.Options.UseForeColor = true;
			this.groupControl2.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl2.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl2.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl2.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl2.Controls.Add(this.timHasta);
			this.groupControl2.Controls.Add(this.timDesde);
			this.groupControl2.Controls.Add(this.labelControl5);
			this.groupControl2.Controls.Add(this.labelControl6);
			this.groupControl2.Location = new System.Drawing.Point(295, 392);
			this.groupControl2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl2.Name = "groupControl2";
			this.groupControl2.Size = new System.Drawing.Size(225, 78);
			this.groupControl2.TabIndex = 86;
			this.groupControl2.Text = "Rango horario";
			// 
			// timHasta
			// 
			this.timHasta.EditValue = new System.DateTime(2012, 8, 3, 0, 0, 0, 0);
			this.timHasta.Location = new System.Drawing.Point(70, 47);
			this.timHasta.Name = "timHasta";
			this.timHasta.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.timHasta.Properties.Appearance.Options.UseFont = true;
			this.timHasta.Properties.Appearance.Options.UseTextOptions = true;
			this.timHasta.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.timHasta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.timHasta.Size = new System.Drawing.Size(103, 20);
			this.timHasta.TabIndex = 12;
			this.timHasta.Tag = "Rango horario final";
			// 
			// timDesde
			// 
			this.timDesde.EditValue = new System.DateTime(2012, 8, 3, 0, 0, 0, 0);
			this.timDesde.Location = new System.Drawing.Point(70, 23);
			this.timDesde.Name = "timDesde";
			this.timDesde.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.timDesde.Properties.Appearance.Options.UseFont = true;
			this.timDesde.Properties.Appearance.Options.UseTextOptions = true;
			this.timDesde.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.timDesde.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.timDesde.Size = new System.Drawing.Size(103, 20);
			this.timDesde.TabIndex = 11;
			this.timDesde.Tag = "Rango horario inicial";
			// 
			// labelControl5
			// 
			this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl5.Location = new System.Drawing.Point(51, 49);
			this.labelControl5.Name = "labelControl5";
			this.labelControl5.Size = new System.Drawing.Size(6, 13);
			this.labelControl5.TabIndex = 70;
			this.labelControl5.Text = "a";
			// 
			// labelControl6
			// 
			this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl6.Location = new System.Drawing.Point(51, 25);
			this.labelControl6.Name = "labelControl6";
			this.labelControl6.Size = new System.Drawing.Size(13, 13);
			this.labelControl6.TabIndex = 69;
			this.labelControl6.Text = "De";
			// 
			// groupControl3
			// 
			this.groupControl3.AccessibleName = "cntDias";
			this.groupControl3.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl3.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl3.Appearance.Options.UseBackColor = true;
			this.groupControl3.Appearance.Options.UseBorderColor = true;
			this.groupControl3.Appearance.Options.UseForeColor = true;
			this.groupControl3.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl3.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl3.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl3.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl3.Controls.Add(this.weekDias);
            this.groupControl3.Location = new System.Drawing.Point(526, 392);
			this.groupControl3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl3.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl3.Name = "groupControl3";
			this.groupControl3.Size = new System.Drawing.Size(236, 78);
			this.groupControl3.TabIndex = 87;
			this.groupControl3.Text = "Dias de la semana";
            // 
            // weekDias
            // 
            this.weekDias.AccessibleName = "chkCntDias";
			this.weekDias.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.weekDias.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.weekDias.Appearance.ForeColor = System.Drawing.Color.Black;
			this.weekDias.Appearance.Options.UseBackColor = true;
			this.weekDias.Appearance.Options.UseFont = true;
			this.weekDias.Appearance.Options.UseForeColor = true;
			this.weekDias.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
			this.weekDias.FirstDayOfWeek = DevExpress.XtraScheduler.FirstDayOfWeek.Monday;
			this.weekDias.Location = new System.Drawing.Point(15, 23);
			this.weekDias.Name = "weekDias";
			this.weekDias.Size = new System.Drawing.Size(197, 53);
			this.weekDias.TabIndex = 13;
			this.weekDias.Tag = "Dias de la semana que tiene vigencia la promoción";
			this.weekDias.UseAbbreviatedDayNames = true;
			// 
			// groupControl4
			// 
			this.groupControl4.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl4.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl4.Appearance.Options.UseBackColor = true;
			this.groupControl4.Appearance.Options.UseBorderColor = true;
			this.groupControl4.Appearance.Options.UseForeColor = true;
			this.groupControl4.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl4.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl4.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl4.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl4.Location = new System.Drawing.Point(82, 192);
			this.groupControl4.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl4.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl4.Name = "groupControl4";
			this.groupControl4.Size = new System.Drawing.Size(911, 10);
			this.groupControl4.TabIndex = 88;
            // 
            // groupControl5
            // 
            this.groupControl5.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl5.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl5.Appearance.Options.UseBackColor = true;
			this.groupControl5.Appearance.Options.UseBorderColor = true;
			this.groupControl5.Appearance.Options.UseForeColor = true;
			this.groupControl5.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl5.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl5.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl5.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl5.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl5.Location = new System.Drawing.Point(57, 378);
			this.groupControl5.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl5.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl5.Name = "groupControl5";
			this.groupControl5.Size = new System.Drawing.Size(705, 10);
			this.groupControl5.TabIndex = 89;
			// 
			// groupControl6
			// 
			this.groupControl6.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl6.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl6.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl6.Appearance.Options.UseBackColor = true;
			this.groupControl6.Appearance.Options.UseBorderColor = true;
			this.groupControl6.Appearance.Options.UseForeColor = true;
			this.groupControl6.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl6.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl6.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl6.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl6.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl6.Location = new System.Drawing.Point(104, -1);
			this.groupControl6.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl6.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl6.Name = "groupControl6";
			this.groupControl6.Size = new System.Drawing.Size(178, 10);
			this.groupControl6.TabIndex = 89;
			// 
			// groupControl8
			// 
			this.groupControl8.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl8.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl8.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl8.Appearance.Options.UseBackColor = true;
			this.groupControl8.Appearance.Options.UseBorderColor = true;
			this.groupControl8.Appearance.Options.UseForeColor = true;
			this.groupControl8.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl8.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl8.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl8.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl8.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl8.Location = new System.Drawing.Point(707, -2);
			this.groupControl8.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl8.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl8.Name = "groupControl8";
			this.groupControl8.Size = new System.Drawing.Size(291, 10);
			this.groupControl8.TabIndex = 91;
			// 
			// groupControl7
			// 
			this.groupControl7.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl7.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl7.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl7.Appearance.Options.UseBackColor = true;
			this.groupControl7.Appearance.Options.UseBorderColor = true;
			this.groupControl7.Appearance.Options.UseForeColor = true;
			this.groupControl7.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl7.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl7.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl7.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl7.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl7.Location = new System.Drawing.Point(347, -2);
			this.groupControl7.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl7.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl7.Name = "groupControl7";
			this.groupControl7.Size = new System.Drawing.Size(302, 10);
			this.groupControl7.TabIndex = 92;
			// 
			// groupControl9
			// 
			this.groupControl9.AccessibleName = "cntMiselanias";
			this.groupControl9.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl9.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl9.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl9.Appearance.Options.UseBackColor = true;
			this.groupControl9.Appearance.Options.UseBorderColor = true;
			this.groupControl9.Appearance.Options.UseForeColor = true;
			this.groupControl9.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl9.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl9.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl9.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl9.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl9.Controls.Add(this.labelControl15);
			this.groupControl9.Controls.Add(this.comboComportamiento);
			this.groupControl9.Location = new System.Drawing.Point(768, 392);
			this.groupControl9.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl9.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl9.Name = "groupControl9";
			this.groupControl9.Size = new System.Drawing.Size(225, 45);
			this.groupControl9.TabIndex = 87;
            this.groupControl9.Text = "Asistente de promociones";
            // 
            // groupControl11
            // 
            this.groupControl11.AccessibleName = "cntMiselaniasAuto";
            this.groupControl11.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.groupControl11.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
            this.groupControl11.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
            this.groupControl11.Appearance.Options.UseBackColor = true;
            this.groupControl11.Appearance.Options.UseBorderColor = true;
            this.groupControl11.Appearance.Options.UseForeColor = true;
            this.groupControl11.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
            this.groupControl11.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
            this.groupControl11.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl11.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl11.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.groupControl11.Controls.Add(this.lblAplicaAutomaticamente);
            this.groupControl11.Controls.Add(this.aplicaAutomaticamente);
            this.groupControl11.Location = new System.Drawing.Point(768, 435);
            this.groupControl11.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.groupControl11.LookAndFeel.UseDefaultLookAndFeel = false;
            this.groupControl11.Name = "groupControl11";
            this.groupControl11.Size = new System.Drawing.Size(225, 35);
            this.groupControl11.TabIndex = 87;
            this.groupControl11.Text = "Promoción automática";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
			this.labelControl15.Appearance.ForeColor = System.Drawing.Color.Black;
			this.labelControl15.Location = new System.Drawing.Point(12, 23);
			this.labelControl15.Name = "labelControl15";
			this.labelControl15.Size = new System.Drawing.Size(59, 13);
			this.labelControl15.TabIndex = 72;
			this.labelControl15.Text = "Visualización";
			// 
			// comboComportamiento
			// 
			this.comboComportamiento.AccessibleName = "cmbComportamiento";
            this.comboComportamiento.Text = "Comportamiento";
            this.comboComportamiento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboComportamiento.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.comboComportamiento.FormattingEnabled = true;
			this.comboComportamiento.Items.AddRange(new object[] {
            "Aplicar al de menor precio",
            "Aplicar al de mayor precio"});
			this.comboComportamiento.Location = new System.Drawing.Point(77, 19);
			this.comboComportamiento.Name = "comboComportamiento";
			this.comboComportamiento.Size = new System.Drawing.Size(140, 21);
			this.comboComportamiento.TabIndex = 15;
			this.comboComportamiento.Tag = "Si es destacada aparecerá primera de la lista y si es no visible no se mostrará e" +
    "n la lista.";
            // 
            // lblAplicaAutomaticamente
            // 
            this.lblAplicaAutomaticamente.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblAplicaAutomaticamente.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblAplicaAutomaticamente.Location = new System.Drawing.Point(12, 16);
            this.lblAplicaAutomaticamente.Name = "lblAplicaAutomaticamente";
            this.lblAplicaAutomaticamente.Size = new System.Drawing.Size(59, 13);
            this.lblAplicaAutomaticamente.TabIndex = 72;
            this.lblAplicaAutomaticamente.Text = "Aplicar";
            // 
            //aplicaAutomaticamente
            //             
            this.aplicaAutomaticamente.Visible = true;
            this.aplicaAutomaticamente.Enabled = true;
            this.aplicaAutomaticamente.Text = "";
            this.aplicaAutomaticamente.Location = new System.Drawing.Point(50,14);
            this.aplicaAutomaticamente.Name = "aplicaAutomaticamente";
            this.aplicaAutomaticamente.Width = 18;
            this.aplicaAutomaticamente.TabIndex = 72;            
            // 
            // groupControl10
            // 
            this.groupControl10.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.groupControl10.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl10.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl10.Appearance.Options.UseBackColor = true;
			this.groupControl10.Appearance.Options.UseBorderColor = true;
			this.groupControl10.Appearance.Options.UseForeColor = true;
			this.groupControl10.AppearanceCaption.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl10.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(162)))), ((int)(((byte)(192)))));
			this.groupControl10.AppearanceCaption.Options.UseBorderColor = true;
			this.groupControl10.AppearanceCaption.Options.UseForeColor = true;
			this.groupControl10.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.groupControl10.Location = new System.Drawing.Point(850, 377);
			this.groupControl10.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.groupControl10.LookAndFeel.UseDefaultLookAndFeel = false;
			this.groupControl10.Name = "groupControl10";
			this.groupControl10.Size = new System.Drawing.Size(143, 10);
			this.groupControl10.TabIndex = 90;
			// 
			// ControlPromociones
			// 
			this.Appearance.BackColor = System.Drawing.Color.White;
			this.Appearance.Options.UseBackColor = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupControl2);
			this.Controls.Add(this.groupControl9);
            this.Controls.Add(this.groupControl11);
			this.Controls.Add(this.labelControl17);
			this.Controls.Add(this.groupControl3);
			this.Controls.Add(this.groupControl7);
			this.Controls.Add(this.groupControl8);
			this.Controls.Add(this.groupControl6);
			this.Controls.Add(this.groupControl5);
			this.Controls.Add(this.groupControl4);
			this.Controls.Add(this.lstBeneficios);
			this.Controls.Add(this.labelControl13);
			this.Controls.Add(this.groupControl1);
			this.Controls.Add(this.labelControl12);
			this.Controls.Add(this.labelControl11);
			this.Controls.Add(this.labelControl10);
			this.Controls.Add(this.labelControl9);
			this.Controls.Add(this.treeParticipantes);
			this.Controls.Add(this.treeFiltroParticipantes);
			this.Controls.Add(this.btnFiltroBeneficio);
			this.Controls.Add(this.btnFiltroCondicion);
			this.Controls.Add(this.flowLayoutPanel2);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.groupControl10);
			this.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
			this.LookAndFeel.UseDefaultLookAndFeel = false;
			this.Name = "ControlPromociones";
			this.Size = new System.Drawing.Size(998, 474);
			this.Load += new System.EventHandler(this.ControlPromociones_Load);
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.treeParticipantes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.treeFiltroParticipantes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
			this.groupControl1.ResumeLayout(false);
			this.groupControl1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaDesde.Properties.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaDesde.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties.VistaTimeProperties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dtFechaHasta.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
			this.groupControl2.ResumeLayout(false);
			this.groupControl2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.timHasta.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timDesde.Properties)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
			this.groupControl3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.weekDias)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.groupControl9)).EndInit();
			this.groupControl9.ResumeLayout(false);
			this.groupControl9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl11)).EndInit();
            this.groupControl11.ResumeLayout(false);
            this.groupControl11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl10)).EndInit();
            this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

    }
}
