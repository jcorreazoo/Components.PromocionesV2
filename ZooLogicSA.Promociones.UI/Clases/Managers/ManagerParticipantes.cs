using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.XtraTreeList;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Columns;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Drawing;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using DevExpress.Data.Filtering.Helpers;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class ManagerParticipantes
    {
        private InterpreteParticipanteFiltroXML _interprete;
        private TreeList lisParticipantes;
        private TreeList lisAtributos;
        private FilterControl filtroCondicion;
        private FilterControl filtroBeneficio;
        private TipoPromocion UltimoTipoSeleccionado = null;
        private int NivelNodo = 0;

        public ManagerParticipantes( string xml, TreeList lstParticipantes, TreeList lstAtributos, 
                                    FilterControl filCondicion, FilterControl filBeneficio, TipoPromocion tp )
        {
            this._interprete = new InterpreteParticipanteFiltroXML( xml );
            this.lisAtributos = lstAtributos;
            this.lisParticipantes = lstParticipantes;
            this.filtroBeneficio = filBeneficio;
            this.filtroCondicion = filCondicion;
            this.InicializarControlParticipantes( tp );
        }

        internal InterpreteParticipanteFiltroXML Interprete
        {
            get 
            {
                return _interprete;
            }
        }

        internal void InicializarControlParticipantes( TipoPromocion tp )
        {
            if ( tp != null )
                this.UltimoTipoSeleccionado = tp;
            this.LimpiarListaAtributos();
            this.ArmarListaParticipantes( UltimoTipoSeleccionado );
            this.ArmarAtributosFiltro( UltimoTipoSeleccionado );
        }

        public void LimpiarListaAtributos()
        {
            this.lisAtributos.Columns.Clear();
            this.lisAtributos.ClearNodes();
            this.lisAtributos.ClearColumnsFilter();
        }
        
        private void AgregarColumnasFiltroParticipante( string descripcolumnas, string columnas, string delimitadorColumnas, string delimitadorValores, string atributocolumna )
        {
            string[] nombres;
            string[] descripciones;
            string[] atributos;
            if ( columnas.Length > 0 )
            {
                int contador = 0;
                nombres = columnas.Split( delimitadorColumnas.ToCharArray() );
                descripciones = descripcolumnas.Split( delimitadorColumnas.ToCharArray() );
                atributos = atributocolumna.Split( delimitadorColumnas.ToCharArray() );
                foreach ( string nombre in nombres )
                {
                    string[] valores = nombre.Split( delimitadorValores.ToCharArray() );
                    string[] valordescrip = descripciones[ contador ].Split( delimitadorValores.ToCharArray() );
                    string[] valoratrib = atributos[contador].Split( delimitadorValores.ToCharArray() );
                    if ( valores.Length > 1 )
                        this.AgregarColumna( this.lisAtributos, contador, valores[1], valordescrip[1], valores[0], 70, valoratrib[1] );
//                    this.AgregarColumna( this.lisAtributos, contador, valores[0], valores[1], 70 );
                    contador = contador + 1;
                }
            }
        }

        private void AgregarColumna( TreeList lista, int idColumna, string nombre, string field, string caption, int ancho, string atri )
        {
            TreeListColumn col = new TreeListColumn();
            lista.Columns.AddRange( new TreeListColumn[] { col } );
            col.OptionsColumn.AllowSort = true;
            col.OptionsColumn.AllowMove = false;
            col.OptionsColumn.AllowMoveToCustomizationForm = false;
            col.OptionsColumn.AllowEdit = false;
            col.OptionsColumn.AllowSize = false;
            col.Caption = caption;
//            col.FieldName = field;
            col.FieldName = atri;
            col.MinWidth = ancho;
//            col.Name = nombre;
            col.Name = atri;
            col.Visible = true;
            col.VisibleIndex = idColumna;
            col.Width = ancho;
//            col.Tag = atri;
            col.Tag = nombre;
            lista.Refresh();
        }

        public string ArmarColumnasAtributosParticipante( int idNodoParticipante )
        {
            string[] columnas = {"","",""};
            string[] NombreDescripcionyAtributo = { "", "", "" };
            string fullDescripcionParticipante = "";
            string fullNombreParticipante = "";
            string fullAtributoParticipante = "";
            this.LimpiarListaAtributos();
            NombreDescripcionyAtributo = this.ObtenerFullNombreyDescripcionParticipanteSeleccionado( idNodoParticipante );
            fullNombreParticipante = NombreDescripcionyAtributo[0];
            fullDescripcionParticipante = NombreDescripcionyAtributo[1];
            fullAtributoParticipante = NombreDescripcionyAtributo[2];
            columnas = this._interprete.ObtenerAtributosParticipanteSeleccionado( fullNombreParticipante, fullDescripcionParticipante, fullAtributoParticipante );
            this.AgregarColumnasFiltroParticipante( columnas[0], columnas[1], InterpreteParticipanteFiltroXML.cDelimitadorColumnas, InterpreteParticipanteFiltroXML.cDelimitadorValorColumna, columnas[2] );
            return columnas[0];
        }

        private string[] ObtenerFullNombreyDescripcionParticipanteSeleccionado( int idNodoParticipanteSeleccionado )
        {
            string[] nombreDescripcionyAtributo = {"","",""};
            TreeListNode nodoParticipanteSeleccionado = null;

            foreach ( TreeListNode nodoParticipante in this.lisParticipantes.Nodes )
            {
                nodoParticipanteSeleccionado = this.ObtenerNodoEnListaParticipantes( nodoParticipante, idNodoParticipanteSeleccionado );
                if ( nodoParticipanteSeleccionado != null )
                    break;
            }
            if ( nodoParticipanteSeleccionado != null )
            {
                nombreDescripcionyAtributo = this.ObtenerDescripcionParentEnListaParticipantes( nodoParticipanteSeleccionado );
            }

            return nombreDescripcionyAtributo;
        }

        private string[] ObtenerDescripcionParentEnListaParticipantes( TreeListNode nodoParticipante )
         {
            string[] nombreDescripcionyAtributo = {"","",""};
            TreeListNode nodoParticipanteParent;

            if ( nodoParticipante.GetValue( 0 ) != null )
                nombreDescripcionyAtributo[1] = (string)nodoParticipante.GetValue( 0 );
            if ( nodoParticipante.GetValue( 1 ) != null )
                nombreDescripcionyAtributo[0] = (string)nodoParticipante.GetValue( 1 );
            if ( nodoParticipante.GetValue( 3 ) != null )
                nombreDescripcionyAtributo[2] = (string)nodoParticipante.GetValue( 3 );
            if ( this.TieneParentEnListaParticipantes( nodoParticipante ) )
            {
                string[] result;
                nodoParticipanteParent = nodoParticipante.ParentNode;
                result = this.ObtenerDescripcionParentEnListaParticipantes( nodoParticipanteParent );
                nombreDescripcionyAtributo[0] = result[0] + "." + nombreDescripcionyAtributo[0];
                nombreDescripcionyAtributo[1] = result[1] + "." + nombreDescripcionyAtributo[1];
                nombreDescripcionyAtributo[2] = result[2] + "." + nombreDescripcionyAtributo[2];
            }

            return nombreDescripcionyAtributo;
        }

        private TreeListNode ObtenerNodoEnListaParticipantes( TreeListNode nodosParticipante, int idNodoParticipante )
        {
            TreeListNode nodoParticipante = null;

            //if ( nodoParticipante != null )
            //    return nodosParticipante;

            if ( nodosParticipante.Id == idNodoParticipante )
            {
                nodoParticipante = nodosParticipante;
            }
            else
            {
                foreach ( TreeListNode nodoHijo in nodosParticipante.Nodes )
                {
                    nodoParticipante = this.ObtenerNodoEnListaParticipantes( nodoHijo, idNodoParticipante );
                    if ( nodoParticipante != null )
                        break;
                }
            }

            return nodoParticipante;
        }

        private bool TieneParentEnListaParticipantes( TreeListNode nodoParticipante )
        {
            return ( nodoParticipante.ParentNode != null );
        }

        private void ArmarAtributosFiltro( TipoPromocion tp )
        {
            ManagerFiltros.InicializarAtributosBasicosFiltro( this.filtroBeneficio );
            ManagerFiltros.InicializarAtributosBasicosFiltro( this.filtroCondicion );

            foreach (InterpreteNodoParticipanteXML participante in this._interprete.Participantes)
            {
                if ( tp != null && tp.VerificarSiElParticipanteAplicaAEsteTipo(participante.qEntidad))
                {
                    this.RecorrerNodosParticipanteParaFiltro(participante);
                }
            }
        }

        private void ArmarAtributosFiltroSegunDetalle( string detalle )
        {
            if ( ManagerReglas.EsDetalleItem( detalle ) )
            {
                ManagerFiltros.InicializarAtributosBasicosFiltro( this.filtroBeneficio );
                ManagerFiltros.InicializarAtributosBasicosFiltro( this.filtroCondicion );
            }
            else
            {
                ManagerFiltros.InicializarFiltro( this.filtroBeneficio );
                ManagerFiltros.InicializarFiltro( this.filtroCondicion );
            }
            for ( int i = 0; i < this._interprete.Participantes.Count; i++ )
            {
                if ( this._interprete.Participantes[i].Detalle.Trim().ToUpper() == detalle.ToUpper().Trim() )
                    this.RecorrerNodosParticipanteParaFiltro( this._interprete.Participantes[i] );
            }
        }

        private void QuitarAtributoDeFiltro( FilterControl filtro, string atributo )
        {
            ManagerFiltros.QuitarAtributoDeFiltro( filtro, atributo );
        }

        private void RecorrerNodosParticipanteParaFiltro( InterpreteNodoParticipanteXML nodoParticipante )
        {
            foreach ( InterpreteNodoAtributosXML nodoAtributos in nodoParticipante.NodosAtributos )
            {
                this.RecorrerNodosAtributosDeParticipante( nodoAtributos );
            }
        }

        private void RecorrerNodosAtributosDeParticipante( InterpreteNodoAtributosXML nodoAtributos )
        {
            foreach( InterpreteNodoAtributoXML nodoAtributo in nodoAtributos.NodosAtributo )
            {
                ManagerFiltros.AgregarAtributoParaFiltro(this.filtroBeneficio, this.ObtenerFullDescripcionAtributo(nodoAtributo), this.ObtenerFullNombreAtributo(nodoAtributo), nodoAtributo); ;
                ManagerFiltros.AgregarAtributoParaFiltro(this.filtroCondicion, this.ObtenerFullDescripcionAtributo(nodoAtributo), this.ObtenerFullNombreAtributo(nodoAtributo), nodoAtributo);

            }
            foreach ( InterpreteNodoParticipanteXML nodoParticipante in nodoAtributos.NodosParticipante )
            {
                this.RecorrerNodosParticipanteParaFiltro( nodoParticipante );
            }
        }

        private bool TieneNodoAtributoPadre( InterpreteNodoAtributoXML nodoAtributo )
        {
            return (nodoAtributo.Parent != null);
        }

        private bool TieneNodoParticipantePadre( InterpreteNodoParticipanteXML nodoParticipante )
        {
            return (nodoParticipante.Parent != null);
        }

        private string ObtenerFullDescripcionAtributo( InterpreteNodoAtributoXML nodoAtributo )
        {
            string fullDescripcion = "";

            if ( this.TieneNodoAtributoPadre( nodoAtributo ) )
                fullDescripcion = this.ObtenerDescripcionPadre( nodoAtributo.Parent  ) + "." + nodoAtributo.Descripcion;
            else
                fullDescripcion = nodoAtributo.Descripcion;

            return fullDescripcion;
        }

        private string ObtenerDescripcionPadre( InterpreteNodoAtributosXML nodoAtributos )
        {
            InterpreteNodoParticipanteXML nodoPadreParticipante;

            nodoPadreParticipante = nodoAtributos.Parent;

            if ( nodoPadreParticipante == null )
                return "";
            string descripcion = nodoPadreParticipante.Descripcion;

            if ( !this.TieneNodoParticipantePadre( nodoPadreParticipante ) )
                descripcion = nodoPadreParticipante.Descripcion;
            else

                descripcion = this.ObtenerDescripcionPadre( nodoPadreParticipante.Parent ) + "." + descripcion;

            return descripcion;
        }

        private string ObtenerNombrePadre( InterpreteNodoAtributosXML nodoAtributos )
        {
            InterpreteNodoParticipanteXML nodoPadreParticipante;

            nodoPadreParticipante = nodoAtributos.Parent;

            if ( nodoPadreParticipante == null )
                return "";
//            string nombre = nodoPadreParticipante.qEntidad;
            string nombre = nodoPadreParticipante.Atributo;

            if ( !this.TieneNodoParticipantePadre( nodoPadreParticipante ) )
                nombre = nodoPadreParticipante.Atributo;
            //    nombre = nodoPadreParticipante.qEntidad;
            else
                nombre = this.ObtenerNombrePadre( nodoPadreParticipante.Parent ) + "." + nombre;

            return nombre;
        }

        private string ObtenerFullNombreAtributo( InterpreteNodoAtributoXML nodoAtributo )
        {
            string fullNombre = "";

            if ( this.TieneNodoAtributoPadre( nodoAtributo ) )
                //fullNombre = this.ObtenerNombrePadre( nodoAtributo.Parent ) + "." + nodoAtributo.Atributo;
                fullNombre = this.ObtenerNombrePadre( nodoAtributo.Parent ) + "." + nodoAtributo.qNombre;
            else
                //fullNombre = nodoAtributo.Atributo;
                fullNombre = nodoAtributo.qNombre;

            return fullNombre;
        }

        private void ArmarListaParticipantes( TipoPromocion tp )
        {
            this.lisParticipantes.ClearNodes();

            foreach (InterpreteNodoParticipanteXML participante in this._interprete.Participantes)
            {
                if ( tp != null && tp.VerificarSiElParticipanteAplicaAEsteTipo(participante.qEntidad))
                {
                    this.RecorrerNodosParticipante(participante, -1);
                }
            }
        }

        private void ArmarListaParticipantesSegunDetalle( string detalle )
        {
            this.lisParticipantes.ClearNodes();
            for ( int i = 0; i < this._interprete.Participantes.Count; i++ )
            {
                if ( this._interprete.Participantes[i].Detalle.Trim().ToUpper() == detalle.ToUpper().Trim() )
                    this.RecorrerNodosParticipante( this._interprete.Participantes[i], -1 );
            }
        }

        private void RecorrerNodosParticipante( InterpreteNodoParticipanteXML nodoParticipante, int idPadre )
        {
            // ESTE ES EL UNICO QUE ESTA BIEN
            this.NivelNodo++;
            bool agregar = true;

            // OPERADORATARJETA no se deberia agregar si el valor es del cupon
            
            if (nodoParticipante.qEntidad == "OPERADORATARJETA" && this.NivelNodo == 2)
            {
                agregar = false;
            }
            

            if (agregar)
            {
                this.AgregarParticipante(nodoParticipante.qEntidad, nodoParticipante.Descripcion, nodoParticipante.Detalle, idPadre, nodoParticipante.Atributo);
                //            this.AgregarParticipante( nodoParticipante.Entidad, nodoParticipante.Descripcion, nodoParticipante.Detalle, idPadre );
                foreach (InterpreteNodoAtributosXML nodoAtributos in nodoParticipante.NodosAtributos)
                {
                    this.RecorrerNodosAtributos(nodoAtributos);
                }
            }
            this.NivelNodo--;

        }

        private void RecorrerNodosAtributos( InterpreteNodoAtributosXML nodoAtributos )
        {
            int idPadre = ObtenerCantidadNodosLista( this.lisParticipantes ) - 1;
            foreach( InterpreteNodoParticipanteXML nodoParticipante in nodoAtributos.NodosParticipante )
            {
                this.RecorrerNodosParticipante( nodoParticipante, idPadre );
            }
        }

        private void AgregarParticipante( string nombre, string descripcion, string grupo, int idPadre, string atri ) 
        {
            this.lisParticipantes.AppendNode( new object[] { descripcion, nombre, grupo, atri, null }, idPadre );
        }

        private int ObtenerCantidadNodosLista( TreeList list )
        {
            int cantidad = list.Nodes.Count;

            foreach ( TreeListNode nodo in list.Nodes )
            {
                cantidad = cantidad + ObtenerCantidadNodos( nodo );
            }            

            return cantidad;
        }

        private int ObtenerCantidadNodos( TreeListNode nodo )
        {
            int cantidad = nodo.Nodes.Count;

            foreach ( TreeListNode nodoHijo in nodo.Nodes )
            {
                cantidad = cantidad + this.ObtenerCantidadNodos( nodoHijo );
            }

            return cantidad;
        }

        public void FiltrarParticipantesSegunGrupo( string fullNombre ) 
        {
            string filtro = "";
            filtro = this.ObtenerNombreFiltro( fullNombre );
            this.FiltrarParticipantes( filtro );
        }

        public void FiltrarParticipantes( string filtro )
        {
            this.ArmarListaParticipantesSegunDetalle(filtro);
            this.ArmarAtributosFiltroSegunDetalle(filtro);
        }

        public void FiltrarParticipantesSegunAtributoBasico()
        {
            // hoy todos los atributos basicos corresponden al detalle ITEMS. Si hubiera que agregar de otros tipo de detalle, habria que diferenciarlos
            string filtro = ManagerReglas.cCodigoDetalleItem;
            this.ArmarListaParticipantesSegunDetalle( filtro );
            this.ArmarAtributosFiltroSegunDetalle( filtro );
        }

        public void FiltrarParticipantesSegunAtributoGeneral()
        {
            string filtro = ManagerReglas.cCodigoCabecera;
            this.ArmarListaParticipantesSegunDetalle(filtro);
            this.ArmarAtributosFiltroSegunDetalle(filtro);
        }

        public void FiltrarAtributoFiltroBeneficio( string atributo )
        {
            this.QuitarAtributoDeFiltro( this.filtroBeneficio, atributo );
        }

        private string ObtenerNombreFiltro( string fullNombre )
        {
            string filtro = "";

            foreach ( InterpreteNodoParticipanteXML nodoParticipante in this._interprete.Participantes )
            {
                if ( this.ObtenerNombreParticipantePrincipal( fullNombre ).ToUpper().Trim()
                    == nodoParticipante.Descripcion.ToUpper().Trim() )
                {
                    filtro = nodoParticipante.Detalle;
                    break;
                }
            }

            return filtro;
        }

        private string ObtenerNombreParticipantePrincipal( string fullNombre )
        {
            string nombre = fullNombre;

            if ( nombre.IndexOf( "." ) > 0 )
                nombre = nombre.Substring( 0, nombre.IndexOf( "." ) );

            return nombre;
        }

        public bool EsOperadorAdmitidoParaLaPropiedad( string propiedad, string operador )
        {
            bool retorno = true;

            if ( propiedad.Equals( "[Cantidad]" ) && !operador.Equals( "Igual" ) )
            {
                retorno = false;
            }

            return retorno;
        }

        public object ObtenerOperadorPermitidoDefaultParaPropiedad( string propiedad )
        {
            object retorno;
            //if ( propiedad.Equals( "[Cantidad]" ) )
            //{
                retorno = ClauseType.Equals;
            //}

            return retorno;

        }
    }
}
