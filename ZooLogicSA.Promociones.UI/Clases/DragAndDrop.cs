using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.Data.Filtering;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public static class DragAndDrop
    {
        private static Color colorIuminado = Color.Aqua;
        private static Color colorNormal = Color.White;
        private static Cursor cursorDragueable = Cursors.Hand;
        public static string informacionDropeada = "";
        public static string informacionColumna = "";
        public static string informacionParticipante = "";
        public static bool Autocompletar = false;
        private static bool dragueando = false;
        private static TreeListState proviene;

        public static TreeListState DragueadoDesde()
        {
            return proviene;
        }

        private static void CambiarPunteroMouse( Control formulario, Cursor cursor )
        {
            formulario.Cursor = cursor;
        }

        public static void MostrarPunteroPuedeDraguear( Control fomulario )
        {
            CambiarPunteroMouse( fomulario, cursorDragueable );
        }

        public static void OcultarPunteroPuedeDraguear( Control fomulario )
        {
            CambiarPunteroMouse( fomulario, Cursors.Default );
        }

        public static void DraguearControl( Control formulario, Control control, TreeListState state )
        {
            if ( !dragueando )
            {
                dragueando = true;
                //            IluminarControlesDropeables( formulario, colorIuminado );
                proviene = state;
                control.DoDragDrop( control, DragDropEffects.Move );
                //            IluminarControlesDropeables( formulario, DragAndDrop.colorNormal );
                CambiarPunteroMouse( formulario, Cursors.Default );
                dragueando = false;
            }
        }

        public static void DropearControl( Control formulario, DragEventArgs datos )
        {
            IDataObject datosRecibidos;
            string[] formatos;
            string nombreTipoFormato = "";
            string[] datosDragObject = {"","",""};
            datosRecibidos = datos.Data;
            formatos = datos.Data.GetFormats();

            if ( formatos.Length > 0 )
            {
                nombreTipoFormato = ObtenerNombreSimpleTipoFormato( formatos[0] );
                datosDragObject = ObtenerDatoSegunFormato( datosRecibidos, nombreTipoFormato );
                informacionDropeada = datosDragObject[0];
                informacionColumna = datosDragObject[1];
                informacionParticipante = datosDragObject[2];
            }
            dragueando = false;
        }

        private static string ObtenerNombreSimpleTipoFormato( string nombreCompletoTipoFormato )
        {
            string resultado = "";
            int ubicacionUltimoPunto = 0;

            ubicacionUltimoPunto = nombreCompletoTipoFormato.LastIndexOf( "." );
            if ( ubicacionUltimoPunto > 0 )
            {
                resultado = nombreCompletoTipoFormato.Substring( ubicacionUltimoPunto + 1 );
            }

            return resultado;
        }

        private static string[] ObtenerDatoSegunFormato( IDataObject ObjetoInformacion, String nombreTipoFormato )
        {
            string[] resultado = {"","",""};
            
            if ( typeof( TextBox ).Name.ToUpper() == nombreTipoFormato.ToUpper() )
            {
                resultado[0] = ObtenerDatoFormatoTextBox( (TextBox)ObjetoInformacion.GetData( typeof( TextBox ) ) );
            }
            if ( typeof( DragObject ).Name.ToUpper() == nombreTipoFormato.ToUpper() )
            {
                resultado[0] = ObtenerDatoFormatoDragObject( (DragObject)ObjetoInformacion.GetData( typeof( DragObject ) ) );
                resultado[1] = ObtenerDatoColumnaFormatoDragObject( (DragObject)ObjetoInformacion.GetData( typeof( DragObject ) ) );
                resultado[2] = ObtenerDatoParticipanteFormatoDragObject( (DragObject)ObjetoInformacion.GetData( typeof( DragObject ) ) );
            }

            return resultado;
        }

        private static string ObtenerDatoFormatoTextBox( TextBox objeto )
        {
            return objeto.Text;
        }

        private static string ObtenerDatoFormatoDragObject( DragObject objeto )
        {
            return objeto.ObtenerData();
        }

        private static string ObtenerDatoColumnaFormatoDragObject( DragObject objeto )
        {
            return objeto.ObtenerDataColumna();
        }

        private static string ObtenerDatoParticipanteFormatoDragObject( DragObject objeto )
        {
            return objeto.ObtenerDataParticipante();
        }

        private static void IluminarControlesDropeables( Control contenedor, Color color )
        {
            foreach ( Control control in contenedor.Controls )
            {
                if ( control.HasChildren )
                {
                    IluminarControlesDropeables( control, color );
                    if ( control.AllowDrop )
                    {
                        control.BackColor = color;
                    }
                }
                else
                {
                    if ( control.AllowDrop )
                    {
                        control.BackColor = color;
                    }
                }
            }
        }

        private static bool FilterEditorTieneColumnaVacia( FilterControl controlFiltro )
        {
            return (controlFiltro.FilterString.IndexOf( "[]" ) >= 0);
        }

        private static int FilterEditorObtenerColumnaVacia( FilterControl controlFiltro )
        {
            return controlFiltro.FilterString.IndexOf( "[]" );
        }

        public static void FilterEditorReemplazarColumnaVacia( FilterControl controlFiltro, string valor )
        {
            string preValor, postValor;
            if ( FilterEditorTieneColumnaVacia( controlFiltro ) )
            {
                preValor = controlFiltro.FilterString.Substring( 0, FilterEditorObtenerColumnaVacia( controlFiltro ) );
                postValor = controlFiltro.FilterString.Substring( FilterEditorObtenerColumnaVacia( controlFiltro ) + 2 );
                //transformar
                controlFiltro.FilterString = preValor + "[" + valor + "]" + postValor;
            }
            else
            {
                string reglaNueva = "[" + valor + "]" + " = ?";
                if ( controlFiltro.FilterCriteria is GroupOperator )
                {
                    GroupOperator newOperator = (controlFiltro.FilterCriteria as GroupOperator);
                    newOperator.Operands.Add( CriteriaOperator.Parse( reglaNueva ) );
                    controlFiltro.FilterCriteria = newOperator;
                }
                else
                {
                    controlFiltro.FilterCriteria = new GroupOperator( GroupOperatorType.And, new CriteriaOperator[] { controlFiltro.FilterCriteria, CriteriaOperator.Parse( reglaNueva ) } );
                }
            }
        }

        private static bool FilterEditorTieneLugarVacio( FilterControl controlFiltro )
        {
            return ( controlFiltro.FilterString.IndexOf( "?" ) >= 0 );
        }

        private static int FilterEditorObtenerLugarVacio( FilterControl controlFiltro )
        {
            return controlFiltro.FilterString.IndexOf( "?" );
        }

        private static void FilterEditorTieneColumnaVaciaEnValorVacio( FilterControl controlFiltro, string valor )
        {
            int indiceLugarVacio;
            int indiceFinCorcheteProximo;

            indiceLugarVacio = FilterEditorObtenerLugarVacio( controlFiltro );
            if ( indiceLugarVacio <= 0 )
                return;
                indiceFinCorcheteProximo = controlFiltro.FilterString.LastIndexOf( "]", indiceLugarVacio );
            if ( indiceFinCorcheteProximo > 0 )
            {
                if ( controlFiltro.FilterString.Substring( indiceFinCorcheteProximo - 1, 1 ) == "[" )
                {
                    ReemplazarColumnaVaciaEnIndiceEspecifico( controlFiltro, indiceFinCorcheteProximo, valor );
                    Autocompletar = true;
                }
            }

            return;
        }

        private static void ReemplazarColumnaVaciaEnIndiceEspecifico( FilterControl controlFiltro, int indiceEspecifico, string valor )
        {
            string preValor, postValor;
            preValor = controlFiltro.FilterString.Substring( 0, indiceEspecifico - 1 );
            postValor = controlFiltro.FilterString.Substring( indiceEspecifico + 2 );
            //transformar
            controlFiltro.FilterString = preValor + "[" + valor + "]" + postValor;
        }

        public static void FilterEditorReemplazarLugarVacio( FilterControl controlFiltro, string valor, string valorColumna )
        {
            string preValor, postValor;
            if ( FilterEditorTieneLugarVacio( controlFiltro ) )
            {
                FilterEditorTieneColumnaVaciaEnValorVacio( controlFiltro, valorColumna );
                preValor = controlFiltro.FilterString.Substring( 0, FilterEditorObtenerLugarVacio( controlFiltro ) );
                postValor = controlFiltro.FilterString.Substring( FilterEditorObtenerLugarVacio( controlFiltro ) + 1 );
                controlFiltro.FilterString = preValor + "'" + valor + "'" + postValor;
            }
            else
            {
                //aca tiene que ir el transformado
                string reglaNueva = "[" + valorColumna + "] = '" + valor + "'";
                if ( controlFiltro.FilterCriteria is GroupOperator )
                {
                    GroupOperator newOperator = (controlFiltro.FilterCriteria as GroupOperator);
                    newOperator.Operands.Add( CriteriaOperator.Parse( reglaNueva, 0 ) );
                    controlFiltro.FilterCriteria = newOperator;
                }
                else
                {
                    controlFiltro.FilterCriteria = new GroupOperator( GroupOperatorType.And, new CriteriaOperator[] { controlFiltro.FilterCriteria, CriteriaOperator.Parse( reglaNueva, 0 ) } );
                }
                Autocompletar = true;
            }
        }
    }
}
