using System;
using System.Collections.Generic;
using System.Linq;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class CalculadorDePartipantesCumplidos : ICalculadorDePartipantesCumplidos
    {
        private ConfiguracionComportamiento configuracionComportamiento;

        public CalculadorDePartipantesCumplidos( ConfiguracionComportamiento configuracionComportamiento )
        {
            this.configuracionComportamiento = configuracionComportamiento;
        }

        public List<ParticipanteFaltante> Obtener( List<ResultadoReglas> resultadoreglas )
        {
            List<ParticipanteFaltante> retorno = new List<ParticipanteFaltante>();

            List<IParticipante> itemsExclusivos = resultadoreglas
                            .Where( x => x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad )
                            .SelectMany( x => x.Participantes )
                            .GroupBy( x => x.Clave + "_" + x.Id )
                            .Where( x => x.Count() == 1 )
                            .Select( x => x.First() )
                            .ToList();


            retorno = (from x in resultadoreglas
                           where x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
                           select new ParticipanteFaltante()
                           {
                               Participante = x.PartPromo,
                               Cantidad = Math.Min( x.Participantes.Where( y => itemsExclusivos.Exists( z => z == y ) ).Sum( y => y.Cantidad ), x.Requerido ),
                               Requerido = x.Requerido
                           }
                       ).ToList();

            retorno = (from x in retorno where x.Cantidad > 0 select x).ToList();

            return retorno;
        }
    }
}
