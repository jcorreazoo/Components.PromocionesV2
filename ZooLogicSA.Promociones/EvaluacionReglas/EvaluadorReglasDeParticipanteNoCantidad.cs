using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comparadores;
using System.IO;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Globalization;
using ZooLogicSA.Promociones.EvaluacionReglas;

namespace ZooLogicSA.Promociones
{
	public class EvaluadorReglasDeParticipanteNoCantidad : IEvaluadorSegunParticipante
    {
        private ConfiguracionComportamiento configuracionComportamiento;

		public EvaluadorReglasDeParticipanteNoCantidad( ConfiguracionComportamiento configuracionComportamiento )
        {
            this.configuracionComportamiento = configuracionComportamiento;
        }

		public List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla, GestorComparaciones gestorComparaciones )
        {
            List<ResultadoReglas> resultadoreglas = new List<ResultadoReglas>();

			List<Regla> reglasSinCantidad = (from x in participanteEnRegla.Reglas
											 where !(x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnRegla.Codigo].Cantidad)
												&& !(x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnRegla.Codigo].CantidadMonto)
											 select x).ToList();

			foreach ( Regla regla in reglasSinCantidad )
            {
                List<IParticipante> participantes = comprobante.ObtenerParticipantesSegunClave( participanteEnRegla.Codigo );

                List<ResultadoReglas> consulta =
                            (
                              from participanteEnComprobante in participantes
                              let atributo = participanteEnComprobante.ObtenerAtributo( regla.Atributo )
                              let cumple = !participanteEnComprobante.CoincidenciasExcluidas.ToList().Exists( x => x == participanteEnRegla.Id.ToString() ) && gestorComparaciones.Comparar( regla.Comparacion, atributo.TipoDato, regla.Valor, atributo.Valor, regla )
                              select new ResultadoReglas
                              {
                                  Regla = regla,
                                  Participantes = new List<IParticipante>() { participanteEnComprobante },
                                  Cumple = cumple,
                                  Requerido = 1,
                                  Satisfecho = Convert.ToDecimal(cumple, new CultureInfo("en-US")),
                                  PartPromo = participanteEnRegla
                              }
                              ).ToList();

                resultadoreglas.AddRange( consulta );
            }

            return resultadoreglas;
        }

		#region IEvaluadorSegunParticipante Members

		#endregion
	}
}
