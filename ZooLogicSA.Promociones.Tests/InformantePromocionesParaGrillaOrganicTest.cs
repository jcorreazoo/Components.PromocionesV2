using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class InformantePromocionesParaGrillaOrganicTest
    {
        public XmlDocument ObtenerComprobanteConBeneficiosParaPruebasInformante()
        {
            XmlDocument comprobanteXml = this.ObtenerComprobanteOriginalParaPruebasInformante();

            XmlNodeList nodos = comprobanteXml.SelectNodes( "Comprobante/Facturadetalle/Item" );

            XmlNode nodoPartPromo = comprobanteXml.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );
            ((XmlElement)nodoPartPromo).SetAttribute( "Consumido", "1" );

            XmlNode nodoCreadoPorPromo = nodoPartPromo.Clone();
            ((XmlElement)nodoCreadoPorPromo).SetAttribute( "Promo", "promo1" );
            ((XmlElement)nodoCreadoPorPromo).SelectSingleNode( "Precio" ).Attributes["Valor"].Value = "5";
            ((XmlElement)nodoCreadoPorPromo).SelectSingleNode( "Cantidad" ).Attributes["Valor"].Value = "1";

            nodoPartPromo.ParentNode.AppendChild( nodoCreadoPorPromo );

            return comprobanteXml;
        }

        public XmlDocument ObtenerComprobanteOriginalParaPruebasInformante()
        {
            XmlDocument comprobanteXml = new XmlDocument();
            comprobanteXml.LoadXml( Resources.ComprobanteBasicoPruebas );

            return comprobanteXml;
        }
    }
}
