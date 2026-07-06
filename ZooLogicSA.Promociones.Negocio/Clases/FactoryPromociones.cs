using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Negocio.Clases
{
    public class FactoryPromociones
    {
        private static List<TipoPromocion> tiposPromocion = new List<TipoPromocion>();

        public static List<TipoPromocion> LlenarListaTipoPromociones() 
        {
            tiposPromocion.Clear();
            tiposPromocion.Add( new Clases.Promociones.LLevaXpagaY() );
            tiposPromocion.Add( new Clases.Promociones.LlevaXtienedescuentoY() );
            tiposPromocion.Add( new Clases.Promociones.DescuentoXcaracteristica() );
            tiposPromocion.Add( new Clases.Promociones.RebajaXcaracteristica() );
			tiposPromocion.Add( new Clases.Promociones.DescuentoBancarioConTope() );
			tiposPromocion.Add( new Clases.Promociones.MontoAplicaDescuento() );
			tiposPromocion.Add( new Clases.Promociones.MontoAplicaMontoFijo() );
            tiposPromocion.Add( new Clases.Promociones.LlevaAValorDeOtraListaDePrecios());
            return tiposPromocion;
        }

    }
}
