using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Core.COM;
using ZooLogicSA.Redondeos;
using System.Threading;

namespace ZooLogicSA.Promociones
{
    public class MotorPromociones : IMotorPromociones
    {
        private List<Promocion> promociones;
        private List<string> participantesNecesarios;
        private Dictionary<string, IComprobante> comprobantes;
        private IValidadorPromociones validadorPromociones;
        private ConfiguracionComportamiento configuracionComportamiento;
        private TransformadorComprobante transformadorComprobantes;
        private CalculadorDeIncumplimiento calculadorDeIncumplimiento;
        private IFactoriaPromociones factoriaPromociones;
        private IArmadorDeCoincidencias armadorDeCoincidencias;
        private List<Exception> errores;
        private List<EntidadRedondeo> redondeos;
        private CalculadorMontoBeneficio calculador;
        private bool evaluarValoresConCuponesIntegrados;

        public MotorPromociones( ConfiguracionComportamiento configuracionComportamiento, IFactoriaPromociones factoria )
        {
            this.factoriaPromociones = factoria;

            this.comprobantes = new Dictionary<string, IComprobante>();
            this.configuracionComportamiento = configuracionComportamiento;

            this.errores = new List<Exception>();
            this.validadorPromociones = this.factoriaPromociones.ObtenerValidadoresPromociones( this.configuracionComportamiento );
            this.transformadorComprobantes = this.factoriaPromociones.ObtenerTransformadorDeComprobantes( this.configuracionComportamiento );

            this.calculadorDeIncumplimiento = this.factoriaPromociones.ObtenerCalculadorDeIncumplimiento( this.configuracionComportamiento );
        
            this.armadorDeCoincidencias = this.factoriaPromociones.ObtenerArmadorDeCoincidencias( this.configuracionComportamiento );

            this.calculador = this.factoriaPromociones.ObtenerCalculadorPrecios( this.configuracionComportamiento );

            this.promociones = new List<Promocion>();
        }

        #region Get Set
        public List<string> ParticipantesNecesarios
        {
            get { return this.participantesNecesarios; }
        }
        #endregion

        public void EstablecerLibreriaPromociones( List<Promocion> promociones )
        {
            this.promociones = promociones;
            this.validadorPromociones.EstablecerLibreriaPromociones( promociones );
            this.ExponerParticipantesNecesarios();
        }

        public void EstablecerLibreriaRedondeos(List<EntidadRedondeo> redondeos)
        {
            this.redondeos = redondeos;
        }

        public void AgregarComprobanteParaEvaluacion( string identificador, string xml )
        {
            IComprobante comprobante = this.factoriaPromociones.ObtenerNuevoComprobante( this.configuracionComportamiento );
            comprobante.Cargar( xml );

            if ( !this.comprobantes.ContainsKey( identificador ) )
            {
                this.comprobantes.Add( identificador, null );
            }

            this.comprobantes[identificador] = comprobante;

        }

        public void AgregarPreciosAdicionalesParaEvaluacion(string identificador, string xml)
        {
            this.comprobantes[identificador].CargarPreciosAdicionales(xml);
        }

        /// <summary>
        /// Sobrecarga para aceptar un ArrayList como parametro (para facilitar la llamada desde FOX).
        /// </summary>
        public List<InformacionPromocion> AplicarPromociones( string idProceso, ArrayList promos )
        {
            List<string> listaPromos = promos.ToArray().Select(x => x.ToString()).ToList();

            return this.AplicarPromociones( idProceso, listaPromos );
        }

        /// <summary>
        /// Evalua y aplica las promos en el orden enviado.
        /// </summary>
        /// <param name="idProceso">El Id del comprobante agregado.</param>
        /// <param name="promos">Las promociones a evaluar y aplicar.</param>
        /// <returns>Lista de InformacionPromocion.</returns>
        public List<InformacionPromocion> AplicarPromociones( string idProceso, List<string> promos )
        {
            List<InformacionPromocion> retorno = new List<InformacionPromocion>();

            IComprobante comprobante = this.ObtenerComprobanteSegunId( idProceso );

            promos.ForEach( codigoPromocion => retorno.AddRange( this.EvaluarYAplicarPromocion( comprobante, codigoPromocion ) ) );

            return retorno;
        }

