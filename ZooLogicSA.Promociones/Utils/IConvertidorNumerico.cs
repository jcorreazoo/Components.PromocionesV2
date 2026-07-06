using System;

namespace ZooLogicSA.Promociones.Utils
{
    public interface IConvertidorNumerico
    {
        decimal ConvertToDecimal( string Cadena );
        int ConvertToInt( string Cadena );
        float ConvertToFloat( string Cadena );
        string ConvertToString( object Objeto );
        string ConvertToString( decimal Numero, bool conCultura );
    }
}
