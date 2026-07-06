using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace ZooLogicSA.Promociones.UI
{
    public partial class MaskPromocion : UserControl
    {
        #region Getters&Setters

        private bool _decimal;
        private int inicioParteEntera;
        private int inicioParteDecimal;
        private int inicioLabel;
        private int finEditable;
        private int LargoParteEntera;
        private int cantDecimales;

        public bool Decimal
        {
            get { return this._decimal; }
            set { this._decimal = value; }
        }

        public int InicioParteEntera
        {
            get { return this.inicioParteEntera; }
            set { this.inicioParteEntera = value; }
        }

        public int InicioParteDecimal
        {
            get { return this.inicioParteDecimal; }
            set { this.inicioParteDecimal = value; }
        }

        public int InicioLabel
        {
            get { return this.inicioLabel; }
            set { this.inicioLabel = value; }
        }

        public int FinEditable
        {
            get { return this.finEditable; }
            set { this.finEditable = value; }
        }

        public int largoParteEntera
        {
            get { return this.LargoParteEntera; }
            set { this.LargoParteEntera = value; }
        }

        #endregion

        private string mascara;

        public string Mascara
        {
            get { return this.mascara; }
            set
            {
                this.mascara = value;

                this.InicializarMascara( value );
            }
        }

        private void InicializarMascara( string value )
        {
            string cadena = value.Replace( @"\", "" );

            if ( cadena.Contains( '.' ) )
            {
                this._decimal = true;
                this.cantDecimales = cadena.LastIndexOf( "9" ) - cadena.IndexOf( "." );
            }
            else
            {
                this._decimal = false;
                this.cantDecimales = 0;
            }

            this.maskedTextBox1.Mask = value;
            this.maskedTextBox1.InsertKeyMode = InsertKeyMode.Overwrite;

            this.inicioLabel = 0;
            if ( cadena.Contains( "9" ) )
            {
                this.finEditable = cadena.LastIndexOf( "9" );
                this.inicioParteEntera = cadena.IndexOf( "9" );
            }
            if ( cadena.Contains( "." ) )
                this.inicioParteDecimal = cadena.IndexOf( "." ) + 1;
            this.largoParteEntera = this.inicioParteDecimal - this.inicioParteEntera - 1;

        }

        public MaskPromocion()
        {
            InitializeComponent();

            this.maskedTextBox1.MouseUp += maskedTextBox1_MouseUp;
            this.maskedTextBox1.MouseDown += maskedTextBox1_MouseUp;
            this.maskedTextBox1.KeyUp += maskedTextBox1_KeyUp;
            this.maskedTextBox1.KeyDown += maskedTextBox1_KeyUp;
        }

        void maskedTextBox1_KeyUp( object sender, KeyEventArgs e )
        {
            RaiseSelectionChanged();
        }

        void maskedTextBox1_MouseUp( object sender, MouseEventArgs e )
        {
            RaiseSelectionChanged();
        }

        private void RaiseSelectionChanged()
        {
            if ( this.maskedTextBox1.SelectionStart <= this.inicioParteEntera )
            {
                this.maskedTextBox1.SelectionStart = this.inicioParteEntera;
                this.maskedTextBox1.SelectionLength = 0;
            }

            if ( this.maskedTextBox1.SelectionStart >= this.inicioParteDecimal + this.cantDecimales )
            {
                this.maskedTextBox1.SelectionStart = this.inicioParteDecimal + this.cantDecimales;
                this.maskedTextBox1.SelectionLength = 0;
            }
        }

        private void maskedTextBox1_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( this._decimal && this.maskedTextBox1.SelectionStart < this.inicioParteDecimal && ( e.KeyChar == '.' || e.KeyChar == ',' ) )
            {
                string texto = this.maskedTextBox1.Text.Substring( inicioParteEntera, this.maskedTextBox1.SelectionStart - inicioParteEntera );

                texto = texto.Trim().PadLeft( largoParteEntera );

                texto = this.maskedTextBox1.Text.Substring( 0, inicioParteEntera ) + texto + this.maskedTextBox1.Text.Substring( inicioParteDecimal - 1 );

                this.maskedTextBox1.Text = texto;

                this.maskedTextBox1.SelectionStart = this.inicioParteDecimal;
                this.maskedTextBox1.SelectionLength = 1;
            }
            else
            {
                base.OnKeyPress( e );
            }
        }

        public string ObtenerIngreso()
        {
            string ingreso = "";

            if (!( this._decimal && this.maskedTextBox1.Text.Replace(",","").Replace(".","").Trim() == "") )
            {
                //string culture = this.maskedTextBox1.Culture.Name;
                string valorIngresado;

                int largo = this.maskedTextBox1.Text.Length;
                if ( finEditable >= largo )
                {
                    valorIngresado = this.maskedTextBox1.Text.Substring( inicioParteEntera ).Trim().Replace( " ", "" );
                }
                else
                {
                    valorIngresado = this.maskedTextBox1.Text.Substring( inicioParteEntera, finEditable - inicioParteEntera + 1 ).Trim().Replace( " ", "" );
                }

                if ( valorIngresado.Replace( ",", "" ).Replace( ".", "" ).Trim() == "" )
                {
                    valorIngresado = "0";
                }

                // Este Convert.ToDecimal va sin CultureInfo porque la cadena debe venir
                // (por el funcionamiento del control de DevExpress)
                // en el formato de la configuracion del sistema
                Decimal valorDecimal;
                if (!String.Equals(Thread.CurrentThread.CurrentCulture, System.Globalization.CultureInfo.CurrentUICulture)) // Por si otra dll cambió el Thread.CurrentThread.CurrentCulture
                {
                    valorDecimal = Convert.ToDecimal(valorIngresado, new CultureInfo(System.Globalization.CultureInfo.CurrentUICulture.ToString()));
                }
                else
                {
                    valorDecimal = Convert.ToDecimal(valorIngresado);
                }
                
                ingreso = valorDecimal.ToString( new CultureInfo( "en-US" ) );
            }
            return ingreso;
        }

        public void SetearValor( string valor )
        {
            if ( String.IsNullOrEmpty( valor ) )
            {
                valor = "0";
            }

            //Decimal valorDecimal = Convert.ToDecimal(valor );
            //string culture = this.maskedTextBox1.Culture.Name;
            //string culture = "en-US";
            Decimal valorDecimal = Convert.ToDecimal( valor, new CultureInfo( "en-US" ) );

            string masc = new string( '#', this.largoParteEntera - 1 );
            masc += "0";
            masc += ".";
            masc += new string( '0', this.cantDecimales );

            // El ToString() va sin CultureInfo porque la cadena debe venir
            // (por el funcionamiento del control de DevExpress)
            // en el formato de la configuracion del sistema
            valor = valorDecimal.ToString( masc ).PadLeft( this.largoParteEntera + this.cantDecimales + 1 );

            this.maskedTextBox1.Text = valor.Trim().PadLeft( finEditable - inicioParteEntera + 1, ' ' );
        }
    }
}
