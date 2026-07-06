using DevExpress.DXperience.Demos;

namespace ZooLogicSA.Promociones.UI.Clases
{

    partial class ExpressionConditionsEditor {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ExpressionConditionsEditor ) );
            this.barManager1 = new DevExpress.XtraBars.BarManager( this.components );
            this.barMain = new DevExpress.XtraBars.Bar();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.txtRegla = new DevExpress.XtraBars.BarStaticItem();
            this.btnAceptar = new DevExpress.XtraBars.BarButtonItem();
            this.btnCancelar = new DevExpress.XtraBars.BarButtonItem();
            this.barAndDockingController1 = new DevExpress.XtraBars.BarAndDockingController( this.components );
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection( this.components );
            this.repositoryItemBorderLineStyle1 = new DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle();
            this.repositoryItemLineSpacing1 = new DevExpress.XtraRichEdit.Design.RepositoryItemLineSpacing();
            this.repositoryItemLineSpacing2 = new DevExpress.XtraRichEdit.Design.RepositoryItemLineSpacing();
            this.FormatItemList = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLineSpacing1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLineSpacing2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormatItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.AllowCustomization = false;
            this.barManager1.AllowQuickCustomization = false;
            this.barManager1.Bars.AddRange( new DevExpress.XtraBars.Bar[] {
            this.barMain} );
            this.barManager1.Controller = this.barAndDockingController1;
            this.barManager1.DockControls.Add( this.barDockControlTop );
            this.barManager1.DockControls.Add( this.barDockControlBottom );
            this.barManager1.DockControls.Add( this.barDockControlLeft );
            this.barManager1.DockControls.Add( this.barDockControlRight );
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection1;
            this.barManager1.Items.AddRange( new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.txtRegla,
            this.btnCancelar,
            this.btnAceptar} );
            this.barManager1.MainMenu = this.barMain;
            this.barManager1.MaxItemId = 20;
            this.barManager1.RepositoryItems.AddRange( new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemBorderLineStyle1,
            this.repositoryItemLineSpacing1,
            this.repositoryItemLineSpacing2} );
            this.barManager1.TransparentEditors = true;
            this.barManager1.UseAltKeyForMenu = false;
            this.barManager1.UseF10KeyForMenu = false;
            // 
            // barMain
            // 
            this.barMain.BarName = "Main menu";
            this.barMain.DockCol = 0;
            this.barMain.DockRow = 0;
            this.barMain.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMain.LinksPersistInfo.AddRange( new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.txtRegla),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAceptar, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancelar)} );
            this.barMain.OptionsBar.AllowQuickCustomization = false;
            this.barMain.OptionsBar.DisableClose = true;
            this.barMain.OptionsBar.DisableCustomization = true;
            this.barMain.OptionsBar.DrawDragBorder = false;
            this.barMain.OptionsBar.RotateWhenVertical = false;
            this.barMain.OptionsBar.UseWholeRow = true;
            this.barMain.Text = "Main menu";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.AccessibleName = "btnMas";
            this.barButtonItem1.Caption = "Agregar";
            this.barButtonItem1.Hint = "Agregar un nuevo participante";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.ImageIndex = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler( this.barButtonItem1_ItemClick );
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.AccessibleName = "btnMenos";
            this.barButtonItem2.Caption = "Eliminar";
            this.barButtonItem2.Enabled = false;
            this.barButtonItem2.Hint = "Eliminar el participante seleccionado";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.ImageIndex = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler( this.barButtonItem2_ItemClick );
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.AccessibleName = "btnEditar";
            this.barButtonItem3.Caption = "Editar";
            this.barButtonItem3.Enabled = false;
            this.barButtonItem3.Hint = "Editar el participante seleccionado";
            this.barButtonItem3.Id = 2;
            this.barButtonItem3.ItemAppearance.Normal.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)) );
            this.barButtonItem3.ItemAppearance.Normal.Options.UseFont = true;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler( this.barButtonItem3_ItemClick );
            // 
            // txtRegla
            // 
            this.txtRegla.AccessibleName = "btnParticipante";
            this.txtRegla.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.txtRegla.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
            this.txtRegla.Caption = "Participante";
            this.txtRegla.Id = 3;
            this.txtRegla.ItemAppearance.Normal.Font = new System.Drawing.Font( "Tahoma", 8.25F, System.Drawing.FontStyle.Bold );
            this.txtRegla.ItemAppearance.Normal.Options.UseFont = true;
            this.txtRegla.Name = "txtRegla";
            this.txtRegla.TextAlignment = System.Drawing.StringAlignment.Near;
            this.txtRegla.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.txtRegla.Width = 125;
            // 
            // btnAceptar
            // 
            this.btnAceptar.AccessibleName = "btnAceptar";
            this.btnAceptar.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnAceptar.Caption = "Aceptar";
            this.btnAceptar.Hint = "Aceptar las reglas";
            this.btnAceptar.Id = 7;
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler( this.btnAceptar_ItemClick );
            // 
            // btnCancelar
            // 
            this.btnCancelar.AccessibleName = "btnCancelar";
            this.btnCancelar.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnCancelar.Caption = "Cancelar";
            this.btnCancelar.Hint = "Cancelar";
            this.btnCancelar.Id = 4;
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnCancelar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler( this.btnCancelar_ItemClick );
            // 
            // barAndDockingController1
            // 
            this.barAndDockingController1.PaintStyleName = "Office2003";
            this.barAndDockingController1.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point( 0, 0 );
            this.barDockControlTop.Size = new System.Drawing.Size( 646, 24 );
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point( 0, 157 );
            this.barDockControlBottom.Size = new System.Drawing.Size( 646, 0 );
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point( 0, 24 );
            this.barDockControlLeft.Size = new System.Drawing.Size( 0, 133 );
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point( 646, 24 );
            this.barDockControlRight.Size = new System.Drawing.Size( 0, 133 );
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject( "imageCollection1.ImageStream" )));
            this.imageCollection1.Images.SetKeyName( 0, "add-16x16.png" );
            this.imageCollection1.Images.SetKeyName( 1, "delete-16x16.png" );
            // 
            // repositoryItemBorderLineStyle1
            // 
            this.repositoryItemBorderLineStyle1.AutoHeight = false;
            this.repositoryItemBorderLineStyle1.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.repositoryItemBorderLineStyle1.Control = null;
            this.repositoryItemBorderLineStyle1.Name = "repositoryItemBorderLineStyle1";
            this.repositoryItemBorderLineStyle1.UseParentBackground = true;
            // 
            // repositoryItemLineSpacing1
            // 
            this.repositoryItemLineSpacing1.AutoHeight = false;
            this.repositoryItemLineSpacing1.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.repositoryItemLineSpacing1.Name = "repositoryItemLineSpacing1";
            this.repositoryItemLineSpacing1.UseParentBackground = true;
            // 
            // repositoryItemLineSpacing2
            // 
            this.repositoryItemLineSpacing2.AutoHeight = false;
            this.repositoryItemLineSpacing2.Buttons.AddRange( new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)} );
            this.repositoryItemLineSpacing2.Name = "repositoryItemLineSpacing2";
            this.repositoryItemLineSpacing2.UseParentBackground = true;
            // 
            // FormatItemList
            // 
            this.FormatItemList.AccessibleName = "lista";
            this.FormatItemList.Appearance.Options.UseTextOptions = true;
            this.FormatItemList.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.FormatItemList.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.FormatItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormatItemList.HighlightedItemStyle = DevExpress.XtraEditors.HighlightStyle.Skinned;
            this.FormatItemList.ItemHeight = 16;
            this.FormatItemList.Location = new System.Drawing.Point( 0, 24 );
            this.FormatItemList.Name = "FormatItemList";
            this.FormatItemList.Size = new System.Drawing.Size( 646, 133 );
            this.FormatItemList.TabIndex = 4;
            this.FormatItemList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler( this.FormatItemList_MouseDoubleClick );
            // 
            // ExpressionConditionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.FormatItemList );
            this.Controls.Add( this.barDockControlLeft );
            this.Controls.Add( this.barDockControlRight );
            this.Controls.Add( this.barDockControlBottom );
            this.Controls.Add( this.barDockControlTop );
            this.Name = "ExpressionConditionsEditor";
            this.Size = new System.Drawing.Size( 646, 157 );
            this.VisibleChanged += new System.EventHandler( this.ExpressionConditionsEditor_VisibleChanged );
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemBorderLineStyle1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLineSpacing1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLineSpacing2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FormatItemList)).EndInit();
            this.ResumeLayout( false );

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar barMain;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.Utils.ImageCollection imageCollection1;
        public DevExpress.XtraEditors.ListBoxControl FormatItemList;
        private DevExpress.XtraBars.BarStaticItem txtRegla;
        private DevExpress.XtraBars.BarButtonItem btnCancelar;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController1;
        private DevExpress.XtraBars.BarButtonItem btnAceptar;
        private DevExpress.XtraRichEdit.Forms.Design.RepositoryItemBorderLineStyle repositoryItemBorderLineStyle1;
        private DevExpress.XtraRichEdit.Design.RepositoryItemLineSpacing repositoryItemLineSpacing1;
        private DevExpress.XtraRichEdit.Design.RepositoryItemLineSpacing repositoryItemLineSpacing2;
    }
}
