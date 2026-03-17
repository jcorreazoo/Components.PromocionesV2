using ZooLogicSA.Promociones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comprobante;
using System.Collections.Generic;
using Rhino.Mocks;

namespace ZooLogicSA.Promociones.Tests
{
    
    
    /// <summary>
    ///This is a test class for JoineadorDeParticipantesTest and is intended
    ///to contain all JoineadorDeParticipantesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class JoineadorDeParticipantesTest
    {
        [TestMethod()]
        public void ObtenerCombinacionesTest()
        {
            JoineadorDeParticipantes target = new JoineadorDeParticipantes();
            
            Dictionary<string, List<IParticipante>> posiblesDestinos = new Dictionary<string,List<IParticipante>>();

            IParticipante participante0 = MockRepository.GenerateMock<IParticipante>();
            participante0.Expect( x => x.Id ).Return( "0" );
            participante0.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante1 = MockRepository.GenerateMock<IParticipante>();
            participante1.Expect( x => x.Id ).Return( "1" );
            participante1.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante2 = MockRepository.GenerateMock<IParticipante>();
            participante2.Expect( x => x.Id ).Return( "2" );
            participante2.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante3 = MockRepository.GenerateMock<IParticipante>();
            participante3.Expect( x => x.Id ).Return( "3" );
            participante3.Expect( x => x.Cantidad ).Return( 1 );

            posiblesDestinos.Add( "1", new List<IParticipante>() { participante0, participante1 } );
            posiblesDestinos.Add( "2", new List<IParticipante>() { participante2 } );
            posiblesDestinos.Add( "3", new List<IParticipante>() { participante3 } );

            List<Dictionary<string, List<IParticipante>>> actual = target.ObtenerCombinaciones( posiblesDestinos );

            Assert.AreEqual( 3, actual.Count, "Mal Total Combinaciones" );
            
            Assert.AreEqual( 3, actual[0].Count, "Mal cantidad en 1er combinacion" );
            Assert.AreEqual( 3, actual[1].Count, "Mal cantidad en 2da combinacion" );

            Assert.AreEqual( "0", actual[0]["1"][0].Id, "Mal Id item 0-0" );
            Assert.AreEqual( "2", actual[0]["2"][0].Id, "Mal Id item 0-1" );
            Assert.AreEqual( "3", actual[0]["3"][0].Id, "Mal Id item 0-2" );

            Assert.AreEqual( "1", actual[1]["1"][0].Id, "Mal Id item 1-0" );
            Assert.AreEqual( "2", actual[1]["2"][0].Id, "Mal Id item 1-1" );
            Assert.AreEqual( "3", actual[1]["3"][0].Id, "Mal Id item 1-2" );
        }

        [TestMethod()]
        public void ObtenerCombinaciones_OTRO_Test()
        {
            JoineadorDeParticipantes target = new JoineadorDeParticipantes();

            Dictionary<string, List<IParticipante>> posiblesDestinos = new Dictionary<string, List<IParticipante>>();

            IParticipante participante0 = MockRepository.GenerateMock<IParticipante>();
            participante0.Expect( x => x.Id ).Return( "0" );
            participante0.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante1 = MockRepository.GenerateMock<IParticipante>();
            participante1.Expect( x => x.Id ).Return( "1" );
            participante1.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante2 = MockRepository.GenerateMock<IParticipante>();
            participante2.Expect( x => x.Id ).Return( "2" );
            participante2.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante3 = MockRepository.GenerateMock<IParticipante>();
            participante3.Expect( x => x.Id ).Return( "3" );
            participante3.Expect( x => x.Cantidad ).Return( 1 );

            posiblesDestinos.Add( "1", new List<IParticipante>() { participante0 } );
            posiblesDestinos.Add( "2", new List<IParticipante>() { participante2 } );
            posiblesDestinos.Add( "3", new List<IParticipante>() { participante3, participante1 } );

            List<Dictionary<string, List<IParticipante>>> actual = target.ObtenerCombinaciones( posiblesDestinos );

            Assert.AreEqual( 3, actual.Count, "Mal Total Combinaciones" );

            Assert.AreEqual( 3, actual[0].Count, "Mal cantidad en 1er combinacion" );
            Assert.AreEqual( 3, actual[1].Count, "Mal cantidad en 2da combinacion" );

            Assert.AreEqual( "0", actual[0]["1"][0].Id, "Mal Id item 0-0" );
            Assert.AreEqual( "2", actual[0]["2"][0].Id, "Mal Id item 0-1" );
            Assert.AreEqual( "3", actual[0]["3"][0].Id, "Mal Id item 0-2" );

            Assert.AreEqual( "0", actual[1]["1"][0].Id, "Mal Id item 1-0" );
            Assert.AreEqual( "2", actual[1]["2"][0].Id, "Mal Id item 1-1" );
            Assert.AreEqual( "1", actual[1]["3"][0].Id, "Mal Id item 1-2" );
        }

    }
}
