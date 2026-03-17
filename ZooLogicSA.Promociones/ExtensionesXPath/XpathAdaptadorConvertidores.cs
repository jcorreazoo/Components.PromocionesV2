using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Globalization;

namespace ZooLogicSA.Promociones.Xpath
{
    public class XpathAdaptadorConvertidores : IXsltContextFunction
    {
        #region IXsltContextFunction Members

        public XPathResultType[] ArgTypes
        {
            get { throw new NotImplementedException(); }
        }

        public object Invoke( XsltContext xsltContext, object[] args, XPathNavigator docContext )
        {
            // Factor
            Factor comparador = (Factor)Factor.Parse( typeof( Factor ), args[0].ToString() );

            // TipoDato
            TipoDato tipoDato = (TipoDato)TipoDato.Parse( typeof( TipoDato ), args[1].ToString() );

            // Valor en XML
            object valorXML = this.ConvertirAlTipo(args[2].ToString(), tipoDato);

            // TipoDato en Regla
            TipoDato tipoDatoEnRegla = this.ObtenerTipoDato( args[3].ToString() );

            // Valor en Regla
            object valorRegla = this.ConvertirAlTipo( args[4].ToString(), tipoDatoEnRegla );

            GestorComparaciones gestor = new GestorComparaciones();
            bool retorno;

            try
            {
                retorno = gestor.Comparadores[comparador].Comparar(tipoDato, valorRegla, valorXML);
            }
            catch (Exception)
            {
                retorno = false;
            }

            return retorno;
        }

        private object ConvertirAlTipo( string valor, TipoDato tipoDato )
        {
            object retorno;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    retorno = valor;
                    break;
                case TipoDato.N:
                    retorno = Convert.ToDecimal(valor, new CultureInfo("en-US"));
                    break;
                case TipoDato.D:

                    int dia = Convert.ToInt32( valor.Substring( 0, 2 ) );
                    int mes = Convert.ToInt32( valor.Substring( 3, 2 ) );
                    int anio = Convert.ToInt32( valor.Substring( 6, 4 ) );

                    retorno = new DateTime( anio, mes, dia, 0, 0, 0 );

                    break;
                case TipoDato.L:
                    retorno = Boolean.Parse( valor );
                    break;
                default:
                    retorno = valor;
                    break;
            }

            return retorno;
        }

        private TipoDato ObtenerTipoDato( string tipo )
        {
            TipoDato retorno;

            switch ( tipo )
            {
                case "System.String":
                    retorno = TipoDato.C;
                    break;
                case "System.Boolean":
                    retorno = TipoDato.L;
                    break;
                case "System.Int32":
                    retorno = TipoDato.N;
                    break;
                case "System.DateTime":
                    retorno = TipoDato.D;
                    break;
                default:
                    retorno = TipoDato.C;
                    break;
            }

            return retorno;
        }

        #region otros miembros de IXsltContextFunction
        public int Maxargs
        {
            get { throw new NotImplementedException(); }
        }

        public int Minargs
        {
            get { throw new NotImplementedException(); }
        }

        public XPathResultType ReturnType
        {
            get { throw new NotImplementedException(); }
        } 
        #endregion

        #endregion
    }
}
