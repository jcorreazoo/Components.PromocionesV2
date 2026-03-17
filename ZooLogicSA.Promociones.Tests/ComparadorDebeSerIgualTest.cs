using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerIgualTest
    {
        [TestMethod]
        public void ComparadorDebeSerIgual_Cadena()
        {
            ComparadorDebeSerIgual comparador = new ComparadorDebeSerIgual();
            bool resultado = comparador.Comparar( TipoDato.C, "00008", "00008" );

            Assert.IsTrue( resultado, "Error Comparacion DebeSerIgual con cadena, debe dar true" );
        }

        [TestMethod]
        public void ComparadorDebeSerIgual_CadenaConMinusculas()
        {
            ComparadorDebeSerIgual comparador = new ComparadorDebeSerIgual(false);
            bool resultado = comparador.Comparar(TipoDato.C, "ART1", "Art1");

            Assert.IsTrue(resultado, "Error Comparacion DebeSerIgual con cadena, debe dar true");
        }

        [TestMethod]
        public void ComparadorDebeSerIgual_Fecha()
        {
            ComparadorDebeSerIgual comparador = new ComparadorDebeSerIgual();
            bool resultado = comparador.Comparar(TipoDato.D, DateTime.ParseExact("21/08/2012", "dd/MM/yyyy", null), DateTime.ParseExact("21/08/2012", "dd/MM/yyyy", null));

            Assert.IsTrue( resultado, "Error Comparacion DebeSerIgual con fecha, debe dar true");
        }
    }
}
