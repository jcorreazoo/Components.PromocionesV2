using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZooLogicSA.Core.COM;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones
{
    public class ServicioEvaluacionPromociones
    {
        private IMotorPromociones motorDePromociones;
        private Thread hilo;
        private object servicioSerializacion;
        private object comprobante;
        private NotificadorServicioPromociones nofificador { get; set; }
        private SynchronizedCollection<InformacionPromocion> retornoAplicables = new SynchronizedCollection<InformacionPromocion>();
        private SynchronizedCollection<InformacionPromocionIncumplida> retornoParciales = new SynchronizedCollection<InformacionPromocionIncumplida>();
        private ManualResetEvent señaladorEncendido;
        private bool señaladorApagado;
        private bool serializacionObsoleta;
		private TestigoCancelacion cancelacion = new TestigoCancelacion();
        private List<string> promocionesValidas;
        private bool refrescarPromocionesValidas = false;
        private ManualResetEvent señaladorEsperaHilo;
        private int delayEventoAplicarPromocionAutomatica = 0;
        private bool habilitarLecturaCodigoBarra = false;
        private bool acumularCantidadesConCodBar = false;
        public object managerPromocionesAutomaticas;
        public bool aplicaPromocionesAutomaticas;
        public bool estaSerializandoYEvaluando;

        public ServicioEvaluacionPromociones( IMotorPromociones motor, IFactoriaPromociones factoriaPromociones )
        {
            this.motorDePromociones = motor;
            this.nofificador = factoriaPromociones.ObtenerNotificadorServicioPromociones();
            this.señaladorEncendido = new ManualResetEvent( false );
            this.señaladorEsperaHilo = new ManualResetEvent( false );
        }

        public void HabilitarSerializacionPorHilos( object servicioSerializacion )
        {
            this.servicioSerializacion = servicioSerializacion;

            this.señaladorEncendido.Reset();
            this.señaladorApagado = false;
            this.refrescarPromocionesValidas = true;
            
            this.promocionesValidas = this.motorDePromociones.ObtenerPromociones().Select( x => x.Id ).ToList();

            if ( this.hilo == null || !this.hilo.IsAlive ) 
            {
                ThreadStart ts = new ThreadStart( this.SerializarComprobante );
                this.hilo = new Thread( ts );
                this.hilo.IsBackground = true;
				this.hilo.Name = "Proceso principal ServicioEvaluacionPromociones";
                this.hilo.Start();
            }
        }

        /// <summary>
        /// Este es el hilo de ejecucion de EvaluarYAplicarPromocion en el motor
        /// </summary>
        /// <param name="estado">Array con el comprobante, el codigo de promocion, y el handle de ejecucion del hilo para avisar la finalizacion</param>
		private void ProcesoAplicables( object estado )
        {
            object[] parametros = (object[])estado;

            string idPromocion = (string)(parametros[1]);
			Thread.CurrentThread.Name = "ProcesoAplicables " + idPromocion;

			ManualResetEvent notificador = (ManualResetEvent)(parametros[2]);
            try
			{
                IComprobante comprobante = this.motorDePromociones.ObtenerCopiaDeComprobante((string)(parametros[0]));

                if ( !this.serializacionObsoleta )
                {
                    List<InformacionPromocion> respuesta = this.motorDePromociones.EvaluarYAplicarPromocion( comprobante, idPromocion, this.cancelacion );
					respuesta.ForEach( x => this.retornoAplicables.Add( x ) );
				}
			}
			catch ( OperationCanceledException )
			{
				this.nofificador.InformacionDebug( "Omitiendo" + idPromocion );
			}
			catch ( Exception ex )
			{
				this.nofificador.InformarMensaje( "Error " + idPromocion );

				Promocion promo = this.motorDePromociones.ObtenerPromociones().Find( x => x.Id.TrimEnd() == idPromocion.TrimEnd() );

				InformacionPromocionIncumplida infoIncumplida = new InformacionPromocionIncumplida()
				{
					IdPromocion = idPromocion,
					Promocion = promo
				};

				InformacionPromocion infoPromocion = new InformacionPromocion( idPromocion ) { infoIncumplida = infoIncumplida };

				this.retornoAplicables.Add( infoPromocion );

				//this.nofificador.ProcesarErrorEnPromocion( info, ex );
			}
			finally
			{
                notificador.Set();
            }
        }

        /// <summary>
        /// Este es el hilo de ejecucion de ObtenerResultadosParciales en el motor
        /// </summary>
        /// <param name="estado">Array con el comprobante, el codigo de promocion, y el handle de ejecucion del hilo para avisar la finalizacion</param>
		private void ProcesoParciales( object estado )
		{
			object[] parametros = (object[])estado;

            IComprobante comprobante = this.motorDePromociones.ObtenerCopiaDeComprobante((string)(parametros[0]));

            string idPromocion = (string)(parametros[1]);
			Thread.CurrentThread.Name = "ProcesoParciales " + idPromocion;

			ManualResetEvent notificador = (ManualResetEvent)(parametros[2]);

			try
			{
				if ( !this.serializacionObsoleta )
				{
					InformacionPromocionIncumplida respuesta = this.motorDePromociones.ObtenerResultadosParciales( comprobante, idPromocion, this.cancelacion );
					this.retornoParciales.Add( respuesta );
				}
			}
			catch ( OperationCanceledException ex )
			{
				this.nofificador.InformacionDebug( "Omitiendo" + idPromocion );
			}
			catch ( Exception ex )
			{
				this.nofificador.InformarMensaje( "Error parcial " + idPromocion );

				Promocion promo = this.motorDePromociones.ObtenerPromociones().Find( x => x.Id.TrimEnd() == idPromocion.TrimEnd() );

				InformacionPromocionIncumplida info = new InformacionPromocionIncumplida()
																			{
																				IdPromocion = idPromocion,
																				Promocion = promo
																			};

				this.nofificador.ProcesarErrorEnPromocion( info, ex );
			}
			finally
			{
				notificador.Set();
			}
		}

		private void SerializarComprobante()
        {
            do
            {                              
                this.señaladorEncendido.WaitOne();
                this.señaladorEncendido.Reset();
                this.serializacionObsoleta = false;
				this.cancelacion.ResetearEstadoCancelacion();
               
                try
                {

                    if ( this.señaladorApagado )
                    {
                        this.nofificador.InformacionDebug( "Apagando..." );
                        this.nofificador.ServicioApagado();
                        break;
                    }

                    this.estaSerializandoYEvaluando = true;

                    this.nofificador.InformacionDebug( "Iniciando por encendido..." );

                    string codigo;
                    string respuesta;
                    string respuestaPreciosAdicionales;

                    #region Serializado del Comprobante
                    try
                    {
                        this.ValidarYFormatearFechaVacia( this.comprobante );
                        codigo = (string)( HerramientasCom.ObtenerPropiedad( this.comprobante, "Codigo" ) );
                        respuesta = (string)( HerramientasCom.EjecutarMetodo( this.servicioSerializacion, "serializarpromociones", this.comprobante ) );

                        String[] ListasDePrecios = this.motorDePromociones.ObtenerListasDePreciosUsadasEnPromociones();
                        respuestaPreciosAdicionales = (string)( HerramientasCom.EjecutarMetodo( this.servicioSerializacion, "SerializarPreciosAdicionales", this.comprobante, ListasDePrecios ) );
                    }
                    catch ( Exception ex )
                    {
                        this.nofificador.ProcesarError( ex );
                        this.nofificador.InformacionDebug( "Error al acceder al comprobante." );
                        this.señaladorEncendido.Reset();
                        continue;
                    }

                    if ( String.IsNullOrEmpty( respuesta ) )
                    {
                        this.nofificador.InformacionDebug( "Serializado erroneo, reintentando..." );
                        continue;
                    }
                    #endregion

                    cancelacion.ThrowIfCancellationRequested( "Cancelando Servicio Evaluacion de Promociones" );

                    this.motorDePromociones.AgregarComprobanteParaEvaluacion( codigo, respuesta );
                    this.motorDePromociones.AgregarPreciosAdicionalesParaEvaluacion( codigo, respuestaPreciosAdicionales );

                    if ( this.refrescarPromocionesValidas )
                    {
                        this.promocionesValidas = this.motorDePromociones.ObtenerPromocionesQueCumplaElParticipanteComprobante( codigo );
                        this.refrescarPromocionesValidas = false;

                        this.retornoAplicables.Clear();

                        List<Promocion> promosTotal = this.motorDePromociones.ObtenerPromociones();
                        
                        this.promocionesValidas.ForEach( x => this.retornoAplicables.Add( new InformacionPromocion( x ) { infoIncumplida = new InformacionPromocionIncumplida() { IdPromocion = x, Promocion = promosTotal.Find( t => t.Id.TrimEnd() == x.TrimEnd() ), Resultados = new List<ResultadoReglas>() } } ) );

                        this.nofificador.PresentarPromocionesAplicables( this.retornoAplicables.ToList() );
                    }

                    cancelacion.ThrowIfCancellationRequested( "Cancelando Servicio Evaluacion de Promociones" );

                    int procesosPromocionesAplicables = this.promocionesValidas.Count;

                    this.retornoAplicables.Clear();

                    #region Tirar hilos Evaluar Promociones Aplicables y esperar

                    if ( procesosPromocionesAplicables > 0 )
                    {
                        ManualResetEvent[] doneEvents = new ManualResetEvent[procesosPromocionesAplicables];

                        for ( int i = 0; i < procesosPromocionesAplicables; i++ )
                        {
                            doneEvents[i] = new ManualResetEvent( false );
                            object[] parametros = new object[] { codigo, this.promocionesValidas[i], doneEvents[i] };

                            ThreadPool.UnsafeQueueUserWorkItem( this.ProcesoAplicables, parametros );

                        }

                        for ( int i = 0; i < Math.Ceiling( procesosPromocionesAplicables / 64M ); i++ )
                        {
                            ManualResetEvent[] doneEvents2 = doneEvents.Skip( 64 * i ).Take( 64 ).ToArray();
                            WaitHandle.WaitAll( doneEvents2 );
                        }
                    }
                    #endregion
                    this.nofificador.PresentarPromocionesAplicables(this.retornoAplicables.ToList());

                    if (!this.serializacionObsoleta)
                    {
                        this.LeePropiedadesManagerPromocionAutomaticas();
                        this.aplicaPromocionesAutomaticas = this.ElCOmprobanteAplicaPromocionesAutomaticas();
                        this.AplicarPromocionesAutomaticas();
                    }
                    
                    this.nofificador.InformacionDebug( "Terminado" );
                }
                catch ( OperationCanceledException ex )
                {
                    this.nofificador.InformacionDebug( "Abort (OperationCanceledException)..." + ex.ToString() );
                    this.señaladorEncendido.Set();
                }
                catch ( Exception ex )
                {
                    this.nofificador.ProcesarError( ex );
                }
                finally
                {
                    this.señaladorEsperaHilo.Set();
                    this.estaSerializandoYEvaluando = false;
                }                

            } while ( true );
        }
        
        private List<string> ObtenerListaPromocionesNoExistentesEnLista( List<string> listaPromocionesAplicadas )
        {
            List<string> retorno = (from promocionValida in this.promocionesValidas
                                                    where !listaPromocionesAplicadas.Exists( promocionAplicada => promocionAplicada == promocionValida )
                                                    select promocionValida
                                                    ).ToList();
            return retorno;
        }

        public void RefrescarPromocionesValidas()
        {
            this.refrescarPromocionesValidas = true;
        }

        public void SerializarEnHilo( object comprobante, bool singlethread = false )
        {
            this.señaladorEsperaHilo.Reset();

            this.comprobante = comprobante;
            this.serializacionObsoleta = true;
			this.cancelacion.PedirCancelacion();
			this.nofificador.InformacionDebug( "Marcando serializacionObsoleta" );
			this.señaladorEncendido.Set();

            if ( singlethread )
            {
                this.señaladorEsperaHilo.WaitOne();
            }
        }

        public void DeshabilitarSerializacionPorHilos()
        {
            this.serializacionObsoleta = true;
            this.señaladorApagado = true;
            this.señaladorEncendido.Set();
        }

        public void AgregarObservador( IObservadorServicioPromociones observador )
        {
            this.nofificador.AgegarObservador( observador );
        }

        public void QuitarObservador(IObservadorServicioPromociones observador)
        {
            this.nofificador.QuitarObservador(observador);
        }

        private void ValidarYFormatearFechaVacia(object comprobante)
        {
            if (((string)(HerramientasCom.ObtenerPropiedad(comprobante, "HORAALTAFW"))).Trim() == "")
            {
                HerramientasCom.SetearPropiedad(comprobante, "HORAALTAFW", "12:00:00");
            }
        }
        public void AplicarPromocionesAutomaticas()
        {
            
            if (this.aplicaPromocionesAutomaticas)
            {
                bool esPrimeraVez = true;
                var promosOrdenadas = this.retornoAplicables.OrderByDescending(retornoAplicables => retornoAplicables.MontoBeneficio).ThenByDescending(retornoAplicables => retornoAplicables.Promocion.FechaModificacion).ThenByDescending(retornoAplicables => retornoAplicables.Promocion.HoraModificacion);

                foreach (var loPromo in promosOrdenadas)
                {
                    if (this.EsPromocionAutomaticaAplicable(loPromo))
                    {
                        if (esPrimeraVez && this.habilitarLecturaCodigoBarra && this.acumularCantidadesConCodBar)
                        {
                            esPrimeraVez = false;
                            Thread.Sleep(this.delayEventoAplicarPromocionAutomatica);
                        }
                        this.AplicarPromocion(loPromo.IdPromocion);
                        break;
                    }
                }
            }
        }

        public bool ElCOmprobanteAplicaPromocionesAutomaticas()
        {
            bool retorno;
                        
            if (this.managerPromocionesAutomaticas != null)
            {
                retorno = (bool)(HerramientasCom.ObtenerPropiedad(this.managerPromocionesAutomaticas, "lAplicaPromocionesAutomaticas"));
            }else
            {
                retorno = false;
            }

            return retorno;
        }

        public bool EsPromocionAutomaticaAplicable(InformacionPromocion promocion)
        {
            if (promocion.MontoBeneficio > 0 && promocion.Promocion.AplicaAutomaticamente)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void LeePropiedadesManagerPromocionAutomaticas()
        {
            if (this.managerPromocionesAutomaticas != null)
            {
                try
                {
                    this.delayEventoAplicarPromocionAutomatica = Convert.ToInt32(HerramientasCom.ObtenerPropiedad(this.managerPromocionesAutomaticas, "nDelayEventoAplicarPromocionAutomatica"));
                    this.habilitarLecturaCodigoBarra = (bool)(HerramientasCom.ObtenerPropiedad(this.managerPromocionesAutomaticas, "lHabilitarLecturaCodigoBarra"));
                    this.acumularCantidadesConCodBar = (bool)(HerramientasCom.ObtenerPropiedad(this.managerPromocionesAutomaticas, "lAcumularCantidadesConCodBar"));
                }
                catch (Exception ex)
                {
                    this.delayEventoAplicarPromocionAutomatica = 0;
                    this.habilitarLecturaCodigoBarra = false;
                    this.acumularCantidadesConCodBar = false;
                }
            }
        }
        
        private void AplicarPromocion(string idPromo)
        {
            HerramientasCom.EjecutarMetodo(this.managerPromocionesAutomaticas, "AplicarPromocionAutomatica", idPromo);
        }

        public void InyectarManagerAutomatico(object manager)
        {
            this.managerPromocionesAutomaticas = manager;
        }

        public void LimpiarManagerAutomatico()
        {
            this.managerPromocionesAutomaticas = null;
        }

        public bool ObtenerEstadoSerializacionYEvaluacion()
        {
            return this.estaSerializandoYEvaluando;
        }

    }
}