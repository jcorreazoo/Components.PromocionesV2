using System.Drawing;
using System.Windows.Forms;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class ControlBotonMaximizar
    {
        private Control control;
        private Control padre;
        private Control formulario;
        private eEstados estado;

        private Point posicionAnterior;
        private Size tamanoAnterior;
        private Control controlAnterior;

        public enum eEstados {
            Minimizado = 0,
            Maximizado = 1
        };

        public Control Boton
        {
            get { return this.control; }
        }

        public ControlBotonMaximizar( Control control, Control padre, Control formulario )
        {
            this.control = control;
            this.padre = padre;
            this.formulario = formulario;
            this.estado = eEstados.Minimizado;
        }

        public void AccionarBoton()        
        {
            if ( this.ObtenerEstado() == eEstados.Minimizado )
            {
                this.MaximizarControl();
                this.SetearEstado( eEstados.Maximizado );
            }
            else
            {
                this.MinimizarControl();
                this.SetearEstado( eEstados.Minimizado );
            }

        }

        public void MaximizarControl()
        {
            this.GuardarTamanoYPosicionAnterior();
            Size tamano = new Size();
            this.padre.Parent = this.formulario;
            this.padre.Location = new Point( 0, 0 );
            tamano.Width = this.formulario.Size.Width;
            tamano.Height = this.formulario.Size.Height;
            this.padre.Size = tamano;
            this.padre.BringToFront();
        }

        public void MinimizarControl()
        {
            this.RestaurarTamanoYPosicionAnterior();
        }

        private void GuardarTamanoYPosicionAnterior()
        {
            this.posicionAnterior = this.padre.Location;
            this.tamanoAnterior = this.padre.Size;
            this.controlAnterior = this.padre.Parent;
        }

        private void RestaurarTamanoYPosicionAnterior()
        {
            this.padre.Location = this.posicionAnterior;
            this.padre.Size = this.tamanoAnterior;
            this.padre.Parent = this.controlAnterior;
            this.padre.Dock = DockStyle.None;
            this.padre.BringToFront();
        }

        public void SetearEstado( eEstados estado )
        {
            this.estado = estado;
        }

        public eEstados ObtenerEstado()
        {
            return this.estado;
        }

        public void UbicarControl()
        {
            Point punto = new Point();
            control.Parent = this.padre;
            punto.Y = this.padre.Height - control.Height - 18;
            punto.X = this.padre.Width - control.Width - 20;
            control.Location = punto;
        }

        public void SetearControl( Point posicionMouse )
        {
            this.UbicarControl();
            this.control.Visible = this.MouseSobreControl( this.padre, posicionMouse );
            this.SetearIndiceImagen( posicionMouse );
        }

        private bool MouseSobreControl( Control control, Point posicionMouse )
        {
            bool lRetorno = false;
            Point ptDesde;
            Point ptHasta;
            ptDesde = control.PointToScreen( new Point( 0, 0 ) );
            ptHasta = control.PointToScreen( new Point( control.Size.Width, control.Size.Height ) );
            if ( posicionMouse.X >= ptDesde.X && posicionMouse.X <= ptHasta.X &&
                posicionMouse.Y >= ptDesde.Y && posicionMouse.Y <= ptHasta.Y )
                lRetorno = true;
            return lRetorno;
        }

        private void SetearIndiceImagen( Point posicionMouse )
        { 
            int indice = 0;
            if ( this.MouseSobreControl( this.control, posicionMouse ) )
                if ( this.ObtenerEstado() == eEstados.Maximizado )
                    indice = 2;
                else
                    indice = 0;
            else
                if ( this.ObtenerEstado() == eEstados.Maximizado )
                    indice = 3;
                else
                    indice = 1;
            ((Button)this.control).ImageIndex = indice;

        }

        private bool TieneScrollHorizontal()        
        {
            bool lRetorno = false;

            if ( this.padre.HasChildren )
            {
                foreach ( Control control in this.padre.Controls )
                {
                    if ( control.GetType().Name == typeof( VScrollBar ).Name && control.Visible )
                    {
                        control.Visible = false;
                        lRetorno = true;
                        break;
                    }
                }
            }

            return lRetorno;
        }

        public void SetearVisibilidad( bool mostrar )
        {
            this.control.Visible = mostrar;
            if ( !mostrar )
            {
                this.estado = eEstados.Minimizado;
            }
        }

        private bool TieneScrollVertical()
        {
            bool lRetorno = false;

            if ( this.padre.HasChildren )
            {
                foreach ( Control control in this.padre.Controls )
                {
                    if ( control.GetType().Name == typeof( VScrollBar ).Name && control.Visible )
                    {
                        control.Visible = false;
                        lRetorno = true;
                        break;
                    }
                }
            }

            return lRetorno;
        }
    }
}
