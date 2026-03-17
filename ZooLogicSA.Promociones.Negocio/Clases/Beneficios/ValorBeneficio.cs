using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Negocio.Clases.Beneficios
{
    public class ValorBeneficio
    {
        //private string idParticipanteRegla;
        //private int cuantos;
        private string valor;

        private List<DestinoBeneficio> destinos;

        public ValorBeneficio()
        {
            this.Destinos = new List<DestinoBeneficio>();
        }

        public List<DestinoBeneficio> Destinos
        {
            get { return this.destinos; }
            set { this.destinos = value; }
        }


        //public string IdParticipanteRegla
        //{
        //    get { return this.idParticipanteRegla; }
        //    set { this.idParticipanteRegla = value; }
        //}
        //public int Cuantos
        //{
        //    get { return this.cuantos; }
        //    set { this.cuantos = value; }
        //}
        public string Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }
    }
}
