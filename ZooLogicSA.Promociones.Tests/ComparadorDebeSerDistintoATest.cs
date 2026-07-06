using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerDinstintoATest
    {
        [TestMethod]
        public void ComparadorDebeSerDinstintoA_Cadena()
        {
            ComparadorDebeSerDistintoA comparador = new ComparadorDebeSerDistintoA();
            bool resultado = comparador.Comparar( TipoDato.C, "Art", "Art1" );
            Assert.IsTrue( resultado, "Error Comparacion DebeSerDinstintoA con cadena, debe dar true");
        }

        [TestMethod]
        public void ComparadorDebeSerDinstintoA_CadenaConMinusculas()
        {
            ComparadorDebeSerDistintoA comparador = new ComparadorDebeSerDistintoA(false);
            bool resultado = comparador.Comparar(TipoDato.C, "ART1", "Art1");
            Assert.IsTrue(!resultado, "Error Comparacion DebeSerDinstintoA con cadena, debe dar false");
        }
    }
}
