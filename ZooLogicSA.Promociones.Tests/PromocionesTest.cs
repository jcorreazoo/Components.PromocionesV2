using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Summary description for PromocionesTest
    /// </summary>
    [TestClass]
    public class PromocionesTest
    {
        public PromocionesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ObtenerCantidadDeTiposDePromocionTest()
        {
            List<TipoPromocion> listaTipoPromocion;
            listaTipoPromocion = FactoryPromociones.LlenarListaTipoPromociones();

            Assert.AreEqual( 8, listaTipoPromocion.Count );
        }

        [TestMethod]
        public void ObtenerEstructuraPromocionTest()
        {
            DateTime ahora = DateTime.Now;
            string resultado = "";
            string resultadoEsperado = "TIPO;1|MASKCONDICION;2|MASKBENEFICIO;3|MASKTOPEDESCUENTO;|MASKCUOTASSINRECARGO;0|FECHADESDE;" + ahora.ToString( "dd/MM/yyyy" ) + "|FECHAHASTA;" + ahora.ToString( "dd/MM/yyyy" ) + "|HORADESDE;16:00:00|HORAHASTA;17:00:00|TIPOPRECIO;ABC|REGLABENEFICIO;ABC|REGLACONDICION;ABC|LISTADEPRECIOS;|FILTROCONDICION;8@9|FILTROBENEFICIO;10@11@12|DIASSEMANA;13@14@15@16|LEYENDA;|COMPORTAMIENTO;|APLICAAUTOMATICAMENTE;False";
            EstructuraPromocion estructura = new EstructuraPromocion();
            estructura.Resultado = resultadoEsperado;
            resultado = estructura.Resultado;
            Assert.AreEqual( resultadoEsperado, resultado, "No anduvo el atributo Resultado" );
        }
    }
}
