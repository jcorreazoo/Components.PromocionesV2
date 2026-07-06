namespace ZooLogicSA.Promociones.UI
{
    partial class MaskPromocion
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // maskedTextBox1
            // 
            this.maskedTextBox1.AccessibleName = "mskPromocionEdit";
            this.maskedTextBox1.Location = new System.Drawing.Point(2, 2);
            this.maskedTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.maskedTextBox1.Name = "maskedTextBox1";
            this.maskedTextBox1.Size = new System.Drawing.Size(369, 20);
            this.maskedTextBox1.TabIndex = 0;
            this.maskedTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedTextBox1_KeyPress);
            // 
            // MaskPromocion
            // 
            this.Controls.Add(this.maskedTextBox1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MaskPromocion";
            this.Size = new System.Drawing.Size(374, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox maskedTextBox1;
    }
}
