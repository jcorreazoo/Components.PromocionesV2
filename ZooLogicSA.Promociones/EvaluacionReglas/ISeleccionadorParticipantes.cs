using System;
using System.Collections.Generic;
namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public interface ISeleccionadorParticipantes
    {
        List<ItemUsabilidad> ObtenerUsabilidadPorParticipanteYRegla( List<ItemUsabilidad> usabilidadLoca, string participante, int idRegla, ResultadoReglas candidato );
    }
}
