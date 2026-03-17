using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZooLogicSA.Core.Visual;
using ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones.Asistente
{
    public class KontrolerAsistente
    {
        private Aspecto aspecto;
        public IArmadorDeLeyenda armador;
        private List<PromocionesEstado> sourceTemporal = new List<PromocionesEstado>();

        public KontrolerAsistente( Aspecto aspecto )
        {
			FactoriaArmadorDeLeyenda fac = new FactoriaArmadorDeLeyenda();

			this.armador = fac.ObtenerArmadorDeLeyenda();
            this.aspecto = aspecto;
        }

        public void AgregarPromociones( List<InformacionPromocion> lista, BindingSource source )
        {
            this.limpiarTemporal(lista);

            foreach ( InformacionPromocion info in lista )
            {
                if ( info.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.NoVisible)
				{
				}
                else
                {
                    this.sourceTemporal.Add( new PromocionesEstado()
                    {
                        Id = info.IdPromocion.TrimEnd(),
                        Descripcion = info.Promocion.Descripcion,
                        LeyendaCliente = info.Promocion.LeyendaAsistente,
						Beneficio = info.MontoBeneficio,
                        Estado = estado.Cumplida,
                        EstadoDibujo = Properties.Resources.Verde,
                        Destacada = ( info.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.Destacada ),
                        Automatica = info.Promocion.AplicaAutomaticamente
                    } );
                }
            }
        }

        public void AgregarPromociones( List<InformacionPromocionIncumplida> lista, BindingSource source )
        {
            
            this.limpiarTemporal(lista);

            foreach ( InformacionPromocionIncumplida promos in lista )
            {
                if ( promos.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.NoVisible )
				{
				}
                else
                {
                    string leyendaFaltanteCompleta = this.ObtenerLeyendaFaltante( promos );
                    string leyendaFaltanteAMostrar = this.ObtenerLeyendaFaltanteAMostrarEnControl( promos );
                    this.sourceTemporal.Add( new PromocionesEstado()
                    {
                        Id = promos.IdPromocion.TrimEnd(),
                        Descripcion = promos.Promocion.Descripcion,
                        LeyendaCliente = promos.Promocion.LeyendaAsistente,
                        FaltanteCompleto = leyendaFaltanteCompleta,
                        Faltante = leyendaFaltanteAMostrar,
                        Beneficio = 0,
                        Estado = this.ObtenerEstado( promos ),
                        EstadoDibujo = this.ObtenerColorEstado( promos ),
                        Destacada = (promos.Promocion.Visualizacion == FormatoPromociones.VisualizacionPromocionAsistenteType.Destacada)
                    } );
                }
            }
        }

        


        public void AgregarPromocionConError( InformacionPromocionIncumplida infoPromo, string mensajeError, BindingSource source )
        {
            this.limpiarTemporal(infoPromo);

            string leyendaFaltanteCompleta = this.ObtenerLeyendaFaltante( infoPromo );
            string leyendaFaltanteAMostrar = this.ObtenerLeyendaFaltanteAMostrarEnControl( infoPromo );

            this.sourceTemporal.Add( new PromocionesEstado()
                    {
                        Id = infoPromo.IdPromocion,
                        Descripcion = infoPromo.Promocion.Descripcion,
                        LeyendaCliente = infoPromo.Promocion.LeyendaAsistente,
                        FaltanteCompleto = leyendaFaltanteCompleta,
                        Faltante = leyendaFaltanteAMostrar,
                        Beneficio = 0,
                        Estado = estado.Incumplida,
                        EstadoDibujo = Properties.Resources.Blanco,
                        Destacada = false
                    } );

            this.AgregarListaPromocionesASource( source );
        }

        private estado ObtenerEstado( InformacionPromocionIncumplida info )
        {
            if (info != null)
            {
                if (info.Resultados != null)
                {
                    return (info.Resultados.Exists(x => x.Satisfecho > 0 && !x.PartPromo.Codigo.Equals("COMPROBANTE"))) ? estado.Parcial : estado.Incumplida;
                }
            }

            return estado.Incumplida;
        }

        public void AgregarListaPromocionesASource( BindingSource source )
        {
            List<PromocionesEstado> sourceAuxiliar;
            
            sourceAuxiliar = this.OrdenarSourceTemporal();

			source.RaiseListChangedEvents = false;

            this.Limpiar(source);

            foreach (PromocionesEstado item in sourceAuxiliar)
            {
				source.Add( item );
			}

			source.RaiseListChangedEvents = true;
			source.ResetBindings( true );
		}

        private List<PromocionesEstado> OrdenarSourceTemporal()
        {
            List<PromocionesEstado> listaOrdenada;

			listaOrdenada = this.sourceTemporal
									.OrderByDescending( x => x.Destacada )
									.ThenBy( x => (int)x.Estado )
									.ThenByDescending( x => x.Beneficio )
									.ThenBy( x => x.Id )
									.ToList();
			
			return listaOrdenada;
        }

        private void QuitarDeLaListaSegunEstado( List<PromocionesEstado> sourceTemporal, estado estado )
        {
            List<PromocionesEstado> quitar = new List<PromocionesEstado>();

            foreach ( PromocionesEstado p in sourceTemporal )
            {
                if ( p.Estado.Equals( estado ) )
                {
                    quitar.Add( p );
                }
            }

            foreach ( PromocionesEstado item in quitar )
            {
                sourceTemporal.Remove(item);
            }
        }

        private void Limpiar( BindingSource source )
        {
            if (source.Count > 0)
            {
                source.Clear();
            }
        }

        private Bitmap ObtenerColorEstado(InformacionPromocionIncumplida info)
        {
            if (info != null)
            {
                if (info.Resultados != null)
                {
                    return (info.Resultados.Exists(x => x.Satisfecho > 0 && !x.PartPromo.Codigo.Equals("COMPROBANTE"))) ? Properties.Resources.Amarillo : Properties.Resources.Blanco;
                }
            }

            return Properties.Resources.Blanco;         
        }

        public void SeteosVisuales( FrmAsistente form )
        {
            this.SetearAspecto( form );
            ZooLogicSA.Core.Formularios.Extendedores.ZooFormExtender memoria = new Core.Formularios.Extendedores.ZooFormExtender();
        }

        private void SetearAspecto( FrmAsistente form )
        {
            Color colorFuente = this.aspecto.ObtenerRGB();

            if ( !string.IsNullOrEmpty( this.aspecto.Icono ) && File.Exists( this.aspecto.Icono ) )
            {
                form.Icon = this.aspecto.ObtenerIcono();
            }
        }

        public string ObtenerLeyendaFaltante(InformacionPromocionIncumplida promo)
        {
            return this.armador.Armar(promo);
        }

        public string ObtenerLeyendaFaltanteAMostrarEnControl(InformacionPromocionIncumplida promo)
        {
            return this.armador.ArmarLeyendaAMostrarEnControl(promo);
        }


		public void LimpiarSobrante( List<InformacionPromocion> info, BindingSource bindingSource )
		{
            if (info != null && sourceTemporal != null)
            {
                this.sourceTemporal.RemoveAll(x => !info.Exists(y => y.IdPromocion.TrimEnd() == x.Id.TrimEnd()));
            }
		
		}

        private void limpiarTemporal(List<InformacionPromocionIncumplida> lista)
        {
            if (lista != null && sourceTemporal != null)
            {
                this.sourceTemporal.RemoveAll(x => lista.Exists(y => y.IdPromocion.TrimEnd() == x.Id.TrimEnd()));
            }    
        }

        private void limpiarTemporal(List<InformacionPromocion> lista)
        {
            if (lista != null && sourceTemporal != null)
            {
                this.sourceTemporal.RemoveAll(x => lista.Exists(y => y.IdPromocion.TrimEnd() == x.Id.TrimEnd()));
            }     
        }

        private void limpiarTemporal(InformacionPromocionIncumplida infoPromo)
        {
            if (infoPromo != null && sourceTemporal != null)
            {
                this.sourceTemporal.RemoveAll(x => infoPromo.IdPromocion.TrimEnd() == x.Id.TrimEnd());
            }       
        }

    }
}