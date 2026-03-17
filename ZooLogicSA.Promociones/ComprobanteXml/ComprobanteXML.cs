using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Xpath;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones
{
    public class ComprobanteXML : IComprobante
    {
        private XmlDocument xml;
        private XmlDocument xmlOriginal;
        private ConfiguracionComportamiento comportamiento;
        private string hash;
        private XmlDocument xmlPreciosAdicionales;
        private ConvertidorNumerico convertNumber;

        public ComprobanteXML( ConfiguracionComportamiento comportamiento )
        {
            this.xml = new XmlDocument();
            this.xmlPreciosAdicionales = new XmlDocument();
            this.comportamiento = comportamiento;
            this.convertNumber = new ConvertidorNumerico();
        }

        public ConfiguracionComportamiento Comportamiento
        {
            get { return this.comportamiento; }
            set { this.comportamiento = value; }
        }

        public string Hash
        {
            get { return this.hash; }
            set { this.hash = value; }
        }

        public List<IParticipante> ObtenerParticipantesSegunClave( string claveParticipante)
        {
            return this.ObtenerParticipantesSegunClave( claveParticipante, false );
        }
        
        private List<IParticipante> ObtenerParticipantesSegunClave( string claveParticipante, bool todos )
        {
            XmlDocument xml = this.xml;
            string claveParticipanteXML = claveParticipante.Replace( ".", "/" );

            string condicionSinRetiroDeEfectico = this.ObtenerCondicionEsRetiroEfectivo( claveParticipanteXML );
            string condicionAtributoVacio = "";
            string condicion = "";

            if (!todos)
            {
                condicionAtributoVacio = this.ObtenerExpresionAtributoVacio( "Promo" );
            }

            if ( condicionAtributoVacio != "" || condicionSinRetiroDeEfectico != "" )
            {
                condicion = "[" + condicionAtributoVacio;
                condicion += ( condicionAtributoVacio != "" && condicionSinRetiroDeEfectico != "" ) ? " and " : "";
                condicion += ( condicionSinRetiroDeEfectico != "") ? condicionSinRetiroDeEfectico : "";
                condicion += "]";
            }

            XmlNodeList nodosParticipante = xml.SelectNodes( claveParticipanteXML + condicion );
            List<IParticipante> retorno = new List<IParticipante>();

            ParticipanteXml elemento;

            foreach ( XmlNode nodoP in nodosParticipante )
            {
                elemento = new ParticipanteXml( this.comportamiento );
                elemento.Clave = claveParticipanteXML.Replace( "/", "." );
                elemento.Nodo = (XmlElement)nodoP;
                retorno.Add( elemento );
            }

            return retorno;
        }

        public void Cargar( string xml )
        {
            this.hash = this.ObtenerChecksum( xml );
            this.xml.LoadXml( xml );
            this.xmlOriginal = (XmlDocument)this.xml.Clone();
        }

        public void CargarPreciosAdicionales(string xml)
        {
            this.xmlPreciosAdicionales.LoadXml(xml);
        }

        private string ObtenerChecksum( string xml )
        {
            byte[] file = ASCIIEncoding.Default.GetBytes( xml );
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash( file );
            string enc = BitConverter.ToString( retVal );
            return enc;
        }

        public IParticipante ObtenerNodoParticipante( string clave, string id, string promo, string beneficio )
        {
            string xpath = this.ObtenerXPathParametros( clave, id, promo, beneficio );

            IParticipante retorno = null;
            XmlElement nodo = (XmlElement)this.xml.SelectSingleNode( xpath );

            if ( nodo != null )
            {
                retorno = new ParticipanteXml( this.comportamiento );
                retorno.Nodo = nodo;
                retorno.Clave = clave;
                if ( nodo.Attributes["Consumido"] != null )
                {
                    retorno.Consumido = this.convertNumber.ConvertToDecimal( nodo.Attributes["Consumido"].Value );
                }
                else
                {
                    retorno.Consumido = 0;
                }
            }

            return retorno;
        }

        private string ObtenerXPathParametros( string clave, string id, string promo, string beneficio)
        {
            string retorno;

            string paramId = "";
            string paramPromo = "";
            string paramBeneficio = "";
            string paramRetiroEfectivo = "";

            if ( !string.IsNullOrEmpty( id ) )
            {
                paramId = this.comportamiento.ConfiguracionesPorParticipante[clave].AtributoId + "/@Valor='" + id + "'";
            }
            else
            {
                throw new Exception( "Comprobante serializado mal formado: falta el identificador para el elemento: " + clave );
            }


            if ( !string.IsNullOrEmpty( promo ) )
            {
                paramPromo = "@Promo='" + promo + "'";
            }
            else
            {
                paramPromo = this.ObtenerExpresionAtributoVacio( "Promo" );
            }

            if ( !string.IsNullOrEmpty( beneficio ) )
            {
                paramBeneficio = "@Beneficio='" + beneficio + "'";
            }
            else
            {
                paramBeneficio = this.ObtenerExpresionAtributoVacio( "Beneficio" );
            }

            paramRetiroEfectivo = this.ObtenerCondicionEsRetiroEfectivo( clave );
            paramRetiroEfectivo = ( paramRetiroEfectivo != "") ? " and " + paramRetiroEfectivo : "";

            retorno = clave.Replace( ".", "/" ) + "[" + paramId + " and " + paramPromo + " and " + paramBeneficio + paramRetiroEfectivo + "]";
            return retorno;
        }

        private string ObtenerExpresionAtributoVacio( string atributo )
        {
            return "( not(@" + atributo + ") or @" + atributo + "='')";
        }

        public XmlDocument ObtenerXml()
        {
            return this.xml;
        }
        
        public XmlDocument ObtenerXmlOriginal()
        {
            return this.xmlOriginal;
        }

        public XmlDocument ObtenerXmlPreciosAdicionales()
        {
            return this.xmlPreciosAdicionales;
        }

        public List<IParticipante> ObtenerParticipantesSegunCondicionDeReglas( ParticipanteRegla participante, int regla )
        {
            string expresion = participante.RelaReglas;

            string xpath = this.ObtenerXpath( participante, expresion );
            string condicionSinRetiroDeEfectico = this.ObtenerCondicionEsRetiroEfectivo( participante.Codigo );
            condicionSinRetiroDeEfectico = ( condicionSinRetiroDeEfectico != "" ) ? " and " + condicionSinRetiroDeEfectico : "";

            #region Preproceso
            XmlNodeList objetivos = this.xml.SelectNodes( participante.Codigo.Replace( ".", "/" ) );

            foreach ( XmlNode objetivo in objetivos )
            {
                XmlNode nodoCantidadExistente = objetivo.SelectSingleNode( this.comportamiento.ConfiguracionesPorParticipante[participante.Codigo].Cantidad );
                if ( nodoCantidadExistente == null )
                {
                    nodoCantidadExistente = this.xml.CreateElement( this.comportamiento.ConfiguracionesPorParticipante[participante.Codigo].Cantidad );
                    ((XmlElement)nodoCantidadExistente).SetAttribute( "Valor", "1" );
                    ((XmlElement)nodoCantidadExistente).SetAttribute( "TipoDato", "N" );
                    ((XmlElement)objetivo).AppendChild( nodoCantidadExistente );
                }
            }
            #endregion

            XPathNavigator nav = this.xml.CreateNavigator();
            
            XPathPersonalizado ctx = new XPathPersonalizado();

			XPathExpression exp = nav.Compile( participante.Codigo.Replace( ".", "/" ) + "[" + this.ObtenerExpresionAtributoVacio( "Promo" ) + " and ( not(@Consumido) or @Consumido<" + this.comportamiento.ConfiguracionesPorParticipante[participante.Codigo].Cantidad + "/@Valor ) and " + "not(contains(@CoincidenciasExcluidas, '" + participante.Id.ToString() + "'))" + " and " + xpath + condicionSinRetiroDeEfectico + "]" );

            exp.SetContext(ctx);

            XPathNodeIterator nodos = nav.Select( exp );

            ParticipanteXml participanteRetorno;
            List<IParticipante> retorno = new List<IParticipante>();
            foreach ( XPathNavigator item in nodos )
            {
                XmlNode nodo = ((IHasXmlNode)item).GetNode();

                participanteRetorno = new ParticipanteXml( this.comportamiento );
                participanteRetorno.Nodo = nodo;
                participanteRetorno.Clave = participante.Codigo;

                if ( nodo.Attributes["Consumido"] != null )
                {
                    participanteRetorno.Consumido = this.convertNumber.ConvertToDecimal( nodo.Attributes["Consumido"].Value );
                }

                retorno.Add( participanteRetorno );
            }

            return retorno;
        }

        private string ObtenerXpath( ParticipanteRegla parti, string relacion )
        {
            string reemplazo = relacion;

            reemplazo = reemplazo.ToLower();

            foreach ( Regla item in parti.Reglas )
            {
                reemplazo = reemplazo.Replace( "{" + item.Id + "}", this.CrearEvaluacionesXPath( parti, item ) );
            }

            return reemplazo;
        }

        private string CrearEvaluacionesXPath( ParticipanteRegla parti, Regla regla )
        {
            string funcionComparacion = regla.Comparacion.ToString();
            string tipoDato = regla.Atributo.Replace( ".", "/" ) + "/@TipoDato";
            string atributo = regla.Atributo.Replace( ".", "/" ) + "/@Valor";

            string valor = regla.Valor.ToString();

            if ( regla.Valor is DateTime )
            {
                valor = ((DateTime)regla.Valor).ToString( "dd/MM/yyyy", CultureInfo.InvariantCulture );
            }

            if (regla.Atributo == "CANTIDAD" && regla.Valor is String)
            {
                valor = this.convertNumber.ConvertToString( regla.Valor );
            }

            if ( this.comportamiento.ConfiguracionesPorParticipante[parti.Codigo].CantidadMonto == regla.Atributo )
			{
				atributo = this.comportamiento.ConfiguracionesPorParticipante[parti.Codigo].Cantidad.Replace( ".", "/" ) + "/@Valor";
				tipoDato = this.comportamiento.ConfiguracionesPorParticipante[parti.Codigo].Cantidad.Replace( ".", "/" ) + "/@TipoDato";
				funcionComparacion = Factor.DebeSerMayorIgualA.ToString();
				valor = ( valor.Contains(".") || valor.Contains(",") ) ? "0.01" : "1";
            }

			if ( this.comportamiento.ConfiguracionesPorParticipante[parti.Codigo].Cantidad == regla.Atributo )
			{
				funcionComparacion = Factor.DebeSerMayorIgualA.ToString();
				valor = ( valor.Contains(".") || valor.Contains(",") ) ? "0.01" : "1";
            }

            string retorno = "EvaluarValores( '" + funcionComparacion + "', string(" + tipoDato + "), string(" + atributo + ") , '" + regla.Valor.GetType() + "' , '" + valor + "' )";
            
            return retorno;
        }

        private string ObtenerCondicionEsRetiroEfectivo( string claveCodigo )
        {
            string CondicionSinRetiroDeEfectico = "";
            string claveCodigoConBarras = claveCodigo.Replace(".", "/");

            if ( claveCodigoConBarras == "COMPROBANTE/VALORESDETALLE/ITEM" )
            {
                CondicionSinRetiroDeEfectico = "ESRETIROEFECTIVO/@Valor='false'";
            }

            return CondicionSinRetiroDeEfectico;
        }
    }
}
