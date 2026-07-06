using System;
using System.Collections.Generic;
using System.Linq;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Comprobante;

namespace ZooLogicSA.Promociones
{
    public class CalculadorDeIncumplimiento
    {
        private ConfiguracionComportamiento configuracionComportamiento;
        private IExtractorDeCoincidenciasDesdeConsumo extractorCoincidencias;
        private ICalculadorDePartipantesCumplidos calculadorCumplidos;
        private ICalculadorDeParticipantesFaltantes calculadorFaltantes;
        private ISimuladorDeResultadoReglas simuladorDeResultadoReglas;
        private ICalculadorDeCombinacionesDeParticipantes calculadorCombinaciones;

        public CalculadorDeIncumplimiento( ConfiguracionComportamiento configuracionComportamiento, IExtractorDeCoincidenciasDesdeConsumo extractorCoincidencias, ICalculadorDePartipantesCumplidos calculadorCumplidos, ICalculadorDeParticipantesFaltantes calculadorFaltantes, ISimuladorDeResultadoReglas simuladorDeResultadoReglas, ICalculadorDeCombinacionesDeParticipantes calculadorCombinaciones )
        {
            this.configuracionComportamiento = configuracionComportamiento;
            this.extractorCoincidencias = extractorCoincidencias;
            this.calculadorCumplidos = calculadorCumplidos;
            this.calculadorFaltantes = calculadorFaltantes;
            this.simuladorDeResultadoReglas = simuladorDeResultadoReglas;
            this.calculadorCombinaciones = calculadorCombinaciones;
        }

        public InformacionPromocionIncumplida Calcular( Promocion promocion, List<ResultadoReglas> resultadoreglas, List<ConsumoParticipanteEvaluado> consumo, IArmadorDeCoincidencias armadorDeCoincidencias, IComprobante comprobante = null )
        {
            InformacionPromocionIncumplida retorno = new InformacionPromocionIncumplida();

            retorno.IdPromocion = promocion.Id;
            retorno.Promocion = promocion;
            retorno.Comprobante = comprobante;

            // para mantener la funcionalidad hasta que la parte visual se termine
            List<ResultadoReglas> resultadosFiltrados = (from x in resultadoreglas
                                                         where x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].Cantidad
                                                         || x.Regla.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[x.PartPromo.Codigo].CantidadMonto
                                                         select x).ToList();

            retorno.Resultados = resultadosFiltrados;

            retorno.TotalFaltante = consumo.Sum( x => x.Requerido ) - consumo.Sum( x => x.Satisfecho );

            retorno.Cumplidos = this.calculadorCumplidos.Obtener( resultadoreglas );

            retorno.FaltanteSeguro = this.calculadorFaltantes.Obtener( resultadoreglas );

            // Para PorArticulo y PorArticuloYCombinacion el déficit global (total qty vs requerido)
            // no refleja la restricción por artículo/combinación. consumo.Satisfecho ya incorpora
            // esa restricción (pre-scan de ObtenerCoincidencias), así que lo usamos en su lugar.
            if ( promocion.AplicacionProductosIguales != ZooLogicSA.Promociones.FormatoPromociones.AplicacionProductosIgualesType.NoAplicar )
            {
                List<ParticipanteFaltante> faltanteDesdeConsumo = (
                    from c in consumo
                    where c.Requerido > c.Satisfecho
                    let partPromo = resultadosFiltrados.FirstOrDefault( r => r.PartPromo.Id == c.IdParticipanteEnRegla )
                    where partPromo != null
                    select new ParticipanteFaltante()
                    {
                        Participante = partPromo.PartPromo,
                        Cantidad = c.Requerido - c.Satisfecho,
                        Requerido = c.Requerido
                    }
                ).ToList();
                if ( faltanteDesdeConsumo.Count > 0 )
                    retorno.FaltanteSeguro = faltanteDesdeConsumo;

                // SatisfechoEfectivo: indica cuánto se satisfizo respetando la restricción
                // de artículo/combinación; determina el color del asistente (gris vs amarillo).
                // Se excluye el participante COMPROBANTE (igual que la lógica legacy en ObtenerEstado).
                string nombreComprobante = this.configuracionComportamiento.NombreComprobante;
                retorno.SatisfechoEfectivo = consumo.Where( c => {
                    ResultadoReglas r = resultadosFiltrados.FirstOrDefault( f => f.PartPromo.Id == c.IdParticipanteEnRegla );
                    return r == null || !r.PartPromo.Codigo.Equals( nombreComprobante );
                } ).Sum( x => x.Satisfecho );
            }

