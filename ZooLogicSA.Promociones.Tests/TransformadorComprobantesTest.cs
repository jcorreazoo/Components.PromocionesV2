using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;
using System.Globalization;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class TransformadorComprobantesTest
    {
        public string ObtenerComprobanteParaPruebasTransformacion()
        {
            XmlDocument comprobanteXml = new XmlDocument();
            comprobanteXml.LoadXml( Resources.ComprobanteBasicoPruebas );

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ComprobanteXML comprobante = new ComprobanteXML( config );
            return comprobanteXml.InnerXml;
        }

        public string ObtenerComprobanteCompletoParaPruebasTransformacion()
        {
            XmlDocument comprobanteXml = new XmlDocument();
            comprobanteXml.LoadXml(Resources.ComprobanteCompleto);

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ComprobanteXML comprobante = new ComprobanteXML(config);
            return comprobanteXml.InnerXml;
        }

        [TestMethod()]
        public void Transformar_CambiarValor_Test()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            
            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            beneficio.Valor = 100;
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 2;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion


            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            Assert.AreEqual( 100, info.DetalleBeneficiado[0].Valor, "No aplico el beneficio al atributo esperado" );
        }

        [TestMethod()]
        public void Transformar_CambiarValor_DistribuirBeneficioVariosItems_Test()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 5 } );
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            beneficio.Valor = "100";
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "0" );
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 6;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            InformacionPromocion promocionAplicada = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            Assert.AreEqual( 1, promocionAplicada.DetalleAfectado[0].Cantidad, "No aplico el beneficio al atributo esperado" );
            Assert.AreEqual( 4, promocionAplicada.DetalleBeneficiado[0].Cantidad, "No aplico el beneficio al atributo esperado" );
            Assert.AreEqual( 1, promocionAplicada.DetalleBeneficiado[1].Cantidad, "No aplico el beneficio al atributo esperado" );
        }

        [TestMethod()]
        public void Transformar_CambiarValor_DistribuirBeneficioVariosItems_ConsumeIgualACantidad_Test()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 4 } );
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            beneficio.Valor = "100";
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "0" );
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 4;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            XmlNodeList nodos = comprobante.ObtenerXml().SelectNodes( "Comprobante/Facturadetalle/Item" );

            Assert.AreEqual( "0", nodos[0].SelectSingleNode( "Cantidad" ).Attributes["Valor"].Value, "No aplico cantidad correcta al item 0" );

            Assert.AreEqual( "4", nodos[1].SelectSingleNode( "Cantidad" ).Attributes["Valor"].Value, "No aplico cantidad correcta al item 1" );

            Assert.AreEqual( "4", nodos[2].Attributes["Consumido"].Value, "No aplico el consumido correcto al item 2" );
            Assert.AreEqual( "4", nodos[2].SelectSingleNode( "Cantidad" ).Attributes["Valor"].Value, "No aplico cantidad correcta al item 2" );

        }

        [TestMethod()]
        public void Transformar_DisminuirEnCantidad_Test()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Cambio = Alteracion.DisminuirEnCantidad;
            beneficio.Atributo = "Precio";
            beneficio.Valor = "1";
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            XmlElement nodoAssert = (XmlElement)comprobante.ObtenerXml().SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos/@Valor='1' and @Promo='1' and not(@Beneficio='')]/Precio" );
            Assert.AreEqual( "9", nodoAssert.Attributes["Valor"].Value, "No aplico el beneficio al atributo esperado" );
        }

        [TestMethod()]
        public void Transformar_IncrementarEnCantidad_Test()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Cambio = Alteracion.IncrementarEnCantidad;
            beneficio.Atributo = "Precio";
            beneficio.Valor = "2";
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            ConfiguracionComportamiento comportamiento = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            //IInformantePromociones informante = new InformantePromociones();
            //Assert.Inconclusive( "Resucitar!!!" );
            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            XmlElement nodoAssert = (XmlElement)comprobante.ObtenerXml().SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos/@Valor='1' and @Promo='1' and not(@Beneficio='')]/Precio" );
            Assert.AreEqual( "12", nodoAssert.Attributes["Valor"].Value, "No aplico el beneficio al atributo esperado" );
        }

        [TestMethod()]
        public void Transformar_ConsumirTodosLosParticipantes_Test()
        {
            ConfiguracionComportamiento configuracionComportamiento = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            IInformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( configuracionComportamiento, calculador, informante );
            FactoriaPromociones factoria = new FactoriaPromociones();
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( configuracionComportamiento );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Promocion
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            #region participantes
            ParticipanteRegla participante;
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";
            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region beneficio poner 100 al descuento, al participante 2 de la promo
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            beneficio.Valor = "100";
            promocion.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 2;
            respuestaEvaluacion.Coincidencias.Add( coincidencia );

            coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "0" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;

            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            //Assert.Inconclusive( "Resucitar!!!" );

            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            XmlNodeList nodos = comprobante.ObtenerXml().SelectNodes( "Comprobante/Facturadetalle/Item" );

            Assert.AreEqual( "1", nodos[2].Attributes["Consumido"].Value, "No aplico el consumido correcto al item 2" );

        }

        [TestMethod()]
        public void TransformadorComprobante_MarcarConsumidosRespetandoValoresOriginales()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( this.ObtenerComprobanteParaPruebasTransformacion() );

            #region Lib Promos
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 2 Item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 3;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            Promocion promocion2 = new Promocion();
            promocion2.Id = "2";
            promocion2.Recursiva = true;

            #region participante 2 Item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{2}";
            promocion2.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion2.Beneficios.Add( beneficio );
            #endregion
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            //coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 3;
            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion2 = new RespuestaEvaluacion();
            respuestaEvaluacion2.Promocion = "2";

            coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "0" );
            //coincidencia.IdParticipanteEnComprobante.Add( "idMock" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;
            respuestaEvaluacion2.Coincidencias.Add( coincidencia );
            #endregion

            FactoriaPromociones factoria = new FactoriaPromociones();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();

            InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            target.FactoriaPromociones = factoria;
            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            InformacionPromocion info2 = target.Transformar( comprobante, promocion, respuestaEvaluacion2, false, 0 );

            XmlNodeList nodos = comprobante.ObtenerXml().SelectNodes( "Comprobante/Facturadetalle/Item" );

            Assert.AreEqual( "1", nodos[2].Attributes["Consumido"].Value, "No aplico el consumido correcto al item nuevo (2)" );
            Assert.AreEqual( "2", nodos[3].Attributes["Consumido"].Value, "No aplico el consumido correcto al item nuevo (3)" );
        }

        [TestMethod()]
        public void BUG6323_DosCondicionesIgualesMismoItem()
        {
            Serializador serializador = new Serializador();
            string xml = Resources.XMLTestBUG6323_Promocion;

            Promocion promocion = serializador.DeserializarPromocion("cod", "", xml);

            ConfiguracionComportamiento config = new ConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Precio = "PRECIOCONIMPUESTOS";
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].CantidadMonto = "MONTO";
            IComprobante comprobante = new ComprobanteXML(config);
            comprobante.Cargar(Resources.ComprobanteBUG6323);
            MotorPromociones motor = new MotorPromociones( config, new FactoriaPromociones());
            List<Promocion> promos = new List<Promocion>();
            promos.Add(promocion);
            motor.EstablecerLibreriaPromociones(promos);
            List<InformacionPromocion> informacion = motor.EvaluarYAplicarPromocion(comprobante, "P");

            Assert.AreEqual( informacion.Count, 1);
            Assert.AreEqual(informacion[0].Afectaciones, 1);
            Assert.AreEqual( informacion[0].DetalleBeneficiado.Count , 1);
            Assert.AreEqual( informacion[0].DetalleAfectado.Count , 1);
            Assert.AreEqual( informacion[0].DetalleBeneficiado[0].Clave, "COMPROBANTE.FACTURADETALLE.ITEM");
            Assert.AreEqual( informacion[0].DetalleBeneficiado[0].AtributoAlterado, "MONTODESCUENTO" );
            Assert.AreEqual( informacion[0].DetalleBeneficiado[0].Cantidad, 2 );
            Assert.AreEqual( informacion[0].DetalleBeneficiado[0].ImporteBeneficioTotal, (float)820.96);
            Assert.AreEqual( informacion[0].DetalleAfectado[0].Cantidad, 1);
            Assert.AreEqual( informacion[0].DetalleAfectado[0].Clave, "COMPROBANTE");
        }

        [TestMethod()]
        public void BUG6323_DosCondicionesIgualesDistintosItems()
        {
            Serializador serializador = new Serializador();
            string xml = Resources.XMLTestBUG6323_Promocion;
            ConfiguracionComportamiento config = new ConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Precio = "PRECIOCONIMPUESTOS";
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].CantidadMonto = "MONTO";
            MotorPromociones motor = new MotorPromociones(config, new FactoriaPromociones());
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = serializador.DeserializarPromocion("cod", "", xml);
            promos.Add(promocion);
            XmlDocument comp = new XmlDocument();
            comp.LoadXml(Resources.ComprobanteBUG6323);
            IComprobante comprobante = new ComprobanteXML(config);
            
            XmlNodeList nodos = comp.SelectNodes("COMPROBANTE/FACTURADETALLE/ITEM");
            ((XmlElement)nodos[0].SelectSingleNode("CANTIDAD")).SetAttribute("Valor", "1");
            comprobante.Cargar(comp.InnerXml);

            motor.EstablecerLibreriaPromociones(promos);
            List<InformacionPromocion> informacion = motor.EvaluarYAplicarPromocion(comprobante, "P");

            Assert.AreEqual(informacion.Count, 1);
            Assert.AreEqual(informacion[0].Afectaciones, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado.Count, 2);
            Assert.AreEqual(informacion[0].DetalleAfectado.Count, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].Clave, "COMPROBANTE.FACTURADETALLE.ITEM");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].AtributoAlterado, "MONTODESCUENTO");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].Cantidad, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].ImporteBeneficioTotal, (float)410.48);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].Clave, "COMPROBANTE.FACTURADETALLE.ITEM");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].AtributoAlterado, "MONTODESCUENTO");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].Cantidad, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].ImporteBeneficioTotal, (float)410.48);
            Assert.AreEqual(informacion[0].DetalleAfectado[0].Cantidad, 1);
            Assert.AreEqual(informacion[0].DetalleAfectado[0].Clave, "COMPROBANTE");
        }

        [TestMethod()]
        public void BUG6323_DosCondicionesIgualesDistintosEIgualesItems()
        {
            Serializador serializador = new Serializador();
            string xml = Resources.XMLTestBUG6323_Promocion;
            ConfiguracionComportamiento config = new ConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Precio = "PRECIOCONIMPUESTOS";
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].CantidadMonto = "MONTO";
            MotorPromociones motor = new MotorPromociones(config, new FactoriaPromociones());
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = serializador.DeserializarPromocion("cod", "", xml);
            promos.Add(promocion);
            XmlDocument comp = new XmlDocument();
            comp.LoadXml(Resources.ComprobanteBUG6323);
            IComprobante comprobante = new ComprobanteXML(config);

            XmlNodeList nodos = comp.SelectNodes("COMPROBANTE/FACTURADETALLE/ITEM");
            ((XmlElement)nodos[0].SelectSingleNode("CANTIDAD")).SetAttribute("Valor", "3");
            comprobante.Cargar(comp.InnerXml);

            motor.EstablecerLibreriaPromociones(promos);
            List<InformacionPromocion> informacion = motor.EvaluarYAplicarPromocion(comprobante, "P");

            Assert.AreEqual(informacion.Count, 1);
            Assert.AreEqual(informacion[0].Afectaciones, 2);
            Assert.AreEqual(informacion[0].DetalleBeneficiado.Count, 2);
            Assert.AreEqual(informacion[0].DetalleAfectado.Count, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].Clave, "COMPROBANTE.FACTURADETALLE.ITEM");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].AtributoAlterado, "MONTODESCUENTO");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].Cantidad, 3);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].ImporteBeneficioTotal, (float)1231.44);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[0].Id, "1330DD6851C94E1491718FDD13351501510631" );
            Assert.AreEqual(informacion[0].DetalleAfectado[0].Cantidad, 1);
            Assert.AreEqual(informacion[0].DetalleAfectado[0].Clave, "COMPROBANTE");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].Clave, "COMPROBANTE.FACTURADETALLE.ITEM");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].AtributoAlterado, "MONTODESCUENTO");
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].Cantidad, 1);
            Assert.AreEqual(informacion[0].DetalleBeneficiado[1].ImporteBeneficioTotal, (float)410.48);
        }

        [TestMethod()]
        public void TransformadorComprobante_XXX()
        {
            #region Lib Promos
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 2 Item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add( "0" );
            coincidencia.IdParticipanteEnComprobante.Add( "1" );
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;
            respuestaEvaluacion.Coincidencias.Add( coincidencia );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();

            FactoriaPromociones factoria = new FactoriaPromociones();
            IInformantePromociones informante = factoria.ObtenerInformantePromociones( config );

            TransformadorComprobante target = new TransformadorComprobante( config, calculador, informante );
            target.FactoriaPromociones = factoria;
            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( ObtenerComprobanteParaPruebasTransformacion() );

            InformacionPromocion info = target.Transformar( comprobante, promocion, respuestaEvaluacion, false, 0 );

            Assert.AreEqual( 1, info.DetalleBeneficiado.Count, "Mal cantidad de items informados" );
            Assert.AreEqual( "0", info.DetalleBeneficiado[0].Id, "Mal id del item informado" );
            // ahora es 4 porque automaticamente trata de reaplicar la coincidencia, (la promo es recursiva)
            Assert.AreEqual( 4, info.DetalleBeneficiado[0].Cantidad, "Mal cantidad del item informado" );
            Assert.AreEqual( "Descuento", info.DetalleBeneficiado[0].AtributoAlterado, "Mal atributo del item informado" );
            Assert.AreEqual( Alteracion.CambiarValor, info.DetalleBeneficiado[0].Alteracion, "Mal alteracion del item informado" );
            Assert.AreEqual( "30", info.DetalleBeneficiado[0].Valor, "Mal valor del item informado" );
        }
        [TestMethod()]
        public void TransformadorComprobante_ExactitudConTope()
        {
            #region Lib Promos
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 2 Item Cantidad DebeSerIgualA 3
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 3;
            participante.Reglas.Add(regla);
            participante.RelaReglas = "{2}";
            promocion.Participantes.Add(participante);
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add(new DestinoBeneficio() { Participante = "2", Cuantos = 1 });
            beneficio.Valor = "40";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add(beneficio);
            promocion.TopeBeneficio = Convert.ToDecimal(71.15);
            #endregion

            #endregion

            #region RespuestaEvaluacion
            RespuestaEvaluacion respuestaEvaluacion = new RespuestaEvaluacion();
            respuestaEvaluacion.Promocion = "1";

            CoincidenciaEvaluacion coincidencia = new CoincidenciaEvaluacion();
            coincidencia.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            coincidencia.IdParticipanteRestante = new List<string>();
            coincidencia.IdParticipanteEnComprobante = new List<string>();
            coincidencia.IdParticipanteEnComprobante.Add("0");
            coincidencia.IdParticipanteEnComprobante.Add("1");
            coincidencia.IdParticipanteEnRegla = "2";
            coincidencia.Consume = 1;
            respuestaEvaluacion.Coincidencias.Add(coincidencia);
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ICalculadorMonto calculador = MockRepository.GenerateMock<ICalculadorMonto>();
            calculador.Expect(s => s.ObtenerPrecio(null, 4)).IgnoreArguments().Return(200);
            
            FactoriaPromociones factoria = new FactoriaPromociones();
            IInformantePromociones informante = factoria.ObtenerInformantePromociones(config);

            TransformadorComprobante target = new TransformadorComprobante(config, calculador, informante);
            target.FactoriaPromociones = factoria;

            ComprobanteXML comprobante = new ComprobanteXML(config);
            comprobante.Cargar(ObtenerComprobanteCompletoParaPruebasTransformacion());

            InformacionPromocion info = target.Transformar(comprobante, promocion, respuestaEvaluacion, true, promocion.TopeBeneficio);

            Assert.AreEqual(1, info.DetalleBeneficiado.Count, "Mal cantidad de items informados");
            Assert.AreEqual("0", info.DetalleBeneficiado[0].Id, "Mal id del item informado");
            // ahora es 4 porque automaticamente trata de reaplicar la coincidencia, (la promo es recursiva)
            Assert.AreEqual(1, info.DetalleBeneficiado[0].Cantidad, "Mal cantidad del item informado");
            Assert.AreEqual("MontoDescuento", info.DetalleBeneficiado[0].AtributoAlterado, "Mal atributo del item informado");
            Assert.AreEqual(Alteracion.CambiarValor, info.DetalleBeneficiado[0].Alteracion, "Mal alteracion del item informado");
            Assert.AreEqual("71.15", info.DetalleBeneficiado[0].Valor, "Mal valor del item informado");
        }
    }
}