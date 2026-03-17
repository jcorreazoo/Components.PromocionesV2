using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones
{
    public class RespuestaEvaluacion
    {
        private bool cumple;
        private List<CoincidenciaEvaluacion> coincidencias;
        private TimeSpan performance;
        private string promocion;
        //private InformacionPromocion infoPromo;

        //public InformacionPromocion InfoPromo
        //{
        //    get { return this.infoPromo; }
        //    set { this.infoPromo = value; }
        //}

        public RespuestaEvaluacion()
        {
            this.coincidencias = new List<CoincidenciaEvaluacion>();
        }

        public bool Cumple
        {
            get { return this.cumple; }
            set { this.cumple = value; }
        }

        public List<CoincidenciaEvaluacion> Coincidencias
        {
            get { return this.coincidencias; }
            set { this.coincidencias = value; }
        }

        public string Promocion
        {
            get { return this.promocion; }
            set { this.promocion = value; }
        }

        public TimeSpan Performance
        {
            get { return this.performance; }
            set { this.performance = value; }
        }

    }
}