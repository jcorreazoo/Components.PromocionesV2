using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Summary description for PromocionesBancarias
    /// </summary>
    [TestClass]
    public class PromocionesBancariasTest
    {
        [TestMethod]
        public void PromocionesBancariasTest_NuevaVersionBeneficioSobreTotalDeCupon()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "LED403D";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Valoresdetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cupon.EntidadFinanciera.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "17";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficio part 2 tiene 20 de descuento
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Monto";
            beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
            beneficio.Valor = "20";

            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteConValor );

            #region comprobante
            XmlNode nodoValor1 = comprobante.SelectSingleNode( "Comprobante/Valoresdetalle/Item[IdItemValores[@Valor='0']]" );
            nodoValor1.SelectSingleNode( "Monto" ).Attributes["Valor"].Value = "40";
            nodoValor1.SelectSingleNode( "RecargoPorcentaje" ).Attributes["Valor"].Value = "10";
            #endregion

            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            Assert.AreEqual( 1, respuesta.Count, "Debe aplicar" );
            Assert.AreEqual( 8.8F, respuesta[0].MontoBeneficio, "Mal monto beneficio" );
            Assert.AreEqual( 4, respuesta[0].Afectaciones, "Mal afectaciones" );
            Assert.AreEqual( "1", respuesta[0].IdPromocion, "Mal promocion aplicada" );

            Assert.AreEqual( 2, respuesta[0].DetalleAfectado.Count, "Mal cantidad de afectados" );

            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleAfectado[0].Clave, "Mal clave afectado 0" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[0].Id, "Mal id afectado 0" );
            Assert.AreEqual( "Cupon.EntidadFinanciera.Codigo", respuesta[0].DetalleAfectado[0].Atributos[0], "Mal atrubuto 0 afectado 0" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[0].Atributos[1], "Mal atrubuto 1 afectado 0" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[1].Clave, "Mal clave afectado 1" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[1].Id, "Mal id afectado 1" );
            Assert.AreEqual( "Articulo.Codigo", respuesta[0].DetalleAfectado[1].Atributos[0], "Mal atrubuto 0 afectado 1" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[1].Atributos[1], "Mal atrubuto 1 afectado 1" );

            Assert.AreEqual( 1, respuesta[0].DetalleBeneficiado.Count, "Mal cantidad de beneficiados" );
            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleBeneficiado[0].Clave, "Mal clave beneficiado" );
            Assert.AreEqual( "0", respuesta[0].DetalleBeneficiado[0].Id, "Mal id beneficiado" );
            Assert.AreEqual( 8.8F, respuesta[0].DetalleBeneficiado[0].ImporteBeneficioTotal, "Mal monto beneficiado" );
        }
        
        [TestMethod]
        public void PromocionesBancariasTest_BugDosValores()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "LED403D";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Valoresdetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cupon.EntidadFinanciera.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "17";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficio part 1 tiene 20 de descuento
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Monto";
            beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
            beneficio.Valor = "20";

            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";

            //File.WriteAllText( @"c:\comprobante_antes.xml", Resources.ComprobanteConValor );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteConValor );

            #region comprobante
            XmlNode nodoValor1 = comprobante.SelectSingleNode( "Comprobante/Valoresdetalle/Item[IdItemValores[@Valor='0']]" );
            nodoValor1.SelectSingleNode( "Monto" ).Attributes["Valor"].Value = "40";

            XmlNode nodoValor2 = nodoValor1.Clone();
            nodoValor2.SelectSingleNode( "IdItemValores" ).Attributes["Valor"].Value = "1";

            nodoValor1.ParentNode.AppendChild( nodoValor2 );
            #endregion

            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            //Serializador s = new Serializador();
            //File.WriteAllText( @"c:\promo.xml", s.Serializar<Promocion>( promocion ) );

            Assert.AreEqual( 1, respuesta.Count, "Debe aplicar" );
            Assert.AreEqual( 8, respuesta[0].MontoBeneficio, "Mal monto beneficio" );
            Assert.AreEqual( 4, respuesta[0].Afectaciones, "Mal afectaciones" );
            Assert.AreEqual( "1", respuesta[0].IdPromocion, "Mal promocion aplicada" );

            Assert.AreEqual( 2, respuesta[0].DetalleAfectado.Count, "Mal cantidad de afectados" );

            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleAfectado[0].Clave, "Mal clave afectado 0" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[0].Id, "Mal id afectado 0" );
            Assert.AreEqual( "Cupon.EntidadFinanciera.Codigo", respuesta[0].DetalleAfectado[0].Atributos[0], "Mal atrubuto 0 afectado 0" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[0].Atributos[1], "Mal atrubuto 1 afectado 0" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[1].Clave, "Mal clave afectado 1" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[1].Id, "Mal id afectado 1" );
            Assert.AreEqual( "Articulo.Codigo", respuesta[0].DetalleAfectado[1].Atributos[0], "Mal atrubuto 0 afectado 1" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[1].Atributos[1], "Mal atrubuto 1 afectado 1" );

            Assert.AreEqual( 1, respuesta[0].DetalleBeneficiado.Count, "Mal cantidad de beneficiados" );
            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleBeneficiado[0].Clave, "Mal clave beneficiado" );
            Assert.AreEqual( "0", respuesta[0].DetalleBeneficiado[0].Id, "Mal id beneficiado" );
            Assert.AreEqual( 8, respuesta[0].DetalleBeneficiado[0].ImporteBeneficioTotal, "Mal monto beneficiado" );

        }
        
        [TestMethod]
        public void PromocionesBancariasTest_SimilFrancesGo()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;
            //promocion.TopeBeneficio = 10000;

            ParticipanteRegla participante;
            Regla regla;

            #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "LED403D";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Valoresdetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cupon.EntidadFinanciera.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "17";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficio part 1 tiene 20 de descuento
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Monto";
            beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
            beneficio.Valor = "20";

            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";

            //File.WriteAllText( @"c:\comprobante_antes.xml", Resources.ComprobanteConValor );

            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteConValor );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            //Serializador s = new Serializador();
            //File.WriteAllText( @"c:\promo.xml", s.Serializar<Promocion>( promocion ) );

            Assert.AreEqual( 1, respuesta.Count, "Debe aplicar" );
            Assert.AreEqual( 8, respuesta[0].MontoBeneficio, "Mal monto beneficio" );
            Assert.AreEqual( 4, respuesta[0].Afectaciones, "Mal afectaciones" );
            Assert.AreEqual( "1", respuesta[0].IdPromocion, "Mal promocion aplicada" );

            Assert.AreEqual( 2, respuesta[0].DetalleAfectado.Count, "Mal cantidad de afectados" );

            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleAfectado[0].Clave, "Mal clave afectado 0" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[0].Id, "Mal id afectado 0" );
            Assert.AreEqual( "Cupon.EntidadFinanciera.Codigo", respuesta[0].DetalleAfectado[0].Atributos[0], "Mal atrubuto 0 afectado 0" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[0].Atributos[1], "Mal atrubuto 1 afectado 0" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[1].Clave, "Mal clave afectado 1" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[1].Id, "Mal id afectado 1" );
            Assert.AreEqual( "Articulo.Codigo", respuesta[0].DetalleAfectado[1].Atributos[0], "Mal atrubuto 0 afectado 1" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[1].Atributos[1], "Mal atrubuto 1 afectado 1" );

            Assert.AreEqual( 1, respuesta[0].DetalleBeneficiado.Count, "Mal cantidad de beneficiados" );
            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleBeneficiado[0].Clave, "Mal clave beneficiado" );
            Assert.AreEqual( "0", respuesta[0].DetalleBeneficiado[0].Id, "Mal id beneficiado" );
            Assert.AreEqual( 8, respuesta[0].DetalleBeneficiado[0].ImporteBeneficioTotal, "Mal monto beneficiado" );

        }
        
        [TestMethod]
        public void PromocionesBancariasTest_SimilFrancesGo_ConTope()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;
            promocion.TopeBeneficio = 2;

            ParticipanteRegla participante;
            Regla regla;

            #region part 1: Comprobante.Facturadetalle.Item.Articulo.Codigo DebeSerIgualA "LED403D"
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "LED403D";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region part 2: Comprobante.ValoresDetalle.Item.Cupon.EntidadFinanciera.Codigo DebeSerIgualA "17"
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Valoresdetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cupon.EntidadFinanciera.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "17";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficio part 1 tiene 20 de descuento
            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Monto";
            beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
            beneficio.Valor = "20";

            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";

            //File.WriteAllText( @"c:\comprobante_antes.xml", Resources.ComprobanteConValor );

            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteConValor );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            //Serializador s = new Serializador();
            //File.WriteAllText( @"c:\promo.xml", s.Serializar<Promocion>( promocion ) );

            Assert.AreEqual( 1, respuesta.Count, "Debe aplicar" );
            Assert.AreEqual( 2, respuesta[0].MontoBeneficio, "Mal monto beneficio" );
            Assert.AreEqual( 1, respuesta[0].Afectaciones, "Mal afectaciones" );
            Assert.AreEqual( "1", respuesta[0].IdPromocion, "Mal promocion aplicada" );

            Assert.AreEqual( 2, respuesta[0].DetalleAfectado.Count, "Mal cantidad de afectados" );

            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleAfectado[0].Clave, "Mal clave afectado 0" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[0].Id, "Mal id afectado 0" );
            Assert.AreEqual( "Cupon.EntidadFinanciera.Codigo", respuesta[0].DetalleAfectado[0].Atributos[0], "Mal atrubuto 0 afectado 0" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[0].Atributos[1], "Mal atrubuto 1 afectado 0" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[1].Clave, "Mal clave afectado 1" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[1].Id, "Mal id afectado 1" );
            Assert.AreEqual( "Articulo.Codigo", respuesta[0].DetalleAfectado[1].Atributos[0], "Mal atrubuto 0 afectado 1" );
            Assert.AreEqual( "Cantidad", respuesta[0].DetalleAfectado[1].Atributos[1], "Mal atrubuto 1 afectado 1" );

            Assert.AreEqual( 1, respuesta[0].DetalleBeneficiado.Count, "Mal cantidad de beneficiados" );
            Assert.AreEqual( "Comprobante.Valoresdetalle.Item", respuesta[0].DetalleBeneficiado[0].Clave, "Mal clave beneficiado" );
            Assert.AreEqual( "0", respuesta[0].DetalleBeneficiado[0].Id, "Mal id beneficiado" );
            Assert.AreEqual( 2, respuesta[0].DetalleBeneficiado[0].ImporteBeneficioTotal, "Mal monto beneficiado" );

        }
    }
}