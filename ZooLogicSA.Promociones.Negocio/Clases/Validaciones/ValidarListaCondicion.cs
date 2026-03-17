using System;
using System.Linq;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarListaCondicion : IValidacionPromocion
    {
        private string _mensajeError = "";

        public bool Validar( params object[] parametros )
        {
            bool validar = this.ValidarParametros( parametros );

            if ( validar )
            {
                if ( parametros[0].ToString() != String.Empty )
                {
                    string listaCondiciones = (string)parametros[0];

                    if ( listaCondiciones.Contains( "[]" ) || listaCondiciones.Contains( "?" ) )
                    {
                        validar = false;
                        this._mensajeError = "Debe completar los atributos";
                    }                    
                }
                else
                {
                    validar = false;
                    this._mensajeError = "Debe cargar al menos una regla.";
                }

            }
            else 
            {
                this._mensajeError = "No son válidos los parámetros.";
            }
            if (validar)// me fijo que no haya mas de un atributo cantidad 
            {
                if (this.ValidarMenosDeUnAtributoCantidad(parametros))
                {
                    if (!this.ValidarAtributoCantidadEnPrimerNodo(parametros))
                    {
                        validar = false;
                        this._mensajeError = "El atributo Cantidad o Monto debe ingresarse solo en el nodo base de las condiciones de la promoción.";
                    }
                }
                else
                {
                    validar = false;
					this._mensajeError = "No se puede ingresar más de un atributo cantidad o monto para cada condición.";
                }
            }
            if (validar)// me fijo que no haya mas de un atributo descuento porcentaje / monto 
            {
                if (!this.ValidarMenosDeUnAtributoDescuento(parametros))
                {
                    validar = false;
                    this._mensajeError = "No se puede ingresar más de un atributo descuento porcentaje o monto para cada condición.";
                }
            }
            return validar;
        }

        private bool ValidarParametros( object[] parametros )
        {
            return ( parametros != null && parametros.Length > 0 && parametros[0].GetType() == typeof( string ));
        }

        public string ObtenerMensajeError()
        {
            return this._mensajeError;
        }

        private bool ValidarMenosDeUnAtributoCantidad(object[] parametros)
        {
            bool retorno = true;
            string[] arr;

            foreach (string CadaCondicion in parametros)
            {
                arr = DevolverCondicionDescompuestaPorCantidad(CadaCondicion);
                if (arr != null)
                {
                    if (arr.Length > 2)
                    {
                        retorno = false;
                        break;
                    }
                }
            }

            return retorno;
        }

        private bool ValidarAtributoCantidadEnPrimerNodo(object[] parametros)
        {
            bool retorno = true;
            string[] arr;

            foreach (string CadaCondicion in parametros)
            {
                arr = DevolverCondicionDescompuestaPorCantidad(CadaCondicion);
                if (arr!=null)
                {
                    //Hasta aca se que hay un atributo cantidad
                    if (this.CantidadEstaEnGrupo(CadaCondicion))
                    {
                        retorno = false;
                        break;
                    }
                }
            }


            return retorno;
        }

        private bool ValidarMenosDeUnAtributoDescuento(object[] parametros)
        {
            bool retorno = true;
            string[] arr;

            foreach (string CadaCondicion in parametros)
            {
                arr = DevolverCondicionDescompuestaPorDescuentoMonto(CadaCondicion);
                if (arr != null)
                {
                    if (arr.Length > 2)
                    {
                        retorno = false;
                        break;
                    }
                }
            }

            if (retorno)
            {
                foreach (string CadaCondicion in parametros)
                {
                    arr = DevolverCondicionDescompuestaPorDescuentoPorcentaje(CadaCondicion);
                    if (arr != null)
                    {
                        if (arr.Length > 2)
                        {
                            retorno = false;
                            break;
                        }
                    }
                }
            }

            return retorno;
        }

        private string[] DevolverCondicionDescompuestaPorCantidad( string condicion )
		{

			string[] delimitador = new string[2];
			string[] arr = null;

			delimitador[0] = "CANTIDAD";
			delimitador[1] = "MONTO";
            if ( condicion.Contains( "CANTIDAD" ) || condicion.Contains( "MONTO") )
			{
				// agrego caracteres al ppio y final para el caso que la regla empiece o termine con el atributo
				string buscar = "  " + condicion + "   ";
				arr = buscar.Split( delimitador, StringSplitOptions.RemoveEmptyEntries );
			}

			return arr;
		}

        private string[] DevolverCondicionDescompuestaPorDescuentoMonto(string condicion)
        {

            string[] delimitador = new string[1];
            string[] arr = null;

            delimitador[0] = "MONTODESCUENTO3";
            if (condicion.Contains("MONTODESCUENTO3"))
            {
                // agrego caracteres al ppio y final para el caso que la regla empiece o termine con el atributo
                string buscar = "  " + condicion + "   ";
                arr = buscar.Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
            }

            return arr;
        }

        private string[] DevolverCondicionDescompuestaPorDescuentoPorcentaje(string condicion)
        {

            string[] delimitador = new string[1];
            string[] arr = null;

            delimitador[0] = "PORCENTAJEDESCUENTO";
            if (condicion.Contains("PORCENTAJEDESCUENTO"))
            {
                // agrego caracteres al ppio y final para el caso que la regla empiece o termine con el atributo
                string buscar = "  " + condicion + "   ";
                arr = buscar.Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
            }

            return arr;
        }

        private bool CantidadEstaEnGrupo(string condicion)
        {
            bool retorno = false;

            long CantidadParentesis = condicion.LongCount(letra => letra.ToString() == "(");

            for (int i = 0; i < (int)CantidadParentesis; i++)
            {
                int PrimerParentesisCierra = condicion.IndexOf(")")+1;
                string CondicionHastaPrimerCierre = condicion.Substring(0, PrimerParentesisCierra);
                int ParentesisAbrePrimerParentesisCierra = CondicionHastaPrimerCierre.LastIndexOf("(");
                string CadenaAReemplazar = condicion.Substring(ParentesisAbrePrimerParentesisCierra,
                    PrimerParentesisCierra-ParentesisAbrePrimerParentesisCierra);
                condicion = condicion.Replace(CadenaAReemplazar,"");
            }
			if ( condicion.Contains( "CANTIDAD" ) || condicion.Contains( "MONTO" ) )
            {
                if (this.CantidadEstaEnGrupoSinParentesis(condicion))
                {
                    retorno = true; 
                }
            }
            else
            {
                retorno = true;
            }
            return retorno;
        }

        private bool CantidadEstaEnGrupoSinParentesis(string condicion)
        {
            bool retorno = false;
            string[] arr;
            int CantidadDeOr = CoincidenciaDe(" Or ", condicion);
            int CantidadDeAnd = CoincidenciaDe(" And ",condicion);
             if (CantidadDeOr.Equals(0) || (CantidadDeAnd.Equals(0)))
            {
                retorno = false; // Si son todos or, o son todos and, esta en el base
            }
            else
            {
                arr = DevolverCondicionDescompuestaPorCantidad(condicion);
                string izq = this.Aizquierda(arr[0]);
                string der = this.Aderecha(arr[1]);
                if ((izq.Equals("NADA")) && !(der.Equals("NADA")))
                {
                    retorno = false;
                } else
                {
                    switch (izq)
	                {
                        case "OR":
                            if (der=="OR")
                            {
                                int CantidadDeAndIz = CoincidenciaDe(" And ",arr[0]);
                                if (CantidadDeAndIz!=0)
                                {
                                    retorno = true;
                                }
                            } else 
	                        {
		                         retorno = true;
	                        }
                            break;
                        case "AND":
                             if (der=="AND")
                            {
                                long CantidadDeOrIz = CoincidenciaDe(" Or ",arr[0]);
                                if (CantidadDeOrIz!=0)
                                {
                                    retorno = true;
                                }
                            }else 
	                        {
		                         retorno = true;
	                        }
                        break;
                        default:
                        break;
                    }
                }

            }
            return retorno;
        }
        private string Aizquierda(string cadenaIzquierda)
        {
            string retorno = "NADA";
            int posOr = cadenaIzquierda.LastIndexOf(" Or ");
            int posAnd = cadenaIzquierda.LastIndexOf(" And ");
            if (posOr == posAnd)
            {
                retorno = "NADA";
            }
            else if (posOr < posAnd)
            {
                retorno = "AND";
            }
            else
            {
                retorno = "OR";
            }
            return retorno;
        }
        private string Aderecha(string cadenaDerecha)
        {
            string retorno = "NADA";
            int posOr = cadenaDerecha.IndexOf(" Or ");
            int posAnd = cadenaDerecha.IndexOf(" And ");
            if (posOr == posAnd)
            {
                retorno = "NADA";
            }
            else if (posOr < posAnd)
            {
                retorno = "OR";
            }
            else
            {
                retorno = "AND";
            }
            return retorno;
        }
        private int CoincidenciaDe(string aBuscar, string en)
        {
            int retorno = 0;
            string[] separador = new string[1];
            separador[0] = aBuscar;
            string[] Resultado = en.Split(separador, StringSplitOptions.None);
            retorno = Resultado.Count() - 1;
            return retorno;
        }
    }
}
