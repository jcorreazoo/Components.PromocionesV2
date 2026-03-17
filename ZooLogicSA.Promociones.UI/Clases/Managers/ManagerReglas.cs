using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.UI.Controllers;
using DevExpress.XtraEditors;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.UI.Clases.Adaptadores;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public static class ManagerReglas
    {
        private const string cCodigoPartipanteComprobante = "COMPROBANTE";
        private const string cCodigoPartipanteItem = "COMPROBANTE.FACTURADETALLE.ITEM";
        private const string cCodigoPartipanteValor = "COMPROBANTE.VALORESDETALLE.ITEM";
        public const string cCodigoCabecera = "";
        public const string cCodigoDetalleItem = "FACTURADETALLE";
        private const string cCodigoDetalleValor = "VALORESDETALLE";
        private const string cAtributoFechaVigencia = "FECHA";
        private const string cAtributoHoraVigencia = "HORAALTAFW";
        private const string cHorarioDefault = "12:00:00 a.m.";
        private static string[] cListaOperadores = { " And ", " Or ", " And Not ", " Or Not ", " Not " };
        private static Promocion promo = new Promocion();

        public static void CrearPromo()
        {
            promo = new Promocion();
            SetearDatosPorDefecto( promo );
        }

        private static void SetearDatosPorDefecto( Promocion promo )
        {
            promo.Recursiva = true;
        }

        public static Promocion Promo
        {
            get
            {
                return promo;
            }
        }

        public static ParticipanteRegla CrearReglaVigencia( DateTime fechaDesde, DateTime fechaHasta, DateTime horaDesde, DateTime horaHasta, string[] dias )
        {
            ParticipanteRegla participanteRegla;
            Regla reglaFechaDesde;
            Regla reglaFechaHasta;
            int idSiguiente = 0;
            int indiceDia = 0;
            string relaDias = "";

            participanteRegla = ObtenerParticipanteComprobante( false );

            reglaFechaDesde = CrearRegla();
            reglaFechaHasta = CrearRegla();

            idSiguiente = ObtenerSiguienteIdParticipanteRegla( cCodigoPartipanteComprobante );
            reglaFechaDesde.Id = idSiguiente;
            reglaFechaDesde.Atributo = cAtributoFechaVigencia;
            reglaFechaDesde.DescripcionAtributo = cAtributoFechaVigencia;
            reglaFechaDesde.Comparacion = Factor.DebeSerMayorIgualA;
            reglaFechaDesde.Operador = " >= ";
            reglaFechaDesde.Valor = fechaDesde;
            reglaFechaDesde.ValorString = fechaDesde.ToString( "yyyy-MM-dd" );
            participanteRegla.Reglas.Add( reglaFechaDesde );

            idSiguiente = idSiguiente + 1;
            reglaFechaHasta.Id = idSiguiente;
            reglaFechaHasta.Atributo = cAtributoFechaVigencia;
            reglaFechaHasta.DescripcionAtributo = cAtributoFechaVigencia;
            reglaFechaHasta.Comparacion = Factor.DebeSerMenorIgualA;
            reglaFechaHasta.Operador = " <= ";
            reglaFechaHasta.Valor = fechaHasta;
            reglaFechaHasta.ValorString = fechaHasta.ToString( "yyyy-MM-dd" );
            participanteRegla.Reglas.Add( reglaFechaHasta );

            if ( participanteRegla.RelaReglas == null )
                participanteRegla.RelaReglas = CrearRelaReglaSimple( reglaFechaDesde.Id.ToString(), "and", reglaFechaHasta.Id.ToString() );
            else
                participanteRegla.RelaReglas = "(" + participanteRegla.RelaReglas + ") and (" + CrearRelaReglaSimple( reglaFechaDesde.Id.ToString(), "and", reglaFechaHasta.Id.ToString() ) + " )";

            if ( FiltrarPorHorario( fechaDesde, fechaHasta ) )
            {
                Regla reglaHoraDesde;
                Regla reglaHoraHasta;
                reglaHoraDesde = CrearRegla();
                reglaHoraHasta = CrearRegla();

                idSiguiente = idSiguiente + 1;
                reglaHoraDesde.Id = idSiguiente;
                reglaHoraDesde.Atributo = cAtributoHoraVigencia;
                reglaHoraDesde.DescripcionAtributo = cAtributoHoraVigencia;
                reglaHoraDesde.Comparacion = Factor.DebeSerMayorIgualA;
                reglaHoraDesde.Operador = " >= ";
                reglaHoraDesde.Valor = horaDesde.ToString("HH:mm:ss");
                reglaHoraDesde.ValorString = horaDesde.ToString( "HH:mm:ss" );
                participanteRegla.Reglas.Add( reglaHoraDesde );

                idSiguiente = idSiguiente + 1;
                reglaHoraHasta.Id = idSiguiente;
                reglaHoraHasta.Atributo = cAtributoHoraVigencia;
                reglaHoraHasta.DescripcionAtributo = cAtributoHoraVigencia;
                reglaHoraHasta.Comparacion = Factor.DebeSerMenorIgualA;
                reglaHoraHasta.Operador = " <= ";
                reglaHoraHasta.Valor = horaHasta.ToString( "HH:mm:ss" );
                reglaHoraHasta.ValorString = horaHasta.ToString( "HH:mm:ss" );
                participanteRegla.Reglas.Add( reglaHoraHasta );
                participanteRegla.RelaReglas = "(" + participanteRegla.RelaReglas + ") and (" + CrearRelaReglaSimple( reglaHoraDesde.Id.ToString(), "and", reglaHoraHasta.Id.ToString() ) + " )";
            }

            foreach ( string dia in dias )
            {
                if ( dia.ToLower() == "true" )
                {
                    Regla reglaDia;
                    reglaDia = CrearRegla();

                    idSiguiente = idSiguiente + 1;
                    reglaDia.Id = idSiguiente;
                    reglaDia.Atributo = cAtributoFechaVigencia;
                    reglaDia.DescripcionAtributo = cAtributoFechaVigencia;
                    reglaDia.Comparacion = Factor.DebeSerIgualADiaDeLaSemana;
                    reglaDia.Operador = " = ";
                    reglaDia.Valor = ConvertirValorDiaDeLaSemana( indiceDia );
                    reglaDia.ValorString = reglaDia.Valor.ToString();
                    participanteRegla.Reglas.Add( reglaDia );

                    if ( relaDias == "" )
                        relaDias = "{" + idSiguiente.ToString().Trim() + "}";
                    else
                        relaDias = relaDias + " or {" + idSiguiente.ToString().Trim() + "}";
                }
                indiceDia = indiceDia + 1;
            }
            if ( relaDias != "" )
                participanteRegla.RelaReglas = "(" + participanteRegla.RelaReglas + ") and ( " + relaDias + " )";

            return participanteRegla;
        }

        public static ParticipanteRegla CrearReglaCondicion( string reglaCondicion, IControllerPromocion control, bool EsBeneficiario )
        {
            ParticipanteRegla participanteRegla = null;
//            string nombreDetalle = "";
//            string fullDescripcion = ObtenerFullDescripcionEnRegla( reglaCondicion );

//            nombreDetalle = control.ObtenerTipoDetalleDelParticipante( fullDescripcion );
            string nombreDetalle = ObtenerNombreDetalleParticipante( reglaCondicion, control );

            participanteRegla = ObtenerParticipanteSegunDetalle( nombreDetalle, EsBeneficiario );
            CrearConjuntoDeReglas( participanteRegla, DetectarYObtenerConjuntoReglasEnCondicion( reglaCondicion ), nombreDetalle );
            CrearRelaReglas( participanteRegla, reglaCondicion );                
            
            return participanteRegla;
        }

        public static void AgregarReglaCondicion( string reglaCondicion, IControllerPromocion control, bool EsBeneficiario )
        {
            ParticipanteRegla participanteRegla = null;
            string nombreDetalle = ObtenerNombreDetalleParticipante( reglaCondicion, control );

            participanteRegla = ObtenerParticipanteSegunDetalle( nombreDetalle, EsBeneficiario );
            CrearConjuntoDeReglas( participanteRegla, DetectarYObtenerConjuntoReglasEnCondicion( reglaCondicion ), nombreDetalle );
//            AgregarConjuntoDeReglas( participanteRegla, DetectarYObtenerConjuntoReglasEnCondicion( reglaCondicion ), nombreDetalle );
            CrearRelaReglas( participanteRegla, reglaCondicion );

            return;
        }

        public static string ObtenerNombreDetalleParticipante( string reglaCondicion, IControllerPromocion control ) 
        {
            string fullDescripcion = ObtenerFullDescripcionEnRegla( reglaCondicion );

            return control.ObtenerTipoDetalleDelParticipante( fullDescripcion );
        }

        private static void CrearConjuntoDeReglas( ParticipanteRegla participanteRegla, List<string> conjuntoReglas, string nombreDetalle )
        {
            int idSiguiente = 0;
            
            idSiguiente = ObtenerSiguienteIdParticipanteRegla( ObtenerCodigoDetalleSegunNombre( nombreDetalle ) );
            foreach ( string sentencia in conjuntoReglas )
            {
                if ( AdaptadorFactor.Instance.DetectarYObtenerFactorDobleEnSentencia( sentencia ) )
                {
                    // La regla compuesta estoy suponiendo que es Between, cuando haya otra, hay que refactorizar
                    string atributoDescripcion = ObtenerAtributoDentroDeSentencia( sentencia );
                    string comparacionStringDesde = " >= ";
                    string valorDesde = ObtenerPrimerValorDentroDeSentencia( sentencia );
                    string comparacionStringHasta = " <= ";
                    string valorHasta = ObtenerUltimoValorDentroDeSentencia( sentencia );

                    Regla reglaCompuesta = CrearRegla();
                    Regla reglaAsociada = CrearRegla();

                    CompletarAtributosRegla( reglaCompuesta, atributoDescripcion, comparacionStringDesde, valorDesde, idSiguiente, true );
                    participanteRegla.Reglas.Add( reglaCompuesta );
                    idSiguiente++;
                    CompletarAtributosRegla( reglaAsociada, atributoDescripcion, comparacionStringHasta, valorHasta, idSiguiente, true );
                    SetearReglaAsociada( reglaCompuesta, reglaAsociada );
                    participanteRegla.Reglas.Add( reglaAsociada );
                    idSiguiente++;
                }
                else
                {
                    string atributoDescripcion = ObtenerAtributoDentroDeSentencia( sentencia );
                    string comparacionString = AdaptadorFactor.Instance.DetectarYObtenerFactorEnSentencia( sentencia );
                    string valor = ObtenerPrimerValorDentroDeSentencia( sentencia );

                    if ( !ExisteReglaEnLista( participanteRegla.Reglas, SetearTextoSentencia( atributoDescripcion, comparacionString, valor ) ) )
                    {
                        Regla nuevaRegla = CrearRegla();

                        CompletarAtributosRegla( nuevaRegla, atributoDescripcion, comparacionString, valor, idSiguiente, false );
                        participanteRegla.Reglas.Add( nuevaRegla );
                        idSiguiente++;
                    }
                }
            }
        }

        //private static void AgregarConjuntoDeReglas( ParticipanteRegla participanteRegla, List<string> conjuntoReglas, string nombreDetalle )
        //{
        //    int idSiguiente = 0;

        //    idSiguiente = ObtenerSiguienteIdParticipanteRegla( ObtenerCodigoDetalleSegunNombre( nombreDetalle ) );
        //    foreach ( string sentencia in conjuntoReglas )
        //    {
        //        if ( AdaptadorFactor.Instance.DetectarYObtenerFactorDobleEnSentencia( sentencia ) )
        //        {
        //            // La regla compuesta estoy suponiendo que es Between, cuando haya otra, hay que refactorizar
        //            string atributoDescripcion = ObtenerAtributoDentroDeSentencia( sentencia );
        //            string comparacionStringDesde = " >= ";
        //            string valorDesde = ObtenerPrimerValorDentroDeSentencia( sentencia );
        //            string comparacionStringHasta = " <= ";
        //            string valorHasta = ObtenerUltimoValorDentroDeSentencia( sentencia );

        //            Regla reglaCompuesta = CrearRegla();
        //            Regla reglaAsociada = CrearRegla();

        //            CompletarAtributosRegla( reglaCompuesta, atributoDescripcion, comparacionStringDesde, valorDesde, idSiguiente, true );
        //            participanteRegla.Reglas.Add( reglaCompuesta );
        //            idSiguiente++;
        //            CompletarAtributosRegla( reglaAsociada, atributoDescripcion, comparacionStringHasta, valorHasta, idSiguiente, true );
        //            SetearReglaAsociada( reglaCompuesta, reglaAsociada );
        //            participanteRegla.Reglas.Add( reglaAsociada );
        //            idSiguiente++;
        //        }
        //        else
        //        {
        //            string atributoDescripcion = ObtenerAtributoDentroDeSentencia( sentencia );
        //            string comparacionString = AdaptadorFactor.Instance.DetectarYObtenerFactorEnSentencia( sentencia );
        //            string valor = ObtenerPrimerValorDentroDeSentencia( sentencia );

        //            if ( !ExisteReglaEnLista( participanteRegla.Reglas, SetearTextoSentencia( atributoDescripcion, comparacionString, valor ) ) )
        //            {
        //                Regla nuevaRegla = CrearRegla();

        //                CompletarAtributosRegla( nuevaRegla, atributoDescripcion, comparacionString, valor, idSiguiente, false );
        //                participanteRegla.Reglas.Add( nuevaRegla );
        //                idSiguiente++;
        //            }
        //        }
        //    }
        //}

        private static void SetearReglaAsociada( Regla regla, Regla reglaAsociada )
        {
            regla.ReglaAsociada = reglaAsociada;
        }

        private static void CompletarAtributosRegla( Regla regla, string descripcionAtributo, 
                            string operador, string valor, int idSiguiente, bool compuesta )
        {
            regla.DescripcionAtributo = "[" + descripcionAtributo + "]";
            regla.Atributo = descripcionAtributo;
            regla.Operador = operador;
            regla.Comparacion = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operador );
            regla.OperadorAlComienzo = AdaptadorFactor.Instance.DebePonerElOperadorAlComienzo( operador );
            regla.Valor = valor.TrimEnd();
            regla.ValorString = "'" + valor + "'";
            regla.Compuesta = compuesta;
            regla.Id = idSiguiente;
            regla.ValorMuestraRelacion = "[" + "" + "]";
        }

        private static void CrearRelaReglas( ParticipanteRegla participanteRegla, string reglaCondicion )
        {
            string preRelaReglas = "";
            if ( participanteRegla.RelaReglas != null )
                preRelaReglas = participanteRegla.RelaReglas;
            string relaReglas = reglaCondicion;
            // La regla compuesta estoy suponiendo que es Between, cuando haya otra, hay que refactorizar
            foreach ( Regla regla in participanteRegla.Reglas )
            {
                if ( regla.Compuesta )
                {
                    if ( regla.ReglaAsociada != null )
                        relaReglas = relaReglas.Replace( regla.ObtenerSentenciaCompuesta(), "( {" + regla.Id.ToString() + "} And {" + regla.ReglaAsociada.Id + "} )" );
                }
                else
                    relaReglas = relaReglas.Replace( regla.ObtenerSentencia(), "( {" + regla.Id.ToString() + "} )" );
            }
            if ( preRelaReglas != "" )
                participanteRegla.RelaReglas = "( " + preRelaReglas + " ) And ( " + relaReglas + " )";
            else
                participanteRegla.RelaReglas = relaReglas;
        }


        private static bool ExisteReglaEnLista( List<Regla> listaReglas, string textoSentencia )
        {
            bool existe = false;

            foreach ( Regla regla in listaReglas )
            {
                if ( regla.ObtenerSentencia() == textoSentencia )
                {
                    existe = true;
                    break;
                }
            }

            return existe;
        }

        private static string SetearTextoSentencia( string atributo, string comparacion, string valor )
        {
            return "[" + atributo + "]" + comparacion + "'" + valor + "'";
        }

        private static string ObtenerAtributoDentroDeSentencia( string sentencia )
        {
            string atributo = "";
            int desde = 0;
            int largo = 0;
            if ( sentencia.Contains("[") && sentencia.Contains("]") )
            {
                desde = sentencia.IndexOf( "[" ) + 1;
                largo = sentencia.IndexOf( "]" ) - desde;
                atributo = sentencia.Substring( desde, largo );
            }

            return atributo;
        }

        private static string ObtenerPrimerValorDentroDeSentencia( string sentencia )
        {
            string valor = "";
            string caracter = "'";

            int desde = 0;
            int largo = 0;

            if ( CantidadDeVecesUnaLetraEnPalabra( sentencia, caracter[0] ) > 1 )
            {
                desde = sentencia.IndexOf( caracter ) + 1;
                largo = sentencia.IndexOf( caracter, desde ) - desde;
                valor = sentencia.Substring( desde, largo );
            }

            return valor;
        }

        private static string ObtenerUltimoValorDentroDeSentencia( string sentencia )
        {
            string valor = "";
            string caracter = "'";

            int hasta = 0;
            int desde = 0;
            int largo = 0;

            if ( CantidadDeVecesUnaLetraEnPalabra( sentencia, caracter[0] ) > 1 )
            {
                hasta = sentencia.LastIndexOf( caracter );
                desde = sentencia.LastIndexOf( caracter, hasta - 1 ) + 1;
                largo = hasta - desde;
                valor = sentencia.Substring( desde, largo );
            }

            return valor;
        }

        private static List<string> DetectarYObtenerConjuntoReglasEnCondicion( string reglaCondicion )
        {
            List<string> conjunto = new List<string>();
            int cantidadReglas = CantidadDeVecesUnaLetraEnPalabra( reglaCondicion, '[' );
            int startIndex = 0;
            int nextIndex = 0;
            int largoOperador = 0;

            for ( int i = 0; i < cantidadReglas; i++ )
            {
                nextIndex = ObtenerIndiceProximaSentencia( reglaCondicion, reglaCondicion.IndexOf( "]", startIndex ), ref largoOperador );
                if ( nextIndex == -1 )
                    conjunto.Add( reglaCondicion.Substring( startIndex ) );
                else
                    conjunto.Add( reglaCondicion.Substring( startIndex, nextIndex - startIndex ) );
                startIndex = nextIndex + largoOperador;
            }

            return conjunto;
        }

        private static int ObtenerIndiceProximaSentencia( string reglaCondicion, int startIndex, ref int largoOperador )
        {
            int indice = -1;

            foreach ( string operador in cListaOperadores )
            {
                try
                {
                    int indiceOperador = reglaCondicion.IndexOf( operador, startIndex );
                    if ( indiceOperador != -1 && (indice == -1 || indiceOperador <= indice) )
                    {
                        indice = indiceOperador;
                        largoOperador = operador.Length;
                    }
                }
                catch
                { 
                }
            }

            return indice;
        }

        private static int CantidadDeVecesUnaLetraEnPalabra( string palabra, char letra )
        {
            int cantidad = 0;

            foreach ( char caracter in palabra )
            {
                if ( caracter == letra )
                {
                    cantidad = cantidad + 1;
                }
            }

            return cantidad;
        }

        public static string ObtenerFullDescripcionEnRegla( string descripcionRegla )
        {
            string fullDescripcion = "";
            int desde;
            int largo;

            if ( descripcionRegla.Contains( "]" ) && descripcionRegla.Contains( "[" ) )
            {
                desde = descripcionRegla.IndexOf( "[" ) + 1;
                largo = descripcionRegla.IndexOf( "]" ) - desde;
                fullDescripcion = descripcionRegla.Substring( desde, largo );
            }

            return fullDescripcion;
        }

        private static ParticipanteRegla ObtenerParticipanteComprobante( bool EsBeneficiario )
        {
            ParticipanteRegla participanteRegla;

            if ( !ExisteParticipanteRegla( cCodigoPartipanteComprobante ) )
                participanteRegla = CrearParticipanteRegla( cCodigoPartipanteComprobante, EsBeneficiario );
            else
                participanteRegla = ObtenerParticipanteReglaExistente( cCodigoPartipanteComprobante );

            return participanteRegla;
        }

        private static ParticipanteRegla ObtenerParticipanteSegunDetalle( string nombreDetalle, bool EsBeneficiario )
        {
            ParticipanteRegla participanteRegla;
            string codigoDetalle = "";

            if ( EsDetalleCabecera( nombreDetalle ) )
            {
                participanteRegla = ObtenerParticipanteComprobante( EsBeneficiario );
            }
            else
            {
                codigoDetalle = ObtenerCodigoDetalleSegunNombre( nombreDetalle );
                participanteRegla = ObtenerParticipanteRegla( codigoDetalle, EsBeneficiario );
            }

            return participanteRegla;
        }

        public static bool EsDetalleCabecera( string nombreDetalle )
        {
            return (nombreDetalle.ToUpper().Trim() == cCodigoCabecera.ToUpper().Trim());
        }

        public static bool EsDetalleItem( string nombreDetalle )
        {
            return (nombreDetalle.ToUpper().Trim() == cCodigoDetalleItem.ToUpper().Trim());
        }

        public static string ObtenerCodigoDetalleSegunNombre( string nombreDetalle )
        {
            string codigo = "";

            if ( nombreDetalle.ToUpper().Trim() == cCodigoDetalleItem )
                codigo = cCodigoPartipanteItem;
            else if ( nombreDetalle.ToUpper().Trim() == cCodigoDetalleValor )
                codigo = cCodigoPartipanteValor;
            else
                codigo = cCodigoPartipanteComprobante;

            return codigo;
        }

        private static ParticipanteRegla ObtenerParticipanteRegla( string tipoDetalle, bool EsBeneficiario )
        {
            ParticipanteRegla participanteRegla;

            participanteRegla = CrearParticipanteRegla( tipoDetalle, EsBeneficiario );

            return participanteRegla;
        }

        private static Regla CrearRegla()
        {
            Regla regla = new Regla();

            return regla;
        }
        private static bool ExisteParticipanteRegla( string codigoParticipante )
        {
            bool resultado;

            resultado = promo.Participantes.Exists( o => o.Codigo == codigoParticipante );

            return resultado;
        }

        private static ParticipanteRegla ObtenerParticipanteReglaExistente( string codigoParticipante )
        {
            ParticipanteRegla regla = null;

            regla = promo.Participantes.Find( o => o.Codigo == codigoParticipante );

            return regla;
        }

        private static ParticipanteRegla CrearParticipanteRegla( string codigoParticipante, bool EsBeneficiario )
        {
            ParticipanteRegla regla = new ParticipanteRegla();

            regla.Codigo = codigoParticipante;
            regla.Id = ObtenerSiguienteIdPromocion();
            regla.Beneficiario = EsBeneficiario;

            return regla;
        }

        public static void AgregarParticipanteRegla( ParticipanteRegla participante )
        {
            promo.Participantes.Add( participante );
        }

        private static string ObtenerSiguienteIdPromocion()
        {
            int resultado = 0;
            int codigo;

            foreach ( ParticipanteRegla participante in promo.Participantes )
            {
                int.TryParse( participante.Id, out codigo );
                if ( codigo > resultado )
                    resultado = codigo;
            }
            resultado = resultado + 1;

            return resultado.ToString();
        }

        private static int ObtenerSiguienteIdParticipanteRegla( string codigoParticipante )
        {
            int resultado = 0;

            foreach ( ParticipanteRegla participante in promo.Participantes )
            {
                if ( participante.Codigo == codigoParticipante )
                {
                    foreach ( Regla regla in participante.Reglas )
                    {
                        if ( regla.Id > resultado )
                            resultado = regla.Id;
                    }
                }
            }
            resultado = resultado + 1;

            return resultado;
        }

        private static string CrearRelaReglaSimple( string id1, string operador, string id2 )
        {
            return "( {" + id1.Trim() + "} " + operador.Trim() + " {" + id2.Trim() + "} )";
        }

        private static bool FiltrarPorHorario( DateTime horaDesde, DateTime horaHasta )
        {
            bool resultado = true;

            if ( horaDesde.ToString() == cHorarioDefault && horaHasta.ToString() == cHorarioDefault )
                resultado = false;

            return resultado;
        }

        private static int ConvertirValorDiaDeLaSemana( int indice )
        {
            int retorno;

            // Domingo = 0, Lunes = 1, Martes = 2, etc
            // Y el array de WeekDays comienza con lunes, o sea indice = 0 es lunes
            if ( indice == 6 )
                retorno = 0;
            else
                retorno = indice + 1;

            return retorno;
        }

        internal static List<string> ObtenerIdParticipanteSegunDetalleYCondicionBeneficiario( List<string> listaParticipantes, string Detalle, IControllerPromocion control, TipoPromocion tipoPromo )
        {
            bool EsParticipanteBeneficiario = tipoPromo.TieneParticipantesBeneficiarios;

            //string id = "";
            List<string> retorno = new List<string>();

            if ( (NoTieneParticipantesSegunDetalle( listaParticipantes, Detalle, control )
                    || TieneMasDeUnParticipanteSegunDetalle( listaParticipantes, Detalle, control )
                ) && !(tipoPromo is RebajaXcaracteristica) )
                retorno.Add( "0" );
            else
            {
                foreach ( ParticipanteRegla participanteRegla in Promo.Participantes )
                {
                    if ( participanteRegla.Codigo.ToLower().Trim() == Detalle.ToLower().Trim() && participanteRegla.Beneficiario == EsParticipanteBeneficiario)
                    {
                        retorno.Add( participanteRegla.Id );
                        if ( !(tipoPromo is RebajaXcaracteristica) )
                            break;
                    }
                }
            }

            return retorno;
        }

        private static bool TieneMasDeUnParticipanteSegunDetalle( List<string> listaParticipantes, string Detalle, IControllerPromocion control )
        {
            string codigoDetalle = "";
            int contador = 0;

            if ( listaParticipantes.Count > 1 )
            {
                foreach ( string participante in listaParticipantes )
                {
                    codigoDetalle = ObtenerCodigoDetalleEnParticipante( participante, control );
                    if ( codigoDetalle.ToLower().Trim() == Detalle.ToLower().Trim() )
                        contador++;
                }
            }

            return ( contador > 1 );
        }

        public static bool NoTieneParticipantesSegunDetalle( List<string> listaParticipantes, string Detalle, IControllerPromocion control )
        {            
            return ( ObtenerCantidadParticipantesSegunDetalle(listaParticipantes, Detalle, control) == 0 );
        }

        public static int ObtenerCantidadParticipantesSegunDetalle(List<string> listaParticipantes, string Detalle, IControllerPromocion control)
        {
            int contador = 0;
            string codigoDetalle = "";


            if (listaParticipantes.Count > 0)
            {
                foreach (string participante in listaParticipantes)
                {
                    codigoDetalle = ObtenerCodigoDetalleEnParticipante(participante, control);
                    if (codigoDetalle.ToLower().Trim() == Detalle.ToLower().Trim())
                        contador++;
                }
            }

            return contador;
        }

        private static string ObtenerCodigoDetalleEnParticipante( string participante, IControllerPromocion control )
        {
            string fullDescripcion;
            string nombreDetalle = "";
            string codigo = "";

            fullDescripcion = ObtenerFullDescripcionEnRegla( participante );
            nombreDetalle = control.ObtenerTipoDetalleDelParticipante( fullDescripcion );
            codigo = ObtenerCodigoDetalleSegunNombre( nombreDetalle );

            return codigo;
        }

        public static string ObtenerCodigoDetalleSegunType( CodigoDetalleType type )
        {
            string codigo = "";

            switch ( type )
            { 
                case CodigoDetalleType.Cabecera:
                    {
                        codigo = cCodigoPartipanteComprobante;
                        break;
                    }
                case CodigoDetalleType.DetalleItem:
                    {
                        codigo = cCodigoPartipanteItem;
                        break;
                    }
                case CodigoDetalleType.DetalleValor:
                    {
                        codigo = cCodigoPartipanteValor;
                        break;
                    }
            }

            return codigo;
        }

        public static void SetearEleccionParticipante( EleccionParticipanteType eleccionParticipanteType )
        {
            promo.EleccionParticipante = eleccionParticipanteType;
        }

        public static void AgregarBeneficio( Beneficio beneficio )
        {
            promo.Beneficios.Add( beneficio );
        }
    }
}