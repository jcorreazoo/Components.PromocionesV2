using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.ComprobanteXml;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Utils;
using ZooLogicSA.Redondeos;
using System.Xml.Linq;

namespace ZooLogicSA.Promociones
{
    public class FactoriaPromociones : IFactoriaPromociones
    {
        public List<Promocion> ObtenerListaPromociones()
        {
            return new List<Promocion>();
        }

        public object ObtenerListaRedondeos()
        {
            return new object();
        }

        public List<object> ObtenerRedondeosFox()
        {
            return new List<object>();
        }


        public IValidadorPromociones ObtenerValidadoresPromociones( ConfiguracionComportamiento configuracionComportamiento )
        {
            List<IEvaluadorReglasDeParticipante> evaluadores = ObtenerEvaluadoresDeReglas( configuracionComportamiento );

            return new ValidadorPromociones( configuracionComportamiento, evaluadores );
        }

        private List<IEvaluadorReglasDeParticipante> ObtenerEvaluadoresDeReglas( ConfiguracionComportamiento configuracionComportamiento )
        {
            List<IEvaluadorReglasDeParticipante> retorno = new List<IEvaluadorReglasDeParticipante>();

			GestorComparaciones gestorComparaciones = new GestorComparaciones();

			ICalculadorMonto c = this.ObtenerCalculadorMonto( configuracionComportamiento );

			retorno.Add( new EvaluadorReglasDeParticipante( new EvaluadorReglasDeParticipanteNoCantidad( configuracionComportamiento ), gestorComparaciones ) );
			retorno.Add( new EvaluadorReglasDeParticipante( new EvaluadorReglasDeParticipanteCantidad( configuracionComportamiento ), gestorComparaciones ) );
			retorno.Add( new EvaluadorReglasDeParticipante( new EvaluadorReglasDeParticipanteCantidadMonto( configuracionComportamiento, c ), gestorComparaciones ) );

            return retorno;
        }

        public TransformadorComprobante ObtenerTransformadorDeComprobantes( ConfiguracionComportamiento configuracionComportamiento )
        {
            ICalculadorMonto calculadorMonto = this.ObtenerCalculadorMonto( configuracionComportamiento );
            IInformantePromociones informantePromociones = this.ObtenerInformantePromociones( configuracionComportamiento );

            return new TransformadorComprobante( configuracionComportamiento, calculadorMonto, informantePromociones );
        }

        public IComprobante ObtenerNuevoComprobante( ConfiguracionComportamiento configuracionComportamiento )
        {
            return new ComprobanteXML( configuracionComportamiento );
        }

        public IComparadorDeParticipantes ObtenerComparadorDeParticipantes( ConfiguracionComportamiento configuracionComportamiento )
        {
            return new ComparadorDeParticipantesXML();
        }

        public Dictionary<string, IEvaluadorMatematico> ObtenerEvaluadoresMatematicos( ConfiguracionComportamiento comportamiento )
        {
            Dictionary<string, IEvaluadorMatematico> retorno = new Dictionary<string, IEvaluadorMatematico>();

            foreach ( KeyValuePair<string, ConfiguracionPorParticipante> configuracion in comportamiento.ConfiguracionesPorParticipante )
            {
                retorno.Add( configuracion.Key, RepositoriosEvaluadorMatematico.ObtenerInstancia( configuracion.Value.FormulaCalculoPrecio ) );
            }

            return retorno;
        }

        public IInformantePromociones ObtenerInformantePromociones( ConfiguracionComportamiento configuracionComportamiento )
        {
            return new InformantePromociones( configuracionComportamiento );
        }

        public CalculadorMontoBeneficio ObtenerCalculadorPrecios( ConfiguracionComportamiento configuracionComportamiento )
        {
            ICalculadorMonto calculadorMonto = this.ObtenerCalculadorMonto( configuracionComportamiento );

            return new CalculadorMontoBeneficio( configuracionComportamiento, calculadorMonto );
        }

