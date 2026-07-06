using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class ResultadoReglas
    {
        private Regla regla;
        private ParticipanteRegla partPromo;
        private bool cumple;
        private Decimal requerido;
        private Decimal satisfecho;
        private List<IParticipante> participantes;

        public ResultadoReglas()
        {
            this.participantes = new List<IParticipante>();
        }

        public Regla Regla
        {
            get { return this.regla; }
            set { this.regla = value; }
        }
        public ParticipanteRegla PartPromo
        {
            get { return partPromo; }
            set { partPromo = value; }
        }
        public bool Cumple
        {
            get { return cumple; }
            set { cumple = value; }
        }
        public Decimal Requerido
        {
            get { return this.requerido; }
            set { this.requerido = value; }
        }
        public Decimal Satisfecho
        {
            get { return this.satisfecho; }
            set { this.satisfecho = value; }
        }
        public List<IParticipante> Participantes
        {
            get { return participantes; }
            set { participantes = value; }
        }
    }
}
