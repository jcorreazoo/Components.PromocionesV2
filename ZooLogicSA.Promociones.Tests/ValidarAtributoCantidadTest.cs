using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace ZooLogicSA.Promociones.Tests
{


    /// <summary>
    ///This is a test class for ValidarAtributoCantidadTest and is intended
    ///to contain all ValidarAtributoCantidadTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarAtributoCantidadTest
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


        /// <summary>
        ///A test for Validar
        ///</summary>
        [TestMethod()]
        public void ValidarQueElAtributoCantidadNoEsteSoloTest()
        {
            string participanteRegla1 = "[Codigo] = '1' And [Cantidad] = '5'";
            string participanteRegla2 = "[Cantidad] = '5'";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add(participanteRegla1);
            participantesRegla.Add(participanteRegla2);
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarAtributoCantidad target = new ValidarAtributoCantidad(participantesRegla, nombreAtributoCantidad);
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "El atributo Cantidad no puede estar solo en una regla de participantes.";
            string actualMessage;

            actual = target.Validar();
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
