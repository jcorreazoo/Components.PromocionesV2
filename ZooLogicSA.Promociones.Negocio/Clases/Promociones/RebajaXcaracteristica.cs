
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
namespace ZooLogicSA.Promociones.Negocio.Clases.Promociones
{
    public class RebajaXcaracteristica : TipoPromocion
    {
        // Monto fijo de descuento por una caracteristica( VALOR EN EL COMBO VISUAL )
        public RebajaXcaracteristica()
        {
            this._tieneCondicionFiltro = true;
            this._tieneCondicionLabel = true;
            this._tieneBeneficionMask = true;
            this.beneficioType = BeneficioType.MontoFijoDeDescuento;
            this._eleccionParticipanteDefault = EleccionParticipanteType.None;
            this._tieneBeneficioUnicoForzado = true;
            //this._tieneParticipantesBeneficiarios = true;
        }

        public override string ObtenerDescripcion()
        {
            return "Monto fijo por una característica";
        }

        public override string ObtenerMascaraCondicion()
        {
            return "";
        }

        public override string ObtenerMascaraBeneficio()
        {
            return "Tiene un monto fijo de 999999999999.99";
        }

        public override string ObtenerLabelBeneficio()
        {
            return "";
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
            return "Debe cargar el monto en el campo beneficio.";
        }

        public override string ObtenerMensajeDeAyudaMascaraBeneficio()
        {
            return "Monto final con impuestos incluídos para todos los participantes";
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
 