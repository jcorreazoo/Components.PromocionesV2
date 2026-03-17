using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;
using System.Xml.Linq;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Summary des cription for TestIntegridad
    /// </summary>
    [TestClass]
    public class TestIntegridad
    {
        [TestMethod]
        public void TestIntegridad_1()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact( "29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "0000000001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = 00100101 OR Item.PRECIO = 200
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = "200";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} or {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Codigo DebeSerIgualA 00100102
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100102";
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{5}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 2 precio baja en 50, Part 3 descuento 100%
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "50";
            beneficio.Cambio = Alteracion.DisminuirEnCantidad;
            beneficio.Atributo = "Precio";
            promocion.Beneficios.Add( beneficio );

            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "3", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );

            promocion = new Promocion();
            promocion.Id = "2";

            #region Comprobante.Fecha DebeSerIgualA  29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "part_002";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";

            promocion.Participantes.Add( participante );

            participante = new ParticipanteRegla();
            participante.Id = "part_003";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 12;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{12}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos descuento 50%
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "part_003", Cuantos = 1 } );
            beneficio.Valor = "50";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );
            //DateTime antes = DateTime.Now;
            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );
            //DateTime despues = DateTime.Now;
            //Assert.Inconclusive((despues-antes).ToString());
            Assert.AreEqual( "1", promocionAplicada[0].IdPromocion, "Mal codigo de primer promo" );

            Assert.AreEqual( "0", promocionAplicada[0].DetalleBeneficiado[0].Id, "Mal codigo de item promo 1 beneficio 1" );
            Assert.AreEqual( 1, promocionAplicada[0].DetalleBeneficiado[0].Cantidad, "Mal cantidad promo 1 beneficio 1" );
            Assert.AreEqual( "Precio", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo promo 1 beneficio 1" );
            Assert.AreEqual( Alteracion.DisminuirEnCantidad, promocionAplicada[0].DetalleBeneficiado[0].Alteracion, "Mal alteracion promo 1 beneficio 1" );
            Assert.AreEqual( "50", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor promo 1 beneficio 1" );

            Assert.AreEqual( "3", promocionAplicada[0].DetalleBeneficiado[1].Id, "Mal codigo de item promo 1 beneficio 2" );
            Assert.AreEqual( 1, promocionAplicada[0].DetalleBeneficiado[1].Cantidad, "Mal cantidad promo 1 beneficio 2" );
            Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[1].AtributoAlterado, "Mal atributo promo 1 beneficio 2" );
            Assert.AreEqual( Alteracion.CambiarValor, promocionAplicada[0].DetalleBeneficiado[1].Alteracion, "Mal alteracion promo 1 beneficio 2" );
            Assert.AreEqual( "100", promocionAplicada[0].DetalleBeneficiado[1].Valor, "Mal valor promo 1 beneficio 2" );

            Assert.AreEqual( "2", promocionAplicada[1].IdPromocion, "Mal codigo segunda promo" );

            Assert.AreEqual( "0", promocionAplicada[1].DetalleBeneficiado[0].Id, "Mal codigo de item promo 2 beneficio 1" );
            Assert.AreEqual( 1, promocionAplicada[1].DetalleBeneficiado[0].Cantidad, "Mal cantidad promo 2 beneficio 1" );
            Assert.AreEqual( "Descuento", promocionAplicada[1].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo promo 2 beneficio 1" );
            Assert.AreEqual( Alteracion.CambiarValor, promocionAplicada[1].DetalleBeneficiado[0].Alteracion, "Mal alteracion promo 2 beneficio 1" );
            Assert.AreEqual( "50", promocionAplicada[1].DetalleBeneficiado[0].Valor, "Mal valor promo 2 beneficio 1" );

        }

        [TestMethod]
        public void TestIntegridad_2x1()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "0000000001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = 00100101 OR Item.PRECIO = 200
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "2";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 2 precio baja en 50, Part 3 descuento 100%
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            Assert.AreEqual( "1", promocionAplicada[0].IdPromocion, "Mal codigo de primer promo" );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];

            Assert.AreEqual( "0", beneficiado1.Id, "Mal id de 1er beneficiado" );
            Assert.AreEqual( 1,   beneficiado1.Cantidad, "Mal cantidad de 1er beneficiado" );

            Assert.AreEqual( "Descuento", beneficiado1.AtributoAlterado, "Mal atributo de 1er beneficiado" );
            Assert.AreEqual( Alteracion.CambiarValor, beneficiado1.Alteracion, "Mal alteracion de 1er beneficiado" );
            Assert.AreEqual( "100", beneficiado1.Valor, "Mal valor de 1er beneficiado" );

            Assert.AreEqual( "Comprobante", afectado1.Clave, "Mal clave de participante 1er afectado" );
            Assert.AreEqual( "guidFactura", afectado1.Id, "Mal codigo de item 1er afectado" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", afectado2.Clave, "Mal clave de participante 2do afectado" );
            Assert.AreEqual( "0", afectado2.Id, "Mal codigo de item 2do afectado" );
            Assert.AreEqual( 1, afectado2.Cantidad, "Mal cantidad de item 2do afectado" );
        }

        [TestMethod]
        public void TestIntegridad_Bug2310_3x1()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact( "29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "0000000001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = 00100101 OR Item.PRECIO = 200
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "3";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 2 precio baja en 50, Part 3 descuento 100%
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo para tener los 3 items con cantidad 1 y el codigo elegible
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[0].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            XmlNode clon = nodos[0].Clone();
            ((XmlElement)clon.SelectSingleNode( "IdItemArticulos" )).SetAttribute( "Valor", "blabla" );
            nodos[0].ParentNode.AppendChild( clon );

            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            Assert.AreEqual( "1", promocionAplicada[0].IdPromocion, "Mal codigo de primer promo" );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];
            ParticipanteAfectado afectado3 = promocionAplicada[0].DetalleAfectado[2];

            Assert.AreEqual( "0", beneficiado1.Id, "Mal id de 1er beneficiado" );
            Assert.AreEqual( 1, beneficiado1.Cantidad, "Mal cantidad de 1er beneficiado" );

            Assert.AreEqual( "Descuento", beneficiado1.AtributoAlterado, "Mal atributo de 1er beneficiado" );
            Assert.AreEqual( Alteracion.CambiarValor, beneficiado1.Alteracion, "Mal alteracion de 1er beneficiado" );
            Assert.AreEqual( "100", beneficiado1.Valor, "Mal valor de 1er beneficiado" );

            Assert.AreEqual( "Comprobante", afectado1.Clave, "Mal clave de participante 1er afectado" );
            Assert.AreEqual( "guidFactura", afectado1.Id, "Mal codigo de item 1er afectado" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", afectado2.Clave, "Mal clave de participante 2do afectado" );
            Assert.AreEqual( "1", afectado2.Id, "Mal codigo de item 2do afectado" );
            Assert.AreEqual( 1, afectado2.Cantidad, "Mal cantidad de item 2do afectado" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", afectado3.Clave, "Mal clave de participante 3er afectado" );
            Assert.AreEqual( "blabla", afectado3.Id, "Mal codigo de item 3er afectado" );
            Assert.AreEqual( 1, afectado3.Cantidad, "Mal cantidad de item 3er afectado" );
        }

        [TestMethod]
        public void TestIntegridad_Bug2510_CantidadDebeSerIgual()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact( "29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = 00100101 OR Item.PRECIO = 200
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "2";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 2 precio baja en 50, Part 3 descuento 100%
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo para tener los 3 items con cantidad 1 y el codigo elegible
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item[Articulo/Codigo/@Valor='00100101']" );
            ((XmlElement)nodos[0].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "2" );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            Assert.AreEqual( "1", promocionAplicada[0].IdPromocion, "Mal codigo de primer promo" );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];

            Assert.AreEqual( "0", beneficiado1.Id, "Mal id de 1er beneficiado" );
            Assert.AreEqual( 1, beneficiado1.Cantidad, "Mal cantidad de 1er beneficiado" );

            Assert.AreEqual( "Descuento", beneficiado1.AtributoAlterado, "Mal atributo de 1er beneficiado" );
            Assert.AreEqual( Alteracion.CambiarValor, beneficiado1.Alteracion, "Mal alteracion de 1er beneficiado" );
            Assert.AreEqual( "100", beneficiado1.Valor, "Mal valor de 1er beneficiado" );

            Assert.AreEqual( "Comprobante", afectado1.Clave, "Mal clave de participante 1er afectado" );
            Assert.AreEqual( "guidFactura", afectado1.Id, "Mal codigo de item 1er afectado" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", afectado2.Clave, "Mal clave de participante 2do afectado" );
            Assert.AreEqual( "0", afectado2.Id, "Mal codigo de item 2do afectado" );
            Assert.AreEqual( 1, afectado2.Cantidad, "Mal cantidad de item 2do afectado" );

        }

        [TestMethod]
        public void TestIntegridad_AplicarAlDeMenorPrecio_Afectados()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region 
            #region Articulo.Familia.Codigo DebeSerIgualA "01"; Cantidad DebeSerIgualA 2
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 2;
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante ); 
            #endregion

            #region Articulo.Grupo.Codigo DebeSerIgualA IMP
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Grupo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "IMP";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante ); 
            #endregion
            #endregion

            #region Beneficios: Part 2 Descuento CambiarValor 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "00" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "IMP" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "BLA" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "IMP" );
            ((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[2].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "1" );

            nodos[0].ParentNode.RemoveChild( nodos[0] );
            #endregion

			//comprobante.Save( @"d:\comprobante.xml" );
			
			string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];

            Assert.AreEqual( "3", beneficiado1.Id, "Mal id de 1er beneficiado" );

            Assert.AreEqual( "2", afectado1.Id, "Mal codigo de item 1er afectado" );

            Assert.AreEqual( "3", afectado2.Id, "Mal codigo de item 2do afectado" );

        }

        [TestMethod]
        public void TestIntegridad_AplicarAlDeMayorPrecio_Afectados()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMayorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region
            #region Articulo.Familia.Codigo DebeSerIgualA "01"; Cantidad DebeSerIgualA 2
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 2;
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Grupo.Codigo DebeSerIgualA IMP
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Grupo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "IMP";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion
            #endregion

            #region Beneficios: Part 2 Descuento CambiarValor 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "00" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "IMP" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "BLA" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Grupo/Codigo" )).SetAttribute( "Valor", "IMP" );
            ((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[2].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "1" );

            nodos[0].ParentNode.RemoveChild( nodos[0] );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];

            Assert.AreEqual( "1", beneficiado1.Id, "Mal id de 1er beneficiado" );

            Assert.AreEqual( "2", afectado1.Id, "Mal codigo de item 1er afectado" );

            Assert.AreEqual( "3", afectado2.Id, "Mal codigo de item 2do afectado" );

        }

        [TestMethod]
        public void TestIntegridad_AplicarAlDeMenorPrecio_Beneficiado()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region Articulo.Familia.Codigo DebeSerIgualA "01"; Cantidad DebeSerIgualA 2
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 3;
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 2 Descuento CambiarValor 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[2].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "2" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "1" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "2" );

            nodos[0].ParentNode.RemoveChild( nodos[0] );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];
            ParticipanteAfectado afectado2 = promocionAplicada[0].DetalleAfectado[1];

            Assert.AreEqual( "2", beneficiado1.Id, "Mal id de 1er beneficiado" );
            Assert.AreEqual( "1", afectado1.Id, "Mal codigo de item 1er afectado" );
            Assert.AreEqual( "3", afectado2.Id, "Mal codigo de item 2do afectado" );

        }

        [TestMethod]
        public void TestIntegridad_MontoFijo()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            IFactoriaPromociones factoria = new FactoriaPromociones();

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "6";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region Articulo.Familia.Codigo DebeSerIgualA "01"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "02"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "02";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "03"
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "03";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 1, 2 y 3 Monto Final es 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "3", Cuantos = 1 } );
            beneficio.Valor = "150";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = config.AtributoMontoFinal;
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "02" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "03" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );

            nodos[0].ParentNode.RemoveChild( nodos[0] );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            Assert.AreEqual( 150F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo" );

            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado (0)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[1].AtributoAlterado, "Mal atributo Alterado (1)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[2].AtributoAlterado, "Mal atributo Alterado (2)");

            Assert.AreEqual("50.0", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio (0)");
            Assert.AreEqual("50.0", promocionAplicada[0].DetalleBeneficiado[1].Valor, "Mal valor beneficio (1)");
            Assert.AreEqual("50.0", promocionAplicada[0].DetalleBeneficiado[2].Valor, "Mal valor beneficio (2)");
        }

        [TestMethod]
        public void TestIntegridad_MontoFijoConDecimales()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            IFactoriaPromociones factoria = new FactoriaPromociones();

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "6";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region Articulo.Familia.Codigo DebeSerIgualA "01"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "02"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "02";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "03"
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "03";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 1, 2 y 3 Monto Final es 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "3", Cuantos = 1 } );
            beneficio.Valor = "100";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = config.AtributoMontoFinal;
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "02" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "03" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "100" );

            nodos[0].ParentNode.RemoveChild( nodos[0] );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            // Un centavo de error, por redondeo loco
            Assert.AreEqual(200.00F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo");

            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado (0)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[1].AtributoAlterado, "Mal atributo Alterado (1)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[2].AtributoAlterado, "Mal atributo Alterado (2)");

            Assert.AreEqual("66.66", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio (0)");
            Assert.AreEqual("66.67", promocionAplicada[0].DetalleBeneficiado[1].Valor, "Mal valor beneficio (1)");
            Assert.AreEqual("66.67", promocionAplicada[0].DetalleBeneficiado[2].Valor, "Mal valor beneficio (2)");
        }

        [TestMethod]
        public void TestIntegridad_MontoFijoQuitarElDelBeneficioNegativo()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            IFactoriaPromociones factoria = new FactoriaPromociones();

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "6";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region Articulo.Familia.Codigo DebeSerIgualA "01"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "02"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "02";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "03"
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "03";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 1, 2 y 3 Monto Final es 100
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "3", Cuantos = 1 } );
            beneficio.Valor = "70";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = config.AtributoMontoFinal;
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[0].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "02" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "03" );
            ((XmlElement)nodos[0].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "60" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "40" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "20" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "20" );
            ((XmlElement)nodos[0].SelectSingleNode("Cantidad")).SetAttribute("Valor", "1");
            ((XmlElement)nodos[1].SelectSingleNode("Cantidad")).SetAttribute("Valor", "1");
            ((XmlElement)nodos[2].SelectSingleNode("Cantidad")).SetAttribute("Valor", "1");
            ((XmlElement)nodos[3].SelectSingleNode("Cantidad")).SetAttribute("Valor", "1");

            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            // Un centavo de error, por redondeo loco
            Assert.AreEqual( 10F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo" );

            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado (0)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[1].AtributoAlterado, "Mal atributo Alterado (1)");
            Assert.AreEqual("MontoDescuento", promocionAplicada[0].DetalleBeneficiado[2].AtributoAlterado, "Mal atributo Alterado (2)");

            Assert.AreEqual("5.00", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio (0)");
            Assert.AreEqual("2.50", promocionAplicada[0].DetalleBeneficiado[1].Valor, "Mal valor beneficio (1)");
            Assert.AreEqual("2.50", promocionAplicada[0].DetalleBeneficiado[2].Valor, "Mal valor beneficio (2)");
        }

        //[TestMethod]
        public void TestIntegridad_MontoFijo_LocuraDelCentavo()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            IFactoriaPromociones factoria = new FactoriaPromociones();

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region Articulo.Familia.Codigo DebeSerIgualA "01"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Familia.Codigo DebeSerIgualA "02"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Familia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "02";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Part 1, 2 Monto Final es 150
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "150";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = config.AtributoMontoFinal;
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            #region bostardaqueo!!!
            XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
            ((XmlElement)nodos[0].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "01" );
            ((XmlElement)nodos[1].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "02" );
            ((XmlElement)nodos[2].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "04" );
            ((XmlElement)nodos[3].SelectSingleNode( "Articulo/Familia/Codigo" )).SetAttribute( "Valor", "04" );
            ((XmlElement)nodos[0].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "200" );
            ((XmlElement)nodos[1].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "220" );
            ((XmlElement)nodos[2].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "20" );
            ((XmlElement)nodos[3].SelectSingleNode( "Precio" )).SetAttribute( "Valor", "20" );

            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

            // Un centavo de error, por redondeo loco
            Assert.AreEqual( 270F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo" );

            Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado (0)" );
            Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[1].AtributoAlterado, "Mal atributo Alterado (1)" );
            Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[2].AtributoAlterado, "Mal atributo Alterado (2)" );

            Assert.AreEqual( "10.0", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio (0)" );
            Assert.AreEqual( "10.0", promocionAplicada[0].DetalleBeneficiado[1].Valor, "Mal valor beneficio (1)" );
            Assert.AreEqual( "10.0", promocionAplicada[0].DetalleBeneficiado[2].Valor, "Mal valor beneficio (2)" );
        }

        [TestMethod]
        public void TestIntegridad_LlevaAValorDeOtraListaDePrecios()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;
            promocion.ListaDePrecios = "LISTA3";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 29/03/11
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "COMPROBANTE";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "FECHA";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add(regla);

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add(participante);
            #endregion

            #region Item.Articulo = A1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "COMPROBANTE.FACTURADETALLE.ITEM";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "ARTICULO.CODIGO";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "A1";
            participante.Reglas.Add(regla);

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "ARTICULO.CODIGO";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "A2";
            participante.Reglas.Add(regla);

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "ARTICULO.CODIGO";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "A3";
            participante.Reglas.Add(regla);

            participante.RelaReglas = "( {3} or {4} or {5} )";
            promocion.Participantes.Add(participante);
            #endregion


            #region Beneficios: Part 2 
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add(new DestinoBeneficio() { Participante = "2", Cuantos = 1 });
            beneficio.Valor = "0";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "MONTODESCUENTO";
            promocion.Beneficios.Add(beneficio);
            #endregion

            promos.Add(promocion);
            #endregion

            ConfiguracionComportamiento config = new ConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Precio = "PRECIOCONIMPUESTOS";
            config.ConfiguracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].CantidadMonto = "MONTO";
            config.ConfiguracionesPorParticipante["COMPROBANTE"].CantidadMonto = "MONTO";

            IFactoriaPromociones factoria = new FactoriaPromociones();
 
            MotorPromociones motorPromociones = new MotorPromociones(config, factoria);
            motorPromociones.EstablecerLibreriaPromociones(promos);

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml(Resources.ComprobanteListaDePrecios);

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion(idProceso, comprobante.InnerXml);

            XmlDocument preciosAdicionales = new XmlDocument();
            preciosAdicionales.LoadXml(Resources.PreciosAdicionales);
            motorPromociones.AgregarPreciosAdicionalesParaEvaluacion(idProceso, preciosAdicionales.InnerXml);

            List<InformacionPromocion> promocionAplicada = motorPromociones.AplicarPromociones(idProceso, new List<string>() { "1" });

            Assert.AreEqual("1", promocionAplicada[0].IdPromocion, "Mal codigo de primer promo");

            ParticipanteBeneficiado beneficiado1 = promocionAplicada[0].DetalleBeneficiado[0];
            ParticipanteBeneficiado beneficiado2 = promocionAplicada[0].DetalleBeneficiado[1];
            ParticipanteAfectado afectado1 = promocionAplicada[0].DetalleAfectado[0];

            Assert.AreEqual("GuidArtic1", beneficiado1.Id, "Mal id de 1er beneficiado");
            Assert.AreEqual(1, beneficiado1.Cantidad, "Mal cantidad de 1er beneficiado");
            Assert.AreEqual("MONTODESCUENTO", beneficiado1.AtributoAlterado.ToUpper(), "Mal atributo de 1er beneficiado");
            Assert.AreEqual(Alteracion.CambiarValor, beneficiado1.Alteracion, "Mal alteracion de 1er beneficiado");
            Assert.AreEqual("20", beneficiado1.Valor, "Mal valor de 1er beneficiado");

            Assert.AreEqual("GuidArtic2", beneficiado2.Id, "Mal id de 2do beneficiado");
            Assert.AreEqual(1, beneficiado2.Cantidad, "Mal cantidad de 2do beneficiado");
            Assert.AreEqual("MONTODESCUENTO", beneficiado2.AtributoAlterado.ToUpper(), "Mal atributo de 2do beneficiado");
            Assert.AreEqual(Alteracion.CambiarValor, beneficiado2.Alteracion, "Mal alteracion de 2do beneficiado");
            Assert.AreEqual("50", beneficiado2.Valor, "Mal valor de 2do beneficiado");

            Assert.AreEqual(2, promocionAplicada[0].DetalleBeneficiado.Count, "Mal la cantidad de ítems beneficiados");

            Assert.AreEqual("COMPROBANTE", afectado1.Clave.ToUpper(), "Mal clave de participante 1er afectado");
            Assert.AreEqual("IdProceso", afectado1.Id, "Mal codigo de item 1er afectado");
        }

    }
}