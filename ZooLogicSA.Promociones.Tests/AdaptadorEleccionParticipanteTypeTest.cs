using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.UI.Clases.Adaptadores;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for AdaptadorEleccionParticipanteTypeTest and is intended
    ///to contain all AdaptadorEleccionParticipanteTypeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AdaptadorEleccionParticipanteTypeTest
    {


        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
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
        ///A test for ObtenerDescripcionSegunIndice
        ///</summary>
        [TestMethod()]
        public void ObtenerDescripcionComboValorAplicarMenorTest()
        {
            AdaptadorEleccionParticipanteType target = new AdaptadorEleccionParticipanteType(); // TODO: Initialize to an appropriate value
            EleccionParticipanteType expected = EleccionParticipanteType.AplicarAlDeMenorPrecio; // TODO: Initialize to an appropriate value
            EleccionParticipanteType actual;
            actual = target.ObtenerIdEleccionParticipanteTypeSegunValorCombo( "Aplicar al de menor precio" );
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ObtenerDescripcionComboValorAplicarMayorTest()
        {
            AdaptadorEleccionParticipanteType target = new AdaptadorEleccionParticipanteType(); // TODO: Initialize to an appropriate value
            EleccionParticipanteType expected = EleccionParticipanteType.AplicarAlDeMayorPrecio; // TODO: Initialize to an appropriate value
            EleccionParticipanteType actual;
            actual = target.ObtenerIdEleccionParticipanteTypeSegunValorCombo( "Aplicar al de mayor precio" );
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual( expected, actual );
        }

        //[TestMethod]
        //public void Prueba_Web()
        //{
        //string respuesta;

        //using ( var wb = new WebClient() )
        //{
        //    var data = new NameValueCollection();
        //    data["start"] = "0";

        //    byte[] respu = wb.UploadValues( "http://fs02:86/grillaprocesos.datos/Builds", "POST", data );

        //    respuesta = Encoding.Default.GetString( respu );
        //}

        //Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive( respuesta ) ;

        //using ( var wb = new WebClient() )
        //{
        //    var data = new NameValueCollection();
        //    data["BuildProyectoDetalleId"] = "30891944";
        //    data["limit"] = "25";
        //    data["start"] = "0";

        //    byte[] respu = wb.UploadValues( "http://fs02:86/grillaprocesos.datos", "POST", data );

        //    respuesta = Encoding.Default.GetString( respu );
        //}

        //Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive();
        //}

    }
}
