using System.Windows.Forms;

namespace ZooLogicSA.Promociones.UI.Clases
{
	public class DragObject : Control{
        private Label label1;
		private string valor;
        private string columna;
        private string participante;

		public DragObject( string Valor, string Columna, string Participante ) {
			this.valor = Valor;
            this.columna = Columna;
            this.participante = Participante;
		}

        public string ObtenerData() 
        {
            return this.valor;
        }

        public string ObtenerDataColumna()
        {
            return this.columna;
        }

        public string ObtenerDataParticipante()
        {
            return this.participante;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point( 0, 0 );
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size( 100, 23 );
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.ResumeLayout( false );
        }
	}
}

