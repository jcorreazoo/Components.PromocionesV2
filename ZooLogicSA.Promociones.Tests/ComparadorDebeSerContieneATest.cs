using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerContieneATest
    {
        [TestMethod]
        public void ComparadorDebeSerContieneA_Cadena()
        {
            ComparadorDebeSerContieneA comparador = new ComparadorDebeSerContieneA();
            bool resultado = comparador.Comparar( TipoDato.C, "ART1", "123ART123" );
            Assert.IsTrue( resultado, "Error Comparacion DebeSerContieneA con cadena, debe dar true");
        }

        [TestMethod]
        public void ComparadorDebeSerContieneA_CadenaConMinusculas()
        {
            ComparadorDebeSerContieneA comparador = new ComparadorDebeSerContieneA(false);
            bool resultado = comparador.Comparar(TipoDato.C, "Art", "123ART123");
            Assert.IsTrue(resultado, "Error Comparacion DebeSerContieneA con cadena, debe dar true");
        }
    }
}