        public IComprobante ObtenerCopiaDeComprobante( string idProceso )
        {
            IComprobante comprobanteOrigen = this.ObtenerComprobanteSegunId( idProceso );
            IComprobante retorno = this.factoriaPromociones.ObtenerNuevoComprobante( this.configuracionComportamiento );
            string xml = comprobanteOrigen.ObtenerXml().InnerXml;
            if (String.IsNullOrEmpty(xml))
            {
                Thread.Sleep(500);
                xml = comprobanteOrigen.ObtenerXml().InnerXml;
            }
            retorno.Cargar( xml );
            retorno.CargarPreciosAdicionales( comprobanteOrigen.ObtenerXmlPreciosAdicionales().InnerXml );

            return retorno;
        }

        private IComprobante ObtenerComprobanteSegunId( string idProceso )
        {
            IComprobante comprobante;
            this.comprobantes.TryGetValue( idProceso, out comprobante );
            return comprobante;
        }

        private Promocion ObtenerPromocion( string codigoPromocion )
        {
            Promocion retorno = null;

            if ( this.promociones != null )
            {
                List<Promocion> promociones = (from x in this.promociones where x.Id.TrimEnd() == codigoPromocion.TrimEnd() select x).ToList();

                if ( promociones.Count > 0 )
                {
                    retorno = promociones[0];
                }
            }

            return retorno;
        }

		public List<InformacionPromocion> EvaluarYAplicarPromocion( IComprobante comprobante, string promo )
		{
            return this.EvaluarYAplicarPromocion( comprobante, promo, new TestigoCancelacion() ); 
        }
		
