using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerTerminaCon : IComparador
    {
        bool compararCaseSensitive = true;
        public ComparadorDebeSerTerminaCon(bool caseSensitive)
        {
            this.compararCaseSensitive = caseSensitive;
        }
        public ComparadorDebeSerTerminaCon()
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
                        retorno = ((string)valorParticipante).EndsWith((string)valorRegla);
                    else
                        retorno = ((string)valorParticipante).EndsWith((string)valorRegla, StringComparison.OrdinalIgnoreCase);                    

                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }

        #endregion
    }
}
