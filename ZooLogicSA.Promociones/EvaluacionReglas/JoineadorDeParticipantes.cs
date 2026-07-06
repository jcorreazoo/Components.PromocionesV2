using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class CombinacionConCantidades
    {
        public Dictionary<string, List<IParticipante>> Participantes { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> CantidadAConsumirPorParticipante { get; set; }
    }

    public class JoineadorDeParticipantes
    {
        public List<Dictionary<string, List<IParticipante>>> ObtenerCombinaciones(Dictionary<string, List<IParticipante>> posiblesDestinos)
        {
            var keys = posiblesDestinos.Keys.ToList();
            if (keys.Count == 0)
                return new List<Dictionary<string, List<IParticipante>>>();

            var subconjuntosPorKey = keys
                .Select(key => GenerarSubconjuntosNoVacios(posiblesDestinos[key]))
                .ToList();

            var resultado = new List<Dictionary<string, List<IParticipante>>>();
            CombinarRecursivo(keys, subconjuntosPorKey, 0, new Dictionary<string, List<IParticipante>>(), resultado);
            return resultado;
        }

        public List<Dictionary<string, List<IParticipante>>> ObtenerCombinacionesConDestinos(Dictionary<string, List<IParticipante>> posiblesDestinos, List<DestinoBeneficio> destinos)
        {
            var keys = posiblesDestinos.Keys.ToList();
            if (keys.Count == 0)
                return new List<Dictionary<string, List<IParticipante>>>();

            // Crear diccionario de límites por participante
            var limitesPorParticipante = destinos.ToDictionary(d => d.Participante, d => d.Cuantos);

            var resultado = new List<Dictionary<string, List<IParticipante>>>();
            CombinarRecursivoConLimites(keys, posiblesDestinos, limitesPorParticipante, 0, new Dictionary<string, List<IParticipante>>(), resultado);
            return resultado;
        }

        public List<CombinacionConCantidades> ObtenerCombinacionesConDestinosConCantidades(Dictionary<string, List<IParticipante>> posiblesDestinos, List<DestinoBeneficio> destinos)
        {
            var keys = posiblesDestinos.Keys.ToList();
            if (keys.Count == 0)
                return new List<CombinacionConCantidades>();

            // Crear diccionario de límites por participante
            var limitesPorParticipante = destinos.ToDictionary(d => d.Participante, d => d.Cuantos);

            var combinacionesBasicas = new List<Dictionary<string, List<IParticipante>>>();
            CombinarRecursivoConLimites(keys, posiblesDestinos, limitesPorParticipante, 0, new Dictionary<string, List<IParticipante>>(), combinacionesBasicas);

            // Convertir a combinaciones con cantidades
            var resultado = new List<CombinacionConCantidades>();
            foreach (var combinacion in combinacionesBasicas)
            {
                var cantidades = CalcularCantidadesOptimas(combinacion, destinos);
                resultado.Add(new CombinacionConCantidades
                {
                    Participantes = combinacion,
                    CantidadAConsumirPorParticipante = cantidades
                });
            }

            return resultado;
        }

        private List<List<IParticipante>> GenerarSubconjuntosNoVacios(List<IParticipante> participantes)
        {
            var subconjuntos = new List<List<IParticipante>>();
            int n = participantes.Count;
            for (int i = 1; i < (1 << n); i++)
            {
                var subconjunto = new List<IParticipante>();
                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                        subconjunto.Add(participantes[j]);
                }
                subconjuntos.Add(subconjunto);
            }
            return subconjuntos;
        }

        private void CombinarRecursivoConLimites(List<string> keys, Dictionary<string, List<IParticipante>> posiblesDestinos, Dictionary<string, decimal> limitesPorParticipante, int nivel, Dictionary<string, List<IParticipante>> actual, List<Dictionary<string, List<IParticipante>>> resultado)
        {
            if (nivel == keys.Count)
            {
                if (actual.All(kvp => kvp.Value.Count > 0))
                    resultado.Add(new Dictionary<string, List<IParticipante>>(actual));
                return;
            }

            var key = keys[nivel];
            var participantesDisponibles = posiblesDestinos[key];
            var limite = limitesPorParticipante.ContainsKey(key) ? limitesPorParticipante[key] : decimal.MaxValue;

            // Generar combinaciones válidas solo para este participante
            var combinacionesValidas = GenerarCombinacionesValidasParaParticipante(participantesDisponibles, limite);

            foreach (var combinacion in combinacionesValidas)
            {
                actual[key] = combinacion;
                CombinarRecursivoConLimites(keys, posiblesDestinos, limitesPorParticipante, nivel + 1, actual, resultado);
                actual.Remove(key);
            }
        }

        private List<List<IParticipante>> GenerarCombinacionesValidasParaParticipante(List<IParticipante> participantes, decimal limite)
        {
            var combinacionesValidas = new List<List<IParticipante>>();
            var n = participantes.Count;

            // Generar todas las combinaciones posibles de items
            for (int i = 1; i < (1 << n); i++)
            {
                var combinacion = new List<IParticipante>();

                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        combinacion.Add(participantes[j]);
                    }
                }

                // Calcular la cantidad de items únicos en esta combinación
                decimal cantidadItemsUnicos = combinacion.Count;
                
                // Solo incluir combinaciones que no excedan el límite en cantidad de items únicos
                if (cantidadItemsUnicos <= limite && PuedeSatisfacerLimite(combinacion, limite))
                {
                    combinacionesValidas.Add(combinacion);
                }
            }

            return combinacionesValidas;
        }

        private bool PuedeSatisfacerLimite(List<IParticipante> items, decimal limite)
        {
            // Verificar si podemos tomar cantidades de los items para satisfacer exactamente el límite
            return PuedeSatisfacerLimiteRecursivo(items, 0, 0, limite);
        }

        private bool PuedeSatisfacerLimiteRecursivo(List<IParticipante> items, int indiceItem, decimal cantidadAcumulada, decimal limite)
        {
            // Si ya alcanzamos el límite exacto, es válido
            if (cantidadAcumulada == limite)
                return true;

            // Si excedimos el límite o no quedan más items, no es válido
            if (cantidadAcumulada > limite || indiceItem >= items.Count)
                return false;

            var itemActual = items[indiceItem];

            // Probar diferentes cantidades del item actual (desde 0 hasta su cantidad máxima)
            for (decimal cantidadAUsar = 0; cantidadAUsar <= itemActual.Cantidad; cantidadAUsar++)
            {
                decimal nuevaAcumulada = cantidadAcumulada + cantidadAUsar;

                // Si con esta cantidad no excedemos el límite, seguir explorando
                if (nuevaAcumulada <= limite)
                {
                    if (PuedeSatisfacerLimiteRecursivo(items, indiceItem + 1, nuevaAcumulada, limite))
                        return true;
                }
            }

            return false;
        }

        private void CombinarRecursivo(List<string> keys, List<List<List<IParticipante>>> subconjuntosPorKey, int nivel, Dictionary<string, List<IParticipante>> actual, List<Dictionary<string, List<IParticipante>>> resultado)
        {
            if (nivel == keys.Count)
            {
                // Controlar que ningún participante se repita más veces que su atributo cantidad
                var todos = actual.Values.SelectMany(x => x).ToList();
                var agrupados = todos.GroupBy(p => p.Clave + ":" + p.Id);
                foreach (var grupo in agrupados)
                {
                    var participante = grupo.First();
                    if (grupo.Count() > participante.Cantidad)
                        return; // No agregar esta combinación
                }
                if (actual.All(kvp => kvp.Value.Count > 0))
                    resultado.Add(new Dictionary<string, List<IParticipante>>(actual));
                return;
            }
            var key = keys[nivel];
            foreach (var subconjunto in subconjuntosPorKey[nivel])
            {
                actual[key] = subconjunto;
                CombinarRecursivo(keys, subconjuntosPorKey, nivel + 1, actual, resultado);
                actual.Remove(key);
            }
        }

        private Dictionary<string, Dictionary<string, decimal>> CalcularCantidadesOptimas(Dictionary<string, List<IParticipante>> combinacion, List<DestinoBeneficio> destinos)
        {
            var resultado = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var destino in destinos)
            {
                if (combinacion.ContainsKey(destino.Participante))
                {
                    var cantidadPorItem = new Dictionary<string, decimal>();
                    var limite = destino.Cuantos;
                    decimal cantidadRestante = limite;

                    // Ordenar items por precio descendente (mejores primero)
                    var itemsOrdenados = combinacion[destino.Participante]
                        .OrderByDescending(x =>
                        {
                            try { return x.ObtenerPrecioUnitario(); } catch { return 0; }
                        })
                        .ToList();

                    // IMPORTANTE: Incluir TODOS los items que están en la combinación
                    foreach (var item in itemsOrdenados)
                    {
                        if (cantidadRestante > 0)
                        {
                            decimal cantidadAUsar = Math.Min(item.Cantidad, cantidadRestante);
                            cantidadPorItem[item.Clave + item.Id] = cantidadAUsar;
                            cantidadRestante -= cantidadAUsar;
                        }
                        else
                        {
                            // Aunque no se necesite más cantidad, el item debe aparecer con cantidad 0
                            cantidadPorItem[item.Clave + item.Id] = 0;
                        }
                    }

                    resultado[destino.Participante] = cantidadPorItem;
                }
            }

            return resultado;
        }
    }
}