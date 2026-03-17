using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ZooLogicSA.Promociones
{
    public class EvaluadorExpresion
    {
        public bool Evaluar( string expresion )
        {
            expresion = expresion.ToLower(CultureInfo.InvariantCulture);
            expresion = expresion.Replace("false", "0");
            expresion = expresion.Replace("true", "1");
            expresion = expresion.Replace(" ", "");
            expresion = expresion.Replace( "and", "&&" );
            expresion = expresion.Replace( "or", "||" );
            string temp;

            do
            {
                temp = expresion;
                expresion = expresion.Replace( "not(1)", "0" );
                expresion = expresion.Replace( "not(0)", "1" );
                expresion = expresion.Replace( "(0)", "0" );
                expresion = expresion.Replace( "(1)", "1" );
                expresion = expresion.Replace( "0&&0", "0" );
                expresion = expresion.Replace("0&&1", "0");
                expresion = expresion.Replace("1&&0", "0");
                expresion = expresion.Replace("1&&1", "1");
                expresion = expresion.Replace("0||0", "0");
                expresion = expresion.Replace("0||1", "1");
                expresion = expresion.Replace("1||0", "1");
                expresion = expresion.Replace("1||1", "1");
            }
            while (temp != expresion);

            if (expresion == "0")
                return false;
            if (expresion == "1")
                return true;
    
            throw new ArgumentException("expression");
        }
    }
}