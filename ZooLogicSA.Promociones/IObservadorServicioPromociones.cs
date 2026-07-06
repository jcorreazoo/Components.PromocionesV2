using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones
{
    public interface IObservadorServicioPromociones
    {
        void PresentarPromocionesAplicables( List<InformacionPromocion> info );
        void ProcesarError( Exception ex );
        void ProcesarErrorEnPromocion( InformacionPromocionIncumplida idPromocion, Exception ex );
        void InformarDebug( string mensaje );
        void InformarApagado();
	}
}
