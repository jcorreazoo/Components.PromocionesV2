using System;
namespace ZooLogicSA.Promociones.Utils
{
    public interface IEvaluadorMatematico
    {
        //Decimal Evaluar( string MathExpression );
        Decimal ObtenerPrecio( Decimal precio, Decimal cantidad, Decimal descuento, Decimal montoDescuento );
    }
}
