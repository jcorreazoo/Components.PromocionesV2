using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public class Beneficio
    {
        private string id;
        private string atributo;

        private List<DestinoBeneficio> destinos;

        private object valor;
        private Alteracion cambio;

        public Beneficio()
        {
            this.id = Guid.NewGuid().ToString( "N" );
            this.destinos = new List<DestinoBeneficio>();
        }

        public Alteracion Cambio
        {
            get { return this.cambio; }
            set { this.cambio = value; }
        }

        public string Atributo
        {
            get { return this.atributo; }
            set { this.atributo = value; }
        }

        public object Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public List<DestinoBeneficio> Destinos
        {
            get { return this.destinos; }
            set { this.destinos = value; }
        }

        public Beneficio Clonar()
        {
            Beneficio retorno = new Beneficio();
            retorno.Id = this.Id;
            retorno.Atributo = this.Atributo;
            retorno.Cambio = this.Cambio;
            retorno.Valor = this.Valor;
            retorno.Destinos = this.Destinos;
            
            return retorno;

        }

    }
}