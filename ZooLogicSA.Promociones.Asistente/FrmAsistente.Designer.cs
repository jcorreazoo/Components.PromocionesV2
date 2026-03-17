namespace ZooLogicSA.Promociones.Asistente
{
    partial class FrmAsistente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.grillaPromo = new DevExpress.XtraGrid.GridControl();
			this.promocionesEstadoBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
			this.colId = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colId = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colDescripcion = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
			this.layoutViewField_colDescripcion = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colBeneficio = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colBeneficio = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colEstado = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colEstado = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colEstadoDibujo = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colEstadoDibujo = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colLeyendaCliente = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colLeyendaCliente = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colFaltante = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colFaltante = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.colDestacada = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
			this.layoutViewField_colDestacada = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
			this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
			this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
			this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
			this.zooFormExtender1 = new ZooLogicSA.Core.Formularios.Extendedores.ZooFormExtender();
			((System.ComponentModel.ISupportInitialize)(this.grillaPromo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.promocionesEstadoBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colId)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colDescripcion)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colBeneficio)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colEstado)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colEstadoDibujo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colLeyendaCliente)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colFaltante)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colDestacada)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
			this.SuspendLayout();
			// 
			// grillaPromo
			// 
			this.grillaPromo.DataSource = this.promocionesEstadoBindingSource;
			this.grillaPromo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grillaPromo.Location = new System.Drawing.Point(0, 0);
			this.grillaPromo.MainView = this.layoutView1;
			this.grillaPromo.Name = "grillaPromo";
			this.grillaPromo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemMemoExEdit1});
			this.grillaPromo.Size = new System.Drawing.Size(299, 622);
			this.grillaPromo.TabIndex = 7;
			this.grillaPromo.ToolTipController = this.toolTipController1;
			this.grillaPromo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.layoutView1});
			// 
			// promocionesEstadoBindingSource
			// 
			this.promocionesEstadoBindingSource.DataSource = typeof(ZooLogicSA.Promociones.Asistente.PromocionesEstado);
			// 
			// layoutView1
			// 
			this.layoutView1.Appearance.CardCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
			this.layoutView1.Appearance.CardCaption.Options.UseBackColor = true;
			this.layoutView1.Appearance.CardCaption.Options.UseFont = true;
			this.layoutView1.Appearance.CardCaption.Options.UseTextOptions = true;
			this.layoutView1.Appearance.CardCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.layoutView1.Appearance.CardCaption.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.layoutView1.Appearance.CardCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.layoutView1.Appearance.FieldCaption.Options.UseTextOptions = true;
			this.layoutView1.Appearance.FieldCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
			this.layoutView1.Appearance.FieldValue.Options.UseTextOptions = true;
			this.layoutView1.Appearance.FieldValue.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
			this.layoutView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.layoutView1.CardCaptionFormat = "Código: {2} -  Beneficio: $ {4}";
			this.layoutView1.CardMinSize = new System.Drawing.Size(150, 93);
			this.layoutView1.CardVertInterval = 0;
			this.layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.colId,
            this.colDescripcion,
            this.colBeneficio,
            this.colEstado,
            this.colEstadoDibujo,
            this.colLeyendaCliente,
            this.colFaltante,
            this.colDestacada});
			this.layoutView1.GridControl = this.grillaPromo;
			this.layoutView1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_colId,
            this.layoutViewField_colBeneficio,
            this.layoutViewField_colEstado,
            this.layoutViewField_colEstadoDibujo,
            this.layoutViewField_colDestacada,
            this.layoutViewField_colLeyendaCliente});
			this.layoutView1.Name = "layoutView1";
			this.layoutView1.OptionsBehavior.Editable = false;
			this.layoutView1.OptionsBehavior.ReadOnly = true;
			this.layoutView1.OptionsBehavior.ScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
			this.layoutView1.OptionsCustomization.AllowFilter = false;
			this.layoutView1.OptionsCustomization.AllowSort = false;
			this.layoutView1.OptionsFilter.AllowFilterEditor = false;
			this.layoutView1.OptionsHeaderPanel.EnableCustomizeButton = false;
			this.layoutView1.OptionsHeaderPanel.ShowCustomizeButton = false;
			this.layoutView1.OptionsItemText.AlignMode = DevExpress.XtraGrid.Views.Layout.FieldTextAlignMode.CustomSize;
			this.layoutView1.OptionsItemText.TextToControlDistance = 4;
			this.layoutView1.OptionsLayout.Columns.AddNewColumns = false;
			this.layoutView1.OptionsLayout.StoreAppearance = true;
			this.layoutView1.OptionsMultiRecordMode.StretchCardToViewHeight = true;
			this.layoutView1.OptionsMultiRecordMode.StretchCardToViewWidth = true;
			this.layoutView1.OptionsSelection.MultiSelect = true;
			this.layoutView1.OptionsSingleRecordMode.StretchCardToViewHeight = true;
			this.layoutView1.OptionsSingleRecordMode.StretchCardToViewWidth = true;
			this.layoutView1.OptionsView.CardArrangeRule = DevExpress.XtraGrid.Views.Layout.LayoutCardArrangeRule.AllowPartialCards;
			this.layoutView1.OptionsView.CardsAlignment = DevExpress.XtraGrid.Views.Layout.CardsAlignment.Near;
			this.layoutView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
			this.layoutView1.OptionsView.ShowHeaderPanel = false;
			this.layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.Column;
			this.layoutView1.PaintStyleName = "MixedXP";
			this.layoutView1.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
			this.layoutView1.TemplateCard = this.layoutViewCard1;
			this.layoutView1.CustomCardCaptionImage += new DevExpress.XtraGrid.Views.Layout.Events.LayoutViewCardCaptionImageEventHandler(this.SetearImagenCabecera);
			this.layoutView1.CustomCardStyle += new DevExpress.XtraGrid.Views.Layout.Events.LayoutViewCardStyleEventHandler(this.layoutView1_CustomCardStyle);
			this.layoutView1.DoubleClick += new System.EventHandler(this.layoutView1_DoubleClick);
			// 
			// colId
			// 
			this.colId.Caption = "Código";
			this.colId.FieldName = "Id";
			this.colId.LayoutViewField = this.layoutViewField_colId;
			this.colId.Name = "colId";
			// 
			// layoutViewField_colId
			// 
			this.layoutViewField_colId.EditorPreferredWidth = 128;
			this.layoutViewField_colId.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colId.Name = "layoutViewField_colId";
			this.layoutViewField_colId.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colId.TextSize = new System.Drawing.Size(67, 20);
			this.layoutViewField_colId.TextToControlDistance = 5;
			// 
			// colDescripcion
			// 
			this.colDescripcion.Caption = "Descripción";
			this.colDescripcion.ColumnEdit = this.repositoryItemMemoEdit1;
			this.colDescripcion.FieldName = "Descripcion";
			this.colDescripcion.LayoutViewField = this.layoutViewField_colDescripcion;
			this.colDescripcion.Name = "colDescripcion";
			// 
			// repositoryItemMemoEdit1
			// 
			this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
			// 
			// layoutViewField_colDescripcion
			// 
			this.layoutViewField_colDescripcion.EditorPreferredWidth = 331;
			this.layoutViewField_colDescripcion.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colDescripcion.Name = "layoutViewField_colDescripcion";
			this.layoutViewField_colDescripcion.Size = new System.Drawing.Size(397, 22);
			this.layoutViewField_colDescripcion.TextSize = new System.Drawing.Size(58, 13);
			// 
			// colBeneficio
			// 
			this.colBeneficio.Caption = "Beneficio";
			this.colBeneficio.FieldName = "Beneficio";
			this.colBeneficio.LayoutViewField = this.layoutViewField_colBeneficio;
			this.colBeneficio.Name = "colBeneficio";
			// 
			// layoutViewField_colBeneficio
			// 
			this.layoutViewField_colBeneficio.EditorPreferredWidth = 128;
			this.layoutViewField_colBeneficio.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colBeneficio.Name = "layoutViewField_colBeneficio";
			this.layoutViewField_colBeneficio.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colBeneficio.TextSize = new System.Drawing.Size(67, 20);
			this.layoutViewField_colBeneficio.TextToControlDistance = 5;
			// 
			// colEstado
			// 
			this.colEstado.Caption = "Cumplimiento";
			this.colEstado.FieldName = "Estado";
			this.colEstado.LayoutViewField = this.layoutViewField_colEstado;
			this.colEstado.Name = "colEstado";
			// 
			// layoutViewField_colEstado
			// 
			this.layoutViewField_colEstado.EditorPreferredWidth = 128;
			this.layoutViewField_colEstado.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colEstado.Name = "layoutViewField_colEstado";
			this.layoutViewField_colEstado.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colEstado.TextSize = new System.Drawing.Size(67, 20);
			this.layoutViewField_colEstado.TextToControlDistance = 5;
			// 
			// colEstadoDibujo
			// 
			this.colEstadoDibujo.Caption = "Estado";
			this.colEstadoDibujo.FieldName = "EstadoDibujo";
			this.colEstadoDibujo.LayoutViewField = this.layoutViewField_colEstadoDibujo;
			this.colEstadoDibujo.Name = "colEstadoDibujo";
			// 
			// layoutViewField_colEstadoDibujo
			// 
			this.layoutViewField_colEstadoDibujo.EditorPreferredWidth = 128;
			this.layoutViewField_colEstadoDibujo.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colEstadoDibujo.Name = "layoutViewField_colEstadoDibujo";
			this.layoutViewField_colEstadoDibujo.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colEstadoDibujo.TextSize = new System.Drawing.Size(67, 20);
			this.layoutViewField_colEstadoDibujo.TextToControlDistance = 5;
			// 
			// colLeyendaCliente
			// 
			this.colLeyendaCliente.Caption = "Mensaje";
			this.colLeyendaCliente.ColumnEdit = this.repositoryItemMemoEdit1;
			this.colLeyendaCliente.FieldName = "LeyendaCliente";
			this.colLeyendaCliente.LayoutViewField = this.layoutViewField_colLeyendaCliente;
			this.colLeyendaCliente.Name = "colLeyendaCliente";
			// 
			// layoutViewField_colLeyendaCliente
			// 
			this.layoutViewField_colLeyendaCliente.EditorPreferredWidth = 272;
			this.layoutViewField_colLeyendaCliente.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colLeyendaCliente.Name = "layoutViewField_colLeyendaCliente";
			this.layoutViewField_colLeyendaCliente.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colLeyendaCliente.TextSize = new System.Drawing.Size(58, 13);
			this.layoutViewField_colLeyendaCliente.TextToControlDistance = 5;
			// 
			// colFaltante
			// 
			this.colFaltante.Caption = "Faltante";
			this.colFaltante.ColumnEdit = this.repositoryItemMemoEdit1;
			this.colFaltante.FieldName = "Faltante";
			this.colFaltante.LayoutViewField = this.layoutViewField_colFaltante;
			this.colFaltante.Name = "colFaltante";
			// 
			// layoutViewField_colFaltante
			// 
			this.layoutViewField_colFaltante.EditorPreferredWidth = 331;
			this.layoutViewField_colFaltante.Location = new System.Drawing.Point(0, 22);
			this.layoutViewField_colFaltante.Name = "layoutViewField_colFaltante";
			this.layoutViewField_colFaltante.Size = new System.Drawing.Size(397, 21);
			this.layoutViewField_colFaltante.TextSize = new System.Drawing.Size(58, 13);
			// 
			// colDestacada
			// 
			this.colDestacada.Caption = "Destacada";
			this.colDestacada.FieldName = "Destacada";
			this.colDestacada.LayoutViewField = this.layoutViewField_colDestacada;
			this.colDestacada.Name = "colDestacada";
			// 
			// layoutViewField_colDestacada
			// 
			this.layoutViewField_colDestacada.EditorPreferredWidth = 128;
			this.layoutViewField_colDestacada.Location = new System.Drawing.Point(0, 0);
			this.layoutViewField_colDestacada.Name = "layoutViewField_colDestacada";
			this.layoutViewField_colDestacada.Size = new System.Drawing.Size(344, 60);
			this.layoutViewField_colDestacada.TextSize = new System.Drawing.Size(67, 20);
			this.layoutViewField_colDestacada.TextToControlDistance = 5;
			// 
			// layoutViewCard1
			// 
			this.layoutViewCard1.CustomizationFormText = "TemplateCard";
			this.layoutViewCard1.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
			this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_colDescripcion,
            this.layoutViewField_colFaltante});
			this.layoutViewCard1.Name = "layoutViewTemplateCard";
			this.layoutViewCard1.OptionsItemText.TextAlignMode = DevExpress.XtraLayout.TextAlignModeGroup.CustomSize;
			this.layoutViewCard1.OptionsItemText.TextToControlDistance = 4;
			this.layoutViewCard1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
			this.layoutViewCard1.Text = "TemplateCard";
			// 
			// repositoryItemMemoExEdit1
			// 
			this.repositoryItemMemoExEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown)});
			this.repositoryItemMemoExEdit1.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
			this.repositoryItemMemoExEdit1.Name = "repositoryItemMemoExEdit1";
			this.repositoryItemMemoExEdit1.NullValuePromptShowForEmptyValue = true;
			this.repositoryItemMemoExEdit1.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.DoubleClick;
			this.repositoryItemMemoExEdit1.ShowIcon = false;
			// 
			// toolTipController1
			// 
			this.toolTipController1.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController1_GetActiveObjectInfo);
			// 
			// zooFormExtender1
			// 
			this.zooFormExtender1.Dock = System.Windows.Forms.DockStyle.Right;
			this.zooFormExtender1.Location = new System.Drawing.Point(345, 7);
			this.zooFormExtender1.Name = "zooFormExtender1";
			this.zooFormExtender1.Size = new System.Drawing.Size(58, 87);
			this.zooFormExtender1.TabIndex = 8;
			// 
			// FrmAsistente
			// 
			this.ClientSize = new System.Drawing.Size(299, 622);
			this.Controls.Add(this.zooFormExtender1);
			this.Controls.Add(this.grillaPromo);
			this.Name = "FrmAsistente";
			this.Text = "Asistente de promociones";
			this.Deactivate += new System.EventHandler(this.FrmAsistente_Deactivate);
			this.Load += new System.EventHandler(this.FrmAsistente_Load);
			((System.ComponentModel.ISupportInitialize)(this.grillaPromo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.promocionesEstadoBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colId)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colDescripcion)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colBeneficio)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colEstado)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colEstadoDibujo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colLeyendaCliente)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colFaltante)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewField_colDestacada)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        //public Core.Formularios.Extendedores.ZooFormExtender zooFormExtender1;
        public System.Windows.Forms.DataGridView CabeceraPromociones;
        private DevExpress.XtraGrid.GridControl grillaPromo;
        private System.Windows.Forms.BindingSource promocionesEstadoBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private Core.Formularios.Extendedores.ZooFormExtender zooFormExtender1;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colId;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colDescripcion;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colBeneficio;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colEstado;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colEstadoDibujo;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colLeyendaCliente;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colFaltante;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn colDestacada;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colId;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colDescripcion;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colBeneficio;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colEstado;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colEstadoDibujo;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colLeyendaCliente;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colFaltante;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_colDestacada;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private DevExpress.Utils.ToolTipController toolTipController1;

    }
}

