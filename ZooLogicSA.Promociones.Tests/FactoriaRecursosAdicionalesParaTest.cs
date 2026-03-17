using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Tests
{
    public static class FactoriaRecursosAdicionalesParaTest
    {
        internal static ConfiguracionComportamiento ObtenerConfiguracionComportamiento()
        {
            ConfiguracionComportamiento config = new ConfiguracionComportamiento();

            config.NombreComprobante = "Comprobante";
            config.AtributoMontoFinal = "MONTOFINAL";
            config.AtributoBeneficioPorMonto = "Monto";

            config.ConfiguracionesPorParticipante = new Dictionary<string, ConfiguracionPorParticipante>();

            config.ConfiguracionesPorParticipante.Add( "Comprobante", new ConfiguracionPorParticipante() );
            config.ConfiguracionesPorParticipante["Comprobante"].AtributoId = "Codigo";
            config.ConfiguracionesPorParticipante["Comprobante"].Cantidad = "Cantidad";
            config.ConfiguracionesPorParticipante["Comprobante"].EsConsumible = false;
            config.ConfiguracionesPorParticipante["Comprobante"].FormulaCalculoPrecio = "0";

            config.ConfiguracionesPorParticipante.Add( "Comprobante.Facturadetalle.Item", new ConfiguracionPorParticipante() );
            config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].AtributoId = "IdItemArticulos";
			config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Cantidad = "Cantidad";
			config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].CantidadMonto = "Monto";
			config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Precio = "Precio";
            config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Descuento = "Descuento";
            config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].MontoDescuento = "MontoDescuento";
            config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].FormulaCalculoPrecio = "( <<PRECIO>> * <<CANTIDAD>> ) * ( 1 - <<DESCUENTO>>/100 ) - <<MONTODESCUENTO>>";
            config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].EsConsumible = true;

            config.ConfiguracionesPorParticipante.Add( "Comprobante.Valoresdetalle.Item", new ConfiguracionPorParticipante() );
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].AtributoId = "IdItemValores";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].Cantidad = "Cantidad";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].Precio = "Monto";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].Descuento = "RecargoPorcentaje";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].FormulaCalculoPrecio = "<<PRECIO>>";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].Total = "Total";
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].EsConsumible = true;
            config.ConfiguracionesPorParticipante["Comprobante.Valoresdetalle.Item"].ConsumePorMonto = true;

            return config;
        }
    }
}