using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerMenorATest
    {
        [TestMethod]
        public void ComparadorDebeSerMenorA_Cadena()
        {
            ComparadorDebeSerMenorA comparador = new ComparadorDebeSerMenorA();
            bool resultado = comparador.Comparar( TipoDato.C, "00010", "00009" );

            Assert.IsTrue( resultado, "Error Comparacion DebeSerMayorA con cadena, debe dar true" );

        }

        [TestMethod]
        public void ComparadorDebeSerMenorA_Fecha()
        {
            ComparadorDebeSerMenorA comparador = new ComparadorDebeSerMenorA();
            bool resultado = comparador.Comparar( TipoDato.D, DateTime.ParseExact("22/08/2012", "dd/MM/yyyy", null), DateTime.ParseExact("21/08/2012", "dd/MM/yyyy", null) );

            Assert.IsTrue( resultado, "Error Comparacion DebeSerMayorA con Fecha, debe dar true" );

        }
    }
}
