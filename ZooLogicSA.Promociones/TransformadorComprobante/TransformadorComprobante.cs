using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Core.COM;
using System.Xml;

namespace ZooLogicSA.Promociones
{
    public class TransformadorComprobante
    {
        private ConfiguracionComportamiento configuracionComportamiento;
        private IInformantePromociones informante;
        private ICalculadorMonto calculador;
        private IFactoriaPromociones factoriaPromociones;

        public IFactoriaPromociones FactoriaPromociones
        {
            get { return factoriaPromociones; }
            set { factoriaPromociones = value; }
        }

        public TransformadorComprobante( ConfiguracionComportamiento configuracionComportamiento, ICalculadorMonto calculador, IInformantePromociones informante )
        {
            this.configuracionComportamiento = configuracionComportamiento;
            this.calculador = calculador;
            this.informante = informante;
        }

        public InformacionPromocion Transformar( IComprobante comprobante, Promocion promocion, RespuestaEvaluacion respuestaEvaluacion, bool tieneTope, decimal saldoTope )
        {
            InformacionPromocion info = new InformacionPromocion( promocion.Id );

            this.informante.InformarAfectacion( info, promocion, 1 );

            List<IParticipante> participantesConsumibles = new List<IParticipante>();
            List<CoincidenciaEvaluacion> coincidencias = this.RevisarCoincidencias( promocion, respuestaEvaluacion, comprobante, saldoTope, out Dictionary<string, decimal> montoConsumirPorTipo );
            List<ParticipanteDatosBasicos> participanteDatosBasicos = this.ObternerParticipantesDeCoincidenciasEnComprobante( coincidencias, comprobante );

            List<int> aplicacionesPorCoincidencias = this.ObternerParticipantesConsumibles( participantesConsumibles, coincidencias, participanteDatosBasicos, comprobante, promocion );
            int reAplicaciones = this.ObtenerPosiblesReAplicaciones( promocion, tieneTope, aplicacionesPorCoincidencias );

            List<IParticipante> consumidos = this.AplicarBeneficiosAConsumibles( promocion, comprobante, participantesConsumibles, respuestaEvaluacion, info, reAplicaciones, saldoTope, montoConsumirPorTipo );

            this.PrepararCoincidenciasRestantes( promocion, consumidos, respuestaEvaluacion, info );

            consumidos.AddRange( this.RevisarCoincidenciasYConsumir( promocion, respuestaEvaluacion, comprobante, info, saldoTope ) );

            #region ObtenerPosiblesReAplicaciones
            if ( reAplicaciones > 0 )
            {
                this.informante.InformarAfectacion( info, promocion, reAplicaciones );

                foreach ( IParticipante item in consumidos )
                {
                    if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[item.Clave].EsConsumible )
                    {
                        if ( !string.IsNullOrEmpty( item.Beneficio ) )
                        {
                            continue;
                        }

                        decimal cantOriginal = item.Cantidad;

                        IParticipante partOriginal = comprobante.ObtenerNodoParticipante( item.Clave, item.Id, null, null );
                        partOriginal.Cantidad -= ( cantOriginal * reAplicaciones );

                        item.Cantidad += ( cantOriginal * reAplicaciones );
                        item.Consumido = item.Cantidad;

                        this.informante.InformarItemReAfectado( info, item, item.Cantidad - cantOriginal );
                    }
                }
            }
            #endregion

            return info;
        }

