using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.UI;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for ValidarPromocionTest and is intended
    ///to contain all ValidarPromocionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ValidarPromocionTest
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
        ///A test for ValidarHorario
        ///</summary>
        [TestMethod()]
        public void ValidarHorarioValidoTest()
        {
            DateTime desde = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime hasta = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarHorario( desde, hasta );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarHorarioValido2Test()
        {
            DateTime desde = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime hasta = new DateTime().AddHours( 2 ); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarHorario( desde, hasta );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarHorarioInvalidoTest()
        {
            DateTime desde = new DateTime().AddSeconds( 1 ); // TODO: Initialize to an appropriate value
            DateTime hasta = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarHorario( desde, hasta );
            Assert.AreEqual( expected, actual );
        }

        /// <summary>
        ///A test for ValidarFechas
        ///</summary>
        [TestMethod()]
        public void ValidarFechasValidoTest()
        {
            DateTime desde = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime hasta = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarFechas( desde, hasta );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarFechasInvalidoTest()
        {
            DateTime desde = new DateTime().AddDays( 2 ); // TODO: Initialize to an appropriate value
            DateTime hasta = new DateTime(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarFechas( desde, hasta );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ValidarDatoObligatorioCargado()
        {
            bool expected = true;
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarDatoObligatorio( "Ok" );
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarDatoObligatorioEnBlanco()
        {
            bool expected = false;
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarDatoObligatorio( "" );
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarDatoObligatorioNulo()
        {
            bool expected = false;
            bool actual;
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarDatoObligatorio(null);
            Assert.AreEqual(expected, actual);
        }

		//[TestMethod()]
		public void FormularioConControlPromocionesPruebaManualNoMeSubas()
        {
            Form formu = new Form();
            ControlPromociones control = new ControlPromociones();
            //control.Enabled = false;
            formu.Controls.Add( control );
            formu.Width = 1000;
            formu.Height = 700;
            formu.ShowDialog();
			Assert.Inconclusive( "Este test es solo para pruebas manuales, no debe correr en integracion continua. Borrar el tag TestMethod al subir." );
        }

        [TestMethod()]
        public void ValidarDiasDeLaSemanaSinseleccionarNinguno()
        {
            bool expected = false;
            bool actual;
            string[] dias = new string[7];
            dias[0] = "False";
            dias[1] = "False";
            dias[2] = "False";
            dias[3] = "False";
            dias[4] = "False";
            dias[5] = "False";
            dias[6] = "False";
            
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarDiasDeLaSemana(dias);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarDiasDeLaSemanaSeleccionandoAlMenosUno()
        {
            bool expected = true;
            bool actual;
            string[] dias = new string[7];
            dias[0] = "False";
            dias[1] = "False";
            dias[2] = "False";
            dias[3] = "False";
            dias[4] = "False";
            dias[5] = "True";
            dias[6] = "False";

            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarDiasDeLaSemana(dias);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ValidarCantidadDeReglasTotalesTest()
        {
 
            bool actual;
            List<string> participantes = new List<string>();

            for (int i = 0; i < 520; i++)
            {
                participantes.Add("[s");
            }
            ValidarPromocion validador = new ValidarPromocion();
            actual = validador.ValidarCantidadDeReglasTotales(participantes);
            Assert.AreEqual(false, actual);
        }

        [TestMethod()]
        public void test()
        {
            string val1 = "pepe";
            string val2 = "Pepé";
            bool r1 = string.Equals(val1, val2, StringComparison.OrdinalIgnoreCase);
            bool r2 = string.Equals(val1, val2, StringComparison.InvariantCultureIgnoreCase);        
            bool r3 = val1.Equals(val2, StringComparison.OrdinalIgnoreCase);
            bool r4 = val1.Equals(val2, StringComparison.InvariantCultureIgnoreCase);

        }

    }
}
