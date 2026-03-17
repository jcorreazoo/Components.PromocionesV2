using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for ValidarCantidadParticipantesTest and is intended
    ///to contain all ValidarCantidadParticipantesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarCantidadParticipantesTest
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
        public void ValidarInvalidoPorMasDeUnaCantidadEnReglaTest()
        {
            int cantidad = 0; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Cantidad] = 1";
            string participanteRegla2 = "[Cantidad] = 1 And [Cantidad] = 2";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add( participanteRegla1 );
            participantesRegla.Add( participanteRegla2 );
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes( cantidad, participantesRegla, nombreAtributoCantidad, "" ); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "Un regla de participante tiene mas de un atributo Cantidad.";
            string actualMessage;

            actual = target.Validar();
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual( expected, actual );
            Assert.AreEqual( expectedMessage, actualMessage );
        }

        [TestMethod()]
        public void ValidarValidoPorTenerSoloUnaReglaTest()
        {
            int cantidad = 0; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Cantidad] = 1";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add( participanteRegla1 );
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes( cantidad, participantesRegla, nombreAtributoCantidad, "" ); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Validar();
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarInValidoPorTenerSoloUnaReglaTest()
        {
            int cantidad = 8; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Cantidad] = 1";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add( participanteRegla1 );
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes( cantidad, participantesRegla, nombreAtributoCantidad, "" ); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Validar();
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarValidoTest()
        {
            int cantidad = 5; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Codigo] = '1' And [Cantidad] = '3'";
            string participanteRegla2 = "[Cantidad] = '2'";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add( participanteRegla1 );
            participantesRegla.Add( participanteRegla2 );
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes( cantidad, participantesRegla, nombreAtributoCantidad, "" ); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            actual = target.Validar();
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarInvalidoPorCantidadDiferenteTest()
        {
            int cantidad = 5; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Codigo] = '1' And [Cantidad] = '5'";
            string participanteRegla2 = "[Cantidad] = '5'";
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add( participanteRegla1 );
            participantesRegla.Add( participanteRegla2 );
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes( cantidad, participantesRegla, nombreAtributoCantidad, "" ); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "No coincide la cantidad de la promoción con la sumatoria de cantidades de las reglas de participantes.";
            string actualMessage;

            actual = target.Validar();
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual( expected, actual );
            Assert.AreEqual( expectedMessage, actualMessage );
        }

        [TestMethod()]
        public void ValidarInvalidoPorCantidadMayorOIgualEnLLevaXPagaY()
        {
            int cantidad = 5; // TODO: Initialize to an appropriate value
            string participanteRegla1 = "[Codigo] = '1' And [Cantidad] >= '5'";            
            List<string> participantesRegla = new List<string>(); // TODO: Initialize to an appropriate value
            participantesRegla.Add(participanteRegla1);            
            string nombreAtributoCantidad = "Cantidad"; // TODO: Initialize to an appropriate value
            ValidarCantidadParticipantes target = new ValidarCantidadParticipantes(cantidad, participantesRegla, nombreAtributoCantidad, "LLevaXPagaY"); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            string expectedMessage = "No puede utilizar una condición distinta a 'igual' para la cantidad en las promociones del tipo 'Lleva una cantidad, paga otra cantidad'.";
            string actualMessage;

            actual = target.Validar();
            actualMessage = target.ObtenerMensajeError();
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedMessage, actualMessage);
        }
    }
}
