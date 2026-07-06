using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerComienzaCon : IComparador
    {
        bool compararCaseSensitive = true;
        public ComparadorDebeSerComienzaCon(bool caseSensitive)
        {
            this.compararCaseSensitive = caseSensitive;
        }
        public ComparadorDebeSerComienzaCon()
        {
        }
        #region IComparador Members

        public bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante )
        {
            bool retorno = true;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    if (this.compararCaseSensitive)
                        retorno = ((string)valorParticipante).StartsWith((string)valorRegla);
                    else
                        retorno = ((string)valorParticipante).StartsWith((string)valorRegla, StringComparison.OrdinalIgnoreCase);                    

                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }

        #endregion
    }
}
