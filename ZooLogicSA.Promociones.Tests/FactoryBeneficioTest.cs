using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Globalization;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for FactoryBeneficioTest and is intended
    ///to contain all FactoryBeneficioTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FactoryBeneficioTest
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
        ///A test for ObtenerBeneficio
        ///</summary>
        [TestMethod()]
        public void AgregarReglaBeneficioLleva2Paga1Test()
        {
            ValorBeneficio valorBeneficio = new ValorBeneficio();
            valorBeneficio.Destinos.Add( new DestinoBeneficio() { Participante = "44", Cuantos = 3 } );
            Beneficio beneficio;

            beneficio = FactoryBeneficio.ObtenerBeneficio( BeneficioType.LLevaXPagaY, valorBeneficio );

            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );

            Assert.AreEqual( "44", beneficio.Destinos[0].Participante );
            Assert.AreEqual( 3, beneficio.Destinos[0].Cuantos );
            Assert.AreEqual( "DESCUENTO", beneficio.Atributo );
            Assert.AreEqual("100", beneficio.Valor.ToString());
            Assert.AreEqual( Alteracion.CambiarValor, beneficio.Cambio );
        }

        [TestMethod()]
        public void AgregarReglaBeneficioPorcentajeFijoDescuentoTest()
        {
            ValorBeneficio valorBeneficio = new ValorBeneficio();
            valorBeneficio.Destinos.Add( new DestinoBeneficio() { Participante = "44", Cuantos = 3 } );
            valorBeneficio.Valor = "47";
            Beneficio beneficio;

            beneficio = FactoryBeneficio.ObtenerBeneficio( BeneficioType.PorcentajeFijoDeDescuento, valorBeneficio );

            Assert.AreEqual( "44", beneficio.Destinos[0].Participante );
            Assert.AreEqual( 3, beneficio.Destinos[0].Cuantos );
            Assert.AreEqual( "DESCUENTO", beneficio.Atributo );
            Assert.AreEqual("47", beneficio.Valor.ToString());
            Assert.AreEqual( Alteracion.CambiarValor, beneficio.Cambio );
        }
        [TestMethod()]
        public void AgregarReglaBeneficioMontoFijoDescuentoTest()
        {
            ValorBeneficio valorBeneficio = new ValorBeneficio();
            valorBeneficio.Destinos.Add( new DestinoBeneficio() { Participante = "44", Cuantos = 3 } );
            valorBeneficio.Valor = "47";
            Beneficio beneficio;

            beneficio = FactoryBeneficio.ObtenerBeneficio( BeneficioType.MontoFijoDeDescuento, valorBeneficio );

            Assert.AreEqual( "44", beneficio.Destinos[0].Participante );
            Assert.AreEqual( 3, beneficio.Destinos[0].Cuantos );
            Assert.AreEqual( "MONTOFINAL", beneficio.Atributo );
            Assert.AreEqual("47", beneficio.Valor.ToString());
            Assert.AreEqual( Alteracion.CambiarValor, beneficio.Cambio );
        }
        [TestMethod()]
        public void AgregarReglaBeneficioLlevaUnoPagaConDescuentoOtroTest()
        {
            ValorBeneficio valorBeneficio = new ValorBeneficio();
            valorBeneficio.Destinos.Add( new DestinoBeneficio() { Participante = "44", Cuantos = 3 } );
            valorBeneficio.Valor = "47";
            Beneficio beneficio;

            beneficio = FactoryBeneficio.ObtenerBeneficio( BeneficioType.LlevaUnoPagaConDescuentoOtro, valorBeneficio );

            Assert.AreEqual( "44", beneficio.Destinos[0].Participante );
            Assert.AreEqual( 3, beneficio.Destinos[0].Cuantos );
            Assert.AreEqual( "DESCUENTO", beneficio.Atributo );
            Assert.AreEqual("47", beneficio.Valor.ToString());
            Assert.AreEqual( Alteracion.CambiarValor, beneficio.Cambio );
        }
    }
}
