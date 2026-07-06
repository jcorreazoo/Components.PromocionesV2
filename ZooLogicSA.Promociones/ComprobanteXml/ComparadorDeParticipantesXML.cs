using System;
using System.Globalization;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones.ComprobanteXml
{
    public class ComparadorDeParticipantesXML : IComparadorDeParticipantes
    {
        public decimal ObtenerDiferenciaDeImporte( IEvaluadorMatematico c, ConfiguracionComportamiento comportamiento, IParticipante participanteOriginal, IParticipante participantePromocionado, float cantidad )
        {
            decimal precioOriginal = this.ObtenerPrecio( c, participanteOriginal, comportamiento, (decimal)cantidad );
            decimal precioNuevo = this.ObtenerPrecio( c, participantePromocionado, comportamiento, (decimal)cantidad );
            decimal beneficio = precioOriginal - precioNuevo;
                        
            return beneficio;
        }

        private decimal ObtenerPrecio( IEvaluadorMatematico c, IParticipante participante, ConfiguracionComportamiento comportamiento, decimal cantidad )
        {
            decimal precio = this.ObtenerValor( participante, comportamiento.ConfiguracionesPorParticipante[participante.Clave].Precio );
            decimal descuento = this.ObtenerValor( participante, comportamiento.ConfiguracionesPorParticipante[participante.Clave].Descuento );
            decimal montoDescuento = this.ObtenerValor( participante, comportamiento.ConfiguracionesPorParticipante[participante.Clave].MontoDescuento );
            return c.ObtenerPrecio( precio, cantidad, descuento, montoDescuento );
        }

        private decimal ObtenerValor( IParticipante participante, string nombreAtributo )
        {
            ConvertidorNumerico convertNumber = new ConvertidorNumerico();
            IAtributo atributo = participante.ObtenerAtributo( nombreAtributo );
            return convertNumber.ConvertToDecimal( atributo.Valor.ToString() );
        }
    }
}