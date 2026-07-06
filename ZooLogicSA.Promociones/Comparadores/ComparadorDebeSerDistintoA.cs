using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerDistintoA : IComparador
    {
        bool compararCaseSensitive = true;
        public ComparadorDebeSerDistintoA(bool caseSensitive)
        {
            this.compararCaseSensitive = caseSensitive;
        }

        public ComparadorDebeSerDistintoA()
        {
        }
        #region IComparador Members

        public bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            bool retorno = true;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    if (this.compararCaseSensitive)
                        retorno = valorRegla.Equals(valorParticipante);                    
                    else
                        retorno = String.Equals((string)valorRegla, (string)valorParticipante, StringComparison.OrdinalIgnoreCase);

                    break;
                case TipoDato.N:
                    retorno = convertNumber.ConvertToDecimal( valorRegla.ToString() ) <= convertNumber.ConvertToDecimal( valorParticipante.ToString() );
                    break;
                case TipoDato.D:
                    retorno = ((DateTime)valorRegla).Equals( (DateTime)valorParticipante );
                    break;
                case TipoDato.L:
                    retorno = (bool)valorRegla == (bool)valorParticipante;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return !retorno;
        }

        #endregion
    }
}
