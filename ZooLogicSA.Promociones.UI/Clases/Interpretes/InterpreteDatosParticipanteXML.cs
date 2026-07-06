using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteDatosParticipanteXML
    {
        public const string cNombreNodoDatosParticipante = "VFPData";
        public const string cNombreNodoRegistro = "Row";
        public const string cNombreNodoAtributos = "Atributos";
        public const string cNombreNodoAtributo = "Atributo";
        public const string cNombreAtributoDescripcion = "Descripcion";
        public const string cNombreAtributoNombre = "Nombre";
        public const string cNombreAtributoDetalle = "Detalle";
        public const string cNombreAtributoEntidad = "Entidad";
        public const string cDelimitadorColumnas = "|";
        public const string cDelimitadorValorColumna = "@";
        private XmlDocument xmlDoc;
        private InterpreteNodoDatosParticipanteXML nodoDatosParticipante;

        public InterpreteDatosParticipanteXML( string xml )
        {
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xml );
            this.nodoDatosParticipante = new InterpreteNodoDatosParticipanteXML();
            this.LlenarNodoDatosParticipante();
        }

        public InterpreteNodoDatosParticipanteXML NodoDatosParticipante
        {
            get
            {
                return this.nodoDatosParticipante;
            }
        }

        private void LlenarNodoDatosParticipante()
        {
            foreach ( XmlNode nodoXML in this.xmlDoc.ChildNodes )
            {
                if ( nodoXML.Name.ToUpper().Trim() == cNombreNodoDatosParticipante.ToUpper().Trim() )
                {
                    foreach ( XmlNode nodoDatoParticipante in nodoXML.ChildNodes )
                    {
                        if ( nodoDatoParticipante.Name.ToUpper().Trim() == cNombreNodoRegistro.ToUpper().Trim() )
                        {
                            InterpreteNodoDatosParticipanteRegistroXML nodoRegistro = this.CrearInterpreteNodoRegistro( nodoDatoParticipante );
                            foreach( XmlAttribute atributo in nodoDatoParticipante.Attributes)
                            {
                                nodoRegistro.AgregarNodoAtributos( this.CrearInterpreteNodoAtributo( atributo.Value ) );
                            }
                            this.NodoDatosParticipante.AgregarNodoRegistro( nodoRegistro );
                        }
                    }
                }
            }
        }

        private InterpreteNodoDatosParticipanteRegistroXML CrearInterpreteNodoRegistro( XmlNode nodoRegistro )
        {
            InterpreteNodoDatosParticipanteRegistroXML interpreteNodoRegistro = new InterpreteNodoDatosParticipanteRegistroXML();

            return interpreteNodoRegistro;
        }

        private InterpreteNodoDatosParticipanteAtributoXML CrearInterpreteNodoAtributo( string valor )
        {
            InterpreteNodoDatosParticipanteAtributoXML interpreteNodoAtributo = new InterpreteNodoDatosParticipanteAtributoXML( valor );

            return interpreteNodoAtributo;
        }

        //private void LlenarNodosAtributos( XmlNode nodoParticipante, InterpreteNodoParticipanteXML interpreteNodoParticipante )
        //{
        //    foreach ( XmlNode nodoAtributos in nodoParticipante.ChildNodes )
        //    {
        //        if ( nodoAtributos.Name == cNombreNodoAtributos )
        //        { 
        //            InterpreteNodoAtributosXML newInterpreteNodoAtributos;
        //            newInterpreteNodoAtributos = this.CrearInterpreteNodoAtributos( interpreteNodoParticipante, nodoAtributos );
        //            interpreteNodoParticipante.AgregarNodoAtributos( newInterpreteNodoAtributos );
        //        }
        //    }
        //}

        //private InterpreteNodoAtributosXML CrearInterpreteNodoAtributos( InterpreteNodoParticipanteXML parent, XmlNode nodoAtributos )
        //{
        //    InterpreteNodoAtributosXML interpreteNodoAtributo = new InterpreteNodoAtributosXML( parent );

        //    this.LlenarNodosAtributo( nodoAtributos, interpreteNodoAtributo, interpreteNodoAtributo );
        //    this.LlenarNodosParticipante( nodoAtributos, interpreteNodoAtributo, interpreteNodoAtributo );

        //    return interpreteNodoAtributo;
        //}

        private void LlenarNodosParticipante( XmlNode nodoAtributos, InterpreteNodoAtributosXML parent, InterpreteNodoAtributosXML interpreteNodoAtributos )
        {
            foreach ( XmlNode nodoParticipante in nodoAtributos.ChildNodes )
            {
//                if ( nodoParticipante.Name == cNombreNodoParticipante )
//                {
//                    InterpreteNodoParticipanteXML newInterpreteNodoParticipante;
//                    newInterpreteNodoParticipante = this.CrearInterpreteNodoParticipante( interpreteNodoAtributos, nodoParticipante );
//                    interpreteNodoAtributos.AgregarNodoParticipante( newInterpreteNodoParticipante );
//                }
            }
        }

        //private void LlenarNodosAtributo( XmlNode nodoAtributos, InterpreteNodoAtributosXML parent, InterpreteNodoAtributosXML interpreteNodoAtributos )
        //{
        //    foreach ( XmlNode nodoAtributo in nodoAtributos.ChildNodes )
        //    {
        //        if ( nodoAtributo.Name == cNombreNodoAtributo )
        //        {
        //            InterpreteNodoAtributoXML newInterpreteNodoAtributo;
        //            newInterpreteNodoAtributo = this.CrearInterpreteNodoAtributo( interpreteNodoAtributos, nodoAtributo );
        //            interpreteNodoAtributos.AgregarNodoAtributo( newInterpreteNodoAtributo );
        //        }
        //    }
        //}

        private void AgregarNodoParticipante( InterpreteNodoParticipanteXML nodo )
        {
//            this._nodosParticipante.Add( nodo );
        }

        public string[] ObtenerAtributosParticipanteSeleccionado( string fullNombreParticipante, string fullDescripcionParticipante )
        {
            string[] atributos = {"",""};

//            atributos = this.ObtenerAtributosParticipanteSegunNodoAtributos( this.ObtenerNodoAtributosSegunParticipante( fullDescripcionParticipante, this._nodosParticipante ), fullNombreParticipante, fullDescripcionParticipante );

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

        public string[] ObtenerAtributosParticipanteSegunNodoAtributos( InterpreteNodoAtributosXML nodoAtributos, string fullNombreParticipante, string fullDescripcionParticipante )
        {
            string[] atributos = {"",""};

            foreach ( InterpreteNodoAtributoXML nodoAtributo in nodoAtributos.NodosAtributo )
            {
                if ( atributos[0] == "" )
                {
                    atributos[0] = nodoAtributo.qNombre + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.qNombre;
//                    atributos[0] = nodoAtributo.Atributo + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.Atributo;
                    atributos[1] = nodoAtributo.Descripcion + cDelimitadorValorColumna + fullDescripcionParticipante + "." + nodoAtributo.Descripcion;
                }
                else
                {
//                    atributos[0] = atributos[0] + cDelimitadorColumnas + nodoAtributo.Atributo + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.Atributo;
                    atributos[0] = atributos[0] + cDelimitadorColumnas + nodoAtributo.qNombre + cDelimitadorValorColumna + fullNombreParticipante + "." + nodoAtributo.qNombre;
                    atributos[1] = atributos[1] + cDelimitadorColumnas + nodoAtributo.Descripcion + cDelimitadorValorColumna + fullDescripcionParticipante + "." + nodoAtributo.Descripcion;
                }
            }

            return atributos;
        }

    }
}
