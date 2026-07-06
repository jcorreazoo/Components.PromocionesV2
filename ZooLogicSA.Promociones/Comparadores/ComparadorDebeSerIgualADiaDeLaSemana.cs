using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class ComparadorDebeSerIgualADiaDeLaSemana : IComparador
    {
        #region IComparador Members

        public bool Comparar( TipoDato tipoDato, object valorRegla, object valorParticipante )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            bool retorno = false;

            switch ( tipoDato )
            {
                case TipoDato.D:
                    retorno = (int)((DateTime)valorParticipante).DayOfWeek == convertNumber.ConvertToInt( valorRegla.ToString() );
                    break;
                default:
                    throw new NotSupportedException();
            }

            return retorno;
        }
        #endregion
    }
}
