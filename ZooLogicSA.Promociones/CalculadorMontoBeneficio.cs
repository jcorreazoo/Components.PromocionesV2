using System.Linq;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones
{
    public class CalculadorMontoBeneficio
    {
        private ICalculadorMonto calculadorMonto;

        public CalculadorMontoBeneficio( ConfiguracionComportamiento configuracionComportamiento, ICalculadorMonto calculador )
        {
            this.calculadorMonto = calculador;
        }

        public float CalcularMontosBeneficio( InformacionPromocion informacion, IComprobante comprobante, ConfiguracionComportamiento configuracionComportamiento )
        {
            foreach ( ParticipanteBeneficiado beneficiado in informacion.DetalleBeneficiado )
            {
                IParticipante participanteOriginal = comprobante.ObtenerNodoParticipante( beneficiado.Clave, beneficiado.Id, "", "" );
                IParticipante participantePromocionado = beneficiado.Participante;

                decimal cantidad = (decimal)beneficiado.Cantidad;
                decimal precioNuevo = this.calculadorMonto.ObtenerPrecio( participantePromocionado, cantidad );
                decimal precioOriginal = this.calculadorMonto.ObtenerPrecio( participanteOriginal, cantidad );

                decimal beneficio = 0;
                if ( configuracionComportamiento.ConfiguracionesPorParticipante[beneficiado.Clave].ConsumePorMonto )
                {
                    beneficio = 0 - precioNuevo;
                }
                else
                {
                    beneficio = precioOriginal - precioNuevo;
                }

                beneficiado.ImporteBeneficioTotal = (float)beneficio;
            }

            return informacion.DetalleBeneficiado.Sum( x => x.ImporteBeneficioTotal );
        }
    }
}