        private void PrepararCoincidenciasRestantes( Promocion promocion, List<IParticipante> consumidos, RespuestaEvaluacion respuestaEvaluacion, InformacionPromocion info )
        {
            List<IParticipante> coincidenciaConsumida;
            foreach ( CoincidenciaEvaluacion item in respuestaEvaluacion.Coincidencias )
            {
                var infoBeneficiado = (from x in info.DetalleBeneficiado
                                       where item.CodigoParticipanteEnComprobante == x.Clave
                                               && item.IdParticipanteEnRegla == x.IdParticipanteRegla
                                               && item.IdParticipanteEnComprobante.Contains( x.Id )
                                       select new { Beneficio = x.Beneficio, Clave = x.Clave }
                                       ).FirstOrDefault();

                coincidenciaConsumida = ( from x in consumidos
                                        where x.Beneficio == ( ( infoBeneficiado == null ) ? "" : infoBeneficiado.Beneficio )
                                            && x.Clave == ( (infoBeneficiado == null ) ? "" : infoBeneficiado.Clave )
                                            && item.IdParticipanteEnComprobante.Contains( x.Id )
                                          select x
                                        ).ToList();

                if ( coincidenciaConsumida.Count != 0 )
                {
                    Beneficio beneficioEnCuestion = promocion.Beneficios.First( x => x.Id == coincidenciaConsumida[0].Beneficio );

                    DestinoBeneficio destinoConsumido = beneficioEnCuestion.Destinos.Find( b => b.Participante == item.IdParticipanteEnRegla );
                    if ( destinoConsumido == null )
                    {
                        destinoConsumido = beneficioEnCuestion.Destinos[0];
                    }
                    if ( destinoConsumido.Participante != "0" && destinoConsumido.Participante != item.IdParticipanteEnRegla )
                    {
                        continue;
                    }
                    bool consumePorMonto = this.VerSiConsumePorMonto( promocion, item );
                    decimal cantConsumida = ( consumePorMonto ) ? item.Consume : destinoConsumido.Cuantos;
                    cantConsumida = this.ObtenerConsumoCoincidenciaSegunAtributo( promocion, item, coincidenciaConsumida, cantConsumida, consumePorMonto );

                    item.Consume -= Math.Min( item.Consume, cantConsumida );
                }

                if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[item.CodigoParticipanteEnComprobante].EsConsumible )
                {
                    item.IdParticipanteEnComprobante.AddRange( item.IdParticipanteRestante );
                }
            }
        }

        private List<int> ObternerParticipantesConsumibles( List<IParticipante> participantesConsumibles, List<CoincidenciaEvaluacion> coincidencias, List<ParticipanteDatosBasicos> participanteDeLosNodos, IComprobante comprobante, Promocion promocion )
        {
            List<int> totalAplicaciones = new List<int>();
            foreach ( CoincidenciaEvaluacion coincidencia in coincidencias )
            {
                List<IParticipante> participantesDeLaCoincidencia = new List<IParticipante>();
                coincidencia.IdParticipanteEnComprobante.ForEach( x => participantesDeLaCoincidencia.Add( comprobante.ObtenerNodoParticipante( coincidencia.CodigoParticipanteEnComprobante, x, null, null ) ) );
                participantesConsumibles.AddRange( participantesDeLaCoincidencia );
                if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].EsConsumible )
                {
                    decimal cantidadAConsumir = this.ObtenerConsumoCoincidenciaSegunAtributo( promocion, coincidencia, participantesDeLaCoincidencia, coincidencia.Consume );
                    List<int> aplicacionesPorCoincidencia = new List<int>();

                    var consumidosAgrupados = from x in participantesDeLaCoincidencia
                                              where this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].EsConsumible
                                              group x by x.Clave + x.Id into z
                                              select new
                                              {
                                                  Clave = z.First().Clave,
                                                  Id = z.First().Id,
                                                  Cantidad = z.Sum( x => x.Cantidad )
                                              };

                    if ( consumidosAgrupados.GroupBy( x => x.Clave ).Count() == 1 )
                    {
                        // Verificar si la promoción utiliza consumo por combinación
                        bool utilizaCombinacion = promocion.UtilizaCosumoPorCombinacion();
                        
                        foreach ( var item in consumidosAgrupados )
                        {
                            if ( cantidadAConsumir <= 0 )
                            {
                                continue;
                            }
                            ParticipanteDatosBasicos participanteDelNodo = participanteDeLosNodos.Find( x => x.Clave == item.Clave && x.Id == item.Id );
                            participanteDelNodo.Usado += 1;
                            decimal usoDelNodo = Convert.ToDecimal(participanteDelNodo.Usado);
                            decimal cantidadDelNodo = participanteDelNodo.Cantidad;
                            
                            // Si utiliza combinación, verificar que el ConsumoPorCombinacion sea suficiente
                            if ( utilizaCombinacion && participanteDelNodo.ConsumoPorCombinacion > 0 )
                            {
                                // Usar la cantidad específica de la combinación en lugar de la cantidad total
                                cantidadDelNodo = Math.Min( cantidadDelNodo, participanteDelNodo.ConsumoPorCombinacion );
                            }
                            
                            decimal cantConsumible = Math.Min( cantidadDelNodo, cantidadAConsumir );
                            aplicacionesPorCoincidencia.Add( Convert.ToInt16( Math.Floor( ( cantidadDelNodo - cantConsumible ) / ( cantidadAConsumir * usoDelNodo ) ) ) );
                            participanteDelNodo.Cantidad -= cantConsumible;
                            cantidadAConsumir -= cantConsumible;
                        }
                    }

                    if ( aplicacionesPorCoincidencia.Count > 0 )
                    {
                        totalAplicaciones.AddRange( aplicacionesPorCoincidencia );
                    }
                }
            }

            return totalAplicaciones;
        }

        private List<ParticipanteDatosBasicos> ObternerParticipantesDeCoincidenciasEnComprobante(List<CoincidenciaEvaluacion> coincidencias, IComprobante comprobante)
        {
            List<ParticipanteDatosBasicos> participanteEnComprobante = new List<ParticipanteDatosBasicos>();

            foreach (CoincidenciaEvaluacion coincidencia in coincidencias)
            {
                List<ParticipanteDatosBasicos> auxPartEnComprobante = (from x in coincidencia.IdParticipanteEnComprobante
                                                                       where this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].EsConsumible
                                                                       select new ParticipanteDatosBasicos()
                                                                       {
                                                                           Clave = coincidencia.CodigoParticipanteEnComprobante,
                                                                           Id = x,
                                                                           Cantidad = comprobante.ObtenerNodoParticipante(coincidencia.CodigoParticipanteEnComprobante, x, null, null).Cantidad,
                                                                           Usado = 0
                                                                       }).ToList();

                participanteEnComprobante.AddRange(auxPartEnComprobante);
            }

            participanteEnComprobante = participanteEnComprobante
                                        .Select(x => x)
                                        .GroupBy(x => x.Clave + x.Id)
                                        .Select(x => x.First())
                                        .ToList();

            return participanteEnComprobante;
        }

        private decimal ObtenerConsumoCoincidenciaSegunAtributo( Promocion promo, CoincidenciaEvaluacion coincidencia, List<IParticipante> consumibles, decimal consumir, bool retornaMontoConsumido = false )
        {
            decimal totalConsumido = 0;
            decimal totalMonto = 0;

            foreach ( IParticipante participanteConsumible in consumibles )
            {
                if (consumir == 0 || participanteConsumible.Cantidad == 0)
                {
                    continue;
                }

                decimal nuevoConsumido;

                bool consumePorMonto = this.VerSiConsumePorMonto( promo, coincidencia );

                if ( consumePorMonto )
                {
                    Decimal precioParticipanteOriginal = participanteConsumible.ObtenerPrecioUnitario();
                    if ( precioParticipanteOriginal <= 0 )
                    {
                        continue;
                    }
                    Decimal consumirSegunMonto = Math.Ceiling( consumir / precioParticipanteOriginal );
                    consumir = consumirSegunMonto;
                }

                if ( participanteConsumible.Cantidad <= consumir )
                {
                    nuevoConsumido = participanteConsumible.Cantidad;
                    consumir -= participanteConsumible.Cantidad;
                }
                else
                {
                    nuevoConsumido = consumir;
                    consumir = 0;
                }

                if ( consumePorMonto )
                {
                    Decimal consumirEnMonto = consumir * participanteConsumible.ObtenerPrecioUnitario();
                    consumir = consumirEnMonto;

                    totalMonto += nuevoConsumido * participanteConsumible.ObtenerPrecioUnitario();
                }

                totalConsumido += nuevoConsumido;
            }

            totalConsumido = ( retornaMontoConsumido ) ? totalMonto : totalConsumido;
            return totalConsumido;
        }

        private List<IParticipante> AplicarBeneficiosAConsumibles( Promocion promocion, IComprobante comprobante, List<IParticipante> consumibles, RespuestaEvaluacion respuestaEvaluacion, InformacionPromocion info, decimal factor, decimal tope, Dictionary<string, decimal> montoConsumirPorTipo )
        {
            List<IParticipante> consumidos = new List<IParticipante>();
            foreach ( Beneficio beneficio in promocion.Beneficios )
            {
                consumidos.AddRange( this.AplicarBeneficio( comprobante, consumibles, respuestaEvaluacion, beneficio, promocion, info, factor, tope, montoConsumirPorTipo ) );
            }
            return consumidos;
        }

        private int ObtenerPosiblesReAplicaciones( Promocion promocion, bool tieneTope, List<int> aplicaciones )
        {
            int retorno = 0;
            
            if ( promocion.Recursiva && !tieneTope && aplicaciones.Count() > 0 )
            {
                retorno = aplicaciones.Min();
            }

            return retorno;
        }

        private List<IParticipante> RevisarCoincidenciasYConsumir( Promocion promocion, RespuestaEvaluacion respuestaEvaluacion, IComprobante comprobante, InformacionPromocion info, decimal tope )
        {
            Dictionary<string, decimal> montoConsumirPorTipo = new Dictionary<string, decimal>();

            if ( this.AlgunBeneficioTieneParticipanteQueConsumePorMonto( promocion.Beneficios, respuestaEvaluacion.Coincidencias ) )
            {
                Beneficio beneficioEnCuestion = promocion.Beneficios.First( x => x.Atributo == this.configuracionComportamiento.AtributoBeneficioPorMonto );

                // Si llegan a meter mas de un destino del beneficio, KABOOM...
                CoincidenciaEvaluacion coincidenciaGobernante = respuestaEvaluacion.Coincidencias.First( x => x.IdParticipanteEnRegla == beneficioEnCuestion.Destinos[0].Participante );

                var participantesCoincidentes = respuestaEvaluacion.Coincidencias.SelectMany(
                                                                x => x.IdParticipanteEnComprobante,
                                                                ( coincidencia, id ) => new
                                                                {
                                                                    Consume = coincidencia.Consume,
                                                                    Participante = comprobante.ObtenerNodoParticipante( coincidencia.CodigoParticipanteEnComprobante, id, null, null )
                                                                }
                                                            ).ToList();

                var montosEnConsumibles = participantesCoincidentes
                                            .Where( x => this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Participante.Clave].EsConsumible )
                                            .GroupBy( x => x.Participante.Clave ).Select( montos =>
                                                                                        montos.Sum( w => ( w.Consume * w.Participante.ObtenerPrecioUnitario() ) )
                                                                                         );

                decimal montoConsumir = montosEnConsumibles.Min();

                if ( tope > 0 )
                {
                    decimal porcConsumir = Convert.ToDecimal( beneficioEnCuestion.Valor, new CultureInfo( "en-US" ) );
                    if ( porcConsumir > 0 )
                    {
                        decimal topeConsumir = tope / ( porcConsumir / 100 );

                        montoConsumir = Math.Min( topeConsumir, montoConsumir );
                    }
                }

                respuestaEvaluacion.Coincidencias.GroupBy( x => x.CodigoParticipanteEnComprobante ).Select( x => x.Key ).ToList().ForEach( x => montoConsumirPorTipo.Add( x, montoConsumir ) );
            }

            List<IParticipante> consumidos = new List<IParticipante>();

            foreach ( CoincidenciaEvaluacion coincidencia in respuestaEvaluacion.Coincidencias )
            {
                decimal consumo = 0;

                List<IParticipante> participantesConsumibles = ObtenerParticipantesOrdenadosPorPrecio( promocion, comprobante, coincidencia );

                if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].ConsumePorMonto && montoConsumirPorTipo.Any( x => x.Key == coincidencia.CodigoParticipanteEnComprobante ) )
                {
                    consumo = montoConsumirPorTipo[coincidencia.CodigoParticipanteEnComprobante];
                }
                else
                {
                    consumo = coincidencia.Consume;
                }

                consumidos.AddRange( this.Consumir( participantesConsumibles, promocion, coincidencia, info, consumo ) );
            }

            return consumidos;
        }

        private List<IParticipante> ObtenerParticipantesOrdenadosPorPrecio( Promocion promocion, IComprobante comprobante, CoincidenciaEvaluacion coincidencia )
        {
            List<IParticipante> consumibles = new List<IParticipante>();
            coincidencia.IdParticipanteEnComprobante
                            .ForEach(
                                x => consumibles.Add( comprobante.ObtenerNodoParticipante( coincidencia.CodigoParticipanteEnComprobante, x, "", "" ) )
                            );

            List<IParticipante> participantesConsumibles;
            /// INVERTIR EL ORDEN ANTED DE CONSUMIR EL QUE NO TIENE BENEFICIO APLICADO !!! ///
            if ( !promocion.EleccionParticipante.Equals( EleccionParticipanteType.AplicarAlDeMayorPrecio ) )
            {
                participantesConsumibles = this.OrdenarParticipantesPorMayorPrecio( consumibles, coincidencia );
            }
            else
            {
                participantesConsumibles = this.OrdenarParticipantesPorMenorPrecio( consumibles, coincidencia );
            }

            return participantesConsumibles;
        }

        private List<IParticipante> OrdenarParticipantesPorMayorPrecio( List<IParticipante> consumibles, CoincidenciaEvaluacion coincidencia )
        {
            List<IParticipante> participantesOrdenados = (
                    from x in consumibles
                    where coincidencia.CodigoParticipanteEnComprobante == x.Clave
                          && coincidencia.IdParticipanteEnComprobante.Contains( x.Id )
                    orderby this.calculador.ObtenerPrecio( x, 1 ) descending
                    select x
                ).ToList();

            return participantesOrdenados;
        }

        private List<IParticipante> OrdenarParticipantesPorMenorPrecio( List<IParticipante> consumibles, CoincidenciaEvaluacion coincidencia )
        {
            List<IParticipante> participantesOrdenados = (
                    from x in consumibles
                    where coincidencia.CodigoParticipanteEnComprobante == x.Clave
                          && coincidencia.IdParticipanteEnComprobante.Contains( x.Id )
                    orderby this.calculador.ObtenerPrecio( x, 1 ) ascending
                    select x
                ).ToList();

            return participantesOrdenados;
        }

        private List<CoincidenciaEvaluacion> RevisarCoincidencias( Promocion promocion, RespuestaEvaluacion respuestaEvaluacion, IComprobante comprobante, decimal tope, out Dictionary<string, decimal> montoPorTipo )
        {
            Dictionary<string, decimal> montoConsumirPorTipo = new Dictionary<string, decimal>();

            if ( this.AlgunBeneficioTieneParticipanteQueConsumePorMonto( promocion.Beneficios, respuestaEvaluacion.Coincidencias ) )
            {
                Beneficio beneficioEnCuestion = promocion.Beneficios.First( x => x.Atributo == this.configuracionComportamiento.AtributoBeneficioPorMonto );

                var participantesCoincidentes = respuestaEvaluacion.Coincidencias.SelectMany(
                                                                x => x.IdParticipanteEnComprobante,
                                                                ( coincidencia, id ) => new
                                                                {
                                                                    Consume = coincidencia.Consume,
                                                                    Participante = comprobante.ObtenerNodoParticipante(coincidencia.CodigoParticipanteEnComprobante, id, null, null)
                                                                }
                                                            ).ToList();

                var montosEnConsumibles = participantesCoincidentes
                                            .Where( x => this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Participante.Clave].EsConsumible )
                                            .GroupBy( x => x.Participante.Clave ).Select( montos =>
                                                                                     montos.Sum( w => ( w.Consume * w.Participante.ObtenerPrecioUnitario() ) )
                                                                                         );

                decimal montoConsumir = montosEnConsumibles.Min();

                if ( tope > 0 )
                {
                    decimal porcConsumir = Convert.ToDecimal( beneficioEnCuestion.Valor, new CultureInfo("en-US") );
                    if ( porcConsumir > 0 )
                    {
                        decimal topeConsumir = tope / ( porcConsumir / 100 );

                        montoConsumir = Math.Min( topeConsumir, montoConsumir );
                    }
                }

                respuestaEvaluacion.Coincidencias.GroupBy( x => x.CodigoParticipanteEnComprobante ).Select( x => x.Key ).ToList().ForEach(x => montoConsumirPorTipo.Add( x, montoConsumir ) );
            }

            montoPorTipo = montoConsumirPorTipo;
            return respuestaEvaluacion.Coincidencias;
        }

        private List<IParticipante> ConsumirCoincidencia( Promocion promocion, List<IParticipante> participante, CoincidenciaEvaluacion coincidencia, InformacionPromocion info, Dictionary<string, decimal> montoConsumirPorTipo, decimal montoConsumir )
        {
            decimal consumo = 0;

            List<IParticipante> consumidos = new List<IParticipante>();

            if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].ConsumePorMonto && montoConsumirPorTipo.Any( x => x.Key == coincidencia.CodigoParticipanteEnComprobante ) )
            {
                consumo = montoConsumirPorTipo[coincidencia.CodigoParticipanteEnComprobante];
            }
            else
            {
                if (promocion.UtilizaCosumoPorCombinacion())
                {
                    consumo = participante[0].ConsumoPorCombinacion; //consume el item 0 ya que siempre viene la lista con un solo item.
                    if (consumo == 0)
                    {
                        consumo = montoConsumir;
                    }
                }
                else
                {
                    consumo = montoConsumir;
                }
                
            }

            consumidos.AddRange( this.Consumir( participante, promocion, coincidencia, info, consumo ) );

            return consumidos;
        }

        private bool AlgunBeneficioTieneParticipanteQueConsumePorMonto( List<Beneficio> beneficios, List<CoincidenciaEvaluacion> coincidencias )
        {            
            return  coincidencias.Exists( x => this.configuracionComportamiento.ConfiguracionesPorParticipante[x.CodigoParticipanteEnComprobante].ConsumePorMonto ) && beneficios.Any(x => x.Atributo == this.configuracionComportamiento.AtributoBeneficioPorMonto);
        }

        private bool VerSiConsumePorMonto( Promocion promo, CoincidenciaEvaluacion coincidencia )
        {
            bool consumePorMonto = promo
                                        .Participantes.Where( p => p.Id == coincidencia.IdParticipanteEnRegla )
                                        .FirstOrDefault()
                                        .Reglas
                                        .Exists( r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].CantidadMonto );
            return consumePorMonto;
        }

        private List<IParticipante> Consumir( List<IParticipante> consumibles, Promocion promo, CoincidenciaEvaluacion coincidencia, InformacionPromocion info, decimal consumir )
        {
            List<IParticipante> retorno = new List<IParticipante>();

            if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].ConsumePorMonto )
            {
				#region ConsumePorMonto
				foreach ( IParticipante participanteConsumible in consumibles )
				{
					if ( consumir == 0 || participanteConsumible.Cantidad == 0 )
					{
						continue;
					}

					IParticipante participanteOriginal = participanteConsumible;
					IParticipante participanteNuevo = participanteOriginal.Clonar();

					decimal montoParticipanteOriginal = this.calculador.ObtenerPrecio( participanteOriginal, participanteOriginal.Cantidad );
					decimal nuevoMonto;

					if ( montoParticipanteOriginal <= consumir )
					{
						consumir = consumir - montoParticipanteOriginal;
						nuevoMonto = montoParticipanteOriginal;
						montoParticipanteOriginal = 0;
					}
					else
					{
						nuevoMonto = consumir;
						montoParticipanteOriginal = montoParticipanteOriginal - consumir;
						consumir = 0;
					}

					participanteNuevo.Promocion = promo.Id;
					participanteNuevo.Beneficio = "";

					if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].EsConsumible )
					{
						participanteOriginal.AplicarValorAAtributo( this.configuracionComportamiento.AtributoBeneficioPorMonto, Alteracion.CambiarValor, montoParticipanteOriginal );
						participanteNuevo.AplicarValorAAtributo( this.configuracionComportamiento.AtributoBeneficioPorMonto, Alteracion.CambiarValor, nuevoMonto );
						participanteOriginal.AgregarAlMismoNivel( participanteNuevo );
					}

					if ( participanteOriginal.ObtenerPrecioUnitario() == 0 )
					{
						participanteOriginal.Consumido = participanteOriginal.Cantidad;
						participanteOriginal.Cantidad = 0;
					}

					this.informante.InformarItemAfectado( info, promo.Id, coincidencia, participanteNuevo );

					retorno.Add( participanteNuevo );
				} 
				#endregion
            }
            else
            {
                foreach ( IParticipante participanteConsumible in consumibles )
                {
                    if ( consumir == 0 || participanteConsumible.Cantidad == 0 )
                    {
                        continue;
                    }

                    IParticipante participanteOriginal = participanteConsumible;
                    IParticipante participanteNuevo = participanteOriginal.Clonar();

                    decimal nuevoConsumido;
                    decimal originalCantidad;

                    bool consumePorMonto = this.VerSiConsumePorMonto( promo, coincidencia );

                    if ( consumePorMonto )
                    {
                        Decimal precioParticipanteOriginal = participanteOriginal.ObtenerPrecioUnitario();

                        if ( precioParticipanteOriginal <= 0 )
                        {
                            continue;
                        }

                        Decimal consumirOriginal = consumir;
                        Decimal consumirSegunMonto = Math.Ceiling( consumir / precioParticipanteOriginal );
                        consumir = consumirSegunMonto;
                    }
                    else
                    {
                        consumir = Math.Min( coincidencia.Consume, consumir );
                    }

                    if ( participanteOriginal.Cantidad <= consumir )
                    {
                        nuevoConsumido = participanteOriginal.Cantidad;
                        originalCantidad = 0;
                        consumir = consumir - participanteOriginal.Cantidad;
                    }
                    else
                    {
                        originalCantidad = participanteOriginal.Cantidad - consumir;
                        nuevoConsumido = consumir;
                        consumir = 0;
                    }

					if ( consumePorMonto )
					{
						Decimal consumirEnMonto = consumir * participanteOriginal.ObtenerPrecioUnitario();
						consumir = consumirEnMonto;
					}

                    participanteNuevo.Promocion = promo.Id;
                    participanteNuevo.Beneficio = "";

                    if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].EsConsumible )
                    {
                        participanteOriginal.Cantidad = originalCantidad;
                        participanteNuevo.Cantidad = nuevoConsumido;
                        participanteNuevo.Consumido = nuevoConsumido;
                        participanteOriginal.AgregarAlMismoNivel( participanteNuevo );
                    }

                    this.informante.InformarItemAfectado( info, promo.Id, coincidencia, participanteNuevo );

                    retorno.Add( participanteNuevo );
                }
            }

            return retorno;
        }

        private List<IParticipante> AplicarBeneficio( IComprobante comprobante, List<IParticipante> participantesConsumibles, RespuestaEvaluacion respuestaEvaluacion, Beneficio beneficio, Promocion promo, InformacionPromocion info, decimal reAplicaciones, decimal tope, Dictionary<string, decimal> montoConsumirPorTipo )
        {
            List<IParticipante> beneficiados = new List<IParticipante>();
            List<IParticipante> itemsConsumidos = new List<IParticipante>();
            decimal factor = reAplicaciones + 1;

            foreach ( DestinoBeneficio destinatario in beneficio.Destinos )
            {
                Factor operadorRegla;
                List<CoincidenciaEvaluacion> listaCoincidencias;
                decimal cuantosSegunDestinatario = destinatario.Cuantos;
                decimal cuantosAplicaBeneficio = cuantosSegunDestinatario;

                if ( destinatario.Participante != "0" )
                {
                    // La lista debería contener siempre 1 solo item
                    listaCoincidencias = ( from x in respuestaEvaluacion.Coincidencias
                                           where x.IdParticipanteEnRegla == destinatario.Participante
                                           select x
                                         ).DefaultIfEmpty().ToList();
                }
                else
                {
                    // La lista puede contener mas de 1 item pero solo para LLevaXPagaY
                    listaCoincidencias = this.CrearCoincidenciaPorErrorSerializado( promo, participantesConsumibles, respuestaEvaluacion.Coincidencias );
                }

                foreach ( CoincidenciaEvaluacion coincidencia in listaCoincidencias )
                {
                    if ( destinatario.Participante != "0" )
                    {
                        operadorRegla = promo.Participantes.Find( p => p.Id == destinatario.Participante ).Reglas.Find( r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].Cantidad || r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].CantidadMonto ).Comparacion;
                    }
                    else
                    {
                        operadorRegla = promo.Participantes.Find( p => p.Id == coincidencia.IdParticipanteEnRegla ).Reglas.Find( r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[coincidencia.CodigoParticipanteEnComprobante].Cantidad ).Comparacion;
                    }

                    List<IParticipante> aConsumirEntreConsumibles;
                    if ( promo.EleccionParticipante.Equals( EleccionParticipanteType.AplicarAlDeMayorPrecio ) )
                    {
                        aConsumirEntreConsumibles = ( from x in participantesConsumibles
                                                      where coincidencia.CodigoParticipanteEnComprobante == x.Clave
                                                        && coincidencia.IdParticipanteEnComprobante.Contains( x.Id )
                                                      orderby this.calculador.ObtenerPrecio(x, 1) descending
                                                      select x
                                                    ).ToList();
                    }
                    else
                    {
                        aConsumirEntreConsumibles = ( from x in participantesConsumibles
                                                      where coincidencia.CodigoParticipanteEnComprobante == x.Clave
                                                        && coincidencia.IdParticipanteEnComprobante.Contains( x.Id )
                                                      orderby this.calculador.ObtenerPrecio(x, 1) ascending
                                                      select x
                                                    ).ToList();
                    }

                    bool consumePorMonto = this.VerSiConsumePorMonto( promo, coincidencia );

                    if ( ( consumePorMonto ) || ( operadorRegla == Factor.DebeSerMayorA || operadorRegla == Factor.DebeSerMayorIgualA ) )
                    {
                        cuantosSegunDestinatario = coincidencia.Consume;
                        cuantosAplicaBeneficio = cuantosSegunDestinatario;
                    }

                    decimal cantidadNueva = 0;
                    decimal cantidadConsumido = 0;
                    decimal cantidadNuevoItem = 0;
                    decimal consumidoNuevoItem = 0;

                    foreach ( IParticipante item in aConsumirEntreConsumibles )
                    {
                        if (cuantosAplicaBeneficio == 0 || item.Cantidad == 0)
                        {
                            continue;
                        }

                        IParticipante partOriginal = comprobante.ObtenerNodoParticipante( item.Clave, item.Id, null, null );
                        decimal partCantOriginal = partOriginal.Cantidad;

                        List<IParticipante> participanteAConsumir = new List<IParticipante>();
                        participanteAConsumir.Add( item );
                        List<IParticipante> itemConsumido = this.ConsumirCoincidencia( promo, participanteAConsumir, coincidencia, info, montoConsumirPorTipo, cuantosAplicaBeneficio );

                        IParticipante participanteOriginal = itemConsumido[0];
                        IParticipante participanteNuevo = participanteOriginal.Clonar();

                        decimal cantidadActualParticipante = participanteOriginal.Cantidad;
                        decimal valorAplicaBeneficio = cantidadActualParticipante;

                        #region ifffffffffffffffffffffffff
                        if ( consumePorMonto )
                        {
                            Decimal consumirEnMonto = cantidadActualParticipante * item.ObtenerPrecioUnitario();
                            valorAplicaBeneficio = consumirEnMonto;
                        }

                        if ( valorAplicaBeneficio <= cuantosAplicaBeneficio )
                        {
                            if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[itemConsumido[0].Clave].ConsumePorMonto )
                            {
                                cantidadNueva = 1;
                                cantidadConsumido = 1;
                            }
                            else
                            {
                                cantidadNueva = 0;
                                cantidadConsumido = 0;
                            }

                            cantidadNuevoItem = cantidadActualParticipante;
                            consumidoNuevoItem = cantidadActualParticipante;

                            cuantosAplicaBeneficio -= valorAplicaBeneficio;
                        }
                        else
                        {
                            if ( consumePorMonto )
                            {
                                Decimal precioParticipanteOriginal = item.ObtenerPrecioUnitario();
                                if ( precioParticipanteOriginal > 0 )
                                {
                                    Decimal consumirEnMonto = Math.Ceiling( valorAplicaBeneficio / precioParticipanteOriginal );
                                    valorAplicaBeneficio = consumirEnMonto;
                                }
                            }

                            cantidadNueva = cantidadActualParticipante - valorAplicaBeneficio;
                            cantidadConsumido = participanteOriginal.Consumido - valorAplicaBeneficio;

                            cantidadNuevoItem = valorAplicaBeneficio;
                            consumidoNuevoItem = valorAplicaBeneficio;

                            cuantosAplicaBeneficio = 0;
                        }
                        #endregion

                        participanteOriginal.Consumido = cantidadConsumido;
                        participanteOriginal.Cantidad = cantidadNueva;

                        participanteNuevo.Consumido = consumidoNuevoItem;
                        participanteNuevo.Cantidad = cantidadNuevoItem;

                        IParticipante participantePromocionadoYaExistente = comprobante.ObtenerNodoParticipante( participanteNuevo.Clave, participanteNuevo.Id, promo.Id, beneficio.Id );

                        // aca agrega siempre beneficiado
                        // o hace item nuevo, o lo agrega a uno igual
                        if ( participantePromocionadoYaExistente != null && participantePromocionadoYaExistente.CompararSegunContenido( participanteNuevo ) )
                        {
                            decimal cantidadExistente = participantePromocionadoYaExistente.Cantidad;
                            decimal cantParticipanteNuevo = participanteNuevo.Cantidad;

                            participantePromocionadoYaExistente.Cantidad = cantidadExistente + cantParticipanteNuevo;
                            participantePromocionadoYaExistente.Consumido = participantePromocionadoYaExistente.Consumido + cuantosSegunDestinatario;
                        }
                        else
                        {
                            participanteNuevo.Promocion = promo.Id;
                            participanteNuevo.Beneficio = beneficio.Id;
                            participanteOriginal.AgregarAlMismoNivel( participanteNuevo );
                            beneficiados.Add( participanteNuevo );
                        }

                        if ( participanteOriginal.Cantidad == 0 )
                        {
                            participanteOriginal.Destruir();
                        }

                        decimal cantOriginalConsumida = participanteNuevo.Cantidad;
                        if ( this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].EsConsumible )
                        {
                            participanteNuevo.Cantidad += ( cantOriginalConsumida * reAplicaciones );
                            participanteNuevo.Consumido = participanteNuevo.Cantidad;
                            this.informante.InformarItemNuevoAfectado( info, participanteNuevo, participanteNuevo.Cantidad );
                        }

                        if ( !this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].ConsumePorMonto )
                        {
                            partOriginal.Cantidad = partCantOriginal - participanteNuevo.Cantidad;
                        }
                    }
                }
            }
            

            Beneficio beneficioFinal = this.ObtenerBeneficioFinal( beneficio, beneficiados, tope, factor );

            bool aplicoTope = (string)beneficioFinal.Valor.ToString() != (string)beneficio.Valor.ToString();

            foreach ( IParticipante participanteNuevo in beneficiados )
            {
                Beneficio beneficioNuevo = beneficioFinal.Clonar();

                if ( beneficioFinal.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].MontoDescuento && promo.ListaDePrecios != "" )
                {

                    string articuloBenef = ((XmlElement)participanteNuevo.Nodo).SelectSingleNode("ARTICULO").SelectSingleNode("CODIGO").Attributes["Valor"].Value;
                    string colorBenef = ((XmlElement)participanteNuevo.Nodo).SelectSingleNode("COLOR").SelectSingleNode("CODIGO").Attributes["Valor"].Value;
                    string talleBenef = ((XmlElement)participanteNuevo.Nodo).SelectSingleNode("TALLE").SelectSingleNode("CODIGO").Attributes["Valor"].Value;


                    decimal precioListaPromocion = 0;

                    XmlDocument xmlPreciosAdicionales = comprobante.ObtenerXmlPreciosAdicionales();
                    XmlNodeList xnList = xmlPreciosAdicionales.GetElementsByTagName( "cprecioartic" );
                    var nodos = new List<XmlNode>( xnList.Cast<XmlNode>() );
                    var consulta = nodos.Find(x =>
                                   x.SelectSingleNode("articulo").InnerText.ToString() == articuloBenef &&
                                   x.SelectSingleNode("color").InnerText.ToString() == colorBenef &&
                                   x.SelectSingleNode("talle").InnerText.ToString() == talleBenef &&
                                   x.SelectSingleNode("lista").InnerText.ToString() == promo.ListaDePrecios.TrimEnd()
                              );
                    if ( consulta != null )
                        precioListaPromocion = Convert.ToDecimal( consulta.SelectSingleNode("precio").InnerText.ToString(), new CultureInfo("en-US") );


                    beneficioNuevo.Valor = ( Math.Round(calculador.ObtenerPrecio(participanteNuevo, participanteNuevo.Cantidad ) -
                        ( precioListaPromocion * participanteNuevo.Cantidad ), 2) ).ToString( new CultureInfo("en-US") );
                    if ( precioListaPromocion == 0 || Convert.ToDecimal( beneficioNuevo.Valor, new CultureInfo("en-US") ) < 0 )
                    {
                        beneficioNuevo.Valor = 0;
                    }
                }


                if ( beneficio.Atributo == this.configuracionComportamiento.AtributoMontoFinal || !string.IsNullOrEmpty( promo.Redondeo ) || aplicoTope )
                {
                    if ( beneficioFinal.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].Descuento )
                    {
                        beneficioNuevo.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteNuevo.Clave].MontoDescuento;
                        decimal valor = Convert.ToDecimal( (string)beneficioNuevo.Valor, CultureInfo.CreateSpecificCulture("en-US") ) / 100;
                        beneficioNuevo.Valor = ( Math.Round(calculador.ObtenerPrecio(participanteNuevo, participanteNuevo.Cantidad ) *
                            (valor), 2) ).ToString( new CultureInfo("en-US") );
                    }
                }
                participanteNuevo.AplicarValorAAtributo( beneficioNuevo.Atributo, beneficioNuevo.Cambio, beneficioNuevo.Valor );
                this.informante.InformarItemBeneficiado( info, promo.Id, participanteNuevo, beneficioNuevo );
            }
            this.AplicarRedondeo( beneficiados, beneficio, promo, factor, tope, info );
            this.CorreccionPorAjuste( beneficiados, beneficio, aplicoTope, tope, info, promo, factor );

            itemsConsumidos.AddRange( beneficiados );
            return itemsConsumidos;
        }

        private void AplicarRedondeo(List<IParticipante> beneficiados, Beneficio beneficio, Promocion promo, decimal factor, decimal tope, InformacionPromocion info)
        {
            if (promo.ObjetoRedondeo != null)
            {

                decimal totalBeneficioPorFactor = (beneficiados.Sum(x => (decimal)x.ObtenerAtributo(configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].MontoDescuento).Valor)) / factor;
                if (totalBeneficioPorFactor < tope || tope == 0)
                {
                    //decimal totalBeneficioPorFactorRedondeado = (decimal)(double)HerramientasCom.EjecutarMetodo(promo.ObjetoRedondeo, "Redondear", totalBeneficioPorFactor);
                    decimal totalBeneficioPorFactorRedondeado = (decimal)(double)promo.ObjetoRedondeo.Redondear((double)totalBeneficioPorFactor);
                    if (totalBeneficioPorFactorRedondeado > tope && tope != 0)
                    {
                        totalBeneficioPorFactorRedondeado = tope;
                    }
                    decimal diferencia = (totalBeneficioPorFactorRedondeado - totalBeneficioPorFactor) * factor;
                    decimal diferenciaPorBeneficiados = diferencia / beneficiados.Count();
                    foreach (IParticipante bene in beneficiados)
                    {
                        decimal nuevoDescuento = (decimal)bene.ObtenerAtributo(configuracionComportamiento.ConfiguracionesPorParticipante[bene.Clave].MontoDescuento).Valor + diferenciaPorBeneficiados;
                        if (nuevoDescuento > (bene.ObtenerPrecioUnitario() * bene.Cantidad))
                        {
                            nuevoDescuento = (bene.ObtenerPrecioUnitario() * bene.Cantidad);
                        }
                        bene.AplicarValorAAtributo(configuracionComportamiento.ConfiguracionesPorParticipante[bene.Clave].MontoDescuento, beneficio.Cambio, nuevoDescuento.ToString(new CultureInfo("en-US")));
                        informante.InformarActualizacionAtributoItemBeneficiado(info, promo.Id, bene, beneficio, configuracionComportamiento.ConfiguracionesPorParticipante[bene.Clave].MontoDescuento);
                    }
                }
            }
        }

        private void CorreccionPorAjuste(List<IParticipante> beneficiados, Beneficio beneficio, bool aplicoTope, decimal tope, InformacionPromocion info, Promocion promo, decimal factor)
        {
            if (beneficio.Atributo == this.configuracionComportamiento.AtributoMontoFinal)
            {
                decimal totalPosta = beneficiados.Sum(x => Math.Round(this.calculador.ObtenerPrecio(x, x.Cantidad), 2));
                decimal totalPromo = Convert.ToDecimal(beneficio.Valor, new CultureInfo("en-US")) * factor;

                decimal diferencia = totalPromo - totalPosta;

                if (diferencia != 0)
                {
                    IParticipante benefici = beneficiados.OrderByDescending(x => this.calculador.ObtenerPrecio(x, 1)).First();
                    benefici.AplicarValorAAtributo(configuracionComportamiento.ConfiguracionesPorParticipante[benefici.Clave].MontoDescuento, beneficio.Cambio, ((decimal)benefici.ObtenerAtributo(this.configuracionComportamiento.ConfiguracionesPorParticipante[benefici.Clave].MontoDescuento).Valor - diferencia).ToString(CultureInfo.InvariantCulture));
                    this.informante.InformarActualizacionAtributoItemBeneficiado(info, promo.Id, benefici, beneficio, configuracionComportamiento.ConfiguracionesPorParticipante[benefici.Clave].MontoDescuento);
                }
            }
            else
            {
                if (aplicoTope)
                {
                    if (beneficiados.Count == 1)
                    {
                        IParticipante beneficiado = beneficiados[0];
                        if (!beneficiado.Clave.ToUpper().Contains("VALORESDETALLE"))
                        {
                            beneficiado.AplicarValorAAtributo(this.configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].MontoDescuento, beneficio.Cambio, tope.ToString(new CultureInfo("en-US")));
                            this.informante.InformarActualizacionAtributoItemBeneficiado(info, promo.Id, beneficiado, beneficio, configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].MontoDescuento);
                        }
                    }
                }
            }
        }

        private List<CoincidenciaEvaluacion> CrearCoincidenciaPorErrorSerializado( Promocion promo, List<IParticipante> participantesConsumidos, List<CoincidenciaEvaluacion> coincidencias )
        {
            List<IParticipante> participantesOrdenadosPorMonto;

            if ( promo.EleccionParticipante.Equals( EleccionParticipanteType.AplicarAlDeMayorPrecio ) )
            {
                participantesOrdenadosPorMonto = ( from x in participantesConsumidos
                                                   where
                                                        !this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].ConsumePorMonto
                                                        &&
                                                        this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].EsConsumible
                                                   orderby this.calculador.ObtenerPrecio( x, 1 ) descending
                                                   select x
                                                 ).ToList();
            }
            else
            {
                participantesOrdenadosPorMonto = ( from x in participantesConsumidos
                                                   where
                                                        !this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].ConsumePorMonto
                                                        &&
                                                        this.configuracionComportamiento.ConfiguracionesPorParticipante[x.Clave].EsConsumible
                                                   orderby this.calculador.ObtenerPrecio( x, 1 ) ascending
                                                   select x
                                                 ).ToList();
            }

            List<CoincidenciaEvaluacion> retorno = new List<CoincidenciaEvaluacion>();
            participantesOrdenadosPorMonto.ForEach( x => retorno.AddRange(
                                                    ( from y in coincidencias
                                                      where y.IdParticipanteEnComprobante.Contains( x.Id )
                                                      select y
                                                    ).ToList() ) 
                                                  );

            return retorno;
        }

        #region Coming soon...
        /*
        private void CorreccionPorRedondeos( IComprobante comprobante , List<IParticipante> beneficiados, Beneficio beneficio )
        {
            if ( beneficio.Atributo == this.configuracionComportamiento.AtributoMontoFinal )
            {
                CalculadorMonto i = new CalculadorMonto( this.configuracionComportamiento );

                Decimal totalPosta = beneficiados.Sum( x => i.ObtenerPrecio( x, x.Cantidad ) );
                Decimal totalPromo = Convert.ToDecimal( beneficio.Valor );

                Decimal diferencia = totalPromo - totalPosta;

                IParticipante masBarato = beneficiados.OrderBy( x => i.ObtenerPrecio(x, 1) ).First();

                Decimal precioObjetivo = i.ObtenerPrecio( masBarato, masBarato.Cantidad ) + diferencia;

                IParticipante participanteOriginal = comprobante.ObtenerNodoParticipante( masBarato.Clave, masBarato.Id, "", "" );
                //IParticipante participantePromocionado = comprobante.ObtenerNodoParticipante( beneficiado.Clave, beneficiado.Id, beneficiado.Promocion, beneficiado.Beneficio );

                throw new NotImplementedException( totalPosta.ToString( new CultureInfo( "En-us" ) ) );
            }
        }
        */
        #endregion

        private Beneficio ObtenerBeneficioFinal( Beneficio beneficio, List<IParticipante> beneficiados, decimal tope, decimal factor )
        {
            Beneficio retorno = beneficio.Clonar();

            string tipoParticipante = beneficiados.FirstOrDefault().Clave;

            if ( beneficio.Atributo.Equals( this.configuracionComportamiento.AtributoMontoFinal ) )
            {
                retorno.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[tipoParticipante].Descuento;
                retorno.Cambio = Alteracion.CambiarValor;
                retorno.Valor = this.ObtenerPorcentajeDescuentoSegunTotal( beneficiados, beneficio.Valor, factor );
            }

			if ( beneficio.Atributo == this.configuracionComportamiento.AtributoBeneficioPorMonto )
			{
				retorno.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[tipoParticipante].Precio;
				retorno.Cambio = Alteracion.CambiarValor;
				retorno.Valor = this.ObtenerMontoABeneficiarSegunTope( beneficiados, beneficio.Valor, tope );
			}

			if ( tope>0 && beneficio.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[tipoParticipante].Descuento )
			{
				retorno.Atributo = beneficio.Atributo;
				retorno.Cambio = Alteracion.CambiarValor;
				retorno.Valor = this.ObtenerDescuentoABeneficiarSegunTope( beneficiados, beneficio.Valor, tope );
			}
			
			/*
			switch ( beneficio.Atributo )
			{
				case "<<MONTOFINAL>>":
					retorno.Atributo = this.configuracionComportamiento.DescuentoEnItem;
					retorno.Cambio = Alteracion.CambiarValor;
					retorno.Valor = this.ObtenerPorcentajeDescuentoSegunTotal( beneficiados, beneficio.Valor );
					break;
				default:
					break;
			}
			*/
            return retorno;
        }

		private object ObtenerDescuentoABeneficiarSegunTope( List<IParticipante> beneficiados, object valorEnBeneficio, decimal tope )
		{
            //if ( beneficiados.Count() > 1 )
            //{
            //	throw new NotImplementedException();
            //}
            decimal total = 0;
            decimal preciototal = 0;

            foreach (var beneficiado in beneficiados)
            {
                decimal precio = (decimal)beneficiado.ObtenerAtributo(this.configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].Precio).Valor;
                decimal descuento = (decimal)beneficiado.ObtenerAtributo(this.configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].Descuento).Valor;
                decimal cantidad = (decimal)beneficiado.ObtenerCantidadSinRestarConsumido(this.configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].Cantidad).Valor;
                decimal monto = precio * cantidad * (1 - descuento / 100);

                total += monto;
                preciototal += precio * cantidad;
            }
			
			decimal factor = Convert.ToDecimal( valorEnBeneficio, new CultureInfo( "en-US" ) );

			decimal nuevoTotal = total * (1 - factor / 100);

			decimal diferencia = nuevoTotal - total;

			if ( tope > 0 && (diferencia * -1) > tope )
			{
                //factor = Math.Round( (tope / precio) * 100, 2 );
                factor = (tope / preciototal) * 100;
            }

			return factor.ToString( new CultureInfo( "en-US" ) );
		}

        private object ObtenerMontoABeneficiarSegunTope( List<IParticipante> beneficiados, object valorEnBeneficio, decimal tope )
        {
            if ( beneficiados.Count() > 1 )
            {
                throw new NotImplementedException();
            }

            IParticipante valor = beneficiados[0];

            decimal precio = (decimal)valor.ObtenerAtributo( this.configuracionComportamiento.ConfiguracionesPorParticipante[valor.Clave].Precio ).Valor;
            decimal recargo = (decimal)valor.ObtenerAtributo( this.configuracionComportamiento.ConfiguracionesPorParticipante[valor.Clave].Descuento ).Valor;
            decimal monto = precio * (1 + recargo / 100);

            decimal total = monto;
            decimal factor = Convert.ToDecimal( valorEnBeneficio, new CultureInfo( "en-US" ) );

            decimal nuevoTotal = total * (1 - factor / 100);

            decimal diferencia = nuevoTotal - total;

            if ( tope > 0 && (diferencia * -1) > tope )
            {
                diferencia = tope * -1;
            }

            return diferencia;
        }

        private object ObtenerPorcentajeDescuentoSegunTotal( List<IParticipante> beneficiados, object totalConPromo, decimal factor )
        {
            decimal totalSinPromo = beneficiados.Sum( x => x.ObtenerPrecioUnitario() * x.Cantidad );

            decimal descuento = 0;

            if ( totalSinPromo != 0 )
            {
                descuento = (1 - ((Convert.ToDecimal(totalConPromo, new CultureInfo("en-US")) * factor) / totalSinPromo)) * 100;
            }

            return descuento.ToString( new CultureInfo( "en-US" ) );
        }
    }
} 