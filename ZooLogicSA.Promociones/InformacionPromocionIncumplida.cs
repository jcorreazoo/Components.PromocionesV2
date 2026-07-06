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
        private decimal satisfechoEfectivo;

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
            this.satisfechoEfectivo = -1m; // -1 = no establecido (usa lógica legacy)
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

        /// <summary>
        /// Cantidad efectivamente satisfecha considerando las restricciones de
        /// AplicacionProductosIguales (PorArticulo / PorArticuloYCombinacion).
        /// -1 indica que no fue establecido (se usa la lógica global de Resultados).
        /// </summary>
        public decimal SatisfechoEfectivo
        {
            get { return this.satisfechoEfectivo; }
            set { this.satisfechoEfectivo = value; }
        }

        public IComprobante Comprobante
        {
            get { return comprobante; }
            set { comprobante = value; }
        }

    }
}
