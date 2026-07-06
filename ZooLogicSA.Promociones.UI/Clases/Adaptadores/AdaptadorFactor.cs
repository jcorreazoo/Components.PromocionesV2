using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Clases.Adaptadores
{
    public class AdaptadorFactor
    {
        private static AdaptadorFactor instance;
        private List<AdaptadorFactorValores> listaValores = new List<AdaptadorFactorValores>();

        private AdaptadorFactor() 
        {
            this.LlenarListaValores();
        }

        public static AdaptadorFactor Instance
        { 
            get
            {
                if ( instance == null )
                {
                    instance = new AdaptadorFactor();
                }
                return instance;
            }
        }

        private void LlenarListaValores()
        {
            AdaptadorFactorValores factorValorIgual = new AdaptadorFactorValores( " = ", Factor.DebeSerIgualA, true );
            AdaptadorFactorValores factorValorMayor = new AdaptadorFactorValores( " > ", Factor.DebeSerMayorA, true );
            AdaptadorFactorValores factorValorMenor = new AdaptadorFactorValores( " < ", Factor.DebeSerMenorA, true );
            AdaptadorFactorValores factorValorMayorIgual = new AdaptadorFactorValores( " >= ", Factor.DebeSerMayorIgualA, true );
            AdaptadorFactorValores factorValorMenorIgual = new AdaptadorFactorValores( " <= ", Factor.DebeSerMenorIgualA, true );
            AdaptadorFactorValores factorValorDistinto = new AdaptadorFactorValores( " <> ", Factor.DebeSerDistintoA, true );
            AdaptadorFactorValores factorValorEntre = new AdaptadorFactorValores( " Between", Factor.DebeSerMayorIgualA, false );
            AdaptadorFactorValores factorValorContiene = new AdaptadorFactorValores( "Contains(", Factor.DebeSerContieneA, true );
            AdaptadorFactorValores factorValorComienzaCon = new AdaptadorFactorValores( "StartsWith(", Factor.DebeSerComienzaCon, true );
            AdaptadorFactorValores factorValorTerminaCon = new AdaptadorFactorValores( "EndsWith(", Factor.DebeSerTerminaCon, true );
            factorValorEntre.SecondValorFactor = Factor.DebeSerMenorIgualA;
            factorValorContiene.OperadorAlComienzo = true;
            factorValorComienzaCon.OperadorAlComienzo = true;
            factorValorTerminaCon.OperadorAlComienzo = true;
            this.listaValores.Add( factorValorIgual );
            this.listaValores.Add( factorValorMayor );
            this.listaValores.Add( factorValorMenor );
            this.listaValores.Add( factorValorMayorIgual );
            this.listaValores.Add( factorValorMenorIgual );
            this.listaValores.Add( factorValorDistinto );
            this.listaValores.Add( factorValorEntre );
            this.listaValores.Add( factorValorContiene );
            this.listaValores.Add( factorValorComienzaCon );
            this.listaValores.Add( factorValorTerminaCon );
        }

        public Factor ObtenerFactorAdaptado( string operadorAdaptar )
        {
            AdaptadorFactorValores valorAdaptado;
            valorAdaptado = this.listaValores.Find( o => o.Valor == operadorAdaptar );

            if ( valorAdaptado == null )
                throw new AdaptadorFactorException( "No existe equivalencia para el operador " + operadorAdaptar );

            return valorAdaptado.ValorFactor;
        }

        public string DetectarYObtenerFactorEnSentencia( string sentencia )
        {
            string factor = "";
            bool found = false;

            foreach ( AdaptadorFactorValores valor in this.listaValores )
            {
                if ( sentencia.Contains( valor.Valor ) )
                {
                    found = true;
                    factor = valor.Valor;
                    break;
                }
            }

            if ( !found )
                throw new AdaptadorFactorException( "No se encontró un operador dentro de la sentencia " + sentencia );

            return factor;
        }

        public bool DetectarYObtenerFactorDobleEnSentencia( string sentencia )
        {
            bool factorDoble = false;

            foreach ( AdaptadorFactorValores valor in this.listaValores )
            {
                if ( sentencia.Contains( valor.Valor ) )
                {

                    factorDoble = !valor.Single;
                    break;
                }
            }
            return factorDoble;
        }

        internal bool DebePonerElOperadorAlComienzo( string operador )
        {
            AdaptadorFactorValores valor;

            valor = this.listaValores.FirstOrDefault( o => o.Valor == operador );

            return valor.OperadorAlComienzo;
        }
    }
}