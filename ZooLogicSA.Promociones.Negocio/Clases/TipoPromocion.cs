using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Negocio.Clases
{
    public abstract class TipoPromocion
    {
        protected bool _tieneCondicionMask = false;
        protected bool _tieneBeneficionMask = false;
        protected bool _tieneCondicionLabel = false;
        protected bool _tieneBeneficionLabel = false;
        protected bool _tieneCondicionFiltro = false;
        protected bool _tieneBeneficionFiltro = false;
        protected bool _tieneTipoPrecio = false;
        protected bool _tieneValidacion = false;
        protected bool _tieneBeneficioUnicoForzado = false;
        protected bool _tieneTopeDeDescuento = false;
        protected bool _tieneRedondeo = false;
        protected bool _tieneListaDePrecios = false;
        protected bool _tieneCuotasSinRecargo = false;
        protected ValidacionPromoType _typeValidacion = ValidacionPromoType.None;
        protected BeneficioType beneficioType = BeneficioType.None;
        protected ValorBeneficio valoresBeneficio = new ValorBeneficio();
        protected CodigoDetalleType _codigoDetalleType = CodigoDetalleType.DetalleItem;
        protected EleccionParticipanteType _eleccionParticipanteDefault = EleccionParticipanteType.None;
        protected bool _tieneParticipantesBeneficiarios = false;

        public bool TieneParticipantesBeneficiarios
        {
            get
            {
                return this._tieneParticipantesBeneficiarios;
            }
        }
        public bool TieneQueForzarCrearBeneficioUnico()
        {
            return this._tieneBeneficioUnicoForzado;
        }

        public CodigoDetalleType codigoDetalleType
        {
            get
            {
                return this._codigoDetalleType;
            }
        }

        public EleccionParticipanteType EleccionParticipanteDefault
        {
            get
            {
                return this._eleccionParticipanteDefault;
            }
        }

        public ValidacionPromoType TypeValidacion()
        {
            return this._typeValidacion;
        }

        public BeneficioType BeneficioType
        {
            get
            {
                return this.beneficioType;
            }
        }

        public bool TieneTipoPrecio()
        {
            return this._tieneTipoPrecio;
        }

        public bool TieneValidacion()
        {
            return this._tieneValidacion;
        }

        public bool TieneCondicionMask()
        {
            return this._tieneCondicionMask;
        }

        public bool TieneBeneficioMask()
        {
            return this._tieneBeneficionMask;
        }

        public bool TieneCondicionLabel()
        {
            return this._tieneCondicionLabel;
        }

        public bool TieneBeneficioLabel()
        {
            return this._tieneBeneficionLabel;
        }

        public bool TieneCondicionFiltro()
        {
            return this._tieneCondicionFiltro;
        }

        public bool tieneTopeDeDescuento()
        {
            return this._tieneTopeDeDescuento;
        }

        public bool TieneBeneficioFiltro()
        {
            return this._tieneBeneficionFiltro;
        }

        public override string ToString()
        {
            return this.ObtenerDescripcion();
        }

        public bool tieneRedondeo()
        {
            return this._tieneRedondeo;
        }
        public bool TieneListaDePrecios()
        {
            return this._tieneListaDePrecios;
        }

        public bool TieneCuotas()
        {
            return this._tieneCuotasSinRecargo;
        }

        public abstract string ObtenerDescripcion();

        public abstract string ObtenerMascaraCondicion();

        public abstract string ObtenerMascaraBeneficio();

        public abstract string ObtenerMascaraTopeDescuento();

        public abstract string ObtenerLabelBeneficio();

        public abstract string ObtenerLabelCondicion();

        public abstract string ObtenerLabelTopeDescuento();

        public abstract string ObtenerLabelListaDePrecios();

        public abstract string ObtenerLabelCuotasSinRecargo();
        protected Beneficio ObtenerBeneficio()
        {           
            return FactoryBeneficio.ObtenerBeneficio( this.beneficioType, this.valoresBeneficio );
        }

        public abstract string ObtenerReglaCondicionSegunMask( string valorMask );

        public abstract bool DebeAgregarReglaCondicionMask( List<string> listaCondiciones );

        public abstract string ObtenerMensajeErrorCondicionMask();

        public abstract string ObtenerMensajeErrorBeneficioMask();

        public abstract string ObtenerMensajeErrorTopeDescuentoMask();

        public abstract string ObtenerMensajeDeAyudaMascaraBeneficio();

        public abstract bool VerificarSiElParticipanteAplicaAEsteTipo(string nodoParticipante);

        public string ObtenerWhereAdicional(string entidad, string nodoPadre)
        {
            string where = "";
            if (entidad == "VALOR")
            {
                if (nodoPadre == "CUPON")
                    where = "tipo = 3";
                else
                    where = "tipo in (1,2,11)";
                
            }
            return where;
        }
    }
}
