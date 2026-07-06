using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace ZooLogicSA.Promociones.Tests
{


    /// <summary>
    ///This is a test class for ValidarCantidadDeRegaloEnBeneficioTest and is intended
    ///to contain all ValidarCantidadDeRegaloEnBeneficioTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarCantidadDeRegaloEnBeneficioTest
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
        public void ValidarMalCantidadParametros()
        {
            ValidarCantidadDeRegaloEnBeneficio target = new ValidarCantidadDeRegaloEnBeneficio(); // TODO: Initialize to an appropriate value
            object[] parametros = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "No se recibieron correctamente la cantidad de parámetros.";
            string actualMessage = "";

            actual = target.Validar(parametros);
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod()]
        public void ValidarMalParametros()
        {
            ValidarCantidadDeRegaloEnBeneficio target = new ValidarCantidadDeRegaloEnBeneficio(); // TODO: Initialize to an appropriate value
            object[] parametros = { "aa", "bb" }; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "No se recibieron correctamente los parámetros.";
            string actualMessage = "";

            actual = target.Validar(parametros);
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod()]
        public void ValidarMal()
        {
            ValidarCantidadDeRegaloEnBeneficio target = new ValidarCantidadDeRegaloEnBeneficio(); // TODO: Initialize to an appropriate value
            int lleva = 4;
            int paga = 8;
            object[] parametros = { lleva, paga }; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "No se puede crear una promoción donde la cantidad que paga es igual o mayor a la que lleva.";
            string actualMessage = "";

            actual = target.Validar(parametros);
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedMessage, actualMessage);
        }

        [TestMethod()]
        public void ValidarOK()
        {
            ValidarCantidadDeRegaloEnBeneficio target = new ValidarCantidadDeRegaloEnBeneficio(); // TODO: Initialize to an appropriate value
            int lleva = 4;
            int paga = 2;
            object[] parametros = { lleva, paga }; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Validar(parametros);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarCantidadDeParticipantesYCantidadDeReglas()
        {
            List<string> reglas = new List<string>(){"sarasa","sarasa2 [CANTIDAD] = '3'"  };

            ValidarCantidadParticipanesAuxiliar target = new ValidarCantidadParticipanesAuxiliar(4, reglas, "CANTIDAD"); // TODO: Initialize to an appropriate value

            bool actual = target.Testear_RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad();
            Assert.AreEqual(true, actual);
        }

        [TestMethod()]
        public void ValidarCantidadDeParticipantesYCantidadDeReglas_Falso()
        {
            List<string> reglas = new List<string>() { "sarasa2 [CANTIDAD] = '7'", "SARASA" };

            ValidarCantidadParticipanesAuxiliar target = new ValidarCantidadParticipanesAuxiliar(7, reglas, "CANTIDAD"); // TODO: Initialize to an appropriate value

            bool actual = target.Testear_RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad();
            Assert.AreEqual(false, actual);
        }

        [TestMethod()]
        public void ValidarCantidadDeParticipantesYCantidadDeReglas_SinAtributo()
        {
            List<string> reglas = new List<string>() { "sarasa", "sarasa2 DAD] = '2'", "pepe", "exacto" };

            ValidarCantidadParticipanesAuxiliar target = new ValidarCantidadParticipanesAuxiliar(4, reglas, "CANTIDAD"); // TODO: Initialize to an appropriate value

            bool actual = target.Testear_RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad();
            Assert.AreEqual(true, actual);
        }

        [TestMethod()]
        public void ValidarCantidadDeParticipantesYCantidadDeReglas_VariosCantidad()
        {
            List<string> reglas = new List<string>() { "sar[CANTIDAD] = '5'asa", "saras[CANTIDAD] = '4'a2 DAD] = '2'", "pe[CANTIDAD] = '1'pe", "exacto" };

            ValidarCantidadParticipanesAuxiliar target = new ValidarCantidadParticipanesAuxiliar(11, reglas, "CANTIDAD"); // TODO: Initialize to an appropriate value

            bool actual = target.Testear_RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad();
            Assert.AreEqual(true, actual);
        }
    }
    public class ValidarCantidadParticipanesAuxiliar : ValidarCantidadParticipantes
    {
        public ValidarCantidadParticipanesAuxiliar(int cantidad, List<string> participantesRegla, string nombreAtributoCantidad) : base(cantidad, participantesRegla, nombreAtributoCantidad, "")
        {
        }

        public bool Testear_RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad()
        {
            return this.RecorrerParticipantesReglaYValidarQueNoCoincidaLaCantidad();
        }
    }
}
