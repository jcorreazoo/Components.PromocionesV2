
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
namespace ZooLogicSA.Promociones.Negocio.Clases.Promociones
{
    public class LlevaXtienedescuentoY : TipoPromocion
    {
        // lleva un producto paga con descuento otro producto( VALOR EN EL COMBO VISUAL )
        public LlevaXtienedescuentoY()
        {
            this._tieneBeneficionLabel = true;
            this._tieneBeneficionFiltro = true;
            this._tieneBeneficionMask = true;
            this._tieneCondicionLabel = true;
            this._tieneCondicionFiltro = true;
            this._tieneTipoPrecio = true;
            this.beneficioType = BeneficioType.LlevaUnoPagaConDescuentoOtro;
            this._tieneParticipantesBeneficiarios = true;
            this._typeValidacion = ValidacionPromoType.ValidarDescuento;
			this._tieneTopeDeDescuento = true;
            this._tieneRedondeo = true;
        }

        public override string ObtenerDescripcion()
        {
            return "Lleva un producto, paga con descuento otro producto";
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
            return "En los que cumplan la/s siguiente/s condición/es :";
        }

        public override string ObtenerLabelCondicion()
        {
            return "Todos los que cumplan la/s siguiente/s condicion/es :";
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
            return "Porcentaje de descuento para los participantes que cumplan las condiciones";
        }

        public override string ObtenerMascaraTopeDescuento()
        {
            throw new System.NotImplementedException();
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
            return true;
        }

        public override string ObtenerLabelListaDePrecios()
        {
            throw new System.NotImplementedException();
        }

        public override string ObtenerLabelCuotasSinRecargo()
        {
            throw new System.NotImplementedException();
        }
    }
}
 