        public Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes> ObtenerSeleccionadoresParticipantes()
        {
            Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes> retorno = new Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes>();

            retorno.Add( EleccionParticipanteType.AplicarAlDeMenorPrecio, new SeleccionadorParticipantesPorMenorPrecio() );
            retorno.Add( EleccionParticipanteType.AplicarAlDeMayorPrecio, new SeleccionadorParticipantesPorMayorPrecio() );
            retorno.Add( EleccionParticipanteType.AplicarATodos, new SeleccionadorParticipantesPorMenorPrecio() );
            retorno.Add( EleccionParticipanteType.None, new SeleccionadorParticipantesPorMenorPrecio() );

            return retorno;
        }

        public IArmadorDeCoincidencias ObtenerArmadorDeCoincidencias( ConfiguracionComportamiento comportamiento )
        {
            Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes> selecccionadoresParticipantes = this.ObtenerSeleccionadoresParticipantes();
            return new ArmadorDeCoincidenciasPorUsabilidad( comportamiento, selecccionadoresParticipantes );
        }

        public List<InformacionPromocion> ObtenerListaInformacionPromocion()
        {
            return new List<InformacionPromocion>();
        }

        public NotificadorServicioPromociones ObtenerNotificadorServicioPromociones()
        {
            return new NotificadorServicioPromociones();
        }

        public CalculadorDeIncumplimiento ObtenerCalculadorDeIncumplimiento( ConfiguracionComportamiento configuracionComportamiento )
        {
            IExtractorDeCoincidenciasDesdeConsumo extractor = new ExtractorDeCoincidenciasDesdeConsumo();
            ICalculadorDePartipantesCumplidos calculadorCumplidos = new CalculadorDePartipantesCumplidos( configuracionComportamiento );
            ICalculadorDeParticipantesFaltantes calculadorFaltantes = new CalculadorDeParticipantesFaltantes( configuracionComportamiento );
            ISimuladorDeResultadoReglas simuladorDeResultadoReglas = new SimuladorDeResultadoReglas( configuracionComportamiento );
            ICalculadorDeCombinacionesDeParticipantes calculadorCombinaciones = new CalculadorDeCombinacionesDeParticipantes();

            return new CalculadorDeIncumplimiento( configuracionComportamiento, extractor, calculadorCumplidos, calculadorFaltantes, simuladorDeResultadoReglas, calculadorCombinaciones );
        }

        public ICalculadorMonto ObtenerCalculadorMonto( ConfiguracionComportamiento configuracionComportamiento )
        {
            Dictionary<string, IEvaluadorMatematico> evaluadores = this.ObtenerEvaluadoresMatematicos( configuracionComportamiento );
            return new CalculadorMonto( configuracionComportamiento, evaluadores );
        }

