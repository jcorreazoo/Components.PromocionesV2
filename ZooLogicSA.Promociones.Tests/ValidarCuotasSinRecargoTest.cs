using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZooLogicSA.Promociones.Tests
{

    /// <summary>
    ///This is a test class for ValidarCuotasSinRecargoTest and is intended
    ///to contain all ValidarCuotasSinRecargoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarCuotasSinRecargoTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void ValidarValidoTest()
        {
            string cantCuotas = "100";
            ValidarCuotasSinRecargo target = new ValidarCuotasSinRecargo(); 
            bool expected = true; 
            bool actual;

            actual = target.Validar(cantCuotas);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarValidoMensajeCuotaMayorTest()
        {
            string cantCuotas = "200";
            ValidarCuotasSinRecargo target = new ValidarCuotasSinRecargo();
            bool actual;
            string expectedMessage = "No se puede asignar una cantidad de cuotas mayor a 150.";
            string actualMessage;

            actual = target.Validar(cantCuotas);

            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod()]
        public void ValidarValidoMensajeCuotaCeroTest()
        {
            string cantCuotas = "0";
            ValidarCuotasSinRecargo target = new ValidarCuotasSinRecargo();
            bool actual;
            string expectedMessage = "Debe asignar una cantidad de cuotas mayor a cero en el beneficio.";
            string actualMessage;

            actual = target.Validar(cantCuotas);

            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expectedMessage, actualMessage);
        }

    }
}
