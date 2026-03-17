using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public class DestinoBeneficio
    {
        private string participante;
        private decimal cuantos;

        public decimal Cuantos
        {
            get { return this.cuantos; }
            set { this.cuantos = value; }
        }

        public string Participante
        {
            get { return this.participante; }
            set { this.participante = value; }
        }
    }
}
