using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class CalculadorDePartipantesCumplidosTest
    {
        [TestMethod]
        public void CalculadorDePartipantesCumplidos_De3Hay1()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            #region items
            IParticipante item1 = MockRepository.GenerateMock<IParticipante>();
            item1.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item1.Expect( x => x.Id ).Return( "1" );
            item1.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante item2 = MockRepository.GenerateMock<IParticipante>();
            item2.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item2.Expect( x => x.Id ).Return( "2" );
            item2.Expect( x => x.Cantidad ).Return( 1 ); 
            #endregion

            #region resultadoReglas
            ResultadoReglas resultado;

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_1" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item1 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "OK" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_2" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado ); 
            #endregion
            
            CalculadorDePartipantesCumplidos calculador = new CalculadorDePartipantesCumplidos( config );

            List<ParticipanteFaltante> cumplidos = calculador.Obtener( resultadoReglas );

            Assert.AreEqual( 1, cumplidos.Count );
            Assert.AreEqual( "OK", cumplidos[0].Participante.Id );
        }

        [TestMethod]
        public void CalculadorDePartipantesCumplidos_De3NoHay()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            #region items
            IParticipante item1 = MockRepository.GenerateMock<IParticipante>();
            item1.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item1.Expect( x => x.Id ).Return( "1" );
            item1.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante item2 = MockRepository.GenerateMock<IParticipante>();
            item2.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item2.Expect( x => x.Id ).Return( "2" );
            item2.Expect( x => x.Cantidad ).Return( 1 );
            #endregion

            #region resultadoReglas
            ResultadoReglas resultado;

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_1" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "OK" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_2" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );
            #endregion

            CalculadorDePartipantesCumplidos calculador = new CalculadorDePartipantesCumplidos( config );

            List<ParticipanteFaltante> cumplidos = calculador.Obtener( resultadoReglas );

            Assert.AreEqual( 0, cumplidos.Count );
        }

        [TestMethod]
        public void CalculadorDePartipantesCumplidos_De3Los3()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            #region items
            IParticipante item1 = MockRepository.GenerateMock<IParticipante>();
            item1.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item1.Expect( x => x.Id ).Return( "1" );
            item1.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante item2 = MockRepository.GenerateMock<IParticipante>();
            item2.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item2.Expect( x => x.Id ).Return( "2" );
            item2.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante item3 = MockRepository.GenerateMock<IParticipante>();
            item3.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item3.Expect( x => x.Id ).Return( "3" );
            item3.Expect( x => x.Cantidad ).Return( 1 );
            #endregion

            #region resultadoReglas
            ResultadoReglas resultado;

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item2 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_1" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item1 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "OK" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Participantes = new List<IParticipante>() { item3 };
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_2" };
            resultado.Requerido = 1;
            resultadoReglas.Add( resultado );
            #endregion

            CalculadorDePartipantesCumplidos calculador = new CalculadorDePartipantesCumplidos( config );

            List<ParticipanteFaltante> cumplidos = calculador.Obtener( resultadoReglas );

            Assert.AreEqual( 3, cumplidos.Count );
        }
    }
}
