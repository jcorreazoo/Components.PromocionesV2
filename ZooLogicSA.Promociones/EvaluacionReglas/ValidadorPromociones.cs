using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comparadores;
using Microsoft.CSharp;
using System.CodeDom;
using System.IO;
using System.Xml;
using System.Reflection;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Utils;
using System.Globalization;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class ValidadorPromociones : IValidadorPromociones
    {
        private List<Promocion> promociones;
        private ConfiguracionComportamiento configuracionComportamiento;
        private List<IEvaluadorReglasDeParticipante> evaluadoresParticipantes;
        private ConvertidorNumerico convertNumber;

        public ValidadorPromociones( ConfiguracionComportamiento comportamiento, List<IEvaluadorReglasDeParticipante> evaluadorParticipantes )
        {
            this.configuracionComportamiento = comportamiento;
            this.evaluadoresParticipantes = evaluadorParticipantes;
            this.convertNumber = new ConvertidorNumerico();
        }

        public ValidadorPromociones()
        {
        }

        public List<ResultadoReglas> ComprobarReglas( IComprobante comprobante, Promocion promocion, bool tlEvaluarValoresConCuponesIntegrados )
        {
            RespuestaEvaluacion respuesta = new RespuestaEvaluacion();
            respuesta.Promocion = promocion.Id;

            List<ResultadoReglas> resultadoreglas = new List<ResultadoReglas>();

            foreach (ParticipanteRegla participanteEnPromocion in promocion.Participantes)
            {
                this.VerificarReglaCantidad( participanteEnPromocion );
                this.VerificarReglaMonto(participanteEnPromocion);
                this.ConvertirReglasCantidadDebeSerMayorA(participanteEnPromocion);
                this.VerificarReglaDescuentoRecargo(participanteEnPromocion, promocion);
                this.VerificarReglasCuponesIntegrados(participanteEnPromocion, promocion, tlEvaluarValoresConCuponesIntegrados);
                this.VerificarReglaTipoValor(participanteEnPromocion);

                if (!String.IsNullOrEmpty(promocion.ListaDePrecios))
                {
                    this.VerificarReglaMonedaDeListaDePrecios(participanteEnPromocion, promocion.ListaDePrecios, comprobante.ObtenerXmlPreciosAdicionales());
                }

                foreach ( IEvaluadorReglasDeParticipante evaluador in this.evaluadoresParticipantes )
                {
                    resultadoreglas.AddRange(evaluador.ObtenerResultados(comprobante, participanteEnPromocion));
                }
			}

            return resultadoreglas;
        }

        public void VerificarReglaTipoValor(ParticipanteRegla participanteEnPromocion)
        {
            if (participanteEnPromocion.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
            {
                List<Regla> reglasTipoValor = (from x in participanteEnPromocion.Reglas
                                               where x.Id == 999994
                                               select x).ToList();

                // si tiene el 99994 es que ya los agrego

                if (reglasTipoValor.Count() == 0)
                {
                    List<Regla> reglas = (from x in participanteEnPromocion.Reglas
                                          where x.Atributo == "VALOR.CODIGO" || x.Atributo == "VALOR.TIPO"
                                          select x).ToList();

                    if (reglas.Count() != 0)
                    {
                        Regla nueva = new Regla();
                        nueva.Atributo = "VALOR.TIPO";
                        nueva.Comparacion = Factor.DebeSerIgualA;
                        nueva.Id = 999994;
                        nueva.Valor = 1;
                        nueva.ValorString = "1";
                        nueva.ValorMuestraRelacion = "Valor tipo Moneda Local";
                        reglas.Add(nueva);
                        participanteEnPromocion.Reglas.Add(nueva);

                        Regla nueva1 = new Regla();
                        nueva1.Atributo = "VALOR.TIPO";
                        nueva1.Comparacion = Factor.DebeSerIgualA;
                        nueva1.Id = 999993;
                        nueva1.Valor = 2;
                        nueva1.ValorString = "2";
                        nueva1.ValorMuestraRelacion = "Moneda Extranjera";
                        reglas.Add(nueva1);
                        participanteEnPromocion.Reglas.Add(nueva1);

                        Regla nueva2 = new Regla();
                        nueva2.Atributo = "VALOR.TIPO";
                        nueva2.Comparacion = Factor.DebeSerIgualA;
                        nueva2.Id = 999992;
                        nueva2.Valor = 3;
                        nueva2.ValorString = "3";
                        nueva2.ValorMuestraRelacion = "Tarjeta de Crédito/Débito";
                        reglas.Add(nueva2);
                        participanteEnPromocion.Reglas.Add(nueva2);

                        Regla nueva3 = new Regla();
                        nueva3.Atributo = "VALOR.TIPO";
                        nueva3.Comparacion = Factor.DebeSerIgualA;
                        nueva3.Id = 999991;
                        nueva3.Valor = 11;
                        nueva3.ValorString = "11";
                        nueva3.ValorMuestraRelacion = "Pago Electrónico";
                        reglas.Add(nueva3);
                        participanteEnPromocion.Reglas.Add(nueva3);

                        participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and ({999994} Or {999993} Or {999992} Or {999991})";
                    }
                }
            }
        }

        private void VerificarReglaDescuentoRecargo(ParticipanteRegla participanteEnPromocion, Promocion promocion)
        {
            if (promocion.Tipo != "4" && participanteEnPromocion.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
            {
                List<Regla> reglas = (from x in participanteEnPromocion.Reglas
                                              where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].PorcenjateDescuentoRecargo
                                              select x).ToList();

                if (reglas.Count() == 0)
                {
                    Regla nueva = new Regla();
                    nueva.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].PorcenjateDescuentoRecargo;
                    nueva.Comparacion = Factor.DebeSerIgualA;
                    nueva.Id = 999996;
                    nueva.Valor = 0;
                    reglas.Add(nueva);

                    participanteEnPromocion.Reglas.Add(nueva);
                    participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and {999996}";
               }
            }
            
        }

        private void VerificarReglasCuponesIntegrados(ParticipanteRegla participanteEnPromocion, Promocion promocion, bool tlEvaluarValoresConCuponesIntegrados)
        {
            if (promocion.Tipo != "4" && participanteEnPromocion.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM")
            {
                List<Regla> reglas = (from x in participanteEnPromocion.Reglas
                                      where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].TieneCuponIntegrado
                                      select x).ToList();

                string regla = " and {999995}";                

                if (!tlEvaluarValoresConCuponesIntegrados && reglas.Count() == 0)
                {
                    Regla nueva = new Regla();
                    nueva.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].TieneCuponIntegrado;
                    nueva.Comparacion = Factor.DebeSerIgualA;
                    nueva.Id = 999995;
                    nueva.Valor = false;
                    reglas.Add(nueva);

                    participanteEnPromocion.Reglas.Add(nueva);
                    participanteEnPromocion.RelaReglas += regla;
                }
                else if (tlEvaluarValoresConCuponesIntegrados && reglas.Count() > 0)
                {
                    reglas.RemoveAll(r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].TieneCuponIntegrado);
                    participanteEnPromocion.Reglas.RemoveAll( r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].TieneCuponIntegrado);
                    participanteEnPromocion.RelaReglas = participanteEnPromocion.RelaReglas.Replace(regla, "");
                }
            }
        }

        private void VerificarReglaCantidad( ParticipanteRegla participanteEnPromocion )
		{
			List<Regla> reglasCantidad = (from x in participanteEnPromocion.Reglas
										  where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad
											|| x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].CantidadMonto
										  select x).ToList();

			if ( reglasCantidad.Count() == 0 )
			{
				Regla nueva = new Regla();
				nueva.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad;
                nueva.Comparacion = Factor.DebeSerIgualA;
                nueva.Id = 999999;
				nueva.Valor = 1;
				reglasCantidad.Add( nueva );

				participanteEnPromocion.Reglas.Add( nueva );
				participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and {999999}";
			}
		}

        private void VerificarReglaMonto(ParticipanteRegla participanteEnPromocion)
        {
            if (this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].AtributoId == "IDITEMVALORES")
            {
                List<Regla> reglasMonto = (from x in participanteEnPromocion.Reglas
                                           where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Precio
                                           select x).ToList();

                if (reglasMonto.Count() == 0)
                {
                    Regla nueva = new Regla();
                    nueva.Atributo = this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Precio;
                    nueva.Comparacion = Factor.DebeSerMayorA;
                    nueva.Id = 999998;
                    nueva.Valor = 0;
                    reglasMonto.Add(nueva);

                    participanteEnPromocion.Reglas.Add(nueva);
                    participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and {999998}";
                }
            }
        }

        private void VerificarReglaMonedaDeListaDePrecios(ParticipanteRegla participanteEnPromocion, string listaDePrecios, XmlDocument xmlPreciosAdicionales)
        {
            if (participanteEnPromocion.Codigo == "COMPROBANTE")
            {

                string monedaListaDePrecios = "PESOS";
                XmlNodeList xnList = xmlPreciosAdicionales.GetElementsByTagName("cprecioartic");
                foreach (XmlNode nodo in xnList)
                {
                    if (nodo.SelectSingleNode("lista").InnerText.ToString().Trim() == listaDePrecios.Trim())
                    {
                        monedaListaDePrecios = nodo.SelectSingleNode("monedalista").InnerText.ToString();
                        break;
                    }
                }


                List<Regla> reglasMoneda = (from x in participanteEnPromocion.Reglas
                                           where x.Atributo == "MONEDACOMPROBANTE.CODIGO" && x.Id == 999997
                                            select x).ToList();

                if (reglasMoneda.Count() == 0)
                {
                    Regla nueva = new Regla();
                    nueva.Atributo = "MONEDACOMPROBANTE.CODIGO";
                    nueva.Comparacion = Factor.DebeSerIgualA;
                    nueva.Id = 999997;
                    nueva.Valor = monedaListaDePrecios;
                    nueva.ValorString = monedaListaDePrecios;
                    nueva.ValorMuestraRelacion = " Moneda " + monedaListaDePrecios;
                    reglasMoneda.Add(nueva);

                    participanteEnPromocion.Reglas.Add(nueva);
                    participanteEnPromocion.RelaReglas = "(" + participanteEnPromocion.RelaReglas + ") and {999997}";
                }
            }
        }

        private void ConvertirReglasCantidadDebeSerMayorA(ParticipanteRegla participanteEnPromocion)
        {
            if (this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].AtributoId == "IDITEMARTICULOS")
            {

                foreach (Regla reglaCantidad in participanteEnPromocion.Reglas)
                {
                    if ( ( reglaCantidad.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].Cantidad
                            || reglaCantidad.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnPromocion.Codigo].CantidadMonto )
                       & ( reglaCantidad.Comparacion == Factor.DebeSerMayorA) )
                    {
                        if (reglaCantidad.Valor.ToString().Contains(".") || reglaCantidad.Valor.ToString().Contains(","))
                        {
                            reglaCantidad.Valor = this.convertNumber.ConvertToString( this.convertNumber.ConvertToDecimal( reglaCantidad.Valor.ToString() ) + 0.01M, true );
                        }
                        else
                        {
                            reglaCantidad.Valor = ( this.convertNumber.ConvertToInt( reglaCantidad.Valor.ToString() ) + 1).ToString();
                        }
                        reglaCantidad.Comparacion = Factor.DebeSerMayorIgualA;
                        reglaCantidad.ValorString = "'" + reglaCantidad.Valor + "'";
                        reglaCantidad.Operador = " >= ";
                    }
                }
            }
        }

        public void EstablecerLibreriaPromociones( List<Promocion> promociones )
        {
            this.promociones = promociones;
        }

        public List<ErrorEvaluador> ObtenerExcepciones()
        {
            List<ErrorEvaluador> retornoErrores = new List<ErrorEvaluador>();

            foreach (IEvaluadorReglasDeParticipante evaluador in this.evaluadoresParticipantes)
            {
                List<ErrorEvaluador> subLista = evaluador.ObtenerExcepciones();
                foreach (ErrorEvaluador excepcion in subLista)
                {
                    retornoErrores.Add(excepcion);
                }
            }
            return retornoErrores;
        }

    }
}
