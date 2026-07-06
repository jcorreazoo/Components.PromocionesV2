using System.Collections.Generic;

namespace ZooLogicSA.Promociones
{
    public class ConsumoParticipanteEvaluado
    {
        private string codigoParticipanteEnComprobante;
        private string idParticipanteEnRegla;
        private List<string> participantesEnComprobante;
        private List<string> participantesRestantes;
        private decimal satisfecho;
        private decimal requerido;
        private List<string> atributos;

        public string CodigoParticipanteEnComprobante
        {
            get { return codigoParticipanteEnComprobante; }
            set { codigoParticipanteEnComprobante = value; }
        }

        public string IdParticipanteEnRegla
        {
            get { return idParticipanteEnRegla; }
            set { idParticipanteEnRegla = value; }
        }

        public List<string> ParticipantesEnComprobante
        {
            get { return participantesEnComprobante; }
            set { participantesEnComprobante = value; }
        }

        public List<string> ParticipantesRestantes
        {
            get { return participantesRestantes; }
            set { participantesRestantes = value; }
        }

        public decimal Satisfecho
        {
            get { return satisfecho; }
            set { satisfecho = value; }
        }

        public decimal Requerido
        {
            get { return requerido; }
            set { requerido = value; }
        }

        public List<string> Atributos
        {
            get { return atributos; }
            set { atributos = value; }
        }
    }
}
