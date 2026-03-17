using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class CasoAcumulacionDeCantidades
    {
        [TestMethod]
        public void DosLineasItemsCumplenConUnParticipanteNoIndividualmente()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "promo1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 2 Item Articulo.Codigo DebeSerIgualA ART21, Cantidad DebeSerIgualA 5
            participante = new ParticipanteRegla();
            participante.Id = "parti_001";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = 5;
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART21";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = "10";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and {2} and {3}";

            promocion.Participantes.Add( participante );

            participante = new ParticipanteRegla();
            participante.Id = "parti_002";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = 2;
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART01";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = "10";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "({1} and {2}) or ( {3} and {4} )";

            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios: Items involucrados 10% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "parti_002", Cuantos = 2 } );
            beneficio.Valor = "20";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

            #region comprobante
            XmlNode nodoItem1 = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART21']]" );
            XmlNode nodoItem2 = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );

            //((XmlElement)nodoItem1).SetAttribute( "Id", "0" );
            //((XmlElement)nodoItem2).SetAttribute( "Id", "1" );
            #endregion

            XmlNode clon_item1 = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos/@Valor='1']" ).Clone();
            XmlNode nodoId = clon_item1.SelectSingleNode( "IdItemArticulos" );
            nodoId.Attributes["Valor"].Value = "2";

            comprobante.SelectSingleNode( "Comprobante/Facturadetalle" ).AppendChild( clon_item1 );

            XmlNode nodo = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos/@Valor='1']/Articulo/Codigo" );
            nodo.Attributes["Valor"].Value = "ART21";

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motor = new MotorPromociones( config, factoria );
            motor.AgregarComprobanteParaEvaluacion( "test", comprobante.InnerXml );
            motor.EstablecerLibreriaPromociones( promos );

            List<InformacionPromocion> respuesta = motor.EvaluarPromocionesIndividualmente( "test", new List<string>() { "promo1" } );

            Assert.IsTrue( respuesta.Count == 1, "DosLineasItemsCumplenConUnParticipanteNoIndividualmente No soportado" );
        }


    }
}