		public List<InformacionPromocion> EvaluarYAplicarPromocion( IComprobante comprobante, string promo, TestigoCancelacion cancelacion )
		{
            Promocion promocion = this.ObtenerPromocion( promo );

            List<InformacionPromocion> retorno = new List<InformacionPromocion>();

            if ( promocion != null && comprobante != null )
            {
                RespuestaEvaluacion respuesta;

                promocion = this.NormalizarPromocionObtenida( promocion );

                bool tieneTope = promocion.TopeBeneficio > 0;
                decimal saldoTope = promocion.TopeBeneficio;
                bool cumplioTodasLasReglasPeroNoElMontoBeneficio = false;

                do
                {
                    saldoTope = promocion.TopeBeneficio;
                    respuesta = new RespuestaEvaluacion();
                    respuesta.Promocion = promocion.Id;

                    List<ResultadoReglas> resultadoreglas = new List<ResultadoReglas>();

                    resultadoreglas = this.validadorPromociones.ComprobarReglas(comprobante, promocion, this.evaluarValoresConCuponesIntegrados);

                    cancelacion.ThrowIfCancellationRequested( "Cancelando EvaluarYAplicarPromocion - Promo ID: " + promo );

                    // otro asigna por cada beneficio (este sabe si tiene que cumplir un monto)
                    // despues aplica los participantes afectados
                    List<ConsumoParticipanteEvaluado> consumo = this.armadorDeCoincidencias.ObtenerCoincidencias( promocion, resultadoreglas );

					cancelacion.ThrowIfCancellationRequested( "Cancelando EvaluarYAplicarPromocion - Promo ID: " + promo );

                    respuesta.Coincidencias = (from x in consumo
                                               where x.Requerido == x.Satisfecho
                                               select new CoincidenciaEvaluacion()
                                               {
                                                   Atributos = x.Atributos,
                                                   CodigoParticipanteEnComprobante = x.CodigoParticipanteEnComprobante,
                                                   Consume = x.Requerido,
                                                   IdParticipanteEnComprobante = x.ParticipantesEnComprobante,
                                                   IdParticipanteRestante = x.ParticipantesRestantes,
                                                   IdParticipanteEnRegla = x.IdParticipanteEnRegla
                                               }).ToList();

					cancelacion.ThrowIfCancellationRequested( "Cancelando EvaluarYAplicarPromocion - Promo ID: " + promo );

                    // aca se sabe si la promo se cumple...
                    InformacionPromocion informacion;
                    if ( respuesta.Coincidencias.Count == promocion.Participantes.Count )
                    {
                        string xmlComprobanteAntesDeLosCambios = comprobante.ObtenerXml().InnerXml;
 
                        this.transformadorComprobantes.FactoriaPromociones = this.factoriaPromociones;
                        promocion.ObjetoRedondeo = this.ObtenerObjetoRedondeo( promocion.Redondeo );

                        informacion = this.transformadorComprobantes.Transformar( comprobante, promocion, respuesta, tieneTope, saldoTope );

                        informacion.MontoBeneficio = this.calculador.CalcularMontosBeneficio( informacion, comprobante, this.configuracionComportamiento );

                        respuesta.Cumple = true;

                        if ( informacion.MontoBeneficio > 0 || promocion.Beneficios.Count == 0 || promocion.Tipo == "4" )
						{
							// esto no tendria que FALLAR jamas con el nuevo modelo
							saldoTope = saldoTope - (decimal)informacion.MontoBeneficio;
							informacion.Promocion = promocion;
							retorno.Add( informacion );
						}
						else
						{
                            cumplioTodasLasReglasPeroNoElMontoBeneficio = true;

                            ParticipanteBeneficiado posiblePicaPleitos = informacion.DetalleBeneficiado.OrderBy( x => x.ImporteBeneficioTotal ).First();
							IParticipante participanteAQuitar = comprobante.ObtenerNodoParticipante( posiblePicaPleitos.Clave, posiblePicaPleitos.Id, null, null );
							List<string> listaExclusiones = participanteAQuitar.CoincidenciasExcluidas.ToList();

							comprobante.Cargar( xmlComprobanteAntesDeLosCambios );

							participanteAQuitar = comprobante.ObtenerNodoParticipante( posiblePicaPleitos.Clave, posiblePicaPleitos.Id, null, null );
							listaExclusiones.Add( posiblePicaPleitos.IdParticipanteRegla );
							participanteAQuitar.CoincidenciasExcluidas = listaExclusiones.ToArray();
						}


                        cancelacion.ThrowIfCancellationRequested( "Cancelando EvaluarYAplicarPromocion - Promo ID: " + promo );
					}
					else
					{
						if ( retorno.Count == 0 )
						{
							informacion = new InformacionPromocion( promo );
							informacion.Promocion = promocion;

							// para mantener la funcionalidad hasta que la parte visual se termine
							List<ResultadoReglas> resultadosFiltrados = (from x in resultadoreglas
																		 where
																			 x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
																			 ||
																			 x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto 
																		 select x).ToList();

							cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );

							List<ConsumoParticipanteEvaluado> sinConsumo = (from x in resultadosFiltrados
																			where !consumo.Exists( y => y.IdParticipanteEnRegla == x.PartPromo.Id )
																			select new ConsumoParticipanteEvaluado()
																			{
																				Satisfecho = 0,
																				IdParticipanteEnRegla = x.PartPromo.Id,
																				Requerido = x.Requerido
																			}).ToList();

							consumo.AddRange( sinConsumo );

                            cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );

                            InformacionPromocionIncumplida infoIncumplida = this.calculadorDeIncumplimiento.Calcular( promocion, resultadoreglas, consumo, armadorDeCoincidencias, comprobante );
                            infoIncumplida.CumplioTodasLasReglasPeroNoElMontoBeneficio = cumplioTodasLasReglasPeroNoElMontoBeneficio;
                            informacion.infoIncumplida = infoIncumplida;
                            retorno.Add( informacion );
                        }

                    }
                }
                while ( respuesta.Cumple && promocion.Recursiva && ( !tieneTope || saldoTope > 0 ) );
            }

            List<InformacionPromocion> retornoReorganizado = this.ReorganizarInformacion( promo, retorno );

