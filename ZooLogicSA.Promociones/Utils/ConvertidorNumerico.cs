using System;
using System.Globalization;
using System.Threading;

namespace ZooLogicSA.Promociones.Utils
{
    public class ConvertidorNumerico : IConvertidorNumerico
    {
        public decimal ConvertToDecimal( string Cadena )
        {
            decimal Retorno = 0M;
            if ( !string.IsNullOrEmpty( Cadena ) )
            {
                decimal.TryParse( Cadena.Replace(',', '.'), NumberStyles.Number, new CultureInfo("en-US"), out Retorno );
            }
            return Retorno;
        }

        public int ConvertToInt( string Cadena )
        {
            int Retorno = 0;
            if ( !string.IsNullOrEmpty( Cadena ) )
            {
                int.TryParse( Cadena, NumberStyles.Integer, new CultureInfo("en-US"), out Retorno );
            }
            return Retorno;
        }

        public float ConvertToFloat( string Cadena )
        {
            float Retorno = 0F;
            if ( !string.IsNullOrEmpty( Cadena ) )
            {
                NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands;
                float.TryParse( Cadena.Replace( ',', '.' ), style, new CultureInfo("en-US"), out Retorno );
            }
            return Retorno;
        }

        public string ConvertToString( object Objeto )
        {
            return Objeto.ToString().Replace( ',', '.' );
        }

        public string ConvertToString( decimal Numero, bool conCultura = false )
        {
            string Retorno;
            if ( conCultura )
            {
                Retorno = Numero.ToString( new CultureInfo("en-US") );
            }
            else 
            {
                Retorno = Numero.ToString();
            }
            return Retorno;
        }
    }
}
