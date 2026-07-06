using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for ValidarListaCondicionTest and is intended
    ///to contain all ValidarListaCondicionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarListaCondicionTest
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
        public void ValidarTestParametrosIncorrectos()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            object[] parametros = null;
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "No son válidos los parámetros.";
            actual = target.Validar( parametros );

            Assert.AreEqual( expected, actual );
            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
        }

        [TestMethod()]
        public void ValidarTestUnErrorFaltaAtributo()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "[] = 'Hola mundo'";
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "Debe completar los atributos";
            actual = target.Validar( parametros );

            Assert.AreEqual( expected, actual );
            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
        }

        [TestMethod()]
        public void ValidarTestUnErrorFaltaAtributo2()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "[Codigo] = '?'";
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "Debe completar los atributos";
            actual = target.Validar( parametros );

            Assert.AreEqual( expected, actual );
            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
        }

        [TestMethod()]
        public void ValidarTestVariosErroresFaltanAtributos()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "[] = 'Hola mundo' And [Codigo] = '?'";
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "Debe completar los atributos";
            actual = target.Validar( parametros );

            Assert.AreEqual( expected, actual );
            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
        }

        [TestMethod()]
        public void ValidarTestTodoBien()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "[Codigo] = '112' And [Descripcion] = 'Hola mundo' And [Cantidad] = '8'";
            bool expected = true;
            bool actual;
            string ExpectedMessageError = "";
            actual = target.Validar( parametros );

            Assert.AreEqual( expected, actual );
            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
        }

        [TestMethod()]
        public void ValidarTestNoPermiteReglaVacia()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "";
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "Debe cargar al menos una regla.";
            actual = target.Validar( parametros );

            Assert.AreEqual( ExpectedMessageError, target.ObtenerMensajeError() );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarTestNoPermiteMasDeUnaCantidad()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "StartsWith([ARTICULO.MATERIAL.DESCRIPCION], '6') And [CANTIDAD] = '5' Or StartsWith([ARTICULO.CODIGO], '5') And [CANTIDAD] = '3'";
            bool expected = false;
            bool actual;
			string ExpectedMessageError = "No se puede ingresar más de un atributo cantidad o monto para cada condición.";
            actual = target.Validar(parametros);

            Assert.AreEqual(ExpectedMessageError, target.ObtenerMensajeError());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarTestNoPermiteCantidadEnOtrosNodosGrupoGrande()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "(StartsWith([ARTICULO.CLASIFICACION.DESCRIPCION], '5') Or StartsWith([ARTICULO.FAMILIA.CODIGO], '3')) And (StartsWith([ARTICULO.MATERIAL.DESCRIPCION], '6') Or StartsWith([ARTICULO.CLASIFICACION.CODIGO], '4') Or [CANTIDAD] = '5')";
            bool expected = false;
            bool actual;
            string ExpectedMessageError = "El atributo Cantidad o Monto debe ingresarse solo en el nodo base de las condiciones de la promoción.";
            actual = target.Validar(parametros);

            Assert.AreEqual(ExpectedMessageError, target.ObtenerMensajeError());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarTestNoPermiteCantidadEnOtrosNodosGrupoChico()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "StartsWith([ARTICULO.CLASIFICACION.DESCRIPCION], '5') Or StartsWith([ARTICULO.FAMILIA.CODIGO], '3') Or StartsWith([ARTICULO.MATERIAL.DESCRIPCION], '6') And [CANTIDAD] = '5'";
            bool expected = false;
            bool actual;
			string ExpectedMessageError = "El atributo Cantidad o Monto debe ingresarse solo en el nodo base de las condiciones de la promoción.";
            actual = target.Validar(parametros);

            Assert.AreEqual(ExpectedMessageError, target.ObtenerMensajeError());
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarTestPermiteCantidadEnCabecera()
        {
            ValidarListaCondicion target = new ValidarListaCondicion();
            string parametros = "[CANTIDAD] = '5' Or StartsWith([ARTICULO.CODIGO], '0') And StartsWith([TALLE.CODIGO], '4') Or StartsWith([ARTICULO.CODIGO], '8') And StartsWith([ARTICULO.DESCRIPCION], '5')";
            bool expected = true;
            bool actual;
            string ExpectedMessageError = "";
            actual = target.Validar(parametros);

            Assert.AreEqual(ExpectedMessageError, target.ObtenerMensajeError());
            Assert.AreEqual(expected, actual);
        }
    }
}
