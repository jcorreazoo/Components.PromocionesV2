using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Clases.Beneficios
{
    public static class FactoryValorBeneficios
    {
        public static ValorBeneficio CrearValorBeneficio( TipoPromocion tipoPromocion, IControllerPromocion control, List<string> idParticipante, params object[] parametros )
        {
            ValorBeneficio valorBeneficio = new ValorBeneficio();

            switch ( tipoPromocion.BeneficioType )
            {
                case BeneficioType.LLevaXPagaY:
                    {
                        decimal cuantos = 1;
                        decimal cuantosLleva = 0;
                        decimal cuantosPaga = 0;
                        decimal.TryParse( control.ObtenerValorMaskCondicion(), out cuantosLleva );
                        decimal.TryParse( control.ObtenerValorMaskBeneficio(), out cuantosPaga );
                        if ( cuantosPaga < cuantosLleva )
                            cuantos = cuantosLleva - cuantosPaga;

                        DestinoBeneficio destino = new DestinoBeneficio();
                        destino.Participante = idParticipante[0];
                        destino.Cuantos = cuantos;
                        valorBeneficio.Destinos.Add( destino );
                        break;
                    }
                case BeneficioType.LlevaUnoPagaConDescuentoOtro:
                    {
                        decimal cuantos = 1;
                        //ParticipanteRegla participante = null;
                        //if ( parametros.Length > 0 )
                        //{
                        //    if ( parametros[0].GetType() == typeof( ParticipanteRegla ) )
                        //        participante = (ParticipanteRegla)parametros[0];
                        //}
                        //else
                        //    participante = ObtenerParticipanteReglaSegunTipoPromocion( tipoPromocion );

                        //if ( participante != null )
                        cuantos = ObtenerValorNodoCantidadEnListaDeParticipantesBeneficiarios( tipoPromocion );

                        //valorBeneficio.Cuantos = cuantos;
                        //valorBeneficio.IdParticipanteRegla = idParticipante;
                        List<ParticipanteRegla> participantes = new List<ParticipanteRegla>();
                        participantes = ObtenerParticipantesReglaSegunTipoPromocion(tipoPromocion);
                        if (participantes != null)
                            foreach (ParticipanteRegla partici in participantes)
                            {
                                cuantos = ObtenerValorNodoCantidadEnParticipanteRegla(partici);
                                DestinoBeneficio destino = new DestinoBeneficio();
                                destino.Participante = partici.Id;
                                destino.Cuantos = cuantos;
                                valorBeneficio.Destinos.Add(destino);
                            }



                        //DestinoBeneficio destino = new DestinoBeneficio();
                        //destino.Participante = idParticipante[0];
                        //destino.Cuantos = cuantos;
                        //valorBeneficio.Destinos.Add( destino );

                        valorBeneficio.Valor = control.ObtenerValorMaskBeneficio();
                        break;
                    }
                case BeneficioType.MontoFijoDeDescuento:
                    {
                        decimal cuantos = 1;
                        List<ParticipanteRegla> participantes = new List<ParticipanteRegla>();
                        if ( parametros.Length > 0 )
                        {
                            if ( parametros[0].GetType() == typeof( ParticipanteRegla ) )
                                participantes[0] = (ParticipanteRegla)parametros[0];
                        }
                        else
                            participantes = ObtenerParticipantesReglaSegunTipoPromocion( tipoPromocion );
                        if ( participantes != null )
                            foreach (ParticipanteRegla partici in participantes)
                            {
                                cuantos = ObtenerValorNodoCantidadEnParticipanteRegla(partici);
                                DestinoBeneficio destino = new DestinoBeneficio();
                                destino.Participante = partici.Id;
                                destino.Cuantos = cuantos;
                                valorBeneficio.Destinos.Add(destino);
                            }

                        //valorBeneficio.Cuantos = cuantos;
                        //valorBeneficio.IdParticipanteRegla = idParticipante;

                        valorBeneficio.Valor = control.ObtenerValorMaskBeneficio();
                        break;
                    }
                case BeneficioType.PorcentajeFijoDeDescuento:
                    {
                        decimal cuantos = 1;
                        ParticipanteRegla participante = null;
                        if ( parametros.Length > 0 )
                        {
                            if ( parametros[0].GetType() == typeof( ParticipanteRegla ) )
                                participante = (ParticipanteRegla)parametros[0];
                        }
                        else
                            participante = ObtenerParticipanteReglaSegunTipoPromocion( tipoPromocion );
                        if ( participante != null )
                            cuantos = ObtenerValorNodoCantidadEnParticipanteRegla( participante );
                        //valorBeneficio.Cuantos = cuantos;
                        //valorBeneficio.IdParticipanteRegla = idParticipante;
                        DestinoBeneficio destino = new DestinoBeneficio();
                        destino.Participante = idParticipante[0];
                        destino.Cuantos = cuantos;
                        valorBeneficio.Destinos.Add( destino );
                        valorBeneficio.Valor = control.ObtenerValorMaskBeneficio();
                        break;
                    }
                case BeneficioType.PorcentajeFijoDeDescuentoBancario:
                    {
                        decimal cuantos = 1;
                        ParticipanteRegla participante = null;
                        if ( parametros.Length > 0 )
                        {
                            if ( parametros[0].GetType() == typeof( ParticipanteRegla ) )
                                participante = (ParticipanteRegla)parametros[0];
                        }
                        else
                            participante = ObtenerParticipanteReglaSegunTipoPromocion( tipoPromocion );
                        if ( participante != null )
                            cuantos = ObtenerValorNodoCantidadEnParticipanteRegla( participante );
                        DestinoBeneficio destino = new DestinoBeneficio();
                        destino.Participante = idParticipante[0];
                        destino.Cuantos = cuantos;
                        valorBeneficio.Destinos.Add( destino );
                        valorBeneficio.Valor = control.ObtenerValorMaskBeneficio();
                        break;
                    }
                case BeneficioType.ValorDeOtraListaDePrecios:
                    {
                        decimal cuantos = 1;
                        ParticipanteRegla participante = null;
                        if (parametros.Length > 0)
                        {
                            if (parametros[0].GetType() == typeof(ParticipanteRegla))
                                participante = (ParticipanteRegla)parametros[0];
                        }
                        else
                            participante = ObtenerParticipanteReglaSegunTipoPromocion(tipoPromocion);
                        if (participante != null)
                            cuantos = ObtenerValorNodoCantidadEnParticipanteRegla(participante);
                        DestinoBeneficio destino = new DestinoBeneficio();
                        destino.Participante = idParticipante[0];
                        destino.Cuantos = cuantos;
                        valorBeneficio.Destinos.Add(destino);
                        valorBeneficio.Valor = control.ObtenerValorMaskBeneficio();
                        break;
                    }
            }
            return valorBeneficio;
        }

        private static decimal ObtenerValorNodoCantidadEnParticipanteRegla( ParticipanteRegla participanteRegla )
        {
            decimal valor = 1;

            foreach ( Regla regla in participanteRegla.Reglas )
            {
                if ( regla.ObtenerSentencia().ToUpper().Contains( "[CANTIDAD]" ) )
                {
                    string valorCantidad = "1";
                    string cadena = regla.ObtenerSentencia().ToUpper();
                    int startIndex = cadena.IndexOf( "[CANTIDAD]" + regla.Operador );
                    valorCantidad = cadena.Substring( cadena.IndexOf( "'", startIndex ) + 1, cadena.IndexOf( "'", cadena.IndexOf( "'", startIndex ) + 1 ) - 1 - cadena.IndexOf( "'", startIndex ) );
                    decimal.TryParse( valorCantidad.Replace(".", ","), out valor );
                    break;
                }
            }

            return valor;
        }

        private static decimal ObtenerValorNodoCantidadEnListaDeParticipantesBeneficiarios( TipoPromocion tipoPromo )
        {
            List<ParticipanteRegla> lista = ObtenerListaDeParticipantesBeneficiarios( tipoPromo );
            decimal valor = 1;

            foreach ( ParticipanteRegla participanteRegla in lista )
            {
                foreach ( Regla regla in participanteRegla.Reglas )
                {
                    if ( regla.ObtenerSentencia().ToUpper().Contains( "[CANTIDAD]" ) )
                    {
                        string valorCantidad = "1";
                        string cadena = regla.ObtenerSentencia().ToUpper();
                        int startIndex = cadena.IndexOf( "[CANTIDAD]" + regla.Operador );
                        valorCantidad = cadena.Substring( cadena.IndexOf( "'", startIndex ) + 1, cadena.IndexOf( "'", cadena.IndexOf( "'", startIndex ) + 1 ) - 1 - cadena.IndexOf( "'", startIndex ) );
                        decimal.TryParse( valorCantidad.Replace(".", ","), out valor );
                        break;
                    }
                }
            }
            return valor;
        }

        private static ParticipanteRegla ObtenerParticipanteReglaSegunTipoPromocion( TipoPromocion tipoPromocion )
        {
            string codigoDetalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );
            ParticipanteRegla participante = null;

            foreach ( ParticipanteRegla parti in ManagerReglas.Promo.Participantes )
            {
                if ( parti.Codigo == codigoDetalle )
                {
                    participante = parti;
                    break;
                }
            }

            return participante;
        }

        private static List<ParticipanteRegla> ObtenerListaDeParticipantesBeneficiarios( TipoPromocion tipoPromocion )
        {
            List<ParticipanteRegla> lista = new List<ParticipanteRegla>(); 
            string codigoDetalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );

            foreach ( ParticipanteRegla parti in ManagerReglas.Promo.Participantes )
            {
                if ( parti.Codigo == codigoDetalle && parti.Beneficiario )
                {
                    lista.Add( parti );
                }
            }

            return lista;
        }

        private static List<ParticipanteRegla> ObtenerParticipantesReglaSegunTipoPromocion(TipoPromocion tipoPromocion)
        {
            string codigoDetalle = ManagerReglas.ObtenerCodigoDetalleSegunType(tipoPromocion.codigoDetalleType);
            List<ParticipanteRegla> participante = new List<ParticipanteRegla>();

            bool hayBeneficio = ManagerReglas.Promo.Participantes.Any(x => x.Beneficiario);

            foreach (ParticipanteRegla parti in ManagerReglas.Promo.Participantes)
            {
                if (parti.Codigo == codigoDetalle)
                {

                    if (hayBeneficio)
                    {
                        if (parti.Beneficiario)
                        {
                            participante.Add(parti);
                        }
                    }
                    else
                    {
                        participante.Add(parti);
                    }

                }
            }

            return participante;
        }

    }
}
