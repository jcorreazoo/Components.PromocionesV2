using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class DescuentoXcaracteristicaTest
    {
        [TestMethod]
        public void VerificaQueCuponNoParticipaEnDescuentoXcaracteristica()
        {
            DescuentoXcaracteristica descuentoPromocion = new DescuentoXcaracteristica();
            String nodoParticipante = "CUPON";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaQueCualquierOtroParticipaEnDescuentoXcaracteristica()
        {
            DescuentoXcaracteristica descuentoPromocion = new DescuentoXcaracteristica();
             String nodoParticipante = "CualquierOtro";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }
    }
}
