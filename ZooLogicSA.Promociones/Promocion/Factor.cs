using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public enum Factor
    {
        None = -1,
        DebeSerIgualA = 0,
        DebeSerMayorA = 1,
        DebeSerMenorA = 2,
        DebeSerIgualADiaDeLaSemana = 3, 
        DebeSerDistintoA = 4,
        DebeSerMayorIgualA = 5,
        DebeSerMenorIgualA = 6,
        DebeSerContieneA = 7,
        DebeSerComienzaCon = 8,
        DebeSerTerminaCon = 9
    }
}