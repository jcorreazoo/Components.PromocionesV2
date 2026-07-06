using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerMenorA : IComparador
    {
        #region IComparador Members

        public bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            bool retorno = true;

            switch ( tipoDato )
            {
                case TipoDato.C:
                    retorno = ((string)valorParticipante).CompareTo( (string)valorRegla ) == -1;
                    break;
                case TipoDato.N:
                    retorno = convertNumber.ConvertToDecimal( valorRegla.ToString() ) > convertNumber.ConvertToDecimal( valorParticipante.ToString() );
                    break;
                case TipoDato.D:
                    retorno = ((DateTime)valorRegla) > (DateTime)valorParticipante;
                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }

        #endregion
    }
}
