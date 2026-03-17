using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
	public class EvaluadorReglasDeParticipante : IEvaluadorReglasDeParticipante
	{
        private GestorComparaciones gestorComparaciones;
		private IEvaluadorSegunParticipante evaluadorSegunParticipante;

		public EvaluadorReglasDeParticipante( IEvaluadorSegunParticipante evaluadorSegunParticipante, GestorComparaciones gestorComparaciones )
        {
            this.gestorComparaciones = gestorComparaciones;

			this.evaluadorSegunParticipante = evaluadorSegunParticipante;
        }


		#region IEvaluadorReglasDeParticipante Members

		public List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla )
		{
			this.gestorComparaciones.LimpiarExcepciones();

			List<ResultadoReglas> resultadoreglas;

			Stopwatch sw = new Stopwatch();
			sw.Start();

			resultadoreglas = this.evaluadorSegunParticipante.ObtenerResultados( comprobante, participanteEnRegla, this.gestorComparaciones );

			long elapsed = sw.ElapsedMilliseconds;

			return resultadoreglas;
		
		}

		public List<ErrorEvaluador> ObtenerExcepciones()
		{
			return this.gestorComparaciones.ObtenerExcepciones();
		}

		#endregion


	}
}
