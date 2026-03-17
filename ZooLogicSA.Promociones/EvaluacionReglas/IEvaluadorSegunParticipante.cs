using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
	public interface IEvaluadorSegunParticipante
	{
		List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla, GestorComparaciones gestorComparaciones );
	}
}
