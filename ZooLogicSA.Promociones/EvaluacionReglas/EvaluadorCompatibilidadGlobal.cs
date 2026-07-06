using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class EvaluadorCompatibilidadGlobal
    {
        private ConfiguracionComportamiento configuracionComportamiento;

        public EvaluadorCompatibilidadGlobal( ConfiguracionComportamiento comportamiento )
        {
            this.configuracionComportamiento = comportamiento;
	    }

        public Dictionary<string, List<ItemCompatibilidadCondicionGlobal>> ConseguirCompatibilidadCondicionesGlobales( Promocion promocion, List<ItemUsabilidad> usabilidadLoca, List<ResultadoReglas> resultadoreglas )
        {
            Dictionary<string, List<ItemCompatibilidadCondicionGlobal>> retorno = new Dictionary<string, List<ItemCompatibilidadCondicionGlobal>>();

            List<Beneficio> beneficiosConCondicionGlobal = promocion.Beneficios
                                                           .Where( x => x.Atributo.Equals( this.configuracionComportamiento.AtributoMontoFinal )
                                                           ).ToList();

            foreach ( Beneficio beneficio in beneficiosConCondicionGlobal )
            {
                retorno.Add( beneficio.Id, this.CrearListaDeCompatibilidad( beneficio, usabilidadLoca, resultadoreglas ) );
            }

            return retorno;
        }

        private List<ItemCompatibilidadCondicionGlobal> CrearListaDeCompatibilidad( Beneficio beneficio, List<ItemUsabilidad> usabilidadLoca, List<ResultadoReglas> resultadoreglas )
        {
            List<ItemCompatibilidadCondicionGlobal> retorno = new List<ItemCompatibilidadCondicionGlobal>();

            Dictionary<string, List<IParticipante>> posiblesDestinos = new Dictionary<string, List<IParticipante>>();

            foreach ( DestinoBeneficio destino in beneficio.Destinos )
            {
                List<ItemUsabilidad> usabilidadParticipanteNecesario = usabilidadLoca
                                                .Where( x => x.IdParticipante == destino.Participante && x.RequeridoPorRegla > 0 )
                                                .Distinct()
                                                .ToList();

                List<IParticipante> agregado = resultadoreglas
                                    .SelectMany( resultado => resultado.Participantes )
                                    .Where( x => usabilidadParticipanteNecesario.Select( y => y.Candidato ).Contains( x.Clave + x.Id ) )
                                    .GroupBy( x => x.Clave + x.Id )
                                    .Select( x => x.First() )
                                    .ToList();

                posiblesDestinos.Add( destino.Participante, agregado );
            }

            JoineadorDeParticipantes joineador = new JoineadorDeParticipantes();

            List<Dictionary<string, List<IParticipante>>> combinaciones = joineador.ObtenerCombinacionesConDestinos( posiblesDestinos, beneficio.Destinos );

            List<ItemCompatibilidadCondicionGlobal> listadoCompatibilidad =
                                    ( from x in combinaciones
                                      let resultado = CalcularCompatibilidadConCantidades( x, beneficio )
                                      let cumple = resultado.suma > Convert.ToDecimal( beneficio.Valor, new CultureInfo("en-US") )
                                      select new ItemCompatibilidadCondicionGlobal
                                      {
                                        Participantes = x,
                                        Suma = cumple,
                                        CantidadAConsumirPorParticipante = resultado.cantidadesAConsumir
                                      } ).ToList();

            listadoCompatibilidad.RemoveAll( x => !x.Suma );

            return listadoCompatibilidad;
        }

        private decimal CalcularMejorValorParaParticipante( List<IParticipante> items, decimal limite )
        {
            if ( items == null || items.Count == 0 )
                return 0;

            // Ordenar items por precio unitario descendente (mejores precios primero)
            var itemsOrdenados = items.OrderByDescending( x => x.ObtenerPrecioUnitario() ).ToList();
            
            decimal valorTotal = 0;
            decimal cantidadUsada = 0;

            foreach ( var item in itemsOrdenados )
            {
                if ( cantidadUsada >= limite )
                    break;

                decimal cantidadNecesaria = limite - cantidadUsada;
                decimal cantidadAUsar = Math.Min( item.Cantidad, cantidadNecesaria );
                
                valorTotal += item.ObtenerPrecioUnitario() * cantidadAUsar;
                cantidadUsada += cantidadAUsar;
            }

            return valorTotal;
        }

        private (decimal suma, Dictionary<string, Dictionary<string, decimal>> cantidadesAConsumir) CalcularCompatibilidadConCantidades( Dictionary<string, List<IParticipante>> combinacion, Beneficio beneficio )
        {
            decimal sumaTotal = 0;
            var cantidadesAConsumir = new Dictionary<string, Dictionary<string, decimal>>();

            foreach ( var kvp in combinacion )
            {
                var destino = beneficio.Destinos.FirstOrDefault( d => d.Participante == kvp.Key );
                var limiteParticipante = destino?.Cuantos ?? 0;
                
                var resultadoParticipante = CalcularMejorValorParaParticipanteConCantidades( kvp.Value, limiteParticipante );
                sumaTotal += resultadoParticipante.valor;
                
                if ( resultadoParticipante.cantidadesUsadas.Count > 0 )
                {
                    cantidadesAConsumir[kvp.Key] = resultadoParticipante.cantidadesUsadas;
                }
            }

            return (sumaTotal, cantidadesAConsumir);
        }

        private (decimal valor, Dictionary<string, decimal> cantidadesUsadas) CalcularMejorValorParaParticipanteConCantidades( List<IParticipante> items, decimal limite )
        {
            if ( items == null || items.Count == 0 )
                return (0, new Dictionary<string, decimal>());

            // IMPORTANTE: Distribuir la cantidad entre TODOS los items de la combinación
            var cantidadesUsadas = new Dictionary<string, decimal>();
            decimal valorTotal = 0;
            int cantidadItems = items.Count;
            
            // Calcular cuánto debe usar de cada item para distribuir equitativamente
            decimal cantidadPorItem = limite / cantidadItems;
            decimal cantidadRestante = limite;

            // Ordenar items por precio unitario descendente (mejores precios primero)
            var itemsOrdenados = items.OrderByDescending( x => x.ObtenerPrecioUnitario() ).ToList();

            // Primera pasada: asignar cantidad básica a cada item
            foreach ( var item in itemsOrdenados )
            {
                if ( cantidadRestante <= 0 )
                {
                    cantidadesUsadas[item.Clave + item.Id] = 0;
                    continue;
                }

                decimal cantidadAUsar = Math.Min( item.Cantidad, Math.Min( cantidadPorItem, cantidadRestante ) );
                cantidadesUsadas[item.Clave + item.Id] = cantidadAUsar;
                valorTotal += item.ObtenerPrecioUnitario() * cantidadAUsar;
                cantidadRestante -= cantidadAUsar;
            }

            // Segunda pasada: distribuir cantidad restante entre items que pueden tomar más
            if ( cantidadRestante > 0 )
            {
                foreach ( var item in itemsOrdenados )
                {
                    if ( cantidadRestante <= 0 ) break;

                    decimal cantidadActual = cantidadesUsadas[item.Clave + item.Id];
                    decimal cantidadAdicionalPosible = item.Cantidad - cantidadActual;
                    decimal cantidadAdicional = Math.Min( cantidadAdicionalPosible, cantidadRestante );

                    if ( cantidadAdicional > 0 )
                    {
                        cantidadesUsadas[item.Clave + item.Id] += cantidadAdicional;
                        valorTotal += item.ObtenerPrecioUnitario() * cantidadAdicional;
                        cantidadRestante -= cantidadAdicional;
                    }
                }
            }

            return (valorTotal, cantidadesUsadas);
        }
    }
}
