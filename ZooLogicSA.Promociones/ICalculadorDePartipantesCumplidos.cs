using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
namespace ZooLogicSA.Promociones
{
    public interface ICalculadorDePartipantesCumplidos
    {
        List<ParticipanteFaltante> Obtener( List<ResultadoReglas> resultadoreglas );
    }
}
