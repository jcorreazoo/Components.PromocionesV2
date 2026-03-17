using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZooLogicSA.Core.Escalares;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Core.Visual;
using System.IO;
using ZooLogicSA.Core.COM;
using System.Linq;

namespace ZooLogicSA.Promociones.Asistente
{
    public class FactoryAsistente : IObservadorServicioPromociones
    {

        public FrmAsistente FrmAsistente { get; set; }
        public delegate void EventoCerrandoElAsistenteEventHandler( object source, EventArgs e );
        private Thread hilo;
        private ServicioEvaluacionPromociones servicio;
        private object manager;
        private ParametrosAsistente ultimosParametros;

        public void EjecutarFormulario( ParametrosAsistente parametros )
        {
            ParameterizedThreadStart ts = new ParameterizedThreadStart( this.EjecutarFormularioEnOtroHilo );
            this.hilo = new Thread( ts );
            this.hilo.Start( parametros );
        }

        private void EjecutarFormularioEnOtroHilo( object parametros )
        {
            SetearAspecto creadorAspecto = new SetearAspecto();
            Aspecto aspecto = creadorAspecto.CrearAspecto( ((ParametrosAsistente)parametros).InformacionAplicacion );
            this.FrmAsistente = new FrmAsistente( (ParametrosAsistente)parametros );
            this.ultimosParametros = (ParametrosAsistente)parametros;
            this.FrmAsistente.FormClosing += this.EventoCerrandoElAsistente;

			this.FrmAsistente.EventoPromocionSeleccionadaParaAplicar += FrmAsistente_EventoPromocionSeleccionadaParaAplicar;
            this.FrmAsistente.ShowDialog();
        }

		void FrmAsistente_EventoPromocionSeleccionadaParaAplicar( string id )
		{
			try
			{
				HerramientasCom.EjecutarMetodo( this.manager, "AplicarPromocion", id );
			}
			catch ( Exception ex )
			{
				this.FrmAsistente.MostrarError( ex.ToString() );
				HerramientasCom.EjecutarMetodo( this.manager, "ProcesarErrorAsistente", ex );
			}
		}

        public void InyectarServicioAsistente( ServicioEvaluacionPromociones servicio )
        {
            this.servicio = servicio;
        }

        public void InformarApagado()
        {
           this.Limpiar();
            
        }

        public void Limpiar()
        {
            this.FrmAsistente.LimpiarGrilla();

        }

        public void EventoCerrandoElAsistente( object sender, EventArgs e )
        {
            if (!this.servicio.aplicaPromocionesAutomaticas)
            {
                this.servicio.DeshabilitarSerializacionPorHilos();
                this.servicio.QuitarObservador(this);
            }            
        }

        public void TraerAlFrente()
        {
            try
            {
                this.FrmAsistente.BringToFront();
            }
            catch (NullReferenceException ex)
            {
                this.FrmAsistente = new FrmAsistente((ParametrosAsistente)this.ultimosParametros);
                this.FrmAsistente.FormClosing += this.EventoCerrandoElAsistente;
                this.FrmAsistente.EventoPromocionSeleccionadaParaAplicar += FrmAsistente_EventoPromocionSeleccionadaParaAplicar;
                this.FrmAsistente.BringToFront();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void TraerAlFrente( ParametrosAsistente param )
        {
            if ( this.hilo != null && this.hilo.IsAlive )
            { }
            else
                this.EjecutarFormulario( param );

            while ( this.FrmAsistente == null )
            {
            }
            this.FrmAsistente.BringToFront();
        }

        public void Dispose()
        {
            this.FrmAsistente.Close();
        }

        public void InyectarManager( object manager )
        {
            this.manager = manager;
        }
        
        #region IObservadorServicioPromociones Members

        public void PresentarPromocionesAplicables( List<InformacionPromocion> info )
        {
			this.InformarDebug( "Total promos: " + info.Count.ToString() );
            this.FrmAsistente.AgregarInformacionPromociones( info );
		}

        public void ProcesarError( Exception ex )
        {
            this.FrmAsistente.MostrarError( ex.ToString() );
            HerramientasCom.EjecutarMetodo( this.manager, "ProcesarErrorAsistente", ex );
        }

        public void ProcesarErrorEnPromocion( InformacionPromocionIncumplida idPromocion, Exception ex )
        {
            this.FrmAsistente.MostrarErrorEnPromocion( idPromocion, ex.ToString() );
            HerramientasCom.EjecutarMetodo( this.manager, "ProcesarErrorAsistente", ex );
        }

        public void InformarDebug( string mensaje )
        {
			//lock ( this )
			//{
			//	File.AppendAllText( @"d:\infoDebugAsistente", mensaje + "\r\n" );
			//}
        }

        #endregion
    }
}
