using System;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerIgual : IComparador
    {
        bool compararCaseSensitive = true;
        public ComparadorDebeSerIgual( bool caseSensitive )
        {
            this.compararCaseSensitive = caseSensitive;
        }
        public ComparadorDebeSerIgual()
        {
        }
        #region IComparador Members

        public bool Comparar( TipoDato tipoDato, object valorEnRegla, object valorEnComprobante )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            bool retorno = true;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    if (this.compararCaseSensitive)
                        retorno = valorEnRegla.Equals(valorEnComprobante);
                    else
                        retorno = String.Equals((string)valorEnRegla, (string)valorEnComprobante, StringComparison.OrdinalIgnoreCase);                    

                    break;
                case TipoDato.N:
                    retorno = convertNumber.ConvertToDecimal( valorEnRegla.ToString() ) == convertNumber.ConvertToDecimal( valorEnComprobante.ToString() );
                    break;
                case TipoDato.D:
                    retorno = ((DateTime)valorEnRegla).Equals( (DateTime)valorEnComprobante );
                    break;
                case TipoDato.L:
                    retorno = (bool)valorEnRegla == (bool)valorEnComprobante;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }

        #endregion
    }
}