        public List<EntidadRedondeo> ObtenerRedondeos(string xmlEntidad, string xmlDetTabla, string xmlDetCent, string xmlDetEnt)
        {
            List<EntidadRedondeo> retorno = new List<EntidadRedondeo>();
            XDocument xdoc = XDocument.Parse(xmlEntidad);
            XDocument xmlTabla = XDocument.Parse(xmlDetTabla);
            XDocument xmlCentavos = XDocument.Parse(xmlDetCent);
            XDocument xmlEnteros = XDocument.Parse(xmlDetEnt);
            var redondeos = xdoc.Element("VFPData").Elements();
            var tabla = xmlTabla.Element("VFPData").Elements();
            var centavo = xmlCentavos.Element("VFPData").Elements();
            var entero = xmlEnteros.Element("VFPData").Elements();
            redondeos.First().Remove();
            tabla.First().Remove();
            centavo.First().Remove();
            entero.First().Remove();
            var pedro = xmlTabla.Element("VFPData").Elements();
            foreach (XElement item in redondeos)
            {
                EntidadRedondeo redo = new EntidadRedondeo();
                redo.Codigo = item.Attribute("codigo").Value.ToString().Trim();
                redo.Descripcion = item.Attribute("descripcion").Value.ToString().Trim();
                redo.RedondeoNormal = ObtenerTipoDeRedondeo(Int32.Parse(item.Attribute("redondeonormal").Value));
                redo.RedondeoPorTabla = ObtenerTipoDeRedondeo(Int32.Parse(item.Attribute("redondeoportabla").Value));
                redo.HabilitaRedondearNormal = Boolean.Parse(item.Attribute("habilitaredondearnormal").Value);
                redo.HabilitaRedondearTermCentavos = Boolean.Parse(item.Attribute("habilitaredondeartermcentavos").Value);
                redo.HabilitaRedondearTermEnteros = Boolean.Parse(item.Attribute("habilitaredondeartermenteros").Value);
                redo.HabilitaRedonearPrecios = Boolean.Parse(item.Attribute("habilitaredonearprecios").Value);
                var detalleTabla = tabla.Where(x => x.Attribute("codigo").Value.ToString().Trim() == redo.Codigo);
                foreach (var itemT in detalleTabla)
                {
                    ItemPorTabla iTabla = new ItemPorTabla();
                    iTabla.Codigo = redo.Codigo;
                    iTabla.Desdeprecio = Convert.ToDouble( itemT.Attribute("desdeprecio").Value, new CultureInfo("en-US"));
                    iTabla.Hastaprecio = Convert.ToDouble(itemT.Attribute("hastaprecio").Value, new CultureInfo("en-US"));
                    iTabla.Redondearen = Convert.ToDouble(itemT.Attribute("redondearen").Value, new CultureInfo("en-US"));
                    redo.DetRedondeoPorTabla.Add(iTabla);
                }

                var detalleCentavo = centavo.Where(x => x.Attribute("codigo").Value.ToString().Trim() == redo.Codigo);
                foreach (var itemC in detalleCentavo)
                {
                    ItemCentavos iCentavo = new ItemCentavos();
                    iCentavo.Codigo = redo.Codigo;
                    iCentavo.Desdeprecio = Convert.ToDouble(itemC.Attribute("desdeprecio").Value, new CultureInfo("en-US"));
                    iCentavo.Hastaprecio = Convert.ToDouble(itemC.Attribute("hastaprecio").Value, new CultureInfo("en-US"));
                    iCentavo.Desde = Convert.ToDouble(itemC.Attribute("desde").Value, new CultureInfo("en-US"));
                    iCentavo.Hasta = Convert.ToDouble(itemC.Attribute("hasta").Value, new CultureInfo("en-US"));
                    iCentavo.Terminacion = Convert.ToDouble(itemC.Attribute("terminacion").Value, new CultureInfo("en-US"));
                    redo.DetRedondeoPorCentavo.Add(iCentavo);
                }
                var detalleEntero = entero.Where(x => x.Attribute("codigo").Value.ToString().Trim() == redo.Codigo);
                foreach (var itemE in detalleEntero)
                {
                    ItemEnteros iEntero = new ItemEnteros();
                    iEntero.Codigo = redo.Codigo;
                    iEntero.Desdeprecio = Convert.ToDouble(itemE.Attribute("desdeprecio").Value, new CultureInfo("en-US"));
                    iEntero.Hastaprecio = Convert.ToDouble(itemE.Attribute("hastaprecio").Value, new CultureInfo("en-US"));
                    iEntero.Desde = Convert.ToDouble(itemE.Attribute("desde").Value, new CultureInfo("en-US"));
                    iEntero.Hasta = Convert.ToDouble(itemE.Attribute("hasta").Value, new CultureInfo("en-US"));
                    iEntero.Terminacion = Convert.ToDouble(itemE.Attribute("terminacion").Value, new CultureInfo("en-US"));
                    iEntero.TerminacionCaracter = itemE.Attribute("terminacioncaracter").Value.ToString().Trim();
                    redo.DetRedondeoPorEntero.Add(iEntero);
                }
                retorno.Add(redo);
            }
            return retorno;
        }

        private TipoDeRedondeo ObtenerTipoDeRedondeo(int valor)
        {
            TipoDeRedondeo retorno = TipoDeRedondeo.Nada;
            switch (valor)
            {
                case 0:
                    retorno = TipoDeRedondeo.Desactivado;
                    break;
                case 1:
                    retorno = TipoDeRedondeo.HaciaArriba;
                    break;
                case 2:
                    retorno = TipoDeRedondeo.HaciaAbajo;
                    break;
                case 3:
                    retorno = TipoDeRedondeo.Normal;
                    break;
            }
            return retorno;
        }
    }
}