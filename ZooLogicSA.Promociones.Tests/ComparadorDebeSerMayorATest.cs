using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerMayorATest
    {
        [TestMethod]
        public void ComparadorDebeSerMayorA_Cadena()
        {
            ComparadorDebeSerMayorA comparador = new ComparadorDebeSerMayorA();
            bool resultado = comparador.Comparar( TipoDato.C, "00001", "00002" );

            Assert.IsTrue( resultado, "Error Comparacion DebeSerMayorA con Cadena, debe dar true" );
        }

        [TestMethod]
        public void ComparadorDebeSerMayorA_Fecha()
        {
            ComparadorDebeSerMayorA comparador = new ComparadorDebeSerMayorA();
            bool resultado = comparador.Comparar( TipoDato.D, DateTime.ParseExact("20/08/2012", "dd/MM/yyyy", null), DateTime.ParseExact("21/08/2012", "dd/MM/yyyy", null) );

            Assert.IsTrue( resultado, "Error Comparacion DebeSerMayorA con Fecha, debe dar true" );
        }
    }
}
