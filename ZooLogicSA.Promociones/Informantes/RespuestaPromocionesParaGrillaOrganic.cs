using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Informantes
{
    public class RespuestaPromocionesParaGrillaOrganic
    {
        private string codigoPromocion;
        private decimal importeTotal;

        public decimal ImporteTotal
        {
            get { return this.importeTotal; }
            set { this.importeTotal = value; }
        }
        public string CodigoPromocion
        {
            get { return this.codigoPromocion; }
            set { this.codigoPromocion = value; }
        }
    }
}
