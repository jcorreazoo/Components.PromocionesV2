using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class CasosPruebaStock
    {
        [TestMethod()]
        public void CasoStockSencillo()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 1 Comprobante Sucursal.Segmentacion DebeSerIgualA SEGMENTA
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Stock";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 10;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
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
            comprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

            InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            Assert.AreEqual( 0, promocionesAplicadas[0].DetalleAfectado.Count, "Solo debe estar afectado el comprobante (todos los items estan en beneficiado)" );

            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleBeneficiado.Count );
            Assert.AreEqual( 4, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
        }

        [TestMethod()]
        public void CasoStockConFamilia()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 1 Comprobante Sucursal.Segmentacion DebeSerIgualA SEGMENTA
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Familia";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );
            
            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Stock";
            regla.Comparacion = Factor.DebeSerMenorIgualA;
            regla.Valor = 10;
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and {2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
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
            comprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

            InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            Assert.AreEqual( 0, promocionesAplicadas[0].DetalleAfectado.Count, "Solo debe estar afectado el comprobante (todos los items estan en beneficiado)" );

            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleBeneficiado.Count );
            Assert.AreEqual( 4, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( "0", promocionesAplicadas[0].DetalleBeneficiado[0].Id );
        }
    }
}