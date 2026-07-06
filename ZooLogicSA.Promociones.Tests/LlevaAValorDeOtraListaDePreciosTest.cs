using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class LlevaAValorDeOtraListaDePreciosTest
    {
        [TestMethod]
        public void VerificaQueCuponNoParticipaEnLlevaAValorDeOtraListaDePrecios()
        {
            LlevaAValorDeOtraListaDePrecios descuentoPromocion = new LlevaAValorDeOtraListaDePrecios();
            String nodoParticipante = "CUPON";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaQueCualquierOtroParticipaEnLLevaXpagaY()
        {
            LlevaAValorDeOtraListaDePrecios descuentoPromocion = new LlevaAValorDeOtraListaDePrecios();
            String nodoParticipante = "CualquierOtro";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }
    }
}
