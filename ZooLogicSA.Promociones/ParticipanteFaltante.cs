using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class ParticipanteFaltante
    {
        private ParticipanteRegla participante;
        private decimal cantidad;
        private decimal requerido;

        public decimal Requerido
        {
            get { return this.requerido; }
            set { this.requerido = value; }
        }
        public decimal Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public ParticipanteRegla Participante
        {
            get { return this.participante; }
            set { this.participante = value; }
        }
        
    }
}
