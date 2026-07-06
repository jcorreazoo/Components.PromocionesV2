using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.ComprobanteXml;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for ComparadorDeParticipantesXMLTest and is intended
    ///to contain all ComparadorDeParticipantesXMLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ComparadorDeParticipantesXMLTest
    {
        [TestMethod()]
        public void ObtenerDifereciaDeImporteTest()
        {
            ComparadorDeParticipantesXML target = new ComparadorDeParticipantesXML(); // TODO: Initialize to an appropriate value

            ConfiguracionComportamiento comportamiento = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IParticipante participanteOriginal = MockRepository.GenerateMock<IParticipante>();
            participanteOriginal.Expect( x => x.ObtenerAtributo( "PRECIO" ) ).Return( new AtributoXml() { Valor = "200" } );
            participanteOriginal.Expect( x => x.ObtenerAtributo( "DESCUENTO" ) ).Return( new AtributoXml() { Valor = "0" } );
            participanteOriginal.Expect( x => x.ObtenerAtributo( "MONTODESCUENTO" ) ).Return( new AtributoXml() { Valor = "0" } );
            
            IParticipante participantePromocionado = MockRepository.GenerateMock<IParticipante>();
            participantePromocionado.Expect( x => x.Cantidad ).Return( 1 );
            participantePromocionado.Expect( x => x.ObtenerAtributo( "PRECIO" ) ).Return( new AtributoXml() { Valor = "200" } );
            participantePromocionado.Expect( x => x.ObtenerAtributo( "DESCUENTO" ) ).Return( new AtributoXml() { Valor = "50" } );
            participantePromocionado.Expect( x => x.ObtenerAtributo( "MONTODESCUENTO" ) ).Return( new AtributoXml() { Valor = "0" } );

            //IEvaluadorMatematico evaluador = FactoriaPromociones.ObtenerEvaluadoresMatematico( comportamiento );

            //Decimal expected = 100M;
            
            //Decimal actual = target.ObtenerDiferenciaDeImporte( evaluador, comportamiento, participanteOriginal, participantePromocionado, 1F );
            
            //Assert.AreEqual( expected, actual );
        }
    }
}