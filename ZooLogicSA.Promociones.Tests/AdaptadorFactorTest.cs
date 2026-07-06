using ZooLogicSA.Promociones.UI.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones;
using ZooLogicSA.Promociones.UI.Clases.Adaptadores;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for AdaptadorFactorTest and is intended
    ///to contain all AdaptadorFactorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdaptadorFactorTest
    {

        /// <summary>
        ///A test for ObtenerFactorAdaptado
        ///</summary>
        [TestMethod()]
        [ExpectedException( typeof( AdaptadorFactorException ))]
        public void ObtenerFactorAdaptadoNoExisteTest()
        {
            string operadorAdaptar = "NOEXISTE"; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoIgualTest()
        {
            string operadorAdaptar = " = "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerIgualA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoMayorTest()
        {
            string operadorAdaptar = " > "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerMayorA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoMenorTest()
        {
            string operadorAdaptar = " < "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerMenorA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoDistintoTest()
        {
            string operadorAdaptar = " <> "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerDistintoA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoMayorIgualTest()
        {
            string operadorAdaptar = " >= "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerMayorIgualA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoMenorIgualTest()
        {
            string operadorAdaptar = " <= "; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerMenorIgualA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void DetectarYObtenerFactorAdaptadoEnSentenciaTest()
        {
            string sentencia = "[Campo] > valor";
            string expected = " > ";
            string actual;

            actual = AdaptadorFactor.Instance.DetectarYObtenerFactorEnSentencia( sentencia );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerFactorAdaptadoContiene()
        {
            string operadorAdaptar = "Contains("; // TODO: Initialize to an appropriate value
            Factor expected = Factor.DebeSerContieneA; // TODO: Initialize to an appropriate value
            Factor actual;
            actual = AdaptadorFactor.Instance.ObtenerFactorAdaptado( operadorAdaptar );
            Assert.AreEqual( expected, actual );
        }
    }
}