            return retornoReorganizado;
        }

        private Promocion NormalizarPromocionObtenida( Promocion promoObtenida )
        {
            bool promoCorregida = false;
            Promocion promoResultante = new Promocion();
            promoResultante = promoObtenida.Clonar();
            List<ParticipanteRegla> nuevosParticipantes = new List<ParticipanteRegla>();

            foreach ( ParticipanteRegla participante in promoObtenida.Participantes )
            {
                bool agregarParticipante = true;
                if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].EsConsumible 
                    && nuevosParticipantes.Exists( p => p.Beneficiario == participante.Beneficiario && p.Codigo == participante.Codigo ) )
                {
                    foreach ( ParticipanteRegla partParaAnalizar in nuevosParticipantes.Where( p => p.Beneficiario == participante.Beneficiario && p.Codigo == participante.Codigo ) )
                    {
                        var paraAnalizar = ( from x in partParaAnalizar.Reglas
                                             where x.Atributo != this.configuracionComportamiento.ConfiguracionesPorParticipante[partParaAnalizar.Codigo].Cantidad
                                             select new { Clave = x.Atributo + x.Operador + x.Valor } 
                                           ).ToList().OrderBy( r => r.Clave );

                        var participanteActual = ( from x in participante.Reglas
                                                   where x.Atributo != this.configuracionComportamiento.ConfiguracionesPorParticipante[partParaAnalizar.Codigo].Cantidad
                                                   select new { Clave = x.Atributo + x.Operador + x.Valor }
                                                 ).ToList().OrderBy( r => r.Clave );

                        if ( paraAnalizar.Count() == participanteActual.Count() && paraAnalizar.SequenceEqual( participanteActual ) )
                        {
                            string idPartParaAnalizar = partParaAnalizar.Id;
                            decimal participanteCantidad = this.ObtenerCantidadDeReglaCantidad( participante );

                            this.CorregirBeneficioDestinoDePromocion(promoResultante, idPartParaAnalizar, participante.Id);

                            this.AcumularCantidadEnReglaCantidad( partParaAnalizar, participanteCantidad );
                            agregarParticipante = false;
                            promoCorregida = true;
                            break;
                        }
                    }
                }

                if ( agregarParticipante )
                {
                    nuevosParticipantes.Add( participante );
                }
            }
            
            if ( promoCorregida )
            {
                promoResultante.Participantes.Clear();
                promoResultante.Participantes.AddRange( nuevosParticipantes );
                this.ReemplazarPromocion( promoResultante );
            }

            return promoResultante;
        }

        private void CorregirBeneficioDestinoDePromocion( Promocion promoResultante, string idParticipanteAEncontar, string idParticipanteAEliminar )
        {
            if ( new[] { "1", "3", "5", "6" }.Any( x => promoResultante.Tipo.Contains(x) ) )
            {
                DestinoBeneficio destinoEncontrado = promoResultante.Beneficios[0].Destinos.Find( b => b.Participante == idParticipanteAEncontar );
                if ( destinoEncontrado != null )
                {
                    DestinoBeneficio destinoAEliminar = promoResultante.Beneficios[0].Destinos.Find( b => b.Participante == idParticipanteAEliminar );
                    if ( destinoAEliminar != null )
                    {
                        destinoEncontrado.Cuantos += destinoAEliminar.Cuantos;
                        promoResultante.Beneficios[0].Destinos.Remove( destinoAEliminar );
                    }
                }
            }
            else
            {
                Beneficio destinoEncontrado = promoResultante.Beneficios.Find( b => b.Destinos[0].Participante == idParticipanteAEncontar );
                if ( destinoEncontrado != null )
                {
                    Beneficio destinoAEliminar = promoResultante.Beneficios.Find( b => b.Destinos[0].Participante == idParticipanteAEliminar );
                    if ( destinoAEliminar != null )
                    {
                        destinoEncontrado.Destinos[0].Cuantos += destinoAEliminar.Destinos[0].Cuantos;
                        promoResultante.Beneficios.Remove( destinoAEliminar );
                    }
                }
            }
        }

        private void AcumularCantidadEnReglaCantidad( ParticipanteRegla participanteEnPromocion, decimal cantidad = 0 )
        {
            List<Regla> reglasCantidad = ( from x in participanteEnPromocion.Reglas
                                          where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad
                                          select x ).ToList();

            if ( reglasCantidad.Count() == 0 )
            {
                Regla nueva = new Regla();
                nueva.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad;
                nueva.Comparacion = Factor.DebeSerIgualA;
                nueva.Id = 999999;
                nueva.Valor = 1M + cantidad;
                nueva.ValorString = "'" + cantidad + "'";
                reglasCantidad.Add( nueva );

                participanteEnPromocion.Reglas.Add( nueva );
                participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and {999999}";
            }
            else
            {
                decimal cantValor = ( reglasCantidad[0].Valor is decimal ? (decimal)reglasCantidad[0].Valor : Convert.ToDecimal(reglasCantidad[0].Valor) );
                cantValor += cantidad;
                reglasCantidad[0].Valor = cantValor;
                reglasCantidad[0].ValorString = "'" + cantValor + "'";
            }
        }

        private void ReemplazarPromocion( Promocion promocion )
        {
            if ( this.promociones != null )
            {
                string codigoPromocion = promocion.Id;
                Promocion promoEncontrada = this.promociones.Find( x => x.Id.TrimEnd() == codigoPromocion.TrimEnd() );
                if ( promoEncontrada != null )
                {
                    promociones[this.promociones.IndexOf( promoEncontrada )] = promocion;
                }
            }
        }

        private decimal ObtenerCantidadDeReglaCantidad( ParticipanteRegla participanteEnPromocion )
        {
            List<Regla> reglasCantidad = ( from x in participanteEnPromocion.Reglas
                                          where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad
                                          select x ).ToList();

            decimal cantidad = 1M;
            if ( reglasCantidad.Count() != 0 )
            {
                cantidad = ( reglasCantidad[0].Valor is decimal ? (decimal)reglasCantidad[0].Valor : Convert.ToDecimal(reglasCantidad[0].Valor) );
            }

            return cantidad;
        }

        private EntidadRedondeo ObtenerObjetoRedondeo(string codigoRedondeo)
        {
            EntidadRedondeo retorno = null;
            try
            {

                foreach (EntidadRedondeo redondeo in this.redondeos)
                {
                    if (redondeo.Codigo.Trim() == codigoRedondeo.Trim())
                    {
                        retorno = redondeo;
                    }
                }
            }
            catch (NullReferenceException)
            {


            }
            return retorno;
        }

        private List<InformacionPromocion> ReorganizarInformacion( string promo, List<InformacionPromocion> informacion )
        {
            List<InformacionPromocion> retorno = new List<InformacionPromocion>();
            if ( informacion.Count > 0 )
            {
				InformacionPromocion info = new InformacionPromocion( promo );

                info.Afectaciones = informacion.Sum( x => x.Afectaciones );
                info.DetalleAfectado = informacion.SelectMany( d => d.DetalleAfectado ).ToList();
                info.DetalleBeneficiado = informacion.SelectMany( d => d.DetalleBeneficiado ).ToList();
                info.MontoBeneficio = (float)Math.Round((decimal)informacion.Sum(d => d.MontoBeneficio), 2);
                info.Demora = informacion.Sum( d => d.Demora );
                info.Promocion = informacion.Select( d => d.Promocion ).FirstOrDefault();

                info.DetalleAfectado = (from x in info.DetalleAfectado
                                        group x by x.Clave + x.Id into g
                                        select new ParticipanteAfectado()
                                        {
                                            Clave = g.Select( d => d.Clave ).First(),
                                            Id = g.Select( d => d.Id ).First(),
                                            Atributos = g.Select( d => d.Atributos ).First(),
                                            Cantidad = g.Sum( d => d.Cantidad )
                                        }).ToList();

                info.DetalleBeneficiado = (from x in info.DetalleBeneficiado
                                           group x by x.Clave + x.Id into g
                                           select new ParticipanteBeneficiado()
                                           {
                                               Clave = g.Select( d => d.Clave ).First(),
                                               Id = g.Select( d => d.Id ).First(),
                                               Cantidad = g.Sum( d => d.Cantidad ),
                                               Promocion = g.Select( d => d.Promocion ).First(),
                                               Alteracion = g.Select( d => d.Alteracion ).First(),
                                               AtributoAlterado = g.Select( d => d.AtributoAlterado ).First(),
                                               Beneficio = g.Select( d => d.Beneficio ).First(),
                                               ImporteBeneficioTotal = (float)Math.Round(g.Sum( d => d.ImporteBeneficioTotal ),2),
                                               Valor = (g.First().AtributoAlterado == "MONTODESCUENTO" ? g.Sum(d => d.ImporteBeneficioTotal).ToString() : g.Select(d => d.Valor).First())
                                           }
                                      ).ToList();

				IEnumerable<InformacionPromocion> respuestasIncumplidas = informacion.Where( x => !Object.ReferenceEquals( x.infoIncumplida, null ) );

				if ( respuestasIncumplidas.Count() > 0 )
				{
					info.infoIncumplida = respuestasIncumplidas.FirstOrDefault().infoIncumplida;
				}

                retorno.Add( info );
            }

            retorno.ForEach( x => x.DetalleAfectado.Where( y => y.Clave == this.configuracionComportamiento.NombreComprobante ).ToList().ForEach( z => z.Cantidad = 1 ) );

            retorno.ForEach( x => x.DetalleAfectado.Where( y => y.Clave == this.configuracionComportamiento.NombreComprobante ).ToList().ForEach( z => z.Atributos.Remove( this.configuracionComportamiento.ConfiguracionesPorParticipante[this.configuracionComportamiento.NombreComprobante].Cantidad ) ) );

            return retorno;
        }

        private void ExponerParticipantesNecesarios()
        {
            this.participantesNecesarios = new List<string>();

            if ( this.promociones != null )
            {
                var participantesPlano = this.promociones.SelectMany( x => x.Participantes ).ToList();

                participantesPlano.ForEach( part => part.Reglas.ForEach( regla => this.participantesNecesarios.Add( part.Codigo + "." + regla.Atributo ) ) );

                this.participantesNecesarios = this.participantesNecesarios.Distinct().ToList();
            }
        }

        /// <summary>
        /// Sobrecarga para aceptar un ArrayList como parametro (para facilitar la llamada desde FOX).
        /// </summary>
        public List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, ArrayList promos)
        {
            return this.EvaluarPromocionesIndividualmente(identificador, promos, false);
        }

        /// <summary>
        /// Sobrecarga para aceptar un ArrayList como parametro (para facilitar la llamada desde FOX).
        /// </summary>
        public List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, ArrayList promos, bool filtrar)
        {
            List<string> listaPromos = promos.ToArray().Select(x => x.ToString()).ToList();
            List<InformacionPromocion> retorno = this.EvaluarPromocionesIndividualmente(identificador, listaPromos, filtrar);
            return retorno;
        }

        public List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, List<string> promos)
        {
            return this.EvaluarPromocionesIndividualmente(identificador, promos, false);
        }

        /// <summary>
        /// Este metodo evalua cada promocion en la lista en forma individual
        /// (las aplicaciones y afectaciones de cada promo no se toman en cuenta en las siguientes evaluaciones).
        /// </summary>
        /// <param name="identificador">Codigo de comprobante a evaluar.</param>
        /// <param name="promos">Lista de promociones a evaluar.</param>
        /// <returns></returns>
        public List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, List<string> promos, bool filtrar)
        {
            List<InformacionPromocion> retorno = new List<InformacionPromocion>();

            IComprobante comprobante = this.ObtenerComprobanteSegunId(identificador);

            foreach (string codigoPromocion in promos)
            {
                retorno.AddRange(this.EvaluarYAplicarPromocion(comprobante, codigoPromocion));

                comprobante.Cargar(comprobante.ObtenerXmlOriginal().InnerXml);
            }

            retorno = retorno.OrderByDescending(x => x.MontoBeneficio).ToList();

            if (filtrar)
            {
                retorno = retorno.Where(x => x.MontoBeneficio > 0 && x.Afectaciones > 0).ToList();
            }

            return retorno;
        }

        public List<Promocion> ObtenerPromociones()
        {
            return this.promociones;
        }

		public InformacionPromocionIncumplida ObtenerResultadosParciales( IComprobante comprobante, string idPromocion )
		{
			return this.ObtenerResultadosParciales( comprobante, idPromocion, new TestigoCancelacion() );
		}

		public InformacionPromocionIncumplida ObtenerResultadosParciales( IComprobante comprobante, string idPromocion, TestigoCancelacion cancelacion )
        {
            Promocion promocion = this.ObtenerPromocion( idPromocion );

            List<ResultadoReglas> resultadoreglas = this.validadorPromociones.ComprobarReglas( comprobante, promocion, this.evaluarValoresConCuponesIntegrados );

			cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );
			
			List<ConsumoParticipanteEvaluado> consumo = this.armadorDeCoincidencias.ObtenerCoincidencias( promocion, resultadoreglas );

			cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );
			
			// para mantener la funcionalidad hasta que la parte visual se termine
            List<ResultadoReglas> resultadosFiltrados = (from x in resultadoreglas
                                                         where
															x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
															||
															x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto
														 select x).ToList();

			cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );
			
			List<ConsumoParticipanteEvaluado> sinConsumo = (from x in resultadosFiltrados
                       where !consumo.Exists( y => y.IdParticipanteEnRegla == x.PartPromo.Id )
                       select new ConsumoParticipanteEvaluado()
                       {
                           Satisfecho = 0,
                           IdParticipanteEnRegla = x.PartPromo.Id,
                           Requerido = x.Requerido
                       } ).ToList();

            consumo.AddRange( sinConsumo );

			cancelacion.ThrowIfCancellationRequested( "Cancelando ObtenerResultadosParciales - Promo ID: " + promocion.Id );
			
			InformacionPromocionIncumplida retorno = this.calculadorDeIncumplimiento.Calcular( promocion, resultadoreglas, consumo, armadorDeCoincidencias, comprobante );

            return retorno;
        }

        public List<string> ObtenerPromocionesQueCumplaElParticipanteComprobante( string codigoComprobante )
        {
            return this.ObtenerPromocionesQueCumplaElParticipanteComprobante( codigoComprobante, this.promociones );
        }

        public List<string> ObtenerPromocionesQueCumplaElParticipanteComprobante( string codigoComprobante, ArrayList promociones )
        {
            List<Promocion> listaPromos = promociones.ToArray().Select( x => this.ObtenerPromocion( x.ToString() ) ).Where( x=> x != null  ).ToList();

            return this.ObtenerPromocionesQueCumplaElParticipanteComprobante( codigoComprobante, listaPromos );
        }

        public List<string> ObtenerPromocionesQueCumplaElParticipanteComprobante( string codigoComprobante, List<Promocion> promociones )
        {
            List<string> retorno = new List<string>();

            IComprobante comprobante = this.ObtenerComprobanteSegunId( codigoComprobante );

            List<InformacionPromocionIncumplida> informacionResultados = new List<InformacionPromocionIncumplida>();

            foreach ( Promocion promo in promociones )
            {
                List<ParticipanteRegla> participantes = (from x in promo.Participantes select x).ToList();

                try
                {
                    promo.Participantes.RemoveAll( x => x.Codigo != this.configuracionComportamiento.NombreComprobante );

                    informacionResultados.Add( new InformacionPromocionIncumplida()
                    {
                        IdPromocion = promo.Id,
                        Promocion = promo,
                        Resultados = this.validadorPromociones.ComprobarReglas( comprobante, promo, this.evaluarValoresConCuponesIntegrados)
                    } );
                }
                catch ( Exception )
                {
                    // ñan ñam
                }
                finally
                {
                    promo.Participantes = participantes;
                }
            }

            informacionResultados.RemoveAll( infoPromo => infoPromo.Resultados.Exists(
                                                        resultado => !resultado.Cumple
                                                            &&
                                                        resultado.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[resultado.PartPromo.Codigo].Cantidad
                                                            &&
                                                        resultado.PartPromo.Codigo == this.configuracionComportamiento.NombreComprobante )
                                                     );

            informacionResultados.RemoveAll(infoPromo =>
                infoPromo.Promocion.Visualizacion.Equals(ZooLogicSA.Promociones.FormatoPromociones
                    .VisualizacionPromocionAsistenteType.NoVisible) && infoPromo.Promocion.AplicaAutomaticamente.Equals(false));

            retorno = informacionResultados.Select( x => x.IdPromocion ).ToList();

            return retorno;
        }

        public String[] ObtenerListasDePreciosUsadasEnPromociones()
        {
            List<String> retorno = (from x in this.promociones where !String.IsNullOrEmpty(x.ListaDePrecios) select x.ListaDePrecios).ToList();
            return retorno.ToArray();
        }

        private void AgregarExcepcion(Exception err)
        {
            this.errores.Add(err);
        }

        public List<ErrorEvaluador> ObtenerExcepciones()
        {
            return this.validadorPromociones.ObtenerExcepciones();
        }

        public void SetearPromosConCuponesIntegrados(bool valor)
        {
            this.evaluarValoresConCuponesIntegrados = valor;
        }


    }
}