using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones
{
    public class CoincidenciaEvaluacion
    {
        private string codigoParticipanteEnComprobante;
        private List<string> idParticipanteEnComprobante;
        private List<string> idParticipanteRestante;
        private string idParticipanteEnRegla;
        private Decimal consume;

        private List<string> atributos;

        public List<string> Atributos
        {
            get { return this.atributos; }
            set { this.atributos = value; }
        }
        //private List<ResultadoReglas> resultados;

        //public List<ResultadoReglas> Resultados
        //{
        //    get { return this.resultados; }
        //    set { this.resultados = value; }
        //}

        public Decimal Consume
        {
            get { return this.consume; }
            set { this.consume = value; }
        }

        public string IdParticipanteEnRegla
        {
            get { return this.idParticipanteEnRegla; }
            set { this.idParticipanteEnRegla = value; }
        }

        public List<string> IdParticipanteEnComprobante
        {
            get { return this.idParticipanteEnComprobante; }
            set { this.idParticipanteEnComprobante = value; }
        }

        public List<string> IdParticipanteRestante
        {
            get { return this.idParticipanteRestante; }
            set { this.idParticipanteRestante = value; }
        }

        public string CodigoParticipanteEnComprobante
        {
            get { return this.codigoParticipanteEnComprobante; }
            set { this.codigoParticipanteEnComprobante = value; }
        }
    }
}
