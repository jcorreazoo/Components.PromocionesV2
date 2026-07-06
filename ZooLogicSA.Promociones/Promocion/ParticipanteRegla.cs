using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public class ParticipanteRegla
    {
        private string id;
        private string codigo;
        private List<Regla> reglas;
        private String relaReglas;

        private bool beneficiario;

        public bool Beneficiario
        {
            get { return this.beneficiario; }
            set { this.beneficiario = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Codigo
        {
            get { return this.codigo; }
            set { this.codigo = value; }
        }

        public List<Regla> Reglas
        {
            get { return this.reglas; }
            set { this.reglas = value; }
        }

        public String RelaReglas
        {
            get { return this.relaReglas; }
            set { this.relaReglas = value; }
        }

        public ParticipanteRegla()
        {
            this.Reglas = new List<Regla>();
        }
    }
}
