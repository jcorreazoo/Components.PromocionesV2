
namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class ItemOrdenSeleccionParticipante
    {
        private string participante;
        private decimal dificultad;

        public decimal Dificultad
        {
            get { return this.dificultad; }
            set { this.dificultad = value; }
        }

        public string Participante
        {
            get { return this.participante; }
            set { this.participante = value; }
        }
    }
}
