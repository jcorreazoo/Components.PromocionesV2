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
    public class EvaluadorReglasDeParticipanteCantidadMonto : IEvaluadorSegunParticipante
    {
        private ConfiguracionComportamiento configuracionComportamiento;
		private ICalculadorMonto calculadorMonto;

		public EvaluadorReglasDeParticipanteCantidadMonto( ConfiguracionComportamiento configuracionComportamiento, ICalculadorMonto calculadorMonto )
        {
            this.configuracionComportamiento = configuracionComportamiento;
			this.calculadorMonto = calculadorMonto;
        }

		public List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla, GestorComparaciones gestorComparaciones )
		{
            List<ResultadoReglas> resultadoreglas = new List<ResultadoReglas>();

            List<Regla> reglasCantidad = (from x in participanteEnRegla.Reglas
										  where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participanteEnRegla.Codigo].CantidadMonto
										  select x).ToList();
            
            foreach ( Regla regla in reglasCantidad )
            {
                List<IParticipante> participantesCantidad = comprobante.ObtenerParticipantesSegunCondicionDeReglas( participanteEnRegla, regla.Id );



                Decimal cantidadParaRegla = participantesCantidad.Sum(x => Convert.ToDecimal( this.calculadorMonto.ObtenerPrecio( x, x.Cantidad ), new CultureInfo("en-US")));

                ResultadoReglas resultadoCantidad = new ResultadoReglas()
															{
																Regla = regla,
																Participantes = participantesCantidad,
																Cumple = gestorComparaciones.Comparar( Factor.DebeSerMayorIgualA, TipoDato.N, regla.Valor, cantidadParaRegla, regla ),
																Requerido = Convert.ToDecimal(regla.Valor, new CultureInfo("en-US")),
																Satisfecho = Convert.ToDecimal(cantidadParaRegla, new CultureInfo("en-US")),
																PartPromo = participanteEnRegla
															};

                resultadoreglas.Add( resultadoCantidad );
            }

            return resultadoreglas;

        }

        private bool EsCantidad( string claveParticipante, string atributo )
        {
            return ( atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[claveParticipante].Cantidad );
        }
    }
}
