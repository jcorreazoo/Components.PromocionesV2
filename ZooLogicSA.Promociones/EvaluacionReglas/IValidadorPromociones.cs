using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public interface IValidadorPromociones
    {
        List<ResultadoReglas> ComprobarReglas( IComprobante comprobante, Promocion promocion, bool tlEvaluarValoresConCuponesIntegrados );
        void EstablecerLibreriaPromociones( List<Promocion> promociones );
        List<ErrorEvaluador> ObtenerExcepciones();
    }
}
