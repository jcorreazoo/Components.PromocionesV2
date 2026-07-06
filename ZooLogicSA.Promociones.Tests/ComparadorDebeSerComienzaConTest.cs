using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerComienzaConTest
    {
        [TestMethod]
        public void ComparadorDebeComienzaconA_Cadena()
        {
            ComparadorDebeSerComienzaCon comparador = new ComparadorDebeSerComienzaCon();
            bool resultado = comparador.Comparar( TipoDato.C, "ART1", "ART123" );
            Assert.IsTrue( resultado, "Error Comparacion DebeSerComienzacon con cadena, debe dar true");
        }

        [TestMethod]
        public void ComparadorDebeSerComienzacon_CadenaConMinusculas()
        {
            ComparadorDebeSerComienzaCon comparador = new ComparadorDebeSerComienzaCon(false);
            bool resultado = comparador.Comparar(TipoDato.C, "Art", "ART123");
            Assert.IsTrue(resultado, "Error Comparacion DebeSerComienzacon con cadena, debe dar true");
        }
    }
}
