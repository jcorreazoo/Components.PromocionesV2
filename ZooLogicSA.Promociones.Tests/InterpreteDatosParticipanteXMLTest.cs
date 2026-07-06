using ZooLogicSA.Promociones.UI.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for InterpreteDatosParticipanteXMLTest and is intended
    ///to contain all InterpreteDatosParticipanteXMLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InterpreteDatosParticipanteXMLTest
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
        ///A test for InterpreteDatosParticipanteXML Constructor
        ///</summary>
        [TestMethod()]
        public void InterpreteDatosParticipanteXMLConstructorTest()
        {
            string xml = Properties.Resources.XMLPruebaDatosParticipante;
            InterpreteDatosParticipanteXML interprete = new InterpreteDatosParticipanteXML( xml );

            Assert.IsNotNull( interprete );
        }

        [TestMethod]
        public void InterpretarDatosParticipanteXMLTest()
        {
            //string xml = Properties.Resources.XMLPruebaDatosParticipante;
            string xml = Properties.Resources.XMLdeFox;
            InterpreteDatosParticipanteXML interprete = new InterpreteDatosParticipanteXML( xml );
            InterpreteNodoDatosParticipanteXML nodoDatosParticipante;

            nodoDatosParticipante = interprete.NodoDatosParticipante;
            Assert.IsNotNull( nodoDatosParticipante );
            // Nodos Registros
            Assert.AreEqual( 6, nodoDatosParticipante.ListaRegistros.Count );
            //Nodos
            // 1
            InterpreteNodoDatosParticipanteRegistroXML nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[0];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            InterpreteNodoDatosParticipanteAtributoXML nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "C1        ", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Cliente 1                     ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "    -  -  ", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "    -  -  ", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "        ", nodoDatoAtributo.Valor );

            //Nodos
            // 2
            nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[1];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "0000000001", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Martín Gomez                  ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "2001-01-01", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "2010-12-09", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "17:02:44", nodoDatoAtributo.Valor );

            //Nodos
            // 3
            nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[2];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "0000000002", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Juan Perez                    ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "2001-01-01", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "2010-12-09", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "17:25:52", nodoDatoAtributo.Valor );

            //Nodos
            // 4
            nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[3];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "0000000003", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Gabriela Gimenez              ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "1980-01-01", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "2011-03-11", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "14:09:45", nodoDatoAtributo.Valor );

            //Nodos
            // 5
            nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[4];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "0000000004", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Mariano Fornes                ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "1980-01-01", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "2011-03-11", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "14:21:47", nodoDatoAtributo.Valor );

            //Nodos
            // 6
            nodoDatoRegistro = nodoDatosParticipante.ListaRegistros[5];
            Assert.AreEqual( 5, nodoDatoRegistro.ListaAtributos.Count );
            //Atributo 1
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[0];
            Assert.AreEqual( "0000000005", nodoDatoAtributo.Valor );
            //Atributo 2
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[1];
            Assert.AreEqual( "Cliente 5                     ", nodoDatoAtributo.Valor );
            //Atributo 3
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[2];
            Assert.AreEqual( "    -  -  ", nodoDatoAtributo.Valor );
            //Atributo 4
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[3];
            Assert.AreEqual( "2012-08-30", nodoDatoAtributo.Valor );
            //Atributo 5
            nodoDatoAtributo = nodoDatoRegistro.ListaAtributos[4];
            Assert.AreEqual( "16:37:13", nodoDatoAtributo.Valor );
        }
    }
}
