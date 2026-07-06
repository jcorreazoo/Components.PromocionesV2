using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones.Informantes
{
    public class ParticipanteBeneficiado
    {
        private string clave;
        private string id;
        private float cantidad;
        private string atributoAlterado;
        private Alteracion alteracion;
        private object valor;

        private IParticipante participante;

        public IParticipante Participante
        {
            get { return this.participante; }
            set { this.participante = value; }
        }

        private float importeBeneficioTotal;

        public float ImporteBeneficioTotal
        {
            get { return this.importeBeneficioTotal; }
            set { this.importeBeneficioTotal = value; }
        }

        private string promocion;

        private string beneficio;
		private string idParticipanteRegla;

        public string Beneficio
        {
            get { return this.beneficio; }
            set { this.beneficio = value; }
        }
        public string Promocion
        {
            get { return this.promocion; }
            set { this.promocion = value; }
        }

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

        public string AtributoAlterado
        {
            get { return this.atributoAlterado; }
            set { this.atributoAlterado = value; }
        }

        public Alteracion Alteracion
        {
            get { return this.alteracion; }
            set { this.alteracion = value; }
        }

        public object Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }

		public string IdParticipanteRegla
        {
			get { return this.idParticipanteRegla; }
			set { this.idParticipanteRegla = value; }
        }
	}
}
