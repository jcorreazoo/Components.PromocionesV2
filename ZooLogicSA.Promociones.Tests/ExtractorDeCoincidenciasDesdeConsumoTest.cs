using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ExtractorDeCoincidenciasDesdeConsumoTest
    {
        [TestMethod]
        public void ExtractorDeCoincidenciasDesdeConsumo_TestBla()
        {
            ExtractorDeCoincidenciasDesdeConsumo extractor = new ExtractorDeCoincidenciasDesdeConsumo();

            List<ConsumoParticipanteEvaluado> consumos = new List<ConsumoParticipanteEvaluado>();

            ConsumoParticipanteEvaluado consumo ;
            
            consumo = new ConsumoParticipanteEvaluado();
            consumo.Requerido = 1;
            consumo.Satisfecho = 1;
            consumo.Atributos = new List<string>() { "Atributo1" };
            consumo.CodigoParticipanteEnComprobante = "ComprobanteMock";
            consumo.IdParticipanteEnRegla = "1";
            consumo.ParticipantesEnComprobante = new List<string>() { "Id_OK" };
            consumos.Add( consumo );

            consumo = new ConsumoParticipanteEvaluado();
            consumo.Requerido = 0;
            consumo.Satisfecho = 1;
            consumo.Atributos = new List<string>() { "Atributo1" };
            consumo.CodigoParticipanteEnComprobante = "ComprobanteMock";
            consumo.IdParticipanteEnRegla = "3";
            consumo.ParticipantesEnComprobante = new List<string>() { "Id_NO" };
            consumos.Add( consumo );

            consumo = new ConsumoParticipanteEvaluado();
            consumo.Requerido = 1;
            consumo.Satisfecho = 2;
            consumo.Atributos = new List<string>() { "Atributo1" };
            consumo.CodigoParticipanteEnComprobante = "ComprobanteMock";
            consumo.IdParticipanteEnRegla = "2";
            consumo.ParticipantesEnComprobante = new List<string>() { "Id_NO" };
            consumos.Add( consumo );

            consumo = new ConsumoParticipanteEvaluado();
            consumo.Requerido = 2;
            consumo.Satisfecho = 2;
            consumo.Atributos = new List<string>() { "Atributo1" };
            consumo.CodigoParticipanteEnComprobante = "ComprobanteMock";
            consumo.IdParticipanteEnRegla = "2";
            consumo.ParticipantesEnComprobante = new List<string>() { "Id_OK" };
            consumos.Add( consumo );

            List<CoincidenciaEvaluacion> coincidencias = extractor.Obtener( consumos );

            Assert.AreEqual( 2, coincidencias.Count );
            Assert.AreEqual( 3, coincidencias.Sum( x=>x.Consume ) );
        }
    }
}
