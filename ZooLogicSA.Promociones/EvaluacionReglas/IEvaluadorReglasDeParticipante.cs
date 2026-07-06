using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
namespace ZooLogicSA.Promociones
{
    public interface IEvaluadorReglasDeParticipante
    {
        //Dictionary<string, List<string>> ParticipantesCompatibles  { get; set; }
        List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla );
        List<ErrorEvaluador> ObtenerExcepciones();
    }
}