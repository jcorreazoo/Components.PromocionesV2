using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Xml;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Summary description for CasosBugs
    /// </summary>
    [TestClass]
    public class MotorPromociones_InformarPromocionesConCumplimientoParcial
    {
        //[TestMethod]
        //public void InformarPromocionesConCumplimientoParcial_DosParticipantesCumpleUno()
        //{
        //    #region Lib Promos
        //    List<Promocion> promos = new List<Promocion>();
        //    Promocion promocion = new Promocion();
        //    promocion.Id = "1";
        //    promocion.Recursiva = true;

        //    ParticipanteRegla participante;
        //    Regla regla;

        //    #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
        //    participante = new ParticipanteRegla();
        //    participante.Id = "1";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region part 2: Comprobante.Facturadetalle Articulo.Codigo DebeSerIgualA ART105
        //    participante = new ParticipanteRegla();
        //    participante.Id = "2";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART105";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region Beneficio part 1 tiene 20 de descuento
        //    Beneficio beneficio = new Beneficio();
        //    beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
        //    beneficio.Atributo = "Monto";
        //    beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
        //    beneficio.Valor = "20";

        //    promocion.Beneficios.Add( beneficio );
        //    #endregion

        //    promos.Add( promocion );
        //    #endregion

        //    ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

        //    MotorPromociones motorPromociones = new MotorPromociones( config );
        //    motorPromociones.EstablecerLibreriaPromociones( promos );

        //    string idProceso = "IdProceso";
            
        //    motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );
        //    IComprobante comprobante = motorPromociones.ObtenerCopiaDeComprobante( idProceso );

        //    List<InformacionPromocionIncumplida> respuesta = motorPromociones.ObtenerResultadosParciales( comprobante, "1" );

        //    Assert.AreEqual( 2, respuesta[0].Resultados.Count, "2 resultados, uno por cada regla" );
        //    Assert.AreEqual( 1, respuesta[0].Resultados[0].Participantes.Count, "primer resultado, un posible participante" );
        //    Assert.AreEqual( 0, respuesta[0].Resultados[1].Participantes.Count, "segundo resultado, ningun posible participante" );
        //}

        //[TestMethod]
        //public void InformarPromocionesConCumplimientoParcial_UnParticipanteCumpleDosItemsOtroNoCumple()
        //{
        //    #region Lib Promos
        //    List<Promocion> promos = new List<Promocion>();
        //    Promocion promocion = new Promocion();
        //    promocion.Id = "1";
        //    promocion.Recursiva = true;

        //    ParticipanteRegla participante;
        //    Regla regla;

        //    #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
        //    participante = new ParticipanteRegla();
        //    participante.Id = "1";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
        //    participante = new ParticipanteRegla();
        //    participante.Id = "2";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART105";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region Beneficio part 1 tiene 20 de descuento
        //    Beneficio beneficio = new Beneficio();
        //    beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
        //    beneficio.Atributo = "Monto";
        //    beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
        //    beneficio.Valor = "20";

        //    promocion.Beneficios.Add( beneficio );
        //    #endregion

        //    promos.Add( promocion );
        //    #endregion

        //    ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

        //    MotorPromociones motorPromociones = new MotorPromociones( config );
        //    motorPromociones.EstablecerLibreriaPromociones( promos );

        //    string idProceso = "IdProceso";

        //    XmlDocument comprobanteXML = new XmlDocument();
        //    comprobanteXML.LoadXml( Resources.ComprobanteBasicoPruebas );
        //    XmlNode nodoItem2 = comprobanteXML.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='0']]" ).Clone();
        //    nodoItem2.SelectSingleNode( "IdItemArticulos" ).Attributes["Valor"].Value = "2";
        //    comprobanteXML.SelectSingleNode( "Comprobante/Facturadetalle" ).AppendChild( nodoItem2 );

        //    motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );
        //    IComprobante comprobante = motorPromociones.ObtenerCopiaDeComprobante( idProceso );

        //    List<InformacionPromocionIncumplida> respuesta = motorPromociones.ObtenerResultadosParciales( comprobante, "1" );
        //    Assert.AreEqual( 2, respuesta[0].Resultados.Count, "2 resultados, uno por cada regla" );
        //    Assert.AreEqual( 2, respuesta[0].Resultados[0].Participantes.Count, "primer resultado, un posible participante" );
        //    Assert.AreEqual( 0, respuesta[0].Resultados[1].Participantes.Count, "segundo resultado, ningun posible participante" );
        //}

        //[TestMethod]
        //public void InformarPromocionesConCumplimientoParcial_MismoItemCumpleDosParticipantesOtroIncumplido()
        //{
        //    #region Lib Promos
        //    List<Promocion> promos = new List<Promocion>();
        //    Promocion promocion = new Promocion();
        //    promocion.Id = "1";
        //    promocion.Recursiva = true;

        //    ParticipanteRegla participante;
        //    Regla regla;

        //    #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
        //    participante = new ParticipanteRegla();
        //    participante.Id = "1";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
        //    participante = new ParticipanteRegla();
        //    participante.Id = "2";
        //    participante.Codigo = "Comprobante.Facturadetalle.Item";

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Familia";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "01";
        //    participante.Reglas.Add( regla );

        //    participante.RelaReglas = "{1}";
        //    promocion.Participantes.Add( participante );
        //    #endregion

        //    #region Beneficio part 1 tiene 20 de descuento
        //    Beneficio beneficio = new Beneficio();
        //    beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
        //    beneficio.Atributo = "Monto";
        //    beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
        //    beneficio.Valor = "20";

        //    promocion.Beneficios.Add( beneficio );
        //    #endregion

        //    promos.Add( promocion );
        //    #endregion

        //    ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

        //    MotorPromociones motorPromociones = new MotorPromociones( config );
        //    motorPromociones.EstablecerLibreriaPromociones( promos );

        //    string idProceso = "IdProceso";

        //    XmlDocument comprobanteXML = new XmlDocument();
        //    comprobanteXML.LoadXml( Resources.ComprobanteBasicoPruebas );
        //    XmlNode nodoItem2 = comprobanteXML.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='1']]" );
        //    nodoItem2.SelectSingleNode( "Articulo/Familia" ).Attributes["Valor"].Value = "02";
        //    comprobanteXML.SelectSingleNode( "Comprobante/Facturadetalle" ).AppendChild( nodoItem2 );

        //    motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );
        //    IComprobante comprobante = motorPromociones.ObtenerCopiaDeComprobante( idProceso );

        //    List<InformacionPromocionIncumplida> respuesta = motorPromociones.ObtenerResultadosParciales( comprobante, "1" );

        //    Assert.AreEqual( 2, respuesta[0].Resultados.Count, "2 resultados, uno por cada regla" );
        //    Assert.AreEqual( 1, respuesta[0].Resultados[0].Participantes.Count, "primer resultado, un posible participante" );
        //    Assert.AreEqual( 1, respuesta[0].Resultados[1].Participantes.Count, "primer resultado, un posible participante" );

        //}
    }
}
