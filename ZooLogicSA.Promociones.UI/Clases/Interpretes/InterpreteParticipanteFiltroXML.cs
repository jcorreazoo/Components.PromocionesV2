using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteParticipanteFiltroXML
    {
        public const string cNombreNodoParticipantes = "Participantes";
        public const string cNombreNodoParticipante = "Participante";
        public const string cNombreNodoAtributos = "Atributos";
        public const string cNombreNodoAtributo = "Atributo";
        public const string cNombreAtributoDescripcion = "Descripcion";
        public const string cNombreAtributoNombre = "Nombre";
        public const string cNombreAtributoDetalle = "Detalle";
        public const string cNombreAtributoEntidad = "Entidad";
        public const string cNombreAtributoAtributo = "Atributo";
        public const string cNombreTipoDato = "TipoDato";
        public const string cNombreLongitud = "Longitud";
        public const string cNombreDecimales = "Decimales";
        public const string cDelimitadorColumnas = "|";
        public const string cDelimitadorValorColumna = "@";
        private XmlDocument xmlDoc;
        private List<InterpreteNodoParticipanteXML> _nodosParticipante;
        private int nivelNodos = 0;

        public InterpreteParticipanteFiltroXML( string xml )
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xml );
            this._nodosParticipante = new List<InterpreteNodoParticipanteXML>();
            this.LlenarListaParticipantes();
        }

        public List<InterpreteNodoParticipanteXML> Participantes
        {
            get
            {
                return _nodosParticipante;
            }
        }

        private void LlenarListaParticipantes()
        {
            foreach ( XmlNode nodoXML in this.xmlDoc.ChildNodes )
            {
                if ( nodoXML.Name == cNombreNodoParticipantes )
                {
                    foreach ( XmlNode nodoParticipante in nodoXML.ChildNodes )
                    {
                        if ( nodoParticipante.Name == cNombreNodoParticipante )
                        {
                            InterpreteNodoParticipanteXML newInterpreteNodoParticipante;
                            newInterpreteNodoParticipante = this.CrearInterpreteNodoParticipante( null, nodoParticipante );
                            this.AgregarNodoParticipante( newInterpreteNodoParticipante );
                        }
                    }
                }
            }
        }
        
        private InterpreteNodoParticipanteXML CrearInterpreteNodoParticipante( InterpreteNodoAtributosXML parent, XmlNode nodoParticipante )
        {
            InterpreteNodoParticipanteXML interpreteNodoParticipante = new InterpreteNodoParticipanteXML( parent );
            if ( nodoParticipante.Attributes[cNombreAtributoDescripcion] != null )
                interpreteNodoParticipante.Descripcion = nodoParticipante.Attributes[cNombreAtributoDescripcion].Value;
            if ( nodoParticipante.Attributes[cNombreAtributoDetalle] != null )
                interpreteNodoParticipante.Detalle = nodoParticipante.Attributes[cNombreAtributoDetalle].Value;
            if ( nodoParticipante.Attributes[cNombreAtributoEntidad] != null )
                interpreteNodoParticipante.qEntidad = nodoParticipante.Attributes[cNombreAtributoEntidad].Value;
            if ( nodoParticipante.Attributes[cNombreAtributoAtributo] != null )
                interpreteNodoParticipante.Atributo = nodoParticipante.Attributes[cNombreAtributoAtributo].Value;
            if (nodoParticipante.Attributes[cNombreTipoDato] != null)
                interpreteNodoParticipante.TipoDato = nodoParticipante.Attributes[cNombreTipoDato].Value;
            if (nodoParticipante.Attributes[cNombreLongitud] != null)
                interpreteNodoParticipante.Longitud = Convert.ToInt32(nodoParticipante.Attributes[cNombreLongitud].Value);
            if (nodoParticipante.Attributes[cNombreDecimales] != null)
                interpreteNodoParticipante.Decimales = Convert.ToInt32(nodoParticipante.Attributes[cNombreDecimales].Value);

            this.nivelNodos++;
            this.LlenarNodosAtributos( nodoParticipante, interpreteNodoParticipante );
            this.nivelNodos--;
            return interpreteNodoParticipante;
        }

        private InterpreteNodoAtributoXML CrearInterpreteNodoAtributo( InterpreteNodoAtributosXML parent, XmlNode nodoAtributo )
        {
            InterpreteNodoAtributoXML interpreteNodoAtributo = new InterpreteNodoAtributoXML( parent );
            if ( nodoAtributo.Attributes[cNombreAtributoDescripcion] != null )
                interpreteNodoAtributo.Descripcion = nodoAtributo.Attributes[cNombreAtributoDescripcion].Value;
            if ( nodoAtributo.Attributes[cNombreAtributoNombre] != null )
                interpreteNodoAtributo.qNombre = nodoAtributo.Attributes[cNombreAtributoNombre].Value;
            if ( nodoAtributo.Attributes[cNombreTipoDato] != null)
                interpreteNodoAtributo.TipoDato = nodoAtributo.Attributes[cNombreTipoDato].Value;
            if (nodoAtributo.Attributes[cNombreLongitud] != null)
                interpreteNodoAtributo.Longitud = Convert.ToInt32(nodoAtributo.Attributes[cNombreLongitud].Value);
            if (nodoAtributo.Attributes[cNombreDecimales] != null)
                interpreteNodoAtributo.Decimales = Convert.ToInt32(nodoAtributo.Attributes[cNombreDecimales].Value);



            return interpreteNodoAtributo;
        }

        private void LlenarNodosAtributos( XmlNode nodoParticipante, InterpreteNodoParticipanteXML interpreteNodoParticipante )
        {
            foreach ( XmlNode nodoAtributos in nodoParticipante.ChildNodes )
            {
                if ( nodoAtributos.Name == cNombreNodoAtributos )
                { 
                    InterpreteNodoAtributosXML newInterpreteNodoAtributos;
                    newInterpreteNodoAtributos = this.CrearInterpreteNodoAtributos( interpreteNodoParticipante, nodoAtributos );
                    interpreteNodoParticipante.AgregarNodoAtributos( newInterpreteNodoAtributos );
                }
            }
        }

        private InterpreteNodoAtributosXML CrearInterpreteNodoAtributos( InterpreteNodoParticipanteXML parent, XmlNode nodoAtributos )
        {
            InterpreteNodoAtributosXML interpreteNodoAtributo = new InterpreteNodoAtributosXML( parent );

            this.LlenarNodosAtributo( nodoAtributos, interpreteNodoAtributo, interpreteNodoAtributo );
            this.LlenarNodosParticipante( nodoAtributos, interpreteNodoAtributo, interpreteNodoAtributo );

            return interpreteNodoAtributo;
        }

        private void LlenarNodosParticipante( XmlNode nodoAtributos, InterpreteNodoAtributosXML parent, InterpreteNodoAtributosXML interpreteNodoAtributos )
        {
            foreach ( XmlNode nodoParticipante in nodoAtributos.ChildNodes )
            {
                if ( nodoParticipante.Name == cNombreNodoParticipante )
                {
                    InterpreteNodoParticipanteXML newInterpreteNodoParticipante;
                    newInterpreteNodoParticipante = this.CrearInterpreteNodoParticipante( interpreteNodoAtributos, nodoParticipante );
                    //OPERADORADETARJETA solo debería aparecer en el nodo del cupón
                    if (newInterpreteNodoParticipante.qEntidad != "OPERADORADETARJETA" || this.nivelNodos != 1)
                    {
                        interpreteNodoAtributos.AgregarNodoParticipante(newInterpreteNodoParticipante);
                    }                    
                }
            }
        }

        private void LlenarNodosAtributo( XmlNode nodoAtributos, InterpreteNodoAtributosXML parent, InterpreteNodoAtributosXML interpreteNodoAtributos )
        {
            foreach ( XmlNode nodoAtributo in nodoAtributos.ChildNodes )
            {
                if ( nodoAtributo.Name == cNombreNodoAtributo )
                {
                    InterpreteNodoAtributoXML newInterpreteNodoAtributo;
                    newInterpreteNodoAtributo = this.CrearInterpreteNodoAtributo( interpreteNodoAtributos, nodoAtributo );
                    //TIPOTARJETA solo debería aparecer en el nodo del cupón
                    if (newInterpreteNodoAtributo.qNombre != "TIPOTARJETA" || this.nivelNodos != 1)
                    {
                        interpreteNodoAtributos.AgregarNodoAtributo(newInterpreteNodoAtributo);
                    }                    
                }
            }
        }

        private void AgregarNodoParticipante( InterpreteNodoParticipanteXML nodo )
        {
            this._nodosParticipante.Add( nodo );
        }

        public string[] ObtenerAtributosParticipanteSeleccionado( string fullNombreParticipante, string fullDescripcionParticipante, string fullAtributoParticipante )
        {
            string[] atributos = {"","",""};

            atributos = this.ObtenerAtributosParticipanteSegunNodoAtributos( this.ObtenerNodoAtributosSegunParticipante( fullDescripcionParticipante, this._nodosParticipante ), fullNombreParticipante, fullDescripcionParticipante, fullAtributoParticipante );

            return atributos;
        }

        private InterpreteNodoAtributosXML ObtenerNodoAtributosSegunParticipante( string fullDescripcionParticipante, List<InterpreteNodoParticipanteXML> nodosParticipante )
        {
            InterpreteNodoAtributosXML nodoAtributos = null;
            string[] listaParticipantes;
            string nombreParticipante;
            bool obtenerNodo;

            if ( fullDescripcionParticipante.IndexOf( "." ) > 0 )
            {
                listaParticipantes = fullDescripcionParticipante.Split( '.' );
                nombreParticipante = listaParticipantes[0];
                obtenerNodo = false;
            }
            else
            {
                nombreParticipante = fullDescripcionParticipante;
                obtenerNodo = true;
            }

            foreach ( InterpreteNodoParticipanteXML nodoParticipante in nodosParticipante )
            {
                if ( nodoParticipante.Descripcion.ToUpper().Trim() == nombreParticipante.ToUpper().Trim() )
                {
                    if ( obtenerNodo )
                    {
                        if ( nodoParticipante.NodosAtributos.Count > 0 )
                            nodoAtributos = nodoParticipante.NodosAtributos[0];
                        break;
                    }
                    else
                    {
                        if ( nodoParticipante.NodosAtributos.Count > 0 )
                            nodoAtributos = ObtenerNodoAtributosSegunParticipante( fullDescripcionParticipante.Substring( fullDescripcionParticipante.IndexOf( "." ) + 1 ), nodoParticipante.NodosAtributos[0].NodosParticipante );
                    }
                }
            }

            return nodoAtributos;
        }

        public string[] ObtenerAtributosParticipanteSegunNodoAtributos( InterpreteNodoAtributosXML nodoAtributos, string fullNombreParticipante, string fullDescripcionParticipante, string fullAtributoParticipante )
        {
            string[] atributos = {"","",""};

            foreach ( InterpreteNodoAtributoXML nodoAtributo in nodoAtributos.NodosAtributo )
            {
                if ( atributos[0] == "" )
                {
                    atributos[0] = nodoAtributo.qNombre + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.qNombre;
                    atributos[1] = nodoAtributo.Descripcion + cDelimitadorValorColumna + fullDescripcionParticipante + "." + nodoAtributo.Descripcion;
                    atributos[2] = nodoAtributo.qNombre + cDelimitadorValorColumna + fullAtributoParticipante + "." + nodoAtributo.qNombre;
                }
                else
                {
                    atributos[0] = atributos[0] + cDelimitadorColumnas + nodoAtributo.qNombre + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.qNombre;
                    atributos[1] = atributos[1] + cDelimitadorColumnas + nodoAtributo.Descripcion + cDelimitadorValorColumna + fullDescripcionParticipante + "." + nodoAtributo.Descripcion;
                    atributos[2] = atributos[2] + cDelimitadorColumnas + nodoAtributo.qNombre + cDelimitadorValorColumna + fullAtributoParticipante + "." + nodoAtributo.qNombre;
                }
            }

            return atributos;
        }

        public string ObtenerTipoDetalle( string fullDescripcion )
        {
            string detalle = "";
            string[] valores;

            if ( ManagerFiltros.EsUnAtributoBasico( fullDescripcion ) )
            {
                detalle = ManagerReglas.cCodigoDetalleItem;
            }
            else
            {
                fullDescripcion = fullDescripcion.Replace( "[", "" );
                fullDescripcion = fullDescripcion.Replace( "]", "" );
                valores = fullDescripcion.Split( '.' );
                if ( valores[0] != null )
                {
                    foreach ( InterpreteNodoParticipanteXML nodoParticipante in this._nodosParticipante )
                    {
                        //if ( nodoParticipante.Descripcion.ToUpper().Trim() == valores[0].ToUpper().Trim() )
                        //if ( nodoParticipante.qEntidad.ToUpper().Trim() == valores[0].ToUpper().Trim() )
                        if ( nodoParticipante.Atributo.ToUpper().Trim() == valores[0].ToUpper().Trim() )
                        {
                            detalle = nodoParticipante.Detalle;
                            break;
                        }
                    }
                }
            }

            return detalle;
        }
        // NO SE ESTA USANDO
        public string ConvertirFullDescripcionEnNombre( string fullDescripcion )
        {
            string nombre = "";

            if ( ManagerFiltros.EsUnAtributoBasico( fullDescripcion ) )
            {
                nombre = fullDescripcion;
            }
            else
            {
                string[] valores = fullDescripcion.Split( '.' );
                InterpreteNodoParticipanteXML participante = this.ObtenerNodoParticipanteSegunAtributo( valores[0] );
                nombre = participante.Descripcion;
            }
            return nombre;
        }
        // NO SE ESTA USANDO
        public string ConvertirFullDescripcionEnAtributo( string fullDescripcion )
        {
            string atributo = "";

            if ( ManagerFiltros.EsUnAtributoBasico( fullDescripcion ) )
            {
                atributo = fullDescripcion;
            }
            else
            {
                string[] valores = fullDescripcion.Split( '.' );
                InterpreteNodoParticipanteXML participante = this.ObtenerNodoParticipanteSegunDescripcion( valores[0] );
//                atributo = participante.Atributo;
                atributo = participante.qEntidad;
                for ( int i = 1; i < valores.Length; i++ )
                {
                    if ( i < valores.Length - 1 )
                    {
                        participante = this.ObtenerParticipanteSegunDescripcion( valores[i], participante.NodosAtributos );
                        atributo = atributo + "." + participante.Atributo;
                    }
                    else
                    {
                        atributo = atributo + "." + this.ObtenerNombreAtributoSegunDescripcion( valores[i], participante.NodosAtributos );
//                        atributo = atributo + "." + this.ObtenerAtributoSegunDescripcion( valores[i], participante.NodosAtributos );
                    }
                }
            }
            return atributo;
        }

        public string ConvertirFullNombreEnDescripcion( string fullNombre )
        {
            string descripcion = "";

            if ( ManagerFiltros.EsUnAtributoBasico( fullNombre ) || ManagerFiltros.EsUnAtributoGeneral( fullNombre ) )
            {
                descripcion = fullNombre;
            }
            else
            {
                string[] valores = fullNombre.Split( '.' );
                InterpreteNodoParticipanteXML participante = this.ObtenerNodoParticipanteSegunNombre( valores[0] );
                descripcion = participante.Descripcion;
                for ( int i = 1; i < valores.Length; i++ )
                {
                    if ( i < valores.Length - 1 )
                    {
                        participante = this.ObtenerParticipanteSegunNombre( valores[i], participante.NodosAtributos );
                        descripcion = descripcion + "." + participante.Descripcion;
                    }
                    else
                    {
                        descripcion = descripcion + "." + this.ObtenerDescripcionAtributoSegunNombre( valores[i], participante.NodosAtributos );
                    }
                }
            }
            return descripcion;
        }

        public string ConvertirFullAtributoEnDescripcion( string fullAtributo )
        {
            string descripcion = "";

            if ( ManagerFiltros.EsUnAtributoBasico( fullAtributo ) )
            {
                descripcion = fullAtributo;
            }
            else
            {
                string[] valores = fullAtributo.Split( '.' );
                InterpreteNodoParticipanteXML participante = this.ObtenerNodoParticipanteSegunAtributo( valores[0] );
                descripcion = participante.Descripcion;
                for ( int i = 1; i < valores.Length; i++ )
                {
                    if ( i < valores.Length - 1 )
                    {
                        participante = this.ObtenerParticipanteSegunAtributo( valores[i], participante.NodosAtributos );
                        descripcion = descripcion + "." + participante.Descripcion;
                    }
                    else
                    {
                        descripcion = descripcion + "." + this.ObtenerDescripcionAtributoSegunAtributo( valores[i], participante.NodosAtributos );
                    }
                }
            }
            return descripcion;
        }        

        private InterpreteNodoParticipanteXML ObtenerParticipanteSegunNombre( string nombre, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoParticipanteXML nodo = null;
            foreach ( var atributo in atributos )
            {
//                nodo = atributo.NodosParticipante.FirstOrDefault( n => n.qEntidad == nombre );
                nodo = atributo.NodosParticipante.FirstOrDefault( n => n.Atributo == nombre );
                if ( nodo != null )
                    break;
            }
            return nodo;
        }

        private InterpreteNodoParticipanteXML ObtenerParticipanteSegunDescripcion( string descripcion, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoParticipanteXML nodo = null;
            foreach ( var atributo in atributos )
            {
                nodo = atributo.NodosParticipante.FirstOrDefault( n => n.Descripcion == descripcion );
                if ( nodo != null )
                    break;
            }
            return nodo;
        }

        private InterpreteNodoParticipanteXML ObtenerParticipanteSegunAtributo( string atributo, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoParticipanteXML nodo = null;
            foreach ( var _atributo in atributos )
            {
                nodo = _atributo.NodosParticipante.FirstOrDefault( n => n.Atributo == atributo );
                if ( nodo != null )
                    break;
            }
            return nodo;
        }

        private string ObtenerNombreAtributoSegunDescripcion( string descripcion, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoAtributoXML nodo = null;
            foreach ( var atributo in atributos )
            {
                nodo = atributo.NodosAtributo.FirstOrDefault( n => n.Descripcion == descripcion );
                if ( nodo != null )
                    break;
            }
            return nodo.qNombre;
        }

        private string ObtenerAtributoSegunDescripcion( string descripcion, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoAtributoXML nodo = null;
            foreach ( var atributo in atributos )
            {
                nodo = atributo.NodosAtributo.FirstOrDefault( n => n.Descripcion == descripcion );
                if ( nodo != null )
                    break;
            }
            return nodo.Atributo;
        }

        private string ObtenerDescripcionAtributoSegunNombre( string nombre, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoAtributoXML nodo = null;
            foreach ( var atributo in atributos )
            {
                nodo = atributo.NodosAtributo.FirstOrDefault( n => n.qNombre == nombre );
                if ( nodo != null )
                    break;
            }
            return nodo.Descripcion;
        }

        private string ObtenerDescripcionAtributoSegunAtributo( string atributo, List<InterpreteNodoAtributosXML> atributos )
        {
            InterpreteNodoAtributoXML nodo = null;
            foreach ( var _atributo in atributos )
            {
                nodo = _atributo.NodosAtributo.FirstOrDefault( n => n.Atributo == atributo );
                if ( nodo != null )
                    break;
            }
            return nodo.Descripcion;
        }

        private InterpreteNodoParticipanteXML ObtenerNodoParticipanteSegunDescripcion( string descripcion )
        {
            var nodo = this._nodosParticipante.FirstOrDefault( n => n.Descripcion == descripcion );
            return nodo;
        }

        private InterpreteNodoParticipanteXML ObtenerNodoParticipanteSegunAtributo( string atributo )
        {
            var nodo = this._nodosParticipante.FirstOrDefault( n => n.Atributo == atributo );
            return nodo;
        }

        private InterpreteNodoParticipanteXML ObtenerNodoParticipanteSegunNombre( string nombre )
        {
//            var nodo = this._nodosParticipante.FirstOrDefault( n => n.qEntidad == nombre );
            var nodo = this._nodosParticipante.FirstOrDefault( n => n.Atributo == nombre );
            return nodo;
        }
    }
}
