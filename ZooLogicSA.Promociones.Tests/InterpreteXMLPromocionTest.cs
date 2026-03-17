using ZooLogicSA.Promociones.UI.Clases.Interpretes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using System.Windows.Forms;
using ZooLogicSA.Promociones.UI.Controllers;
using Rhino.Mocks;
using ZooLogicSA.Promociones.UI.Clases;
using ZooLogicSA.Promociones.Negocio.Clases;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;
using System;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for InterpreteXMLPromocionTest and is intended
    ///to contain all InterpreteXMLPromocionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InterpreteXMLPromocionTest
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
        public void SerializarPromoValido()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            LLevaXpagaY tipoPromo = new LLevaXpagaY();
            List<string> listaCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );
            string[] dias = { "false", "false", "false", "false", "false", "true", "true" };
            listaCondiciones.Add( "[Cantidad] = '4'" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );
            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );
            string xmlPromo = interprete.XML;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlPromo );
        }

        [TestMethod()]
        [ExpectedException( typeof( System.Xml.XmlException ) )]
        public void SerializarPromoInvalido()
        {
            Promocion promo = new Promocion();
            string xmlPromo = "Hola mundo!";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( xmlPromo );
        }

        [TestMethod()]
        public void VerificarNodosObligatorios()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            LLevaXpagaY tipoPromo = new LLevaXpagaY();
            List<string> listaCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );
            string[] dias = { "false", "false", "false", "false", "false", "true", "true" };
            listaCondiciones.Add( "[Cantidad] = '4'" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( "" );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );
            control.Expect( x => x.ObtenerVisualizacion() ).Return( VisualizacionPromocionAsistenteType.Normal );

            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );
            int contadorNodos = 0;
            int contadorNodoPromocion = 0;

            XmlNode NodoId = null;
            XmlNode NodoEleccionParticipante = null;
            XmlNode NodoInformacionControl = null;
            XmlNode NodoParticipantes = null;
            XmlNode NodoBeneficios = null;
            XmlNode NodoRecursiva = null;
            XmlNode NodoTope = null;
            XmlNode NodoVisualizacion = null;
            XmlNode NodoCuotasSinRecargo = null;
            XmlNode NodoAplicaAutomaticamente = null;
            XmlNode NodoFechaDeModificacion = null;
            XmlNode NodoConMedioDePago = null;

            foreach ( XmlNode nodo in xmlDoc.ChildNodes )
            {
                if ( nodo.Name == "Promocion" )
                {
                    contadorNodos++;
                    contadorNodoPromocion = nodo.ChildNodes.Count;
                    NodoId = nodo.SelectSingleNode( "Id" );
                    NodoEleccionParticipante = nodo.SelectSingleNode( "EleccionParticipante" );
                    NodoParticipantes = nodo.SelectSingleNode( "Participantes" );
                    NodoBeneficios = nodo.SelectSingleNode( "Beneficios" );
                    NodoRecursiva = nodo.SelectSingleNode( "Recursiva" );
                    NodoInformacionControl = nodo.SelectSingleNode( "InformacionControl" );
                    NodoTope = nodo.SelectSingleNode( "TopeBeneficio" );
                    NodoVisualizacion = nodo.SelectSingleNode( "Visualizacion" );
                    NodoCuotasSinRecargo = nodo.SelectSingleNode( "CuotasSinRecargo" );
                    NodoAplicaAutomaticamente = nodo.SelectSingleNode("AplicaAutomaticamente");
                    NodoFechaDeModificacion = nodo.SelectSingleNode("FechaModificacion");
                    NodoConMedioDePago = nodo.SelectSingleNode("ConMedioDePago");
                }
            }
            Assert.AreEqual( 1, contadorNodos );
            Assert.AreEqual( 12, contadorNodoPromocion );
            Assert.IsNotNull( NodoId );
            Assert.IsNotNull( NodoEleccionParticipante );
            Assert.IsNotNull( NodoParticipantes );
            Assert.IsNotNull( NodoBeneficios );
            Assert.IsNotNull( NodoRecursiva );
            Assert.IsNotNull( NodoInformacionControl );
            Assert.IsNotNull( NodoVisualizacion );
            Assert.IsNotNull( NodoTope );
            Assert.IsNotNull( NodoCuotasSinRecargo );
            Assert.IsNotNull( NodoAplicaAutomaticamente) ;
            Assert.IsNotNull( NodoFechaDeModificacion );
            Assert.IsNotNull( NodoConMedioDePago );

        }

        [TestMethod()]
        public void VerificarPromocion2x1ConUnSoloParticipante()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "TIPO;1|MASKCONDICION;2|MASKBENEFICIO;3|FECHADESDE;A|FECHAHASTA;B|HORADESDE;6|HORAHASTA;7|TIPOPRECIO;ABC|FILTROCONDICION;8@9|FILTROBENEFICIO;10@11@12|DIASSEMANA;13@14@15@16";
            EleccionParticipanteType expectedValorEleccionParticipante = EleccionParticipanteType.None;
            LLevaXpagaY tipoPromo = new LLevaXpagaY();

            #region MOCKS

            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );
            List<string> listaCondiciones = new List<string>();
            List<string> listaReglaParticipantesCondiciones = new List<string>();
            listaCondiciones.Add( "[Cantidad] = '2'" );
            listaCondiciones.Add( "[Cliente.Codigo] = '01'" );
            listaCondiciones.Add( "[Valor.Codigo] = 'VISA'" );
