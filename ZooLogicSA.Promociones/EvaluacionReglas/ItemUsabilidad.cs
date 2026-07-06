using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones
{
    public class ItemUsabilidad
    {
        private string candidato;
        private string idParticipante;
        private int idRegla;
        private bool esBeneficiario;
        private Decimal cantidadEnItem;
        private Decimal requeridoPorRegla;
        private Decimal dificultad;
        private string clave;

        private Decimal precio;

        public Decimal Precio
        {
            get { return this.precio; }
            set { this.precio = value; }
        }

        public string IdParticipante
        {
            get { return this.idParticipante; }
            set { this.idParticipante = value; }
        }

        public int IdRegla
        {
            get { return this.idRegla; }
            set { this.idRegla = value; }
        }

        public bool EsBeneficiario
        {
            get { return this.esBeneficiario; }
            set { this.esBeneficiario = value; }
        }

        public string Candidato
        {
            get { return this.candidato; }
            set { this.candidato = value; }
        }

        public Decimal RequeridoPorRegla
        {
            get { return this.requeridoPorRegla; }
            set { this.requeridoPorRegla = value; }
        }

        public Decimal CantidadEnItem
        {
            get { return this.cantidadEnItem; }
            set { this.cantidadEnItem = value; }
        }

        public Decimal Dificultad
        {
            get { return this.dificultad; }
            set { this.dificultad = value; }
        }

        public Decimal AporteRelativoDelCandidato
        {
            get
            {
                return this.RequeridoPorRegla / this.CantidadEnItem;
                //return this.aporteRelativoDelCandidato;
            }
            //set { this.aporteRelativoDelCandidato = value; }
        }

        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }

    }
}
