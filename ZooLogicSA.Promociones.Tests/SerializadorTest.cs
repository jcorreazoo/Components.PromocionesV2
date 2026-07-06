using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Tests.Properties;
namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for SerializadorTest and is intended
    ///to contain all SerializadorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SerializadorTest
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
        public void DeSerializarTest()
        {
            Serializador serializador = new Serializador();
            string xml = Resources.XMLPruebaPromocion;

            List<Promocion> a = serializador.DeSerializar<List<Promocion>>( xml );
            Assert.AreEqual( 1, a.Count, "Nose obtuvieron correctamente la cantidad de registros." );
            Assert.AreEqual( "1", a[ 0 ].Id, "No obtuvo correctamente el valor del Id de la promocion." );
            Assert.AreEqual( 3, a[0].Participantes.Count, "No se obtuvieron correctamente la cantidad de Participantes." );
        }

        [TestMethod()]
        public void DeSerializarPromocionTest()
        {
            Serializador serializador = new Serializador();
            string xml = Resources.XMLDeUnaPromocion;

            Promocion a = serializador.DeserializarPromocion( "cod", "", xml );
            Assert.AreEqual( "1", a.Id, "No obtuvo correctamente el valor del Id de la promocion." );
        }
    }
}
