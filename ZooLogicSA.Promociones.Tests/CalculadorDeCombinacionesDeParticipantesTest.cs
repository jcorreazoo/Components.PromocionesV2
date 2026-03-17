using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;


namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class CalculadorDeCombinacionesDeParticipantesTest
    {
        [TestMethod]
        public void ObtenerCombinaciones_3Participantes_CombinacionesDe3()
        {
            CalculadorDeCombinacionesDeParticipantes calculador = new CalculadorDeCombinacionesDeParticipantes();

            List<ParticipanteRegla> participantes = new List<ParticipanteRegla>();

            participantes.Add( new ParticipanteRegla() { Id = "1" } );
            participantes.Add( new ParticipanteRegla() { Id = "2" } );
            participantes.Add( new ParticipanteRegla() { Id = "3" } );

            List<CombinacionParticipanteFaltantes> resultado = calculador.ObtenerCombinaciones( participantes, 3 );

            Assert.AreEqual( 27, resultado.Count );
        }

        [TestMethod]
        public void ObtenerCombinaciones_3Participantes_CombinacionesDe2()
        {
            CalculadorDeCombinacionesDeParticipantes calculador = new CalculadorDeCombinacionesDeParticipantes();

            List<ParticipanteRegla> participantes = new List<ParticipanteRegla>();

            participantes.Add( new ParticipanteRegla() { Id = "1" } );
            participantes.Add( new ParticipanteRegla() { Id = "2" } );
            participantes.Add( new ParticipanteRegla() { Id = "3" } );

            List<CombinacionParticipanteFaltantes> resultado = calculador.ObtenerCombinaciones( participantes, 2 );

            Assert.AreEqual( 9, resultado.Count );
        }

        [TestMethod]
        public void ObtenerCombinaciones_3Participantes_Combinaciones2Iguales()
        {
            CalculadorDeCombinacionesDeParticipantes calculador = new CalculadorDeCombinacionesDeParticipantes();

            List<ParticipanteRegla> participantes = new List<ParticipanteRegla>();

            participantes.Add( new ParticipanteRegla() { Id = "1" } );
            participantes.Add( new ParticipanteRegla() { Id = "2" } );
            participantes.Add( new ParticipanteRegla() { Id = "3" } );

            List<CombinacionParticipanteFaltantes> resultado = calculador.ObtenerCombinaciones( participantes, 2 );

            Assert.AreEqual( 1, resultado[0].Faltantes.Count );
            Assert.AreEqual( 2, resultado[0].Faltantes[0].Cantidad );
            Assert.AreEqual( 2, resultado[1].Faltantes.Count );
            Assert.AreEqual( 1, resultado[1].Faltantes[0].Cantidad );
            Assert.AreEqual( 1, resultado[1].Faltantes[1].Cantidad );
        }

    }
}
