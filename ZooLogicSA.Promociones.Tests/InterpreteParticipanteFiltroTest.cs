using ZooLogicSA.Promociones.UI.Clases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Negocio.Clases;
using System.Collections.Generic;
namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for InterpreteParticipanteFiltroTest and is intended
    ///to contain all InterpreteParticipanteFiltroTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InterpreteParticipanteFiltroTest
    {
        

        private TestContext testContextInstance;

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

        [TestMethod]
        public void InterpreteParticipanteFiltro()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );

            Assert.IsNotNull( interprete );
        }

        [TestMethod]
        public void InterpretarParticipanteFiltroXML()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            List<InterpreteNodoParticipanteXML> listaParticipantes;

            listaParticipantes = interprete.Participantes;
            // Lista de participantes
            Assert.AreEqual( 5, listaParticipantes.Count );
            //Nodos
            // 1
            InterpreteNodoParticipanteXML nodo = listaParticipantes[0];
            Assert.AreEqual( "CLIENTE", nodo.qEntidad.ToUpper() );
            Assert.AreEqual( "ZCLIENTE", nodo.Descripcion.ToUpper() );
            Assert.AreEqual( "", nodo.Detalle.ToUpper() );
            Assert.AreEqual( "ACLIENTE", nodo.Atributo.ToUpper() );
            //        SubNodos Atributos
            //        1
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            InterpreteNodoAtributosXML nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Atributo
            //        1                    1
            InterpreteNodoAtributoXML nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "NOMBRE", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZNOMBRE", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Cantidad
            Assert.AreEqual( 1, nodoAtributos.NodosParticipante.Count );
            //        SubNodos Atributos - Participante
            nodo = nodoAtributos.NodosParticipante[0];
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Participante - Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Participante - Atributos - Atributo
            //        1                    1                          1
            nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    1                          2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "DESCRIPCION", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZDESCRIPCIÓN", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Atributos - Participante - Cantidad
            Assert.AreEqual( 0, nodoAtributos.NodosParticipante.Count );



            // 2
            nodo = listaParticipantes[1];
            Assert.AreEqual( "VENDEDOR", nodo.qEntidad.ToUpper() );
            Assert.AreEqual( "ZVENDEDOR", nodo.Descripcion.ToUpper() );
            Assert.AreEqual( "AVENDEDOR", nodo.Atributo.ToUpper() );
            Assert.AreEqual( "", nodo.Detalle.ToUpper() );
            //        SubNodos Atributos
            //        1
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Atributo
            //        1                    1
            nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "NOMBRE", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZNOMBRE", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Cantidad
            Assert.AreEqual( 0, nodoAtributos.NodosParticipante.Count );
            
            // 3
            nodo = listaParticipantes[2];
            Assert.AreEqual( "ARTICULO", nodo.qEntidad.ToUpper() );
            Assert.AreEqual( "ZARTÍCULO", nodo.Descripcion.ToUpper() );
            Assert.AreEqual( "AARTICULO", nodo.Atributo.ToUpper() );
            Assert.AreEqual( "FACTURADETALLE", nodo.Detalle.ToUpper() );
            //        SubNodos Atributos
            //        1
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Atributo
            //        1                    1
            nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "DESCRIPCION", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZDESCRIPCIÓN", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Cantidad
            Assert.AreEqual( 1, nodoAtributos.NodosParticipante.Count );
            //        SubNodos Atributos - Participante
            nodo = nodoAtributos.NodosParticipante[0];
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Participante - Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Participante - Atributos - Atributo
            //        1                    1                          1
            nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    1                          2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "DESCRIPCION", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZDESCRIPCIÓN", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Atributos - Participante - Cantidad
            Assert.AreEqual( 0, nodoAtributos.NodosParticipante.Count );
            
            // 4
            nodo = listaParticipantes[3];
            Assert.AreEqual( "COLOR", nodo.qEntidad.ToUpper() );
            Assert.AreEqual( "ZCOLOR", nodo.Descripcion.ToUpper() );
            Assert.AreEqual( "ACOLOR", nodo.Atributo.ToUpper() );
            Assert.AreEqual( "FACTURADETALLE", nodo.Detalle.ToUpper() );
            //        SubNodos Atributos
            //        1
            Assert.AreEqual( 1, nodo.NodosAtributos.Count );
            nodoAtributos = nodo.NodosAtributos[0];
            //        SubNodos Atributos - Atributo - Cantidad
            Assert.AreEqual( 2, nodoAtributos.NodosAtributo.Count );
            //        SubNodos Atributos - Atributo
            //        1                    1
            nodoAtributo = nodoAtributos.NodosAtributo[0];
            Assert.AreEqual( "CODIGO", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZCÓDIGO", nodoAtributo.Descripcion.ToUpper() );
            //        1                    2
            nodoAtributo = nodoAtributos.NodosAtributo[1];
            Assert.AreEqual( "DESCRIPCION", nodoAtributo.qNombre.ToUpper() );
            Assert.AreEqual( "ZDESCRIPCIÓN", nodoAtributo.Descripcion.ToUpper() );
            //        SubNodos Atributos - Participante - Cantidad
            Assert.AreEqual( 0, nodoAtributos.NodosParticipante.Count );
        }

