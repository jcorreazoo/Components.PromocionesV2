using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class ComprobanteXMLTest
    {
        [TestMethod()]
        public void ObtenerParticipantesSegunClaveTest()
        {
            ComprobanteXML target = new ComprobanteXML( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );
            target.Cargar( Resources.ComprobanteBasicoPruebas );

            string claveParticipante = "Comprobante";
            //string claveAtributo = "Cliente.Codigo";

            List<IParticipante> actual;

            actual = target.ObtenerParticipantesSegunClave( claveParticipante );

            Assert.IsTrue(actual.Count == 1, actual.Count.ToString(new CultureInfo("en-US")));
            Assert.IsTrue( actual[0].Clave == "Comprobante", actual[0].Clave );
            Assert.IsTrue( actual[0].Id == "0", actual[0].Id );
            
            //Assert.IsTrue( actual[0].TipoDato == TipoDato.C, actual[0].TipoDato.ToString() );
            //Assert.IsTrue( (string)actual[0].Valor == "00002", (string)actual[0].Valor );
        }

        [TestMethod()]
        public void ObtenerParticipantesSegunCondicionDeReglasTest()
        {
            ComprobanteXML target = new ComprobanteXML( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );
            target.Cargar( Resources.ComprobanteBasicoPruebas );

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
            regla.Valor = "ART01";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "10";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART01";
            participante.Reglas.Add(regla);

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "10";
            participante.Reglas.Add(regla);

            participante.RelaReglas = "(({1} and {2}) and {3}) or ({4} and {5})";

            promocion.Participantes.Add( participante );
            #endregion

            List<IParticipante> actual = target.ObtenerParticipantesSegunCondicionDeReglas( participante, 3 );

            Assert.IsTrue(actual.Count == 1, actual.Count.ToString(new CultureInfo("en-US")));
            Assert.AreEqual( "Comprobante.Facturadetalle.Item1", actual[0].Clave+actual[0].Id );
        }

        /// <summary>
        ///A test for Cargar
        ///</summary>
        [TestMethod()]
        public void CargarTest()
        {
            ComprobanteXML target = new ComprobanteXML( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );
            string xml = Resources.ComprobanteBasicoPruebas;
            
            target.Cargar( xml );

            Assert.AreEqual( "4F-E6-7C-FE-75-63-E1-12-0B-B3-27-57-AC-34-E9-AD", target.Hash );
        }

        [TestMethod()]
        public void VerificarSeteoDeXmlPreciosAdicionales()
        {
            ComprobanteXML target = new ComprobanteXML(FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento());
            string xml = Resources.PreciosAdicionales;

            target.CargarPreciosAdicionales(xml);
            string xmlRetorno = target.ObtenerXmlPreciosAdicionales().InnerXml;
            Assert.AreEqual(xml.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", ""), xmlRetorno.Replace(" ", ""));
        }
    }
}