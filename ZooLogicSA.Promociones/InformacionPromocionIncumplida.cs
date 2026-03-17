using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones
{
    public class InformacionPromocionIncumplida
    {
        private string idPromocion;
        private List<ResultadoReglas> resultados;
        private Promocion promocion;
        private decimal totalFaltante;
        private bool cumplioTodasLasReglasPeroNoElMontoBeneficio;
        private IComprobante comprobante;

        private List<ParticipanteFaltante> faltanteSeguro;
        private List<ParticipanteFaltante> cumplidos;
        private List<CombinacionParticipanteFaltantes> faltantePosibles;

        public InformacionPromocionIncumplida()
        {
            this.faltanteSeguro = new List<ParticipanteFaltante>();
            this.faltantePosibles = new List<CombinacionParticipanteFaltantes>();
            this.cumplidos = new List<ParticipanteFaltante>();
            this.idPromocion = "";
            this.cumplioTodasLasReglasPeroNoElMontoBeneficio = false;
        }

        public decimal TotalFaltante
        {
            get { return this.totalFaltante; }
            set { this.totalFaltante = value; }
        }

        public List<CombinacionParticipanteFaltantes> FaltantePosibles
        {
            get { return this.faltantePosibles; }
            set { this.faltantePosibles = value; }
        }

        public List<ParticipanteFaltante> Cumplidos
        {
            get { return this.cumplidos; }
            set { this.cumplidos = value; }
        }

        public List<ParticipanteFaltante> FaltanteSeguro
        {
            get { return this.faltanteSeguro; }
            set { this.faltanteSeguro = value; }
        }

        public Promocion Promocion
        {
            get { return promocion; }
            set { promocion = value; }
        }
        
        public string IdPromocion
        {
            get { return idPromocion; }
            set { idPromocion = value != null ? value : ""; }
        }

        public List<ResultadoReglas> Resultados
        {
            get { return resultados; }
            set { resultados = value; }
        }

        public bool CumplioTodasLasReglasPeroNoElMontoBeneficio
        {
            get { return cumplioTodasLasReglasPeroNoElMontoBeneficio; }
            set { cumplioTodasLasReglasPeroNoElMontoBeneficio = value; }
        }

        public IComprobante Comprobante
        {
            get { return comprobante; }
            set { comprobante = value; }
        }

    }
}
