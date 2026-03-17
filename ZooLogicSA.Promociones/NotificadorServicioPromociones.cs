using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones
{
    public class NotificadorServicioPromociones
    {
        public List<IObservadorServicioPromociones> observadores = new List<IObservadorServicioPromociones>();

        #region Informar a Observadores
        public void PresentarPromocionesAplicables( List<InformacionPromocion> info )
        {
            foreach ( IObservadorServicioPromociones observador in this.observadores )
            {
                observador.PresentarPromocionesAplicables( info );
            }
        }

        public void ProcesarError( Exception ex )
        {
            foreach ( IObservadorServicioPromociones observador in this.observadores )
            {
                observador.ProcesarError( ex );
            }
        }

        public void ProcesarErrorEnPromocion( InformacionPromocionIncumplida idPromocion, Exception ex )
        {
            foreach ( IObservadorServicioPromociones observador in this.observadores )
            {
                observador.ProcesarErrorEnPromocion( idPromocion, ex );
            }
        }

        public void InformarMensaje( string mensaje )
        {
            //foreach ( IObservadorServicioPromociones observador in this.observadores )
            //{
            //    observador.InformarMensaje( mensaje );
            //}
        }

        public void InformacionDebug( string mensaje )
        {
            foreach ( IObservadorServicioPromociones observador in this.observadores )
            {
                observador.InformarDebug( mensaje );
            }
        }
        #endregion

        public void AgegarObservador( IObservadorServicioPromociones observador )
        {
			lock ( this.observadores )
			{
				this.observadores.RemoveAll( x => x.GetType().Equals( observador.GetType() ) );
				this.observadores.Add( observador );
			}
		}

        public void QuitarObservador( IObservadorServicioPromociones observador )
        {
			lock ( this.observadores )
			{
				this.observadores.Remove( observador );
			}
        }

        public void QuitarTodosLosObservadores()
        {
			lock ( this.observadores )
			{
				this.observadores.Clear();
			}
        }

        public void ServicioApagado()
        {
            foreach ( IObservadorServicioPromociones observador in this.observadores )
            {
                observador.InformarApagado();
            }
        }
	}
}