//-------------------------------------------------------------------------------------------------//
        [TestMethod()]
        public void ConvertirFullDescripcionEnAtributoSimpleTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "ARTICULO.DESCRIPCION";
            string actual;
            string fullDescripcion = "zArtículo.zDescripción";

            actual = interprete.ConvertirFullDescripcionEnAtributo( fullDescripcion );
            Assert.AreEqual( expected, actual.ToUpper() );
        }

        [TestMethod()]
        public void ConvertirFullDescripcionConUnNivelEnAtributoTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "CLIENTE.ACLASIFICACION.CODIGO";
            string actual;
            string fullDescripcion = "zCliente.zClasificación de cliente.zCódigo";

            actual = interprete.ConvertirFullDescripcionEnAtributo( fullDescripcion );
            Assert.AreEqual( expected, actual.ToUpper() );
        }

        [TestMethod()]
        public void ConvertirFullDescripcionConDosNivelesEnAtribuoTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "VALOR.AOPERADORDETARJETA.AENTIDADFINANCIERA.CODIGO";
            string actual;
            string fullDescripcion = "zValor.zOperador de tarjeta.zEntidad pagadora.zCódigo";

            actual = interprete.ConvertirFullDescripcionEnAtributo( fullDescripcion );
            Assert.AreEqual( expected, actual.ToUpper() );
        }
//-------------------------------------------------------------------------------------------------//
        [TestMethod()]
        public void ConvertirFullNombreEnDescripcionSimpleTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "zArtículo.zDescripción";
            string actual;
            string fullNombre = "aARTICULO.DESCRIPCION";

            actual = interprete.ConvertirFullNombreEnDescripcion( fullNombre );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ConvertirFullNombreEnDescripcionConUnNivelTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "zCliente.zClasificación de cliente.zCódigo";
            string actual;
            string fullNombre = "aCLIENTE.aCLASIFICACION.CODIGO";

            actual = interprete.ConvertirFullNombreEnDescripcion( fullNombre );
            Assert.AreEqual( expected, actual );
        }

        [TestMethod()]
        public void ConvertirFullNombreEnDescripcionConDosNivelesTest()
        {
            string xml = Properties.Resources.XMLPruebaParticipanteFiltro;
            InterpreteParticipanteFiltroXML interprete = new InterpreteParticipanteFiltroXML( xml );
            string expected = "zValor.zOperador de tarjeta.zEntidad pagadora.zCódigo";
            string actual;
            string fullNombre = "aVALOR.aOPERADORDETARJETA.aENTIDADFINANCIERA.CODIGO";

            actual = interprete.ConvertirFullNombreEnDescripcion( fullNombre );
            Assert.AreEqual( expected, actual );
        }
    }
}
