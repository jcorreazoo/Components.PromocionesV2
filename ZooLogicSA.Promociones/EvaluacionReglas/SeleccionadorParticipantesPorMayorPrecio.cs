using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class SeleccionadorParticipantesPorMayorPrecio : ISeleccionadorParticipantes
    {
        public List<ItemUsabilidad> ObtenerUsabilidadPorParticipanteYRegla( List<ItemUsabilidad> usabilidadLoca, string participante, int idRegla, ResultadoReglas candidato )
        {
            List<ItemUsabilidad> retorno = new List<ItemUsabilidad>();

            retorno.AddRange(
                                (from x in usabilidadLoca
                                 where x.IdParticipante == participante
                                      && x.IdRegla == idRegla
                                      && candidato.Participantes.Select( d => d.Clave + d.Id ).Contains( x.Candidato )
                                 orderby x.Precio descending, x.Dificultad
                                 select x)
                                    .ToList()
                             );

            return retorno;
        }
    }
}
