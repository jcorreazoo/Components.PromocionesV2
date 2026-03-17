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
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones
{
	public class EvaluadorReglasDeParticipanteCantidad : IEvaluadorSegunParticipante
    {
        private ConfiguracionComportamiento configuracionComportamiento;

		public EvaluadorReglasDeParticipanteCantidad( ConfiguracionComportamiento configuracionComportamiento )
        {
            this.configuracionComportamiento = configuracionComportamiento;
        }

		public List<ResultadoReglas> ObtenerResultados( IComprobante comprobante, ParticipanteRegla participanteEnRegla, GestorComparaciones gestorComparaciones )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            List<ResultadoReglas> resultadoreglas = new List<ResultadoReglas>();

            List<Regla> reglasCantidad = (from x in participanteEnRegla.Reglas 
												where x.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[ participanteEnRegla.Codigo ].Cantidad
												select x).ToList();


            foreach ( Regla regla in reglasCantidad )
            {
                List<IParticipante> participantesCantidad = comprobante.ObtenerParticipantesSegunCondicionDeReglas( participanteEnRegla, regla.Id );

                Decimal cantidadParaRegla = participantesCantidad.Sum(x => Convert.ToDecimal(x.Cantidad, new CultureInfo("en-US")));
                //////Factor factorParaCompararSiCumple = (regla.Comparacion == Factor.DebeSerMayorA) ? Factor.DebeSerMayorA : Factor.DebeSerMayorIgualA ;

                if (regla.Atributo == "CANTIDAD" && regla.Valor is String)
                {
                    regla.Valor = convertNumber.ConvertToString( regla.Valor );
                }

                ResultadoReglas resultadoCantidad = new ResultadoReglas
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
    }
}
