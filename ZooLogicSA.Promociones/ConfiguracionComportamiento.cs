using System.Collections.Generic;

namespace ZooLogicSA.Promociones
{
    public class ConfiguracionComportamiento
    {
        private string nombreComprobante;
        private string atributoMontoFinal;
        private Dictionary<string, ConfiguracionPorParticipante> configuracionesPorParticipante;

        private string atributoBeneficioPorMonto;

        public ConfiguracionComportamiento()
        {
            this.nombreComprobante = "COMPROBANTE";
            this.atributoMontoFinal = "MONTOFINAL";
            this.atributoBeneficioPorMonto = "MONTO";


            this.configuracionesPorParticipante = new Dictionary<string, ConfiguracionPorParticipante>();

            this.configuracionesPorParticipante.Add( "COMPROBANTE", new ConfiguracionPorParticipante() );
            this.configuracionesPorParticipante["COMPROBANTE"].AtributoId = "CODIGO";
            this.configuracionesPorParticipante["COMPROBANTE"].Cantidad = "CANTIDAD";
            this.configuracionesPorParticipante["COMPROBANTE"].EsConsumible = false;
            this.configuracionesPorParticipante["COMPROBANTE"].FormulaCalculoPrecio = "0";
            this.configuracionesPorParticipante["COMPROBANTE"].DescuentoMnto = "MONTODESCUENTO3";
            this.configuracionesPorParticipante["COMPROBANTE"].DescuentoPorc = "PORCENTAJEDESCUENTO";
            this.configuracionesPorParticipante["COMPROBANTE"].Total = "TOTAL";

            this.configuracionesPorParticipante.Add( "COMPROBANTE.FACTURADETALLE.ITEM", new ConfiguracionPorParticipante() );
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].AtributoId = "IDITEMARTICULOS";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Cantidad = "CANTIDAD";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Precio = "PRECIO";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].Descuento = "DESCUENTO";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].MontoDescuento = "MONTODESCUENTO";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].FormulaCalculoPrecio = "( <<PRECIO>> * <<CANTIDAD>> ) * ( 1 - <<DESCUENTO>>/100 ) - <<MONTODESCUENTO>>";
            this.configuracionesPorParticipante["COMPROBANTE.FACTURADETALLE.ITEM"].EsConsumible = true;

            this.configuracionesPorParticipante.Add( "COMPROBANTE.VALORESDETALLE.ITEM", new ConfiguracionPorParticipante() );
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].AtributoId = "IDITEMVALORES";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].Cantidad = "CANTIDAD";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].Precio = "MONTO";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].Descuento = "RECARGOPORCENTAJE";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].FormulaCalculoPrecio = "<<PRECIO>>";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].EsConsumible = true;
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].ConsumePorMonto = true;
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].PorcenjateDescuentoRecargo = "PORCENTAJEDESREC";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].TieneCuponIntegrado = "AUTORIZACIONPOS";
            this.configuracionesPorParticipante["COMPROBANTE.VALORESDETALLE.ITEM"].EsRetiroDeEfectivo = "ESRETIROEFECTIVO";
        }

        public string ObtenerAtributoCantidadSegunTipoDePromocion(string participante, string tipoPromo)
        {
            string atributo;
            if (participante == "COMPROBANTE.VALORESDETALLE.ITEM")
            {
                if (tipoPromo == "4")
                {
                    atributo = "CANTIDAD";
                }
                else
                {
                    atributo = this.configuracionesPorParticipante[participante].Cantidad;                    
                }
            }
            else 
            {
                atributo = this.configuracionesPorParticipante[participante].Cantidad;
            }
            return atributo;
        }

        #region Get/Set
        public string AtributoBeneficioPorMonto
        {
            get { return this.atributoBeneficioPorMonto; }
            set { this.atributoBeneficioPorMonto = value; }
        }

        public string NombreComprobante
        {
            get { return this.nombreComprobante; }
            set { this.nombreComprobante = value; }
        }

        public string AtributoMontoFinal
        {
            get { return this.atributoMontoFinal; }
            set { this.atributoMontoFinal = value; }
        }

        public Dictionary<string, ConfiguracionPorParticipante> ConfiguracionesPorParticipante
        {
            get { return this.configuracionesPorParticipante; }
            set { this.configuracionesPorParticipante = value; }
        }

        #endregion    
    }
}