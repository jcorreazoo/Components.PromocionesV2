using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerContieneA : IComparador
    {
        bool compararCaseSensitive = true;
        public ComparadorDebeSerContieneA(bool caseSensitive)
        {
            this.compararCaseSensitive = caseSensitive;
        }
        public ComparadorDebeSerContieneA()
        {
        }
        #region IComparador Members0

        public bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante )
        {
            bool retorno = true;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    if (this.compararCaseSensitive)
                        retorno = ((string)valorParticipante).Contains((string)valorRegla);                    
                    else
                        retorno = ((string)valorRegla).IndexOf((string)valorRegla, StringComparison.OrdinalIgnoreCase) >= 0;

                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }

        #endregion
    }
}
