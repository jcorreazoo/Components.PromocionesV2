using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public enum Alteracion
    {
        Ninguna,
        CambiarValor,
        IncrementarEnCantidad,
        DisminuirEnCantidad,
        IncrementarEnPorcentaje,
        DisminuirEnPorcentaje
        //DisminuirEnPorcentajeCompensadoConParticipanteAuxiliar
    }
}