using ZooLogicSA.Promociones.UI.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Windows.Forms;
using System;
using DevExpress.XtraEditors;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for InterpreteEstructuraPromocionTest and is intended
    ///to contain all InterpreteEstructuraPromocionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InterpreteEstructuraPromocionTest
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
        ///Test para verificar que se cree una regla en base a los atributos de vigencia
        ///</summary>
        [TestMethod()]
        public void CrearPromocionTest()
        {
            ManagerReglas.CrearPromo();
            Assert.IsNotNull( ManagerReglas.Promo, "No se creó la promoción." );
        }

        [TestMethod()]
        public void AgregarReglaVigenciaUnicidadDeParticipanteComprobanteTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            ParticipanteRegla itemExistente;
            int contador = 0;
            string[] vacio = {};

            ManagerReglas.CrearPromo();
            itemExistente = new ParticipanteRegla();
            itemExistente.Codigo = "Comprobante";
            ManagerReglas.AgregarParticipanteRegla( itemExistente );
            item = ManagerReglas.CrearReglaVigencia( DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, vacio );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "Comprobante" )
                    contador = contador + 1;
            }
            Assert.AreEqual( 1, contador );
        }

        [TestMethod()]
        public void ObtenerTipoDeDetalleDeLaReglaTest()
        {
            InterpreteParticipanteFiltroXML xmlParticipante = new InterpreteParticipanteFiltroXML( Properties.Resources.XMLPruebaParticipanteFiltro );
            ExpressionConditionsEditor expEditorCondicion = new ExpressionConditionsEditor();
            expEditorCondicion.FormatItemList.Items.Add( "[aCLIENTE.CODIGO] = 1" );
            expEditorCondicion.FormatItemList.Items.Add( "[aCOLOR.CODIGO] = 1" );
            expEditorCondicion.FormatItemList.Items.Add( "[aARTICULO.CODIGO] = 1" );
            expEditorCondicion.FormatItemList.Items.Add( "[aARTICULO.aFAMILIA.DESCRIPCION] = 1" );
            
            Assert.AreEqual( "", xmlParticipante.ObtenerTipoDetalle( (string)expEditorCondicion.FormatItemList.Items[0] ) );
            Assert.AreEqual( "FacturaDetalle", xmlParticipante.ObtenerTipoDetalle( (string)expEditorCondicion.FormatItemList.Items[1] ) );
            Assert.AreEqual( "FacturaDetalle", xmlParticipante.ObtenerTipoDetalle( (string)expEditorCondicion.FormatItemList.Items[2] ) );
            Assert.AreEqual( "FacturaDetalle", xmlParticipante.ObtenerTipoDetalle( (string)expEditorCondicion.FormatItemList.Items[3] ) );
        }

        [TestMethod()]
        public void AgregarReglaCondicionUnicidadDeParticipanteComprobanteTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            ParticipanteRegla itemExistente;
            int contador = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            itemExistente = new ParticipanteRegla();
            itemExistente.Codigo = "Comprobante";
            ManagerReglas.AgregarParticipanteRegla( itemExistente );
            item = ManagerReglas.CrearReglaCondicion( "[CLIENTE.CODIGO] = 0010010101", controler, false );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "Comprobante" )
                    contador = contador + 1;
            }
            Assert.AreEqual( 1, contador );
        }

        [TestMethod()]
        public void AgregarReglaCondicionDeParticipanteComprobanteTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contador = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "( [CLIENTE.CODIGO] > 'CLI1' And [ARTICULO.CODIGO] < '0010010103' ) Or ( [CLIENTE.CODIGO] = 'CLI2' And [ARTICULO.CODIGO] < '0010010103' )", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE" )
                    contador = contador + 1;
            }
            // participante regla comprobante unica. Y ademas que sea la unica
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 3, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorA, reglaTestear.Comparacion );
            Assert.AreEqual( " > ", reglaTestear.Operador );
            Assert.AreEqual( "CLI1", reglaTestear.Valor );
            Assert.AreEqual( "'CLI1'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            // Regla 2 [Cliente.Codigo] < '0010010103'
            reglaTestear = participanteRegla.Reglas[1];
            Assert.AreEqual( "ARTICULO.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[ARTICULO.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMenorA, reglaTestear.Comparacion );
            Assert.AreEqual( " < ", reglaTestear.Operador );
            Assert.AreEqual( "0010010103", reglaTestear.Valor );
            Assert.AreEqual( "'0010010103'", reglaTestear.ValorString );
            Assert.AreEqual( 2, reglaTestear.Id );

            // Regla 3 [Cliente.Codigo] = '0010030101'
            reglaTestear = participanteRegla.Reglas[2];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " = ", reglaTestear.Operador );
            Assert.AreEqual( "CLI2", reglaTestear.Valor );
            Assert.AreEqual( "'CLI2'", reglaTestear.ValorString );
            Assert.AreEqual( 3, reglaTestear.Id );

            Assert.AreEqual( "( ( {1} ) And ( {2} ) ) Or ( ( {3} ) And ( {2} ) )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionDeParticipanteVariosParentesisComprobanteTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contador = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "( [CLIENTE.CODIGO] > 'CLI1' And [ARTICULO.CODIGO] < '0010010103' ) Or ( [CLIENTE.CODIGO] = 'CLI2' And [ARTICULO.CODIGO] < '0010010103' ) And ( [CLIENTE.CLASIFICACION.CODIGO] = '001' Or [VENDEDOR.CODIGO] = '002' )", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE" )
                    contador = contador + 1;
            }
            // participante regla comprobante unica. Y ademas que sea la unica
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 5, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorA, reglaTestear.Comparacion );
            Assert.AreEqual( " > ", reglaTestear.Operador );
            Assert.AreEqual( "CLI1", reglaTestear.Valor );
            Assert.AreEqual( "'CLI1'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            // Regla 2 [Cliente.Codigo] < '0010010103'
            reglaTestear = participanteRegla.Reglas[1];
            Assert.AreEqual( "ARTICULO.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[ARTICULO.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMenorA, reglaTestear.Comparacion );
            Assert.AreEqual( " < ", reglaTestear.Operador );
            Assert.AreEqual( "0010010103", reglaTestear.Valor );
            Assert.AreEqual( "'0010010103'", reglaTestear.ValorString );
            Assert.AreEqual( 2, reglaTestear.Id );

            // Regla 3 [Cliente.Codigo] = '0010030101'
            reglaTestear = participanteRegla.Reglas[2];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " = ", reglaTestear.Operador );
            Assert.AreEqual( "CLI2", reglaTestear.Valor );
            Assert.AreEqual( "'CLI2'", reglaTestear.ValorString );
            Assert.AreEqual( 3, reglaTestear.Id );

            // Regla 4 [Cliente.Clasificacion de cliente.Codigo] = '001'
            reglaTestear = participanteRegla.Reglas[3];
            Assert.AreEqual( "CLIENTE.CLASIFICACION.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CLASIFICACION.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " = ", reglaTestear.Operador );
            Assert.AreEqual( "001", reglaTestear.Valor );
            Assert.AreEqual( "'001'", reglaTestear.ValorString );
            Assert.AreEqual( 4, reglaTestear.Id );

            // Regla 5 [Vendedor.Codigo] = '002'
            reglaTestear = participanteRegla.Reglas[4];
            Assert.AreEqual( "VENDEDOR.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[VENDEDOR.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " = ", reglaTestear.Operador );
            Assert.AreEqual( "002", reglaTestear.Valor );
            Assert.AreEqual( "'002'", reglaTestear.ValorString );
            Assert.AreEqual( 5, reglaTestear.Id );

            Assert.AreEqual( "( ( {1} ) And ( {2} ) ) Or ( ( {3} ) And ( {2} ) ) And ( ( {4} ) Or ( {5} ) )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorBetweenTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[CLIENTE.CODIGO] Between('CLI1', 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 2, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            Regla segundaRegla;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            segundaRegla = participanteRegla.Reglas[1];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " >= ", reglaTestear.Operador );
            Assert.AreEqual( "CLI1", reglaTestear.Valor );
            Assert.AreEqual( "'CLI1'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );
            Assert.IsTrue( reglaTestear.Compuesta );
            Assert.AreEqual( segundaRegla, reglaTestear.ReglaAsociada );

            // Regla 2 [Cliente.Codigo] < '0010010103'
            Assert.AreEqual( "CLIENTE.CODIGO", segundaRegla.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", segundaRegla.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMenorIgualA, segundaRegla.Comparacion );
            Assert.AreEqual( " <= ", segundaRegla.Operador );
            Assert.AreEqual( "CLI5", segundaRegla.Valor );
            Assert.AreEqual( "'CLI5'", segundaRegla.ValorString );
            Assert.AreEqual( 2, segundaRegla.Id );
            Assert.IsTrue( segundaRegla.Compuesta );
            Assert.AreEqual( null, segundaRegla.ReglaAsociada );

            Assert.AreEqual( "( {1} And {2} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorNotBetweenTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "Not [CLIENTE.CODIGO] Between('CLI1', 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 2, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            Regla segundaRegla;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            segundaRegla = participanteRegla.Reglas[1];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " >= ", reglaTestear.Operador );
            Assert.AreEqual( "CLI1", reglaTestear.Valor );
            Assert.AreEqual( "'CLI1'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );
            Assert.IsTrue( reglaTestear.Compuesta );
            Assert.AreEqual( segundaRegla, reglaTestear.ReglaAsociada );

            // Regla 2 [Cliente.Codigo] < '0010010103'
            Assert.AreEqual( "CLIENTE.CODIGO", segundaRegla.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", segundaRegla.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMenorIgualA, segundaRegla.Comparacion );
            Assert.AreEqual( " <= ", segundaRegla.Operador );
            Assert.AreEqual( "CLI5", segundaRegla.Valor );
            Assert.AreEqual( "'CLI5'", segundaRegla.ValorString );
            Assert.AreEqual( 2, segundaRegla.Id );
            Assert.IsTrue( segundaRegla.Compuesta );
            Assert.AreEqual( null, segundaRegla.ReglaAsociada );

            Assert.AreEqual( "Not ( {1} And {2} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorContieneTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "Contains([CLIENTE.CODIGO], 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerContieneA, reglaTestear.Comparacion );
            Assert.AreEqual( "Contains(", reglaTestear.Operador );
            Assert.AreEqual( "CLI5", reglaTestear.Valor );
            Assert.AreEqual( "'CLI5'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorNoContieneTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "Not Contains([CLIENTE.CODIGO], 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerContieneA, reglaTestear.Comparacion );
            Assert.AreEqual( "Contains(", reglaTestear.Operador );
            Assert.AreEqual( "CLI5", reglaTestear.Valor );
            Assert.AreEqual( "'CLI5'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "Not ( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorContieneYBetweenTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[CLIENTE.CODIGO] Between('CLI1', 'CLI5') And Contains([CLIENTE.CODIGO], 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 3, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            Regla segundaRegla;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            segundaRegla = participanteRegla.Reglas[1];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " >= ", reglaTestear.Operador );
            Assert.AreEqual( "CLI1", reglaTestear.Valor );
            Assert.AreEqual( "'CLI1'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );
            Assert.IsTrue( reglaTestear.Compuesta );
            Assert.AreEqual( segundaRegla, reglaTestear.ReglaAsociada );

            // Regla 2 [Cliente.Codigo] < '0010010103'
            Assert.AreEqual( "CLIENTE.CODIGO", segundaRegla.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", segundaRegla.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMenorIgualA, segundaRegla.Comparacion );
            Assert.AreEqual( " <= ", segundaRegla.Operador );
            Assert.AreEqual( "CLI5", segundaRegla.Valor );
            Assert.AreEqual( "'CLI5'", segundaRegla.ValorString );
            Assert.AreEqual( 2, segundaRegla.Id );
            Assert.IsTrue( segundaRegla.Compuesta );
            Assert.AreEqual( null, segundaRegla.ReglaAsociada );

            // Regla 3 Contains([Cliente.Codigo], 'CLI5')
            reglaTestear = participanteRegla.Reglas[2];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerContieneA, reglaTestear.Comparacion );
            Assert.AreEqual( "Contains(", reglaTestear.Operador );
            Assert.AreEqual( "CLI5", reglaTestear.Valor );
            Assert.AreEqual( "'CLI5'", reglaTestear.ValorString );
            Assert.AreEqual( 3, reglaTestear.Id );

            Assert.AreEqual( "( {1} And {2} ) And ( {3} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorComienzaConTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "StartsWith([CLIENTE.CODIGO], 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 StartsWith([Cliente.Codigo], 'CLI5')
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerComienzaCon, reglaTestear.Comparacion );
            Assert.AreEqual( "StartsWith(", reglaTestear.Operador );
            Assert.AreEqual( "CLI5", reglaTestear.Valor );
            Assert.AreEqual( "'CLI5'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaCondicionOperadorTerminaConTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "EndsWith([CLIENTE.CODIGO], 'CLI5')", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 EndsWith([Cliente.Codigo], 'CLI5')
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "CLIENTE.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[CLIENTE.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerTerminaCon, reglaTestear.Comparacion );
            Assert.AreEqual( "EndsWith(", reglaTestear.Operador );
            Assert.AreEqual( "CLI5", reglaTestear.Valor );
            Assert.AreEqual( "'CLI5'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarReglaVigenciaTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            DateTime hoy = DateTime.Now;
            DateTime manana = DateTime.Now.AddDays( 1 );
            DateTime mediodia = new DateTime( 1,1,1,0,30,0);
            DateTime noche = new DateTime( 1,1,1, 23,45,0);
            string[] dias = { "true", "false", "true", "true", "false", "false", "false" };                
            int contador = 0;

            ManagerReglas.CrearPromo();

            item = ManagerReglas.CrearReglaVigencia( hoy, manana, mediodia, noche, dias );
            ManagerReglas.AgregarParticipanteRegla( item );
            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE" )
                    contador = contador + 1;
            }
            // participante regla comprobante
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( "COMPROBANTE", item.Codigo );
            Assert.AreEqual( "1", item.Id );
            Assert.AreEqual( 7, item.Reglas.Count );
            // fechas
            Assert.AreEqual( 1, item.Reglas[0].Id );
            Assert.AreEqual( "FECHA", item.Reglas[0].Atributo );
            Assert.AreEqual( Factor.DebeSerMayorIgualA, item.Reglas[0].Comparacion );
            Assert.AreEqual( hoy, item.Reglas[0].Valor );
            Assert.AreEqual( 2, item.Reglas[1].Id );
            Assert.AreEqual( "FECHA", item.Reglas[1].Atributo );
            Assert.AreEqual( Factor.DebeSerMenorIgualA, item.Reglas[1].Comparacion );
            Assert.AreEqual( manana, item.Reglas[1].Valor );
            // horario
            Assert.AreEqual( 3, item.Reglas[2].Id );
            Assert.AreEqual( "HORAALTAFW", item.Reglas[2].Atributo );
            Assert.AreEqual( Factor.DebeSerMayorIgualA, item.Reglas[2].Comparacion );
            Assert.AreEqual( mediodia.ToString("HH:mm:ss"), item.Reglas[2].Valor );
            Assert.AreEqual( 4, item.Reglas[3].Id );
            Assert.AreEqual( "HORAALTAFW", item.Reglas[3].Atributo );
            Assert.AreEqual( Factor.DebeSerMenorIgualA, item.Reglas[3].Comparacion );
            Assert.AreEqual( noche.ToString( "HH:mm:ss" ), item.Reglas[3].Valor );
            // dias de la semana
            Assert.AreEqual( 5, item.Reglas[4].Id );
            Assert.AreEqual( "FECHA", item.Reglas[4].Atributo );
            Assert.AreEqual( Factor.DebeSerIgualADiaDeLaSemana, item.Reglas[4].Comparacion );
            Assert.AreEqual( 1, item.Reglas[4].Valor );
            Assert.AreEqual( 6, item.Reglas[5].Id );
            Assert.AreEqual( "FECHA", item.Reglas[5].Atributo );
            Assert.AreEqual( Factor.DebeSerIgualADiaDeLaSemana, item.Reglas[5].Comparacion );
            Assert.AreEqual( 3, item.Reglas[5].Valor );
            Assert.AreEqual( 7, item.Reglas[6].Id );
            Assert.AreEqual( "FECHA", item.Reglas[6].Atributo );
            Assert.AreEqual( Factor.DebeSerIgualADiaDeLaSemana, item.Reglas[6].Comparacion );
            Assert.AreEqual( 4, item.Reglas[6].Valor );
            // Rela Reglas
            Assert.IsTrue( item.RelaReglas.Contains( "{1} and {2}" ) );
            Assert.IsTrue( item.RelaReglas.Contains( "{3} and {4}" ) );
            Assert.IsTrue( item.RelaReglas.Contains( "( {5} or {6} or {7} )" ) );
        }

        [TestMethod()]
        public void AgregarReglaCondicionDeParticipanteFacturaDetalleTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contador = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[aARTICULO.CODIGO] > '001'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE.FACTURADETALLE.ITEM" )
                    contador = contador + 1;
            }
            // participante regla comprobante unica. Y ademas que sea la unica
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "aARTICULO.CODIGO", reglaTestear.Atributo );
            Assert.AreEqual( "[aARTICULO.CODIGO]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorA, reglaTestear.Comparacion );
            Assert.AreEqual( " > ", reglaTestear.Operador );
            Assert.AreEqual( "001", reglaTestear.Valor );
            Assert.AreEqual( "'001'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarAtributoCantidadEnReglaCondicionDeParticipanteFacturaDetalleTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contador = 0;

            ManagerReglas.CrearPromo();
            
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[Cantidad] = '4'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE.FACTURADETALLE.ITEM" )
                    contador = contador + 1;
            }
            // participante regla comprobante unica. Y ademas que sea la unica
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cantidad] = '4'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "Cantidad", reglaTestear.Atributo );
            Assert.AreEqual( "[Cantidad]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerIgualA, reglaTestear.Comparacion );
            Assert.AreEqual( " = ", reglaTestear.Operador );
            Assert.AreEqual( "4", reglaTestear.Valor );
            Assert.AreEqual( "'4'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        //[TestMethod()]
        //public void AgregarAtributoPreciodEnReglaCondicionDeParticipanteFacturaDetalleTest()
        //{
        //    ControllerPromocion controler = new ControllerPromocion();
        //    ParticipanteRegla item;
        //    int contador = 0;

        //    ManagerReglas.CrearPromo();
        //    controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
        //    item = ManagerReglas.CrearReglaCondicion( "[Precio] >= '56'", controler );
        //    ManagerReglas.AgregarParticipanteRegla( item );

        //    foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
        //    {
        //        if ( participante.Codigo == "Comprobante.Facturadetalle.Item" )
        //            contador = contador + 1;
        //    }
        //    // participante regla comprobante unica. Y ademas que sea la unica
        //    Assert.AreEqual( 1, contador );
        //    Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

        //    ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
        //    Assert.AreEqual( 1, participanteRegla.Reglas.Count );

        //    Regla reglaTestear;
        //    // Regla 1 [Precio] = '4'
        //    reglaTestear = participanteRegla.Reglas[0];
        //    Assert.AreEqual( "Precio", reglaTestear.Atributo );
        //    Assert.AreEqual( "[Precio]", reglaTestear.DescripcionAtributo );
        //    Assert.AreEqual( Factor.DebeSerMayorIgualA, reglaTestear.Comparacion );
        //    Assert.AreEqual( " >= ", reglaTestear.Operador );
        //    Assert.AreEqual( "56", reglaTestear.Valor );
        //    Assert.AreEqual( "'56'", reglaTestear.ValorString );
        //    Assert.AreEqual( 1, reglaTestear.Id );

        //    Assert.AreEqual( "{1}", participanteRegla.RelaReglas );
        //}

        [TestMethod()]
        public void AgregarReglaCondicionDeParticipanteValorDetalleTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contador = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[aVALOR.COD] > '001'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM" )
                    contador = contador + 1;
            }
            // participante regla comprobante unica. Y ademas que sea la unica
            Assert.AreEqual( 1, contador );
            Assert.AreEqual( 1, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );

            Regla reglaTestear;
            // Regla 1 [Cliente.Codigo] > '0010010101'
            reglaTestear = participanteRegla.Reglas[0];
            Assert.AreEqual( "aVALOR.COD", reglaTestear.Atributo );
            Assert.AreEqual( "[aVALOR.COD]", reglaTestear.DescripcionAtributo );
            Assert.AreEqual( Factor.DebeSerMayorA, reglaTestear.Comparacion );
            Assert.AreEqual( " > ", reglaTestear.Operador );
            Assert.AreEqual( "001", reglaTestear.Valor );
            Assert.AreEqual( "'001'", reglaTestear.ValorString );
            Assert.AreEqual( 1, reglaTestear.Id );

            Assert.AreEqual( "( {1} )", participanteRegla.RelaReglas );
        }

        [TestMethod()]
        public void AgregarVariosReglaCondicionDeParticipanteFacturaDetalleTest()
        {
            ControllerPromocion controler = new ControllerPromocion();
            ParticipanteRegla item;
            int contadorDetalle = 0;
            int contadorValores = 0;

            ManagerReglas.CrearPromo();
            controler.InicializarManagerParticipantes( Properties.Resources.XMLPruebaParticipanteFiltro );
            item = ManagerReglas.CrearReglaCondicion( "[aARTICULO.CODIGO] > '001' And [aARTICULO.CODIGO] < '005'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );
            item = ManagerReglas.CrearReglaCondicion( "[aARTICULO.CODIGO] > '222' And [aARTICULO.CODIGO] < '333' And [aARTICULO.CODIGO] = '444'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );
            item = ManagerReglas.CrearReglaCondicion( "[aVALOR.COD] = 'PESOS'", controler, false );
            ManagerReglas.AgregarParticipanteRegla( item );

            foreach ( ParticipanteRegla participante in ManagerReglas.Promo.Participantes )
            {
                if ( participante.Codigo == "COMPROBANTE.FACTURADETALLE.ITEM" )
                    contadorDetalle = contadorDetalle + 1;
                if ( participante.Codigo == "COMPROBANTE.VALORESDETALLE.ITEM" )
                    contadorValores = contadorValores + 1;
            }
            Assert.AreEqual( 2, contadorDetalle );
            Assert.AreEqual( 1, contadorValores );
            Assert.AreEqual( 3, ManagerReglas.Promo.Participantes.Count );

            ParticipanteRegla participanteRegla = ManagerReglas.Promo.Participantes[0];
            Assert.AreEqual( 2, participanteRegla.Reglas.Count );

            participanteRegla = ManagerReglas.Promo.Participantes[1];
            Assert.AreEqual( 3, participanteRegla.Reglas.Count );

            participanteRegla = ManagerReglas.Promo.Participantes[2];
            Assert.AreEqual( 1, participanteRegla.Reglas.Count );
        }
    }
}
