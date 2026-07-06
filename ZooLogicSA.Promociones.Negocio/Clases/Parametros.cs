using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace ZooLogicSA.Promociones.Negocio.Clases
{
    public class Parametros
    {
        private static Dictionary<string, string> diccionarioArchivoINI = new Dictionary<string, string>();

        private static String LeerArchivo( string nombreArchivo )
        {
            try
            {
                StreamReader srLector = new StreamReader( nombreArchivo, System.Text.Encoding.Default );

                String Buffer = srLector.ReadToEnd();
                srLector.Close();

                return Buffer;

            }
            catch ( Exception )
            {
                return null;
            }
        }

        public static string ObtenerNombreProducto()
        {
            string ArchivoINI = "";
            string nombreProducto = "Zoo Logic";

            if ( System.Diagnostics.Debugger.IsAttached )
                nombreProducto = "Zoo Logic";
            else
            {
                string direccionAplicacion = ObtenerDirectorioDeLaAplicacion();
                ArchivoINI = direccionAplicacion + "Aplicacion.ini";
                string archivo = LeerArchivo( ArchivoINI );
                if ( archivo != null )
                {
                    ObtenerDiccionarioContenidoINI( archivo );
                    if ( diccionarioArchivoINI.ContainsKey( "nombrecomercial" ) )
                        nombreProducto = diccionarioArchivoINI["nombrecomercial"];
                }
            }

            return nombreProducto;
        }

        private static void ObtenerDiccionarioContenidoINI( String ArchivoINI )
        {

            if ( ArchivoINI != null )
            {

                //Separo el archivo en enters...
                string[] parseArchivoINI = ArchivoINI.Split( new string[] { Environment.NewLine }, StringSplitOptions.None );

                foreach ( string item in parseArchivoINI )
                {
                    //Separo los campos por xxx = yyy
                    string[] parseCampoArchivoINI = item.Split( new string[] { "=" }, StringSplitOptions.None );

                    //Si tiene un igual, es un field campo = valor.. sino no importa.
                    if ( parseCampoArchivoINI.Length > 1 )
                        diccionarioArchivoINI.Add( parseCampoArchivoINI[0].ToLowerInvariant().Trim(), parseCampoArchivoINI[1].ToLowerInvariant().Trim() );

                }

            }

        }

        private static String NavegarCarpeta( string direccionCarpeta, int nivelRetroceso )
        {

            String[] Parser = direccionCarpeta.Split( '\\' );

            String resultado = "";

            for ( int i = 0; i < Parser.Length - 1 - nivelRetroceso; i++ )
            {
                resultado += Parser[i] + "\\";
            }

            return resultado;

        }

        private static String ObtenerDirectorioDeLaAplicacion()
        {
            return NavegarCarpeta( AppDomain.CurrentDomain.BaseDirectory, 1 );
        }
    }
}
