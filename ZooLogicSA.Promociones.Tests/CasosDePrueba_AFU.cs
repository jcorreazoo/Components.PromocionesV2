using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class CasosDePrueba_AFU
    {
        /// <summary>
        /// con % de descuento por sucursal, sobre todos los productos
        /// </summary>
        [TestMethod()]
        public void AFU_1_Caso1()
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
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Sucursal.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "SUCUTEST";
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );

            #endregion

            #region participante 2 Item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Todos los items, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
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

            InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado.Count, "Solo debe estar afectado el comprobante (todos los items estan en beneficiado)" );

            Assert.AreEqual( "Comprobante", promocionesAplicadas[0].DetalleAfectado[0].Clave );
            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado[0].Atributos.Count );
            Assert.AreEqual( "Sucursal.Codigo", promocionesAplicadas[0].DetalleAfectado[0].Atributos[0] );

            Assert.AreEqual( 4, promocionesAplicadas[0].DetalleBeneficiado.Count );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
        }

        /// <summary>
        /// con % de descuento por codigo de segmentacion de sucursal, sobre algun producto
        /// </summary>
        [TestMethod()]
        public void AFU_1_Caso2()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 1 Comprobante Sucursal.Segmentacion.Codigo DebeSerIgualA SEGMENTA
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Sucursal.Segmentacion.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "SEGMENTA";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            #region participante 2 Item Articulo.Codigo DebeSerIgualA 00100101 and Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );
            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{1} and {2}";
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

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            Assert.AreEqual( 1, promocionesAplicadas.Count );

            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado.Count );
            Assert.AreEqual( "Comprobante", promocionesAplicadas[0].DetalleAfectado[0].Clave );
            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado[0].Cantidad );
            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado[0].Atributos.Count );

            Assert.AreEqual( 2, promocionesAplicadas[0].DetalleBeneficiado.Count );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
        }

        /// <summary>
        /// por determinado color, otro precio
        /// </summary>
        [TestMethod()]
        public void AFU_3()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 2 Item Color.Codigo DebeSerIgualA 276
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Color.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "276";
            participante.Reglas.Add( regla );
            participante.RelaReglas = "{1} and {2}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: participante 2, precio 200
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = 200;
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Precio";
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

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            //File.WriteAllText( @"c:\promociones.xml", Serializador.Serializar<List<Promocion>>( promos ) );

            Assert.AreEqual( 1, promocionesAplicadas.Count );
            
            Assert.AreEqual( 0, promocionesAplicadas[0].DetalleAfectado.Count );
            
            Assert.AreEqual( 2, promocionesAplicadas[0].DetalleBeneficiado.Count );
            
            Assert.AreEqual( "2", promocionesAplicadas[0].DetalleBeneficiado[0].Id );
            Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleBeneficiado[0].Clave );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[0].Cantidad );
            Assert.AreEqual( "Precio", promocionesAplicadas[0].DetalleBeneficiado[0].AtributoAlterado );
            Assert.AreEqual( Alteracion.CambiarValor, promocionesAplicadas[0].DetalleBeneficiado[0].Alteracion );
            Assert.AreEqual( 200, promocionesAplicadas[0].DetalleBeneficiado[0].Valor );

            Assert.AreEqual( "3", promocionesAplicadas[0].DetalleBeneficiado[1].Id );
            Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleBeneficiado[1].Clave );
            Assert.AreEqual( 10, promocionesAplicadas[0].DetalleBeneficiado[1].Cantidad );
            Assert.AreEqual( "Precio", promocionesAplicadas[0].DetalleBeneficiado[1].AtributoAlterado );
            Assert.AreEqual( Alteracion.CambiarValor, promocionesAplicadas[0].DetalleBeneficiado[1].Alteracion );
            Assert.AreEqual( 200, promocionesAplicadas[0].DetalleBeneficiado[1].Valor );
        }
    }
}