//            listaReglaParticipantesCondiciones.Add( "[Cantidad] = '2'" );
            listaReglaParticipantesCondiciones.Add( "[Cliente.Codigo] = '01'" );
            listaReglaParticipantesCondiciones.Add( "[Valor.Codigo] = 'VISA'" );

            string[] dias = { "false", "false", "false", "false", "false", "true", "true" };

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "2" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "1" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORESDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaReglaParticipantesCondiciones );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 3, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( ((( {1} and {2} )) and (( {3} and {4} ) )) and ( {5} or {6} ) ) And ( ( {7} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 7, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
//            Assert.AreEqual( ahora.ToString("T"), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString());
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 5
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[4];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 6
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[5];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "0", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "0", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 7
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[6];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cliente.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "7", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cliente.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 1, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.SelectSingleNode( "Regla" );
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 3
            #region NODO 3
            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.VALORESDETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 1, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.SelectSingleNode( "Regla" );
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Valor.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'VISA'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Valor.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "VISA", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "1", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "100", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            Assert.AreEqual( expectedValorEleccionParticipante.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocion2x1ConUnParticipanteItem()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "TIPO;1|MASKCONDICION;2|MASKBENEFICIO;3|FECHADESDE;A|FECHAHASTA;B|HORADESDE;6|HORAHASTA;7|TIPOPRECIO;ABC|FILTROCONDICION;8@9|FILTROBENEFICIO;10@11@12|DIASSEMANA;13@14@15@16";
            EleccionParticipanteType expectedValorEleccionParticipante = EleccionParticipanteType.None;
            LLevaXpagaY tipoPromo = new LLevaXpagaY();

            #region MOCKS

            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );
            List<string> listaCondiciones = new List<string>();
            List<string> listaReglaParticipantesCondiciones = new List<string>();
            listaCondiciones.Add( "[Cliente.Codigo] = '01'" );
            listaCondiciones.Add( "( [Articulo.Codigo] = 'ART' ) And ( [Cantidad] = '2' )" );
            listaCondiciones.Add( "[Valor.Codigo] = 'VISA'" );
            
            listaReglaParticipantesCondiciones.Add( "[Articulo.Codigo] = 'ART'" );
            listaReglaParticipantesCondiciones.Add( "[Cliente.Codigo] = '01'" );
            listaReglaParticipantesCondiciones.Add( "[Valor.Codigo] = 'VISA'" );

            string[] dias = { "false", "false", "false", "false", "false", "true", "true" };

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "2" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "1" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Articulo.Codigo" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORESDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaReglaParticipantesCondiciones );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 3, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( ((( {1} and {2} )) and (( {3} and {4} ) )) and ( {5} or {6} ) ) And ( ( {7} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 7, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            //            Assert.AreEqual( ahora.ToString("T"), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 5
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[4];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 6
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[5];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "0", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "0", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 7
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[6];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cliente.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "7", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cliente.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( ( {1} ) ) And ( ( {2} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'ART'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "ART", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion


            // NODO 3
            #region NODO 3
            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.VALORESDETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 1, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.SelectSingleNode( "Regla" );
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Valor.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'VISA'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Valor.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "VISA", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "1", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "100", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            Assert.AreEqual( expectedValorEleccionParticipante.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocion2x1ConVariosParticipantes()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "TIPO;1|MASKCONDICION;2|MASKBENEFICIO;3|FECHADESDE;A|FECHAHASTA;B|HORADESDE;6|HORAHASTA;7|TIPOPRECIO;ABC|FILTROCONDICION;8@9|FILTROBENEFICIO;10@11@12|DIASSEMANA;13@14@15@16";
            LLevaXpagaY tipoPromo = new LLevaXpagaY();

            #region MOCKS

            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );
            List<string> listaCondiciones = new List<string>();
            List<string> listaReglaParticipantesCondiciones = new List<string>();
            listaCondiciones.Add( "[Cliente.Codigo] = 'CFK'" );
            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] > '01'" );
            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] < '99'" );
            listaCondiciones.Add( "[Valor.Codigo] = 'VISA'" );

            string[] dias = { "true", "false", "false", "true", "false", "false", "false" };

            listaReglaParticipantesCondiciones.Add( "[Cliente.Codigo] = 'CFK'" );
            listaReglaParticipantesCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] > '01'" );
            listaReglaParticipantesCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] < '99'" );
            listaReglaParticipantesCondiciones.Add( "[Valor.Codigo] = 'VISA'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "4" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "3" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORESDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaReglaParticipantesCondiciones );
            control.Expect( x => x.ObtenerDescripcionComboTipoPrecio() ).Return( "Aplicar al de mayor precio" );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );
            #endregion

            //control.ObtenerValorMaskBeneficio()
            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 4, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( ((( {1} and {2} )) and (( {3} and {4} ) )) and ( {5} or {6} ) ) And ( ( {7} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 7, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 5
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[4];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 6
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[5];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualADiaDeLaSemana", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );
            //          SUBNODO 7
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[6];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cliente.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'CFK'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "7", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cliente.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "CFK", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} ) And ( {2} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " > ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 3
            #region NODO 3

            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {3} ) And ( {4} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " < ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'99'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "99", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 4
            #region NODO 4

            nodoParticipanteRegla = nodoPromocionParticipantes[3];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.VALORESDETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 1, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.SelectSingleNode( "Regla" );
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Valor.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'VISA'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Valor.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "VISA", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "0", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "1", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "100", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocionUnaCaracteristicaTiene50PorCientoDeDescuento()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "Estructura";
            DescuentoXcaracteristica tipoPromo = new DescuentoXcaracteristica();

            #region MOCKS

            List<string> listaCondiciones = new List<string>();
            List<string> listaReglaParticipantesCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );

            string[] dias = { "false", "false", "false", "false", "false", "false", "false" };

            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );

            listaReglaParticipantesCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "50" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaReglaParticipantesCondiciones );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 2, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "(( {1} and {2} )) and (( {3} and {4} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 4, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            
            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} ) And ( {2} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "50", nodoPromocionBeneficio_Valor.InnerText );

            #endregion
            
            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
