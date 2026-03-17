using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class ItemCompatibilidadCondicionGlobal
    {
        private Boolean cumple;
        private Dictionary<string, List<IParticipante>> participantes;
        private Dictionary<string, Dictionary<string, decimal>> cantidadAConsumirPorParticipante;

        public Boolean Suma
        {
            get { return this.cumple; }
            set { this.cumple = value; }
        }

        public Dictionary<string, List<IParticipante>> Participantes
        {
            get { return this.participantes; }
            set { this.participantes = value; }
        }

        public Dictionary<string, Dictionary<string, decimal>> CantidadAConsumirPorParticipante
        {
            get { return this.cantidadAConsumirPorParticipante; }
            set { this.cantidadAConsumirPorParticipante = value; }
        }

        public ItemCompatibilidadCondicionGlobal()
        {
            this.participantes = new Dictionary<string, List<IParticipante>>();
            this.cantidadAConsumirPorParticipante = new Dictionary<string, Dictionary<string, decimal>>();
        }
    }
}
