using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI
{
    public class DatosDeParticipantesReglasEventsArgs : EventArgs 
    {
        private List<ParticipanteRegla> participantesReglas;
        public DatosDeParticipantesReglasEventsArgs( List<ParticipanteRegla> lista )
        {
            this.participantesReglas = lista;
        }

        public List<ParticipanteRegla> ParticipantesReglas
        {
            get
            {
                return participantesReglas;
            }
            set
            {
                participantesReglas = value;
            }
        }
    }
}
