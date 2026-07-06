using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Informantes
{
    public class ParticipanteAfectado
    {
        private string clave;
        private string id;
        private float cantidad;
        private List<string> atributos;
		private string idParticipanteRegla;

        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public float Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public List<string> Atributos
        {
            get { return this.atributos; }
            set { this.atributos = value; }
        }

		public string IdParticipanteRegla
        {
			get { return this.idParticipanteRegla; }
			set { this.idParticipanteRegla = value; }
        }
	}
}
