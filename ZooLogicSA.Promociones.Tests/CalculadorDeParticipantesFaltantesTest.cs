using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class CalculadorDeParticipantesFaltantesTest
    {
        [TestMethod]
        public void CalculadorDeParticipantesFaltantes_NoHay()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            ResultadoReglas resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.Requerido = 1;
            resultado.Satisfecho = 2;
            resultado.Cumple = true;
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item" };

            resultadoReglas.Add( resultado );

            CalculadorDeParticipantesFaltantes calculador = new CalculadorDeParticipantesFaltantes( config );
            List<ParticipanteFaltante> faltante = calculador.Obtener( resultadoReglas );

            Assert.AreEqual( faltante.Count, 0 );
        }

        [TestMethod]
        public void CalculadorDeParticipantesFaltantes_1FaltanteEntre3()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            ResultadoReglas resultado ;

            resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.Requerido = 1;
            resultado.Satisfecho = 1;
            resultado.Cumple = true;
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id= "Mal_1" };
            resultadoReglas.Add( resultado );
            
            resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.Requerido = 2;
            resultado.Satisfecho = 1;
            resultado.Cumple = false;
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "OK" };
            resultadoReglas.Add( resultado );

            resultado = new ResultadoReglas();
            resultado.Regla = new Regla() { Atributo = "Cantidad" };
            resultado.Requerido = 1;
            resultado.Satisfecho = 1;
            resultado.Cumple = true;
            resultado.PartPromo = new ParticipanteRegla() { Codigo = "Comprobante.Facturadetalle.Item", Id = "Mal_2" };
            resultadoReglas.Add( resultado );

            CalculadorDeParticipantesFaltantes calculador = new CalculadorDeParticipantesFaltantes( config );
            List<ParticipanteFaltante> faltante = calculador.Obtener( resultadoReglas );

            Assert.AreEqual( 1, faltante.Count );
            Assert.AreEqual( "OK", faltante[0].Participante.Id );
            Assert.AreEqual( 1, faltante[0].Cantidad );
        }
    }
}