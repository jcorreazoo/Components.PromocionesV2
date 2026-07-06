using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class DescuentoBancarioConTopeTest
    {
        [TestMethod]
        public void VerificaSiCuponParticipaEnDescuentoBancarioConTope()
        {
            DescuentoBancarioConTope descuentoPromocion = new DescuentoBancarioConTope();
            String nodoParticipante = "CUPON";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaSiValorParticipaEnDescuentoBancarioConTope()
        {
            DescuentoBancarioConTope descuentoPromocion = new DescuentoBancarioConTope();
            String nodoParticipante = "VALOR";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsFalse(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }

        [TestMethod]
        public void VerificaQueCualquierOtroParticipaEnDescuentoBancarioConTope()
        {
            DescuentoBancarioConTope descuentoPromocion = new DescuentoBancarioConTope();
            String nodoParticipante = "CualquierOtro";
            bool resultado = false;

            resultado = descuentoPromocion.VerificarSiElParticipanteAplicaAEsteTipo(nodoParticipante);

            Assert.IsTrue(resultado, "El participante " + nodoParticipante + " no aplica al tipo " + descuentoPromocion.GetType().Name + ".");
        }
    }
}
