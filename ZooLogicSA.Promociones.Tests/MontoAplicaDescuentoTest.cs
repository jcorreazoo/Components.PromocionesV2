using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class MontoAplicaDescuentoTest
    {
        [TestMethod]
        public void VerificaQueCuponNoParticipaEnDescuentoXcaracteristica()
        {
            MontoAplicaDescuento descuentoPromocion = new MontoAplicaDescuento();
            String nodoParticipante = "CUPON";

            bool resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsFalse(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaQueValorNoParticipaEnDescuentoXcaracteristica()
        {
            MontoAplicaDescuento descuentoPromocion = new MontoAplicaDescuento();
            String nodoParticipante = "VALOR";

            bool resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsFalse(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaQueCualquierOtroParticipaEnDescuentoXcaracteristica()
        {
            MontoAplicaDescuento descuentoPromocion = new MontoAplicaDescuento();
            String nodoParticipante = "CualquierOtro";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }
    }
}
