using System.Collections.Generic;

namespace ZooLogicSA.Promociones
{
    public class CombinacionParticipanteFaltantes
    {
        private List<ParticipanteFaltante> faltantes;

        public CombinacionParticipanteFaltantes()
        {
            this.faltantes = new List<ParticipanteFaltante>();
        }

        public List<ParticipanteFaltante> Faltantes
        {
            get { return this.faltantes; }
            set { this.faltantes = value; }
        }
    }
}
