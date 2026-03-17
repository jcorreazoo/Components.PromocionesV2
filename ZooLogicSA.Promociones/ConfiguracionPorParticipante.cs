using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones
{
    public class ConfiguracionPorParticipante
    {
        private string atributoId;
        private string cantidad;
        private string formulaCalculoPrecio;
        private string precio;
        private string descuento;
        private string montoDescuento;
        private Boolean esConsumible;
        private Boolean consumePorMonto;
        private string total;
		private string cantidadMonto;

        private string descuentoMnto;
        private string descuentoPorc;

        private string porcenjateDescuentoRecargo;
        private string tieneCuponIntegrado;

        private string esRetiroDeEfectivo;

        public Boolean ConsumePorMonto
        {
            get { return this.consumePorMonto; }
            set { this.consumePorMonto = value; }
        }

        public string AtributoId
        {
            get { return this.atributoId; }
            set { this.atributoId = value; }
        }

        public string Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public string FormulaCalculoPrecio
        {
            get { return this.formulaCalculoPrecio; }
            set { this.formulaCalculoPrecio = value; }
        }

        public string Precio
        {
            get { return this.precio; }
            set { this.precio = value; }
        }

        public string Descuento
        {
            get { return this.descuento; }
            set { this.descuento = value; }
        }

        public string MontoDescuento
        {
            get { return this.montoDescuento; }
            set { this.montoDescuento = value; }
        }

        public Boolean EsConsumible
        {
            get { return this.esConsumible; }
            set { this.esConsumible = value; }
        }

        public string Total
        {
            get { return this.total; }
            set { this.total = value; }
        }

		public string CantidadMonto
		{
			get { return this.cantidadMonto; }
			set { this.cantidadMonto = value; }
		}


        public string PorcenjateDescuentoRecargo
        {
            get { return this.porcenjateDescuentoRecargo; }
            set { this.porcenjateDescuentoRecargo = value; }
        }

        public string TieneCuponIntegrado
        {
            get { return this.tieneCuponIntegrado; }
            set { this.tieneCuponIntegrado = value; }
        }

        public string DescuentoMnto
        {
            get { return this.descuentoMnto; }
            set { this.descuentoMnto = value; }
        }

        public string DescuentoPorc
        {
            get { return this.descuentoPorc; }
            set { this.descuentoPorc = value; }
        }

        public string EsRetiroDeEfectivo
        {
            get { return this.esRetiroDeEfectivo; }
            set { this.esRetiroDeEfectivo = value; }
        }

    }

}
