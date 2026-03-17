using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Comparadores;

namespace ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda
{
	public class ArmadorDeLeyendaSegunParticipanteFaltanteGenerico : IArmadorDeLeyendaSegunParticipanteFaltante
	{
        #region IArmadorDeLeyendaSegunParticipanteFaltanteGenerico Members
        public string ObtenerLeyendaSegunRegla(ParticipanteFaltante participante)
        {
            throw new NotImplementedException();
        }

        //public string ObtenerCondicionFaltanteMonto(ParticipanteFaltante participante)
        //{ 
        
        //}
        public string ObtenerLeyendaSegunRegla( ParticipanteFaltante participante, InformacionPromocionIncumplida info )
		{
			string cantidadFaltante = "";
			string condicionFaltante = "";
			string leyenda = "";

			// Obtener un participante cualquiera para acceder al comprobante
			IParticipante cualquierParticipante = null;
			if (info.Comprobante != null)
			{
				var participantesComprobante = info.Comprobante.ObtenerParticipantesSegunClave("COMPROBANTE");
				if (participantesComprobante != null && participantesComprobante.Any())
				{
					cualquierParticipante = participantesComprobante[0];
				}
			}

			foreach ( Regla item in participante.Participante.Reglas )
			{
				// Evaluar si la regla se cumple para reglas que no sean CANTIDAD, MONTO o PORCENTAJEDESREC
				bool reglaSeCumple = false;
				if (item.Atributo != "CANTIDAD" && item.Atributo != "MONTO" && item.Atributo != "PORCENTAJEDESREC" && cualquierParticipante != null)
				{
					try
					{
						// Intentar obtener el valor del atributo del comprobante
						var atributo = cualquierParticipante.ObtenerAtributo(item.Atributo);
						if (atributo != null && atributo.Valor != null)
						{
							reglaSeCumple = CompararValores(item.Comparacion, atributo.TipoDato, item.Valor, atributo.Valor, item);
						}
					}
					catch
					{
						// Si falla la obtención del atributo, asumimos que no se cumple
						reglaSeCumple = false;
					}
				}
				else if (item.Atributo == "CANTIDAD" || item.Atributo == "MONTO")
				{
					// Para CANTIDAD y MONTO, verificar si se cumplió según los resultados
					var resultado = info.Resultados.FirstOrDefault(r => r.PartPromo.Id == participante.Participante.Id);
					if (resultado != null)
					{
						reglaSeCumple = resultado.Satisfecho >= resultado.Requerido;
					}
				}

				// Si la regla se cumple, continuar con la siguiente (no agregar a la leyenda)
				if (reglaSeCumple)
				{
					continue;
				}

				if ( item.Atributo == "CANTIDAD" )
				{
                    if (info.Promocion.Tipo != "4" && participante.Participante.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
                    {
                        if (condicionFaltante.Substring(condicionFaltante.Length - 2) == "y ")
                        {
                            condicionFaltante = condicionFaltante.Substring(0, condicionFaltante.Length - 3) + "con ";
                        }
                        condicionFaltante += "monto mayor a " + participante.Cantidad + "   ";
                    }
                    else
                    {
                        cantidadFaltante = participante.Cantidad + " ";
                    }
					
				}
				else if ( item.Atributo == "MONTO" )
				{
                    if (info.Promocion.Tipo != "4" && participante.Participante.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
                    { }
                    else
                    {
                        if (item.Id == 999998)
                        {
                            if (condicionFaltante.Substring(condicionFaltante.Length - 2) == "y ")
                            {
                                condicionFaltante = condicionFaltante.Substring(0, condicionFaltante.Length - 3) + "con ";
                            }
                            condicionFaltante += "monto mayor a 0" + "   ";
                        }
                        else
                        {
                            condicionFaltante += "Monto x " + participante.Cantidad + "   ";
                        }
                    }                    
                }
                else if (item.Atributo == "PORCENTAJEDESREC" && item.Id == 999996)
                {
                    if (info.Promocion.Tipo != "4" && participante.Participante.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
                    {
                        if (condicionFaltante.Substring(condicionFaltante.Length - 2) == "y ")
                        {
                            condicionFaltante = condicionFaltante.Substring(0, condicionFaltante.Length - 3);
                        }
                        condicionFaltante = condicionFaltante.TrimEnd() + " y sin porcentaje de descuento o recargo" + "   ";
                    }
                }
                else
				{
					if ( item.ValorMuestraRelacion != string.Empty && item.ValorMuestraRelacion != "[]")
					{
                        condicionFaltante += item.ValorMuestraRelacion + this.obtenerCondicion( participante.Participante, item );
					}
					else
					{
                        condicionFaltante += this.interpretarAtributo( item.Atributo ) + " " + this.interpretarComparacion( item.Comparacion ) + this.AjustarValor( item.ValorString ) + this.obtenerCondicion(participante.Participante, item);
					}

                }
			}

			if ( condicionFaltante.Length > 3 )
				leyenda = cantidadFaltante + condicionFaltante.Substring( 0, condicionFaltante.Length - 3 );
			else
				leyenda = cantidadFaltante + condicionFaltante;

			return leyenda.TrimEnd();
		}

        private string AjustarValor(string valorString)
        {
            string retorno = valorString;
            if (retorno.Substring(retorno.Count()-1,1) == "'")
            {
                retorno = retorno.Substring(0, retorno.Count() - 1).TrimEnd() + "'";
            }
            return retorno;
        }

        private string interpretarAtributo(string atributo)
        {
            string retorno = atributo;
            var partes = atributo.Split('.');
            if (partes.Count() >= 2)
            {
                if ( (partes[(partes.Count() - 1)].ToUpper() == "CODIGO") || (partes[(partes.Count() - 1)].ToUpper() == "DESCRIPCION"))
                {
                    retorno = partes[(partes.Count() - 2)] + " " + partes[(partes.Count() - 1)];
                }
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(retorno.ToLower());
        }
        #endregion

        private string obtenerCondicion( ParticipanteRegla participanteRegla, Regla regla )
        {
            string logica = participanteRegla.RelaReglas.Replace( "(", "" ).Replace( ")", "" );
            int id = regla.Id;
            string condicion = " y ";

            if ( id > 0 && participanteRegla.Reglas.Exists( x=>x.Id == id ) )
                if (id == 999994 || id == 999993 || id == 999992 || id == 999991)
                {
                    condicion = "Or";
                }
                else
                {
                    string inicial = "{" + id.ToString() + "}";
                    string final = "{" + (id + 1).ToString() + "}";

                    if ( logica.IndexOf( final ) < 0 )
                    {
                        final = "{999999}";
                    }
                    int largo = logica.IndexOf( final ) - (logica.IndexOf( inicial ) + inicial.Length);
                    if (largo < 0 )
                    {
                        largo = 0;
                    }
                    condicion = logica.Substring( logica.IndexOf( inicial ) + inicial.Length, largo );
                }
            return this.interpretarCondicion( condicion.Trim() );
        }
 
		private string interpretarCondicion( string condicion )
		{
			string retorno = " y ";
			if ( condicion == "Or" )
			{
				retorno = " o ";
			}
			return retorno;
		}

        private string interpretarComparacion( Factor comparador)
        {
            string retorno = "";
            if (comparador.Equals(Factor.DebeSerComienzaCon))
            {
                retorno = "que comience con ";
            } else if(comparador.Equals(Factor.DebeSerContieneA)) {
                retorno = "que contenga ";
            }
            else if (comparador.Equals(Factor.DebeSerDistintoA))
            {
                retorno = "distinto de ";
            }
            else if (comparador.Equals(Factor.DebeSerIgualA))
            {
                retorno = "igual a ";
            }
            else if (comparador.Equals(Factor.DebeSerMayorA))
            {
                retorno = "mayor a ";
            }
            else if (comparador.Equals(Factor.DebeSerMayorIgualA))
            {
                retorno = "mayor o igual a ";
            }
            else if (comparador.Equals(Factor.DebeSerMenorA))
            {
                retorno = "menor a ";
            }
            else if (comparador.Equals(Factor.DebeSerMenorIgualA))
            {
                retorno = "menor o igual a ";
            }
            else if (comparador.Equals(Factor.DebeSerTerminaCon))
            {
                retorno = "que termine con ";
            }
            else if (comparador.Equals(Factor.DebeSerComienzaCon))
            {
                retorno = "que comience con";
            }
            else if (comparador.Equals(Factor.None))
            {
                retorno = comparador.ToString();
            }
            return retorno;
        }

		private bool CompararValores(Factor comparacion, TipoDato tipoDato, object valorActual, object valorEsperado, Regla regla)
		{
			try
			{
				GestorComparaciones gestor = new GestorComparaciones();
				return gestor.Comparar(comparacion, tipoDato, valorActual, valorEsperado, regla);
			}
			catch
			{
				return false;
			}
		}
    }
}
