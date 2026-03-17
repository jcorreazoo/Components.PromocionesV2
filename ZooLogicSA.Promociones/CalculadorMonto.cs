using System;
using System.Collections.Generic;
using System.Globalization;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones
{
    public class CalculadorMonto : ICalculadorMonto
    {
        private ConfiguracionComportamiento comportamiento;
        
        private Dictionary<string, IEvaluadorMatematico> evaluadores;

        public CalculadorMonto( ConfiguracionComportamiento comportamiento, Dictionary<string, IEvaluadorMatematico> evaluadores )
        {
            this.comportamiento = comportamiento;
            this.evaluadores = evaluadores;
        }

        public decimal ObtenerPrecio(IParticipante participante, decimal cantidad)
        {
            IEvaluadorMatematico evaluador = this.evaluadores[participante.Clave];

            decimal precio = this.ObtenerMontoDelAtributo( participante, this.comportamiento.ConfiguracionesPorParticipante[participante.Clave].Precio );
            decimal descuento = this.ObtenerMontoDelAtributo( participante, this.comportamiento.ConfiguracionesPorParticipante[participante.Clave].Descuento );
            decimal montoDescuento = this.ObtenerMontoDelAtributo( participante, this.comportamiento.ConfiguracionesPorParticipante[participante.Clave].MontoDescuento );

            return evaluador.ObtenerPrecio(precio, cantidad, descuento, montoDescuento);
        }

        private decimal ObtenerMontoDelAtributo(IParticipante participante, string nombreAtributo)
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            decimal retorno = 0M;

            if (!string.IsNullOrEmpty(nombreAtributo))
            {
                IAtributo atributo = participante.ObtenerAtributo(nombreAtributo);
                if (atributo.Valor != null)
                {
                    retorno = convertNumber.ConvertToDecimal(atributo.Valor.ToString());
                }
            }

            return retorno;
        }
    }
}
