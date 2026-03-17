using System.Collections.Generic;
using System.Linq;

namespace ZooLogicSA.Promociones
{
    public class CalculadorDeParticipantesFaltantes : ICalculadorDeParticipantesFaltantes
    {
        private ConfiguracionComportamiento configuracionComportamiento;

        public CalculadorDeParticipantesFaltantes( ConfiguracionComportamiento configuracionComportamiento )
        {
            this.configuracionComportamiento = configuracionComportamiento;
        }

        public List<ParticipanteFaltante> Obtener( List<ResultadoReglas> resultadoreglas )
        {
            List<ParticipanteFaltante> retorno;
            
            retorno = (from x in resultadoreglas
                                where x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
                                    &&
                                    !x.Cumple
                                select new ParticipanteFaltante() { Participante = x.PartPromo, Cantidad = x.Requerido - x.Satisfecho, Requerido = x.Requerido }
                                                     ).ToList();

            return retorno;
        }
    }
}
