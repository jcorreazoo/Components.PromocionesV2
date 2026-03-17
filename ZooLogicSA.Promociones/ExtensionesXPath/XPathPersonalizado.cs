using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;

namespace ZooLogicSA.Promociones.Xpath
{
    public class XPathPersonalizado : XsltContext
    {

        // Private collection for variables
        //private XsltArgumentList m_Args = null;

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public XPathPersonalizado()
        //{

        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="nametable"></param>
        //public XPathPersonalizado(NameTable nametable)
        //    : base(nametable)
        //{

        //}

        ///// <summary>
        ///// Constructor
        ///// </summary>
        ///// <param name="nametable"></param>
        //public XPathPersonalizado( NameTable nametable, XsltArgumentList Args )
        //    : base(nametable)
        //{
        //    ArgumentList = Args;
        //}
        
        
        //public XsltArgumentList ArgumentList
        //{
        //    get { return this.m_Args; }
        //    set { this.m_Args = value; }
        //}

        public override int CompareDocument( string baseUri, string nextbaseUri )
        {
            throw new NotImplementedException();
        }

        public override bool PreserveWhitespace( XPathNavigator node )
        {
            throw new NotImplementedException();
        }

        public override IXsltContextFunction ResolveFunction( string prefix, string name, XPathResultType[] ArgTypes )
        {
            IXsltContextFunction func = null;

            switch ( name )
            {
                case "EvaluarValores":
                    func = new XpathAdaptadorConvertidores();
                    break;
                default:
                    break;
            }
            return func;
        }

        public override IXsltContextVariable ResolveVariable( string prefix, string name )
        {
            throw new NotImplementedException();
        }

        public override bool Whitespace
        {
            get { throw new NotImplementedException(); }
        }
    }
}
