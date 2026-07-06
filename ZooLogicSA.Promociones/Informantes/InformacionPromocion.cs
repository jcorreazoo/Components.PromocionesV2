using System.Collections.Generic;
using System;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Informantes
{
    public class InformacionPromocion
    {
        private string idPromocion;
		private Promocion promocion;
		private long demora;
        
		private List<ParticipanteBeneficiado> detalleBeneficiado;
        private List<ParticipanteAfectado> detalleAfectado;
        private int afectaciones;
        private float montoBeneficio;

		public InformacionPromocion( string promo )
		{
			this.detalleBeneficiado = new List<ParticipanteBeneficiado>();
			this.detalleAfectado = new List<ParticipanteAfectado>();
			this.idPromocion = promo == null ? "" : promo;
		}

        #region Get/Set
        public string IdPromocion
        {
            get { return this.idPromocion; }
        }

        public List<ParticipanteBeneficiado> DetalleBeneficiado
        {
            get { return this.detalleBeneficiado; }
            set { this.detalleBeneficiado = value; }
        }

        public List<ParticipanteAfectado> DetalleAfectado
        {
            get { return this.detalleAfectado; }
            set { this.detalleAfectado = value; }
        }

        public int Afectaciones
        {
            get { return this.afectaciones; }
            set { this.afectaciones = value; }
        }

        public float MontoBeneficio
        {
            get { return this.montoBeneficio; }
            set { this.montoBeneficio = value; }
        }

		public Promocion Promocion
		{
			get { return promocion; }
			set { promocion = value; }
		}

		public long Demora
		{
			get { return demora; }
			set { demora = value; }
		}

        #endregion

        public InformacionPromocionIncumplida infoIncumplida { get; set; }
    }
}