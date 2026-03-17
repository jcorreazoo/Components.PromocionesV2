
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
namespace ZooLogicSA.Promociones.Negocio.Clases.Promociones
{
    public class DescuentoBancarioConTope : TipoPromocion
    {
        // PORCENTAJE DE DESCUENTO POR UNA CARACTERISTICA ( VALOR EN EL COMBO VISUAL )
        public DescuentoBancarioConTope()
        {
            this._tieneBeneficionMask = true;
            this._tieneCondicionLabel = true;
            this._tieneCondicionFiltro = true;
            this._tieneTopeDeDescuento = true;
            this._tieneCuotasSinRecargo = true;
            this.beneficioType = BeneficioType.PorcentajeFijoDeDescuentoBancario;
            this._eleccionParticipanteDefault = EleccionParticipanteType.AplicarAlDeMayorPrecio;
            this._typeValidacion = ValidacionPromoType.ValidarDescuentoYCuotas;
            this._tieneBeneficioUnicoForzado = true;
            this._codigoDetalleType = CodigoDetalleType.DetalleValor;
        }

        public override string ObtenerDescripcion()
        {
            return "Bancaria: % de descuento con tope";
        }

        public override string ObtenerMascaraCondicion()
        {
            return "";
        }

        public override string ObtenerMascaraBeneficio()
        {
            return @"Tienen 999.99 \% de descuento";
        }

        public override string ObtenerLabelBeneficio()
        {
            return "";
        }

        public override string ObtenerLabelCondicion()
        {
            return "Todos los que cumplan la/s siguiente/s condición/es :";
        }

        public override string ObtenerReglaCondicionSegunMask( string valorMask )
        {
            return "";
        }

        public override bool DebeAgregarReglaCondicionMask( List<string> listaCondiciones )
        {
            return false;
        }

        public override string ObtenerMensajeErrorCondicionMask()
        {
            return "";
        }

        public override string ObtenerMensajeErrorBeneficioMask()
        {
            return "Debe cargar el porcentaje en el campo beneficio.";
        }

        public override string ObtenerMensajeDeAyudaMascaraBeneficio()
        {
            return "Porcentaje de descuento para los participantes que cumplan las condiciones.";
        }

        public override string ObtenerMascaraTopeDescuento()
        {
            return @"tienen 999.99 \% de descuento";
        }

        public override string ObtenerLabelTopeDescuento()
        {
            throw new System.NotImplementedException();
        }

        public override string ObtenerMensajeErrorTopeDescuentoMask()
        {
            throw new System.NotImplementedException();
        }

        public override bool VerificarSiElParticipanteAplicaAEsteTipo(string nodoParticipante)
        {
            return (nodoParticipante != "VALOR");
        }

        public override string ObtenerLabelListaDePrecios()
        {
            throw new System.NotImplementedException();
        }

        public override string ObtenerLabelCuotasSinRecargo()
        {
            return "Tope de cuotas sin recargo:";
        }
    }
}
