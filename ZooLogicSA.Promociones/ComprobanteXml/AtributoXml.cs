using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones
{
    public class AtributoXml : IAtributo
    {
        private object valor;
        private TipoDato tipoDato;

        public object Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }

        public TipoDato TipoDato
        {
            get { return this.tipoDato; }
            set { this.tipoDato = value; }
        }
    }
}
