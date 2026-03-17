using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ComparadorDebeSerTerminaConTest
    {
        [TestMethod]
        public void ComparadorDebeSerTerminaCon_Cadena()
        {
            ComparadorDebeSerTerminaCon comparador = new ComparadorDebeSerTerminaCon();
            bool resultado = comparador.Comparar( TipoDato.C, "ART", "123ART" );
            Assert.IsTrue( resultado, "Error Comparacion DebeSerTerminaCon con cadena, debe dar true");
        }

        [TestMethod]
        public void ComparadorDebeSerTerminaCon_CadenaConMinusculas()
        {
            ComparadorDebeSerTerminaCon comparador = new ComparadorDebeSerTerminaCon(false);
            bool resultado = comparador.Comparar(TipoDato.C, "Art", "123ART");
            Assert.IsTrue(resultado, "Error Comparacion DebeSerTerminaCon con cadena, debe dar true");
        }
    }
}