            List<ParticipanteFaltante> evaluados = new List<ParticipanteFaltante>();
            evaluados.AddRange( retorno.Cumplidos );
            evaluados.AddRange( retorno.FaltanteSeguro );

            evaluados = evaluados.GroupBy( x=>x.Participante ).Select( x => new ParticipanteFaltante() { Participante = x.First().Participante, Cantidad = x.Sum( y=>y.Cantidad), Requerido = x.First().Requerido } ).ToList();

                //(from x in retorno.Cumplidos
                //                                    from y in retorno.FaltanteSeguro
                //                                    group 
                //            select new ParticipanteFaltante() { Participante = x.Participante, Cantidad = x.Cantidad + y.Cantidad, Requerido = x.Requerido }).ToList();

            List<ParticipanteRegla> participantesACombinar = (from participante in promocion.Participantes
                                                              where
                                                                    !evaluados.Exists( evaluado => participante == evaluado.Participante && evaluado.Cantidad == evaluado.Requerido )
                                                              select participante
                                                                ).ToList();

            
            //CalculadorDeCombinacionesDeParticipantes calculadorCombinaciones = new CalculadorDeCombinacionesDeParticipantes();
            List<CombinacionParticipanteFaltantes> combinaciones = this.calculadorCombinaciones.ObtenerCombinaciones( participantesACombinar, ( retorno.TotalFaltante - retorno.FaltanteSeguro.Count ) );

            retorno.FaltantePosibles = this.CalcularCombinacionesFaltantesExitosas( promocion, resultadoreglas, armadorDeCoincidencias, combinaciones, retorno.FaltanteSeguro );

            return retorno;
        }

        private List<CombinacionParticipanteFaltantes> CalcularCombinacionesFaltantesExitosas( Promocion promocion, List<ResultadoReglas> resultadoreglas, IArmadorDeCoincidencias armadorDeCoincidencias, List<CombinacionParticipanteFaltantes> combinaciones, List<ParticipanteFaltante> imposibles )
        {
            List<CombinacionParticipanteFaltantes> exitos = new List<CombinacionParticipanteFaltantes>();

            Dictionary<string, List<CombinacionParticipanteFaltantes>> gruposDeCombinaciones = combinaciones
                                                                            .GroupBy( x => String.Join( ".",
                                                                                            x.Faltantes
                                                                                                .OrderBy( y => y.Participante.Codigo + y.Participante.Id + y.Cantidad )
                                                                                                .Select( y => y.Participante.Codigo + y.Participante.Id + y.Cantidad )
                                                                                                .ToArray() )
                                                                                    ).ToDictionary( x => x.Key, x => x.ToList() );

            foreach ( List<CombinacionParticipanteFaltantes> combinacionesAProbar in gruposDeCombinaciones.Values )
            {
                CombinacionParticipanteFaltantes combinacion = combinacionesAProbar.First();

                this.simuladorDeResultadoReglas.AgregarDummies( resultadoreglas, imposibles, combinacion );

                List<ConsumoParticipanteEvaluado> consumo = armadorDeCoincidencias.ObtenerCoincidencias( promocion, resultadoreglas );

                List<CoincidenciaEvaluacion> coincidencias = this.extractorCoincidencias.Obtener( consumo );
                
                if ( coincidencias.Count() == consumo.Count() )
                {
                    exitos.AddRange( combinacionesAProbar );
                }

                this.simuladorDeResultadoReglas.QuitarDummies( resultadoreglas, imposibles, combinacion );
            }

            return exitos;
        }

    }
}
