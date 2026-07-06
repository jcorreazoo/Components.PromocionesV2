using System;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Utils
{

    public sealed class RepositoriosEvaluadorMatematico : IEvaluadorMatematico
    {
        private static Dictionary<string, RepositoriosEvaluadorMatematico> instancias = new Dictionary<string, RepositoriosEvaluadorMatematico>();
        private static readonly object padlock = new object();

        private MethodInfo compilacion;

        public static RepositoriosEvaluadorMatematico ObtenerInstancia(string FormulaCalculoPrecio)
        {
            RepositoriosEvaluadorMatematico instancia = null;
            lock (padlock)
            {
                if (!instancias.TryGetValue(FormulaCalculoPrecio, out instancia))
                {
                    instancia = new RepositoriosEvaluadorMatematico(FormulaCalculoPrecio);
                    instancias.Add(FormulaCalculoPrecio, instancia);
                }
            }
            
            return instancia;
        }

        private RepositoriosEvaluadorMatematico(string FormulaCalculoPrecio)
        {
            this.compilacion = this.ObtenerCompilacion(FormulaCalculoPrecio);
        }

        public decimal ObtenerPrecio(decimal precio, decimal cantidad, decimal descuento, decimal montoDescuento)
        {
            return (Decimal)this.compilacion.Invoke(null, new object[] { precio, cantidad, descuento, montoDescuento });
        }

        private MethodInfo ObtenerCompilacion(string FormulaCalculoPrecio)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = false;

            string formula = this.ObtenerCadenaConFormula(FormulaCalculoPrecio);

            string tmpModuleSource = "namespace ns"
                                    + "{"
                                    + "using System;"
                                    + "class class1{"
                                    + "     public static decimal Eval( Decimal precio, Decimal cantidad, Decimal descuento, Decimal montoDescuento )"
                                    + "     {"
                                    + "         return " + formula + ";"
                                    + "     }"
                                    + "}} ";

            CompilerResults compilado = codeProvider.CompileAssemblyFromSource(compilerParameters, tmpModuleSource);

            MethodInfo retorno = compilado.CompiledAssembly.GetType("ns.class1").GetMethod("Eval");

            return retorno;
        }

        private string ObtenerCadenaConFormula(string formula)
        {
            formula = formula.Replace("<<PRECIO>>", "precio");
            formula = formula.Replace("<<CANTIDAD>>", "cantidad");
            formula = formula.Replace("<<DESCUENTO>>", "descuento");
            formula = formula.Replace("<<MONTODESCUENTO>>", "montoDescuento");

            return formula;
        }
    }
}

