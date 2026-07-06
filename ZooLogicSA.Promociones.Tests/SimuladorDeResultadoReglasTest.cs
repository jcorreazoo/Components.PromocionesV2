using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class SimuladorDeResultadoReglasTest
    {
        [TestMethod]
        public void SimuladorDeResultadoReglas_Simple()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            SimuladorDeResultadoReglas simulador = new SimuladorDeResultadoReglas( config );

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            #region resultadoReglas
            ResultadoReglas resultado;

            resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "1" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );
            #endregion

            List<ParticipanteFaltante> imposibles = new List<ParticipanteFaltante>();

            CombinacionParticipanteFaltantes combinacion = new CombinacionParticipanteFaltantes();

            simulador.AgregarDummies( resultadoReglas, imposibles, combinacion );

            Assert.AreEqual( 1, resultadoReglas.Count );
        }

        [TestMethod]
        public void SimuladorDeResultadoReglas_ConImposibles()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            SimuladorDeResultadoReglas simulador = new SimuladorDeResultadoReglas( config );

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            List<ParticipanteFaltante> imposibles = new List<ParticipanteFaltante>();
            ParticipanteFaltante p = new ParticipanteFaltante();
            p.Participante = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "1" };
            p.Cantidad = 1;
            imposibles.Add( p );

            #region resultadoReglas
            ResultadoReglas resultado;

            resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = imposibles[0].Participante;
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );
            #endregion

            CombinacionParticipanteFaltantes combinacion = new CombinacionParticipanteFaltantes();

            simulador.AgregarDummies( resultadoReglas, imposibles, combinacion );

            Assert.AreEqual( 1, resultadoReglas.Count );
        }
    }
}
