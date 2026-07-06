using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Tests.Properties;
using System.Linq;

namespace ZooLogicSA.Promociones.Tests
{
	[TestClass]
	public class EvaluadorReglasDeParticipanteTest
	{
		[TestMethod]
		public void TestMethod1()
		{
			ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

			ComprobanteXML comprobante = new ComprobanteXML( config );
			comprobante.Cargar( Resources.ComprobanteBasicoPruebas );

			#region Participante
			ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
			participanteEnRegla.Id = "idParti";
			participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

			Regla regla = new Regla();
			regla.Id = 2;
			regla.Atributo = "Cantidad";
			regla.Comparacion = Factor.DebeSerMayorIgualA;
			regla.Valor = 5;
			participanteEnRegla.Reglas.Add( regla );

			participanteEnRegla.RelaReglas = "{2}";
			#endregion

			GestorComparaciones gestor = new GestorComparaciones();

			IEvaluadorSegunParticipante InstanciaMockeada = MockRepository.GenerateMock<IEvaluadorSegunParticipante>();
			InstanciaMockeada.Expect( x => x.ObtenerResultados( comprobante, participanteEnRegla, gestor ) ).Return( new List<ResultadoReglas>() );
			
			EvaluadorReglasDeParticipante e = new EvaluadorReglasDeParticipante( InstanciaMockeada, gestor );

			List<ResultadoReglas> resultados = e.ObtenerResultados( comprobante, participanteEnRegla );

			InstanciaMockeada.VerifyAllExpectations();

		}

        [TestMethod()]
        public void VerificarReglaTipoValorTest()
        {

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            ComprobanteXML comprobante = new ComprobanteXML(config);
            comprobante.Cargar(Resources.ComprobanteBasicoPruebas);

            #region Participante
            ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
            participanteEnRegla.Id = "idParti";
            participanteEnRegla.Codigo = "COMPROBANTE.VALORESDETALLE.ITEM";

            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "VALOR.DESCRIPCION";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "C";
            participanteEnRegla.Reglas.Add(regla);

            participanteEnRegla.RelaReglas = "{2}";

            ValidadorPromociones validadortest = new ValidadorPromociones();
            validadortest.VerificarReglaTipoValor(participanteEnRegla);

            Assert.AreEqual(1, participanteEnRegla.Reglas.Count);

            Regla regla3 = new Regla();
            regla3.Id = 3;
            regla3.Atributo = "VALOR.CODIGO";
            regla3.Comparacion = Factor.DebeSerIgualA;
            regla3.Valor = "C";
            participanteEnRegla.Reglas.Add(regla3);

            participanteEnRegla.RelaReglas = "{3}";
            #endregion

            //ValidadorPromociones validadortest = new ValidadorPromociones();
            validadortest.VerificarReglaTipoValor(participanteEnRegla);
            Assert.AreEqual(6, participanteEnRegla.Reglas.Count);

            // Si lo llamo de nuevo no debe cambiar la cantidad de reglas
            validadortest.VerificarReglaTipoValor(participanteEnRegla);
            Assert.AreEqual(6, participanteEnRegla.Reglas.Count);
        }
    }
}
