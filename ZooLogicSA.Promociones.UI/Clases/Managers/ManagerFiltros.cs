using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public static class ManagerFiltros
    {
//		private static string[] AtributosBasicos = { "Cantidad", "Stock", "Monto" };
		private static Dictionary<string, string> AtributosBasicos = new Dictionary<string, string>() {
                                                                                        { "Cantidad", "Cantidad" },
                                                                                        { "Stock", "Stock" },
                                                                                        { "Monto", "Monto" }};


        private static Dictionary<string, string> AtributosGenerales = new Dictionary<string, string>() {
                                                                                        { "MontoDescuento3", "Descuento Monto" },
                                                                                        { "PorcentajeDescuento", "Descuento Porcentaje" },
                                                                                        { "Total", "Total" }};


        private static ClauseType[] OperadoresValidos = { ClauseType.Equals, ClauseType.DoesNotEqual, ClauseType.Greater,
                                                            ClauseType.GreaterOrEqual, ClauseType.Less, ClauseType.LessOrEqual,
                                                            ClauseType.Between, ClauseType.NotBetween, ClauseType.Contains,
                                                            ClauseType.BeginsWith, ClauseType.EndsWith, ClauseType.DoesNotContain };


        public static void InicializarFiltro( FilterControl filtro )
        {
            filtro.FilterColumns.Clear();
            filtro.FilterColumns.Add( new UnboundFilterColumn( "", "", typeof( string ), new RepositoryItemTextEdit(), FilterColumnClauseClass.Lookup ) );

            InicializarAtributosGeneralesFiltro(filtro);
        }

        public static void InicializarAtributosBasicosFiltro( FilterControl filtro )
        {
            InicializarFiltro( filtro );
            RepositoryItemTextEdit ri = new RepositoryItemTextEdit();
            ri.MaxLength = 10;
            ri.Mask.MaskType = MaskType.Numeric;
            ri.Mask.EditMask = new string('#', 9) + "0." + new string('#', 2);

			foreach ( var atributo in AtributosBasicos )
			{
                filtro.FilterColumns.Add( new UnboundFilterColumn( atributo.Value, atributo.Key.ToUpper(), typeof( string ), ri, FilterColumnClauseClass.Generic ) );
			}

        }

        public static bool EsUnAtributoBasico( string nombreAtributo )
        { 
            bool es = false;

            foreach( var atributo in AtributosBasicos )
            {
                if ( atributo.Key.ToUpper() == nombreAtributo.ToUpper().Trim() )
                {
                    es = true;
                    break;
                }
            }

            return es;
        }

        public static void InicializarAtributosGeneralesFiltro(FilterControl filtro)
        {
            RepositoryItemTextEdit ri = new RepositoryItemTextEdit();
            ri.MaxLength = 10;
            ri.Mask.MaskType = MaskType.Numeric;
            ri.Mask.EditMask = new string('#', 9) + "0." + new string('#', 2);

            foreach (var atributo in AtributosGenerales)
            {
                filtro.FilterColumns.Add(new UnboundFilterColumn(atributo.Value, atributo.Key.ToUpper(), typeof(string), ri, FilterColumnClauseClass.Generic));
            }

        }

        public static bool EsUnAtributoGeneral(string nombreAtributo)
        {
            bool es = false;

            foreach (var atributo in AtributosGenerales)
            {
                if (atributo.Key.ToUpper() == nombreAtributo.ToUpper().Trim())
                {
                    es = true;
                    break;
                }
            }

            return es;
        }

        public static void AgregarAtributoParaFiltro( FilterControl filtro, string Descripcion, string Nombre, InterpreteNodoAtributoXML nodo ) 
        {

            RepositoryItemTextEdit ri = new RepositoryItemTextEdit();
            ri.MaxLength = nodo.Longitud;

            switch (nodo.TipoDato)
            {
                case "N":
                    ri.Mask.MaskType = MaskType.Numeric;
                    if (nodo.Decimales != 0)
                    {
                        ri.Mask.EditMask = new string('#', nodo.Longitud-1 ) + "0." + new string('#', nodo.Decimales );
                    }
                    else
                    {
                        ri.Mask.EditMask = new string('#', nodo.Longitud-1 ) + "0";
                    }
                    break;
                case "D":
                    ri.Mask.MaskType = MaskType.DateTime;
                    ri.Mask.EditMask = "d";
                    break;
            }

            if ( filtro.FilterColumns.GetFilterColumnByCaption( Descripcion ) == null )
            {
                filtro.FilterColumns.Add(new UnboundFilterColumn(Descripcion, Nombre, typeof(string), ri, FilterColumnClauseClass.String));
            }
        }

        public static void QuitarAtributoDeFiltro( FilterControl filtro, string atributo )
        {
            int indiceFiltro = ExisteAtributoEnFiltro( filtro, atributo );

            if ( indiceFiltro >= 0 )
            {
                filtro.FilterColumns.RemoveAt( indiceFiltro );
            }
        }

        public static bool EsUnOperadorValido( ClauseType operador, string condicion )
        {
            bool valido = false;

            foreach ( ClauseType operadorValido in OperadoresValidos )
            {
                if (operador == operadorValido)
                {
                    valido = true;
                    if ((EsAtributoCantidad(condicion) || EsAtributoMonto(condicion)) && operador != ClauseType.Equals && operador != ClauseType.GreaterOrEqual && operador != ClauseType.Greater)
                    {
                        valido = false;
                    }
                    break;
                }
            }

            return valido;
        }

        public static string NombreAtributoBasicoCantidad()
        {
            return "CANTIDAD";
        }

        public static string NombreAtributoBasicoMonto()
        {
            return "MONTO";
        }

        private static int ExisteAtributoEnFiltro( FilterControl filtro, string atributo = "" )
        {
            int existe = -1;

            if (filtro != null)
            {
                foreach (FilterColumn item in filtro.FilterColumns)
                {
                    if (item.FullName.ToUpper().Trim() == atributo.ToUpper().Trim())
                    {
                        existe = filtro.FilterColumns.IndexOf(item);
                        break;
                    }
                }
            }
            return existe;
        }

        private static bool EsAtributoCantidad( string condicion )
        {
            return (ManagerFiltros.NombreAtributoBasicoCantidad() == condicion.Substring( condicion.LastIndexOf('[') + 1, condicion.LastIndexOf(']') - 1).ToUpper() );
        }

        private static bool EsAtributoMonto( string condicion )
        {
            return (ManagerFiltros.NombreAtributoBasicoMonto() == condicion.Substring( condicion.LastIndexOf('[') + 1, condicion.LastIndexOf(']') - 1).ToUpper() );
        }
    }
}
