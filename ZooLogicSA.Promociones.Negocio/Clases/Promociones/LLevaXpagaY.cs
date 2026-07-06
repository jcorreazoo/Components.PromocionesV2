using System.Collections.Generic;
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;

namespace ZooLogicSA.Promociones.Negocio.Clases.Promociones
{
    public class LLevaXpagaY : TipoPromocion
    {
        // Lleva una cantidad paga otra cantidad ( VALOR EN EL COMBO VISUAL )
        public LLevaXpagaY()
        {
            this._tieneBeneficionMask = true;
            this._tieneCondicionMask = true;
            this._tieneCondicionLabel = true;
            this._tieneCondicionFiltro = true;
            this._tieneTipoPrecio = true;
            this._tieneValidacion = true;
            this._typeValidacion = ValidacionPromoType.ValidarCantidadParticipantes;
            this.beneficioType = BeneficioType.LLevaXPagaY;
        }

        public override string ObtenerDescripcion()
        {
            return "Lleva una cantidad, paga otra cantidad";
        }

        public override string ObtenerMascaraCondicion()
        {
            return @"\Llev\ando 999";
        }

        public override string ObtenerMascaraBeneficio()
        {
            return @"P\ag\a 999";
        }

        public override string ObtenerLabelBeneficio()
        {
            return "";
        }

        public override string ObtenerLabelCondicion()
        {
            return "Y si cumple la/s siguiente/s condición/es : ";
        }

        public override string ObtenerReglaCondicionSegunMask( string valorMask )
        {
            return "[CANTIDAD] = '" + valorMask.Trim() + "'";
        }

        public override bool DebeAgregarReglaCondicionMask( List<string> listaCondiciones )
        {
            return false;
        }

        public override string ObtenerMensajeErrorCondicionMask()
        {
            return "Debe cargar la cantidad en el campo condición.";
        }

        public override string ObtenerMensajeErrorBeneficioMask()
        {
            return "Debe cargar la cantidad en el campo beneficio.";
        }

        public override string ObtenerMensajeDeAyudaMascaraBeneficio()
        {
            return "Cantidad de artículos que se abonan.";
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