//            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );
            Assert.AreEqual( EleccionParticipanteType.AplicarATodos.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocionVariasCaracteristicasTiene60PorCientoDeDescuento()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "Estructura";
            DescuentoXcaracteristica tipoPromo = new DescuentoXcaracteristica();

            #region MOCKS

            List<string> listaCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );

            string[] dias = { "false", "false", "false", "false", "false", "false", "false" };

            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );
            listaCondiciones.Add( "[Cantidad] = '3' And [Articulo.Codigo] = '22'" );
            listaCondiciones.Add( "[Cantidad] = '4' And [Articulo.Codigo] = '33'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            //            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "4" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "60" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaCondiciones );
            //            control.Expect( x => x.ObtenerDescripcionComboTipoPrecio() ).Return( "Aplicar al de mayor precio" );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            //control.ObtenerValorMaskBeneficio()
            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 4, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "(( {1} and {2} )) and (( {3} and {4} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 4, nodoParticipanteRegla_Reglas.ChildNodes.Count );
            
            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString());
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            
            #endregion
            
            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} ) And ( {2} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion
            
            // NODO 3
            #region NODO 3

            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {3} ) And ( {4} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'3'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'22'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "22", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion
            
            // NODO 4
            #region NODO 4

            nodoParticipanteRegla = nodoPromocionParticipantes[3];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {5} ) And ( {6} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'4'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'33'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "33", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion
            
            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 3, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );

            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "60", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            // NODO 2
            #region NODO 2

            nodoPromocionBeneficio = nodoPromocionBeneficios[1];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "3", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "3", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );

            //          subnodo Cambio
            nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "60", nodoPromocionBeneficio_Valor.InnerText );            

            #endregion

            // NODO 3
            #region NODO 3

            nodoPromocionBeneficio = nodoPromocionBeneficios[2];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "4", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "4", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "60", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion
            
            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            //            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );
            Assert.AreEqual( EleccionParticipanteType.AplicarATodos.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocionUnaCaracteristicaTiene130PesosPrecioFijo()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "Estructura";
            RebajaXcaracteristica tipoPromo = new RebajaXcaracteristica();

            #region MOCKS

            List<string> listaCondiciones = new List<string>();
            List<string> listaReglaParticipantesCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );

            string[] dias = { "false", "false", "false", "false", "false", "false", "false" };

            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );

            listaReglaParticipantesCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            //            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "4" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "130" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaReglaParticipantesCondiciones );
            //            control.Expect( x => x.ObtenerDescripcionComboTipoPrecio() ).Return( "Aplicar al de mayor precio" );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            //control.ObtenerValorMaskBeneficio()
            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 2, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "(( {1} and {2} )) and (( {3} and {4} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 4, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );

            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} ) And ( {2} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "MONTOFINAL", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "130", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            //            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );
            Assert.AreEqual( EleccionParticipanteType.None.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        //[TestMethod()]
        public void VerificarPromocionVariasCaracteristicasTiene170PesosPrecioFijo()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "Estructura";
            RebajaXcaracteristica tipoPromo = new RebajaXcaracteristica();

            #region MOCKS

            List<string> listaCondiciones = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );

            string[] dias = { "false", "false", "false", "false", "false", "false", "false" };

            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );
            listaCondiciones.Add( "[Cantidad] = '3' And [Articulo.Codigo] = '22'" );
            listaCondiciones.Add( "[Cantidad] = '4' And [Articulo.Codigo] = '33'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            //            control.Expect( x => x.ObtenerValorMaskCondicion() ).Return( "4" );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "170" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cliente.Codigo" ) ).Return( "" );
            //            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Valor.Codigo" ) ).Return( "VALORDETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesCondicion() ).Return( listaCondiciones );
            //            control.Expect( x => x.ObtenerDescripcionComboTipoPrecio() ).Return( "Aplicar al de mayor precio" );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            //control.ObtenerValorMaskBeneficio()
            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 4, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "(( {1} and {2} )) and (( {3} and {4} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 4, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );

            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "{1} And {2}", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 3
            #region NODO 3

            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "{3} And {4}", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'3'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'22'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "22", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 4
            #region NODO 4

            nodoParticipanteRegla = nodoPromocionParticipantes[3];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "{5} And {6}", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'4'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'33'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "33", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 3, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "2", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "MONTODESCUENTOCONIMPUESTOS", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "170", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            // NODO 2
            #region NODO 2

            nodoPromocionBeneficio = nodoPromocionBeneficios[1];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "3", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "3", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "MONTODESCUENTOCONIMPUESTOS", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "170", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            // NODO 3
            #region NODO 3

            nodoPromocionBeneficio = nodoPromocionBeneficios[2];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "4", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "4", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "MONTODESCUENTOCONIMPUESTOS", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "170", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            //            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );
            Assert.AreEqual( EleccionParticipanteType.AplicarATodos.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }

        [TestMethod()]
        public void VerificarPromocionVariasCaracteristicasHaceQueCiertosArticulosTengan80PorcientoDeDescuento()
        {
            IControllerPromocion control = MockRepository.GenerateMock<IControllerPromocion>();
            string expectedValorEstructura = "Estructura";
            LlevaXtienedescuentoY tipoPromo = new LlevaXtienedescuentoY();

            #region MOCKS

            List<string> listaCondiciones = new List<string>();
            List<string> listaBeneficios = new List<string>();
            DateTime hoy = DateTime.Now;
            DateTime manana = hoy.AddDays( 1 );
            DateTime ahora = new DateTime().AddHours( 1 );
            DateTime enunrato = ahora.AddHours( 1 );

            string[] dias = { "false", "false", "false", "false", "false", "false", "false" };

            listaCondiciones.Add( "[Cantidad] = '2' And [Articulo.Codigo] = '01'" );
            listaCondiciones.Add( "[Cantidad] = '3' And [Articulo.Codigo] = '22'" );
            listaCondiciones.Add( "[Cantidad] = '4' And [Articulo.Codigo] = '33'" );

            listaBeneficios.Add( "[Articulo.Familia.Codigo] = 'PANTALON'" );
            listaBeneficios.Add( "[Cantidad] = '5' And [Articulo.Material.Codigo] = 'LANA'" );

            control.Expect( x => x.ObtenerValorEstructuraInterprete() ).Return( expectedValorEstructura );
            control.Expect( x => x.ObtenerValorMaskBeneficio() ).Return( "80" );
            control.Expect( x => x.ObtenerTipoPromocionSeleccionada() ).Return( tipoPromo );
            control.Expect( x => x.ObtenerListaCondicionesSegunTipoPromocion( tipoPromo ) ).Return( listaCondiciones );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Cantidad" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerTipoDetalleDelParticipante( "Articulo.Familia.Codigo" ) ).Return( "FACTURADETALLE" );
            control.Expect( x => x.ObtenerListaReglaParticipantesBeneficio() ).Return( listaBeneficios );
            control.Expect( x => x.ObtenerDescripcionComboTipoPrecio() ).Return( "Aplicar al de mayor precio" );
            control.Expect( x => x.ActualizarInformacionInterprete() );
            control.Expect( x => x.ObtenerVigenciaFechaDesde() ).Return( hoy );
            control.Expect( x => x.ObtenerVigenciaFechaHasta() ).Return( manana );
            control.Expect( x => x.ObtenerVigenciaHoraDesde() ).Return( ahora );
            control.Expect( x => x.ObtenerVigenciaHoraHasta() ).Return( enunrato );
            control.Expect( x => x.ObtenerVigenciaDiasSemana() ).Return( dias );

            #endregion

            InterpreteXMLPromocion interprete = new InterpreteXMLPromocion( control, "1" );

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml( interprete.XML );

            XmlNode nodoPromocion = xmlDoc.SelectSingleNode( "Promocion" );

            #region NODO ID

            XmlNode nodoPromocionId = nodoPromocion.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoPromocionId.InnerText );

            #endregion

            #region NODO INFORMACIONCONTROL

            XmlNode nodoPromocionInformacionControl = nodoPromocion.SelectSingleNode( "InformacionControl" );
            Assert.AreEqual( expectedValorEstructura, nodoPromocionInformacionControl.InnerText );

            #endregion

            #region NODO PARTICIPANTES

            XmlNodeList nodoPromocionParticipantes = nodoPromocion.SelectNodes( "Participantes/ParticipanteRegla" );
            Assert.AreEqual( 6, nodoPromocionParticipantes.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoParticipanteRegla = nodoPromocionParticipantes[0];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            XmlNode nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            XmlNode nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            XmlNode nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "(( {1} and {2} )) and (( {3} and {4} ) )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            XmlNode nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            XmlNode nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 4, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            XmlNode nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            XmlNode nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            XmlNode nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            XmlNode nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( hoy.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            XmlNode nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            XmlNode nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            XmlNode nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            XmlNode nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            XmlNode nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( hoy.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( manana.ToString( "yyyy-MM-dd" ), nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "FECHA", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( manana.ToShortDateString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortDateString() );
            //          SUBNODO 3
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[2];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " >= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMayorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( ahora.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );
            //          SUBNODO 4
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[3];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " <= ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText ).ToShortTimeString() );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerMenorIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "HORAALTAFW", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( enunrato.ToShortTimeString(), DateTime.Parse( nodoParticipanteRegla_Reglas_Regla_Valor.InnerText ).ToShortTimeString() );

            #endregion

            // NODO 2
            #region NODO 2

            nodoParticipanteRegla = nodoPromocionParticipantes[1];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {1} ) And ( {2} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'2'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "1", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'01'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "2", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "01", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 3
            #region NODO 3

            nodoParticipanteRegla = nodoPromocionParticipantes[2];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {3} ) And ( {4} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'3'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "3", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'22'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "22", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 4
            #region NODO 4

            nodoParticipanteRegla = nodoPromocionParticipantes[3];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {5} ) And ( {6} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'4'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "4", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'33'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "33", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion

            // NODO 5
            #region NODO 5

            nodoParticipanteRegla = nodoPromocionParticipantes[4];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {7} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "true", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 1, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1 [Articulo.Familia.Codigo] = 'PANTALON'"
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Familia.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'PANTALON'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "7", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Familia.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "PANTALON", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );


            #endregion

            // NODO 6
            #region NODO 6

            nodoParticipanteRegla = nodoPromocionParticipantes[5];
            Assert.AreEqual( 5, nodoParticipanteRegla.ChildNodes.Count );
            //          subnodo Id
            nodoParticipanteRegla_Id = nodoParticipanteRegla.SelectSingleNode( "Id" );
            Assert.AreEqual( "6", nodoParticipanteRegla_Id.InnerText );
            //          subnodo Codigo
            nodoParticipanteRegla_Codigo = nodoParticipanteRegla.SelectSingleNode( "Codigo" );
            Assert.AreEqual( "COMPROBANTE.FACTURADETALLE.ITEM", nodoParticipanteRegla_Codigo.InnerText );
            //          subnodo RelaReglas
            nodoParticipanteRegla_RelaReglas = nodoParticipanteRegla.SelectSingleNode( "RelaReglas" );
            Assert.AreEqual( "( {8} ) And ( {9} )", nodoParticipanteRegla_RelaReglas.InnerText );
            //          subnodo Beneficiario
            nodoParticipanteRegla_Beneficiario = nodoParticipanteRegla.SelectSingleNode( "Beneficiario" );
            Assert.AreEqual( "true", nodoParticipanteRegla_Beneficiario.InnerText );
            //          subnodo Reglas
            nodoParticipanteRegla_Reglas = nodoParticipanteRegla.SelectSingleNode( "Reglas" );
            Assert.AreEqual( 2, nodoParticipanteRegla_Reglas.ChildNodes.Count );

            //          SUBNODO 1
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[0];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Cantidad]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'5'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "8", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Cantidad", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "5", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            //          SUBNODO 2
            //          subnodo Reglas - subnodo Regla
            nodoParticipanteRegla_Reglas_Regla = nodoParticipanteRegla_Reglas.ChildNodes[1];
            //          subnodo Reglas - subnodo Regla - subnodo OperadorAlComienzo
            nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "OperadorAlComienzo" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_OperadorAlComienzo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Operador
            nodoParticipanteRegla_Reglas_Regla_Operador = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Operador" );
            Assert.AreEqual( " = ", nodoParticipanteRegla_Reglas_Regla_Operador.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo DescripcionAtributo
            nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "DescripcionAtributo" );
            Assert.AreEqual( "[Articulo.Material.Codigo]", nodoParticipanteRegla_Reglas_Regla_DescripcionAtributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo ValorString
            nodoParticipanteRegla_Reglas_Regla_ValorString = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "ValorString" );
            Assert.AreEqual( "'LANA'", nodoParticipanteRegla_Reglas_Regla_ValorString.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Id
            nodoParticipanteRegla_Reglas_Regla_Id = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Id" );
            Assert.AreEqual( "9", nodoParticipanteRegla_Reglas_Regla_Id.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Comparacion
            nodoParticipanteRegla_Reglas_Regla_Comparacion = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Comparacion" );
            Assert.AreEqual( "DebeSerIgualA", nodoParticipanteRegla_Reglas_Regla_Comparacion.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Atributo
            nodoParticipanteRegla_Reglas_Regla_Atributo = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "Articulo.Material.Codigo", nodoParticipanteRegla_Reglas_Regla_Atributo.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Compuesta
            nodoParticipanteRegla_Reglas_Regla_Compuesta = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Compuesta" );
            Assert.AreEqual( "false", nodoParticipanteRegla_Reglas_Regla_Compuesta.InnerText );
            //          subnodo Reglas - subnodo Regla - subnodo Valor
            nodoParticipanteRegla_Reglas_Regla_Valor = nodoParticipanteRegla_Reglas_Regla.SelectSingleNode( "Valor" );
            Assert.AreEqual( "LANA", nodoParticipanteRegla_Reglas_Regla_Valor.InnerText );

            #endregion
            #endregion

            #region NODO BENEFICIOS

            XmlNodeList nodoPromocionBeneficios = nodoPromocion.SelectNodes( "Beneficios/Beneficio" );
            Assert.AreEqual( 1, nodoPromocionBeneficios.Count );

            // NODO 1
            #region NODO 1

            XmlNode nodoPromocionBeneficio = nodoPromocionBeneficios[0];
            Assert.AreEqual( 5, nodoPromocionBeneficio.ChildNodes.Count );
            //          subnodo Cuantos
            XmlNodeList nodoPromocionBeneficio_DestinoBeneficio = nodoPromocionBeneficio.SelectNodes( "Destinos/DestinoBeneficio" );
            Assert.AreEqual( "5", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Participante" ).InnerText );
            Assert.AreEqual( "1", nodoPromocionBeneficio_DestinoBeneficio[0].SelectSingleNode( "Cuantos" ).InnerText );
            //          subnodo Cambio
            XmlNode nodoPromocionBeneficio_Cambio = nodoPromocionBeneficio.SelectSingleNode( "Cambio" );
            Assert.AreEqual( "CambiarValor", nodoPromocionBeneficio_Cambio.InnerText );
            //          subnodo Atributo
            XmlNode nodoPromocionBeneficio_Atributo = nodoPromocionBeneficio.SelectSingleNode( "Atributo" );
            Assert.AreEqual( "DESCUENTO", nodoPromocionBeneficio_Atributo.InnerText );
            //          subnodo Valor
            XmlNode nodoPromocionBeneficio_Valor = nodoPromocionBeneficio.SelectSingleNode( "Valor" );
            Assert.AreEqual( "80", nodoPromocionBeneficio_Valor.InnerText );

            #endregion

            #endregion

            #region ELECCIONPARTICIPANTE

            XmlNode nodoPromocionEleccionParticipante = nodoPromocion.SelectSingleNode( "EleccionParticipante" );
            //            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );
            Assert.AreEqual( EleccionParticipanteType.AplicarAlDeMayorPrecio.ToString(), nodoPromocionEleccionParticipante.InnerText );

            #endregion

            #region RECURSIVA

            XmlNode nodoPromocionRecursiva = nodoPromocion.SelectSingleNode( "Recursiva" );
            Assert.AreEqual( "true", nodoPromocionRecursiva.InnerText );

            #endregion

        }
    }
}
