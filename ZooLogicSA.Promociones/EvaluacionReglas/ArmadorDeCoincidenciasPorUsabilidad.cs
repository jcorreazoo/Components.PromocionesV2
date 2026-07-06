using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class ArmadorDeCoincidenciasPorUsabilidad : IArmadorDeCoincidencias
    {
        private ConfiguracionComportamiento configuracionComportamiento;
        private Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes> seleccionadoresParticipantes;
		private CalculadorDeUsabilidadSegunTipoCantidad calculadorDeUsabilidadSegunTipoCantidad;

        public ArmadorDeCoincidenciasPorUsabilidad( ConfiguracionComportamiento comportamiento, Dictionary<EleccionParticipanteType,ISeleccionadorParticipantes> seleccionadores )
        {
            this.configuracionComportamiento = comportamiento;
            this.seleccionadoresParticipantes = seleccionadores;

			this.calculadorDeUsabilidadSegunTipoCantidad = new CalculadorDeUsabilidadSegunTipoCantidad( this.configuracionComportamiento );

        }

		private object locker = new object();

        #region IArmadorDeCoincidencias Members

        public List<ConsumoParticipanteEvaluado> ObtenerCoincidencias( Promocion promocion, List<ResultadoReglas> resultadoreglas )
        {
            List<ConsumoParticipanteEvaluado> retorno = new List<ConsumoParticipanteEvaluado>();

            List<ParticipanteRegla> participantesEnPromo = promocion.Participantes;

            List<ResultadoReglas> resultadosCantidad;
            List<ItemUsabilidad> usabilidadLoca = this.calculadorDeUsabilidadSegunTipoCantidad.ObtenerUsabilidad( resultadoreglas, out resultadosCantidad );

            var ordenNivelReglas = from x in usabilidadLoca
                                   group x by x.IdParticipante + x.IdRegla into g
								   select new { parti = g.Select( d => d.IdParticipante ).First(), regla = g.Select( d => d.IdRegla ).First(), dific = g.Sum( i => i.Dificultad ) };

            var ordenNivelParticipante = from x in ordenNivelReglas
                                         group x by x.parti into g
                                         select new { participante = g.Select( d => d.parti ).First(), dific = g.Min( i => i.dific ) };


            bool consumePorMonto = participantesEnPromo.Exists( p => p.Reglas.Exists(r => r.Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[p.Codigo].CantidadMonto ));
            if ( consumePorMonto )
            {
                ordenNivelReglas = ordenNivelReglas.OrderBy( x => x.dific );
                ordenNivelParticipante = ordenNivelParticipante.OrderBy( x => x.dific );
            }
            else
            {
                ordenNivelReglas = ordenNivelReglas.OrderByDescending(x => x.dific);
                ordenNivelParticipante = ordenNivelParticipante.OrderByDescending(x => x.dific);
            }

            #region debug

            //lock (locker)
            //{
            //    string archivo = @"c:\temp\resultadoreglas.txt";

            //    File.WriteAllText(archivo, "=========================\r\n");

            //    File.AppendAllText(archivo, "= resultadoreglas =========================\r\n");
            //    resultadoreglas.ForEach(x => File.AppendAllText(archivo, x.PartPromo.Id + "__" + x.Regla.Id + "\t" + x.Cumple + "\t(" + x.Satisfecho + "/" + x.Requerido + ")\t " + x.Regla.Atributo.PadLeft(50) + "\t" + String.Join("*", x.Participantes.Select(y => y.Clave + y.Id).ToArray()) + "\r\n"));

            //    File.AppendAllText(archivo, "= USABILIDAD =========================\r\n");
            //    //.ToList().ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.PartPromo.Id + "\t" + x.Regla.Id + "\t" + x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\r\n" + String.Join( "\r\n", x.Participantes.Select( s => s.Clave + s.Id + "\t" + s.Valor.ToString() ).ToArray() ) + "\r\n(" + x.Regla.Atributo + ")\r\n" ) );
            //    File.AppendAllText(archivo, "Parti\tregla\tcandid\tcantidad\tprecio\trequerido\taporte\tdificultad\r\n");
            //    usabilidadLoca.ForEach(x => File.AppendAllText(archivo, x.IdParticipante + "\t" + x.IdRegla + "\t" + x.Candidato + "\t" + x.CantidadEnItem + "\t" + x.Precio + "\t" + x.RequeridoPorRegla + "\t" + x.AporteRelativoDelCandidato.ToString().PadLeft(20) + "\t" + x.Dificultad.ToString().PadLeft(20) + "\r\n"));

            //    File.AppendAllText(archivo, "= DIFIC  ordenNivelReglas=========================\r\n");
            //    //.ToList().ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.PartPromo.Id + "\t" + x.Regla.Id + "\t" + x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\r\n" + String.Join( "\r\n", x.Participantes.Select( s => s.Clave + s.Id + "\t" + s.Valor.ToString() ).ToArray() ) + "\r\n(" + x.Regla.Atributo + ")\r\n" ) );
            //    ordenNivelReglas.ToList().ForEach(x => File.AppendAllText(archivo, x.parti + "\t" + x.regla + "\t" + x.dific + "\r\n"));

            //    File.AppendAllText(archivo, "= DIFIC ordenNivelParticipante=========================\r\n");
            //    //.ToList().ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.PartPromo.Id + "\t" + x.Regla.Id + "\t" + x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\r\n" + String.Join( "\r\n", x.Participantes.Select( s => s.Clave + s.Id + "\t" + s.Valor.ToString() ).ToArray() ) + "\r\n(" + x.Regla.Atributo + ")\r\n" ) );
            //    ordenNivelParticipante.ToList().ForEach(x => File.AppendAllText(archivo, x.participante + "\t" + x.dific + "\r\n"));
            //}

            #endregion

            EvaluadorCompatibilidadGlobal evaluadorCompatibilidadGlobal = new EvaluadorCompatibilidadGlobal( configuracionComportamiento );
            Dictionary<string, List<ItemCompatibilidadCondicionGlobal>> compatibilidadCondicionGlobal = evaluadorCompatibilidadGlobal.ConseguirCompatibilidadCondicionesGlobales( promocion, usabilidadLoca, resultadoreglas );

            ISeleccionadorParticipantes seleccionador;
            if (!this.seleccionadoresParticipantes.TryGetValue(promocion.EleccionParticipante, out seleccionador))
            {
                seleccionador = this.seleccionadoresParticipantes[0];
            }

            // arranca por el mas dificil de cumplir
            foreach ( var item in ordenNivelParticipante )
            {
                var resultadoCantidadDelParticipante = from x in resultadosCantidad
                                                       where x.PartPromo.Id == item.participante
                                                       select x;

                var reglas = from x in ordenNivelReglas
                             where x.parti == item.participante
                             select x;


                // aca me fijo si hay beneficio que le pega al participante
                //var beneficio = compatibilidadCondicionGlobal.Where( x => x.Value.Any( y => y.Participantes.Keys.Any( o => o.Equals( item.participante ) ) ) ).FirstOrDefault();
                Beneficio beneficioAsociadoAlParticipante = promocion.Beneficios.Where( x => x.Destinos.Any( y => y.Participante.Equals( item.participante ) ) ).FirstOrDefault();
 
                // empieza por la regla de cantidad mas facil
                foreach ( var itemRegla in reglas )
                {
                    if ( retorno.Select( x => x.IdParticipanteEnRegla ).Contains( item.participante ) )
                    {
                        continue;
                    }

                    ResultadoReglas candidato = resultadoreglas.Find( x => x.PartPromo.Id == item.participante && x.Regla.Id == itemRegla.regla );

                    #region debug
                    //File.AppendAllText( @"c:\resultadoreglas.txt", "\r\n===Procesar del Participante " + itemRegla.parti + " la regla " + itemRegla.regla + " (dificultad: " + itemRegla.dific + " )=============================\r\n" );
                    #endregion

                    ParticipanteRegla participante = participantesEnPromo.Find( x => x.Id == candidato.PartPromo.Id );

                    List<ItemUsabilidad> usabilidadEnParticipanteYRegla = seleccionador.ObtenerUsabilidadPorParticipanteYRegla( usabilidadLoca, itemRegla.parti, itemRegla.regla, candidato );

                    #region debug
                    //File.AppendAllText( @"c:\resultadoreglas.txt", reemplazo + " es correcto, sacar algun item de " + itemRegla.parti + "_" + itemRegla.regla + "\r\n" );
                    //lock ( locker )
                    //{
                    //	string archivo = @"d:\resultadoreglas.txt";
                    //	File.AppendAllText( archivo, "==orden por usabilidad ===== \r\n" );
                    //	File.AppendAllText( archivo, String.Join( "\r\n", usabilidadEnParticipanteYRegla.Select( x => x.Candidato + "(dif: " + x.Dificultad + ")" + "(precio: " + x.Precio.ToString() + ")" ).ToArray() ) + "\r\n" );
                    //}
                    #endregion

                    Decimal requerimientoDeRegla = candidato.Requerido;
                    Decimal nuevaCantidad = 0;
                    List<string> colaboradores = new List<string>();
                    List<string> restantes = new List<string>();

                    // PorArticulo: cada aplicación solo puede consumir de un único artículo
                    // (código de artículo, agrupando todas las filas del comprobante con ese código).
                    // Pre-scan: elige el código de artículo cuya suma de cantidades disponibles sea
                    // la más alta que alcance el requerimiento.
                    // Clave de agrupación: AtributoCodigoArticulo cuando está configurado;
                    //   en caso contrario usa AtributoId (comportamiento previo, fila a fila).
                    bool esPorArticulo = promocion.AplicacionProductosIguales == AplicacionProductosIgualesType.PorArticulo;
                    string atributoIdParticipante = this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].AtributoId;
                    string atributoCodigoArticuloPorArt = this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].AtributoCodigoArticulo;
                    // Usar código de artículo si está configurado; si no, usar AtributoId (fila-a-fila).
                    string atributoAgrupacionPorArticulo = !string.IsNullOrEmpty(atributoCodigoArticuloPorArt)
                        ? atributoCodigoArticuloPorArt : atributoIdParticipante;
                    string valorArticuloFijado = null;
                    if (esPorArticulo && !string.IsNullOrEmpty(atributoAgrupacionPorArticulo))
                    {
                        Dictionary<string, decimal> qtdPorArticulo = new Dictionary<string, decimal>();
                        decimal dummyQtd = 0m;
                        foreach (ItemUsabilidad u in usabilidadEnParticipanteYRegla)
                        {
                            string idIt = u.Candidato.Replace(candidato.Participantes[0].Clave, "");
                            IParticipante p = candidato.Participantes.FirstOrDefault(pp => pp.Id == idIt);
                            if (p == null) continue;
                            // Los dummies (simulación de faltante) no tienen artículo conocido;
                            // su cantidad se suma a cada artículo real al evaluar el umbral.
                            if (p is ParticipanteDummy) { dummyQtd += u.CantidadEnItem; continue; }
                            IAtributo atrib = p.ObtenerAtributo(atributoAgrupacionPorArticulo);
                            if (atrib == null || atrib.Valor == null) continue;
                            string key = atrib.Valor.ToString();
                            if (!qtdPorArticulo.ContainsKey(key)) qtdPorArticulo[key] = 0m;
                            qtdPorArticulo[key] += u.CantidadEnItem;
                        }
                        decimal maxQtd = -1m;
                        string bestFallbackArticulo = null;
                        decimal bestFallbackQtdArticulo = -1m;
                        foreach (var kv in qtdPorArticulo)
                        {
                            // Rastrear el mejor grupo real (sin dummies) para el fallback.
                            if (kv.Value > bestFallbackQtdArticulo) { bestFallbackArticulo = kv.Key; bestFallbackQtdArticulo = kv.Value; }
                            decimal effective = kv.Value + dummyQtd;
                            if (effective >= candidato.Requerido && effective > maxQtd)
                            {
                                valorArticuloFijado = kv.Key;
                                maxQtd = effective;
                            }
                        }
                        if (valorArticuloFijado == null)
                        {
                            // Ningún grupo (con o sin dummies) alcanza el umbral.
                            // Usar el mejor grupo real para Satisfecho parcial correcto:
                            // TotalFaltante = requerido - bestGroupQty en lugar de requerido - 0.
                            valorArticuloFijado = bestFallbackArticulo ?? string.Empty;
                        }
                    }

                    // PorArticuloYCombinacion: cada aplicación solo puede consumir ítems de la
                    // misma fila del comprobante (mismo Id de fila).
                    // Ítems donde TODOS los atributos de combinación están vacíos son inválidos.
                    // La clave es el Id de fila (valor de AtributoId), que es equivalente a la
                    // combinación artículo+color+talle ya que cada fila es una combinación única.
                    bool esPorArticuloYCombinacion = promocion.AplicacionProductosIguales == AplicacionProductosIgualesType.PorArticuloYCombinacion;
                    string[] atributosCombinacion = null;
                    {
                        string configCombinacion = this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].Combinacion;
                        if (!string.IsNullOrEmpty(configCombinacion))
                            atributosCombinacion = configCombinacion.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    string valorCombinacionFijada = null;
                    if (esPorArticuloYCombinacion)
                    {
                        Dictionary<string, decimal> qtdPorCombinacion = new Dictionary<string, decimal>();
                        decimal dummyQtd = 0m;
                        foreach (ItemUsabilidad u in usabilidadEnParticipanteYRegla)
                        {
                            string idIt = u.Candidato.Replace(candidato.Participantes[0].Clave, "");
                            IParticipante p = candidato.Participantes.FirstOrDefault(pp => pp.Id == idIt);
                            if (p == null) continue;
                            // Los dummies (simulación de faltante) no tienen combinación conocida;
                            // su cantidad se suma a la mejor combinación real al evaluar el umbral.
                            if (p is ParticipanteDummy) { dummyQtd += u.CantidadEnItem; continue; }
                            // Excluir ítems donde TODOS los atributos de combinación están vacíos.
                            if (atributosCombinacion != null && atributosCombinacion.Length > 0)
                            {
                                bool todosCombVacios = true;
                                foreach (string atribComb in atributosCombinacion)
                                {
                                    IAtributo atribCombVal = p.ObtenerAtributo(atribComb);
                                    string val = (atribCombVal != null && atribCombVal.Valor != null) ? atribCombVal.Valor.ToString().Trim() : string.Empty;
                                    if (!string.IsNullOrEmpty(val)) { todosCombVacios = false; break; }
                                }
                                if (todosCombVacios) continue;
                            }
                            // Clave = contenido real de la combinación (artículo + atributos de combinación).
                            // Múltiples filas del comprobante con idénticos valores de combinación forman un único grupo.
                            string key = this.ConstruirClaveCombinacion( p, atributoCodigoArticuloPorArt, atributosCombinacion );
                            if (!qtdPorCombinacion.ContainsKey(key)) qtdPorCombinacion[key] = 0m;
                            qtdPorCombinacion[key] += u.CantidadEnItem;
                        }
                        decimal maxQtd = -1m;
                        string bestFallbackCombinacion = null;
                        decimal bestFallbackQtdCombinacion = -1m;
                        foreach (var kv in qtdPorCombinacion)
                        {
                            // Rastrear la mejor combinación real para el fallback.
                            if (kv.Value > bestFallbackQtdCombinacion) { bestFallbackCombinacion = kv.Key; bestFallbackQtdCombinacion = kv.Value; }
                            decimal effective = kv.Value + dummyQtd;
                            if (effective >= candidato.Requerido && effective > maxQtd)
                            {
                                valorCombinacionFijada = kv.Key;
                                maxQtd = effective;
                            }
                        }
                        if (valorCombinacionFijada == null)
                        {
                            // Ninguna combinación válida (con o sin dummies) alcanza el umbral.
                            // Usar la mejor combinación real para Satisfecho parcial correcto:
                            // TotalFaltante = requerido - bestComboQty en lugar de requerido - 0.
                            // Si no hay combinaciones reales (todos vacíos), sentinel vacío.
                            valorCombinacionFijada = bestFallbackCombinacion ?? string.Empty;
                        }
                    }

                    foreach ( ItemUsabilidad itemAUsar in usabilidadEnParticipanteYRegla )
                        {
                            string idItem = itemAUsar.Candidato.Replace( candidato.Participantes[0].Clave, "" );

                            if ( requerimientoDeRegla == 0 || itemAUsar.CantidadEnItem == 0 )  
                            {
                                if (itemAUsar.CantidadEnItem > 0)
                                {
                                    restantes.Add(idItem);
                                }

                                continue;
                            }

                            // PorArticulo: solo pasar ítems del artículo pre-elegido por el scan.
                            // Los dummies pasan siempre (representan el ítem faltante de cualquier artículo).
                            if (esPorArticulo && !string.IsNullOrEmpty(atributoAgrupacionPorArticulo))
                            {
                                IParticipante partEnComprobante = candidato.Participantes.FirstOrDefault(p => p.Id == idItem);
                                if (!(partEnComprobante is ParticipanteDummy))
                                {
                                    string valorArticuloActual = null;
                                    if (partEnComprobante != null)
                                    {
                                        IAtributo atribId = partEnComprobante.ObtenerAtributo(atributoAgrupacionPorArticulo);
                                        valorArticuloActual = atribId != null && atribId.Valor != null ? atribId.Valor.ToString() : null;
                                    }
                                    if (valorArticuloActual != valorArticuloFijado)
                                        continue;
                                }
                            }

                            // PorArticuloYCombinacion: solo pasar ítems de la fila pre-elegida.
                            // Ítems con todos los atributos de combinación vacíos son inválidos.
                            // Los dummies pasan siempre (representan el ítem faltante de la combinación elegida).
                            if (esPorArticuloYCombinacion)
                            {
                                IParticipante partEnComprobante = candidato.Participantes.FirstOrDefault(p => p.Id == idItem);
                                if (!(partEnComprobante is ParticipanteDummy))
                                {
                                    // Excluir ítems con todos los atributos de combinación vacíos.
                                    if (atributosCombinacion != null && atributosCombinacion.Length > 0 && partEnComprobante != null)
                                    {
                                        bool todosCombVacios = true;
                                        foreach (string atribComb in atributosCombinacion)
                                        {
                                            IAtributo atribCombVal = partEnComprobante.ObtenerAtributo(atribComb);
                                            string val = (atribCombVal != null && atribCombVal.Valor != null) ? atribCombVal.Valor.ToString().Trim() : string.Empty;
                                            if (!string.IsNullOrEmpty(val)) { todosCombVacios = false; break; }
                                        }
                                        if (todosCombVacios) continue;
                                    }
                                    // Solo los ítems cuya combinación real coincida con la pre-seleccionada.
                                    string claveCombinacionActual = partEnComprobante != null
                                        ? this.ConstruirClaveCombinacion( partEnComprobante, atributoCodigoArticuloPorArt, atributosCombinacion )
                                        : string.Empty;
                                    if ( claveCombinacionActual != valorCombinacionFijada )
                                        continue;
                                }
                            }

                            // aca me fijo si el item esta habilitado por condicion global
                            List<ItemCompatibilidadCondicionGlobal> compatibilidadConOtrosItems;
                            ItemCompatibilidadCondicionGlobal combinacionElegida = null;
                            decimal cantidadDisponibleParaItem = itemAUsar.CantidadEnItem; // valor por defecto

                            bool EsMonto = participante.Reglas.Where(r => r.Id == itemRegla.regla).FirstOrDefault().Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].CantidadMonto;


                            if ( beneficioAsociadoAlParticipante != null && compatibilidadCondicionGlobal.TryGetValue( beneficioAsociadoAlParticipante.Id, out compatibilidadConOtrosItems ))
                            {
                                Boolean existeParticipanteEnLista = ( compatibilidadConOtrosItems.Any( x => x.Participantes.ContainsKey( item.participante ) ) );
                                //if ( !existeParticipanteEnLista )
                                //{
                                //    continue;
                                //}

                                Boolean existeCombinacion = compatibilidadConOtrosItems.Any( x => x.Participantes[item.participante].Any( p => p.Id == idItem ) );

                                if ( existeCombinacion  || EsMonto )
                                {
                                    // Buscar la combinación que contiene este item y extraer la cantidad específica
                                    combinacionElegida = compatibilidadConOtrosItems.FirstOrDefault( x => x.Participantes[item.participante].Any( p => p.Id == idItem ) );
                                    
                                    if (
                                         promocion.UtilizaCosumoPorCombinacion() &&
                                         combinacionElegida != null && 
                                         combinacionElegida.CantidadAConsumirPorParticipante != null &&
                                         combinacionElegida.CantidadAConsumirPorParticipante.ContainsKey( item.participante ) &&
                                         combinacionElegida.CantidadAConsumirPorParticipante[item.participante].ContainsKey( itemAUsar.Candidato ) )
                                    {
                                        // Usar la cantidad específica de la combinación
                                        cantidadDisponibleParaItem = combinacionElegida.CantidadAConsumirPorParticipante[item.participante][itemAUsar.Candidato];
                                        
                                        // Establecer ConsumoPorCombinacion en el participante correspondiente
                                        var participanteEnComprobante = candidato.Participantes.FirstOrDefault(p => p.Id == idItem);
                                        if (participanteEnComprobante != null)
                                        {
                                            participanteEnComprobante.ConsumoPorCombinacion = cantidadDisponibleParaItem;
                                        }
                                    }
                                    
                                    if (promocion.UtilizaCosumoPorCombinacion() && !EsMonto)//( new[] { "3", "6" }.Any( x => promocion.Tipo.Contains(x) ) )
                                        compatibilidadConOtrosItems.RemoveAll( x => !x.Participantes[item.participante].Any( p => p.Id == idItem ) );
                                }
                                else
                                {
                                    if (!existeCombinacion && compatibilidadConOtrosItems.Count > 0 && !EsMonto
                                        && promocion.AplicacionProductosIguales != ZooLogicSA.Promociones.FormatoPromociones.AplicacionProductosIgualesType.PorArticulo)
                                    {
                                        continue;
                                    }
                                
                                }
                            }

                            colaboradores.Add( idItem );

                            ItemUsabilidad lineaUsabilidad = usabilidadLoca.Find( x => x.IdParticipante == itemAUsar.IdParticipante && x.IdRegla == itemAUsar.IdRegla && x.Candidato == itemAUsar.Candidato );

                        #region debug
                        //lock ( locker )
                        //{
                        //	string archivo = @"d:\resultadoreglas.txt";
                        //	File.AppendAllText( archivo, "consumiendo: " + requerimientoDeRegla + " de " + lineaUsabilidad.CantidadEnItem + " en el item" + lineaUsabilidad.Candidato + "\r\n" );
                        //}
                        #endregion

                        if (participante.Reglas.Where(r => r.Id == itemRegla.regla).FirstOrDefault().Atributo == this.configuracionComportamiento.ConfiguracionesPorParticipante[participante.Codigo].CantidadMonto)
                        {
							if ( lineaUsabilidad.Precio <= 0 )
							{
								continue;
							}

							Decimal requerimientoDeReglaPorMonto = requerimientoDeRegla / lineaUsabilidad.Precio;

                            this.ResolverCantidadesUsadas(ref requerimientoDeReglaPorMonto, ref nuevaCantidad, itemAUsar.CantidadEnItem, "xMonto");

                            Decimal cantidadUsada = itemAUsar.CantidadEnItem - nuevaCantidad;

                            requerimientoDeRegla = Math.Max( requerimientoDeRegla - (lineaUsabilidad.Precio * cantidadUsada) ,  0 );
						}
						else
						{
							this.ResolverCantidadesUsadas( ref requerimientoDeRegla, ref nuevaCantidad, cantidadDisponibleParaItem, "xCantidad" );
						}

                        // Actualiza la cantidad en la usabilidad
                        if (promocion.UtilizaCosumoPorCombinacion())
                        {
                            usabilidadLoca.ForEach(x => x.CantidadEnItem = (x.Candidato == itemAUsar.Candidato) ? x.CantidadEnItem - ( cantidadDisponibleParaItem - nuevaCantidad ) : x.CantidadEnItem);
                        }
                        else
                        {
                            usabilidadLoca.ForEach(x => x.CantidadEnItem = (x.Candidato == itemAUsar.Candidato) ? nuevaCantidad : x.CantidadEnItem);
                        }
                            

                        #region debug
                        //lock ( locker )
                        //{
                        //	string archivo = @"d:\resultadoreglas.txt";
                        //	File.AppendAllText( archivo, "quedan: " + requerimientoDeRegla + " por cumplir\r\n" );
                        //}
                        #endregion

                        // Respuesta
                        if ( retorno.Exists( x => x.IdParticipanteEnRegla == candidato.PartPromo.Id ) )
                        {
                            retorno.Find( x => x.IdParticipanteEnRegla == candidato.PartPromo.Id ).Satisfecho = candidato.Requerido - requerimientoDeRegla;
                        }
                        else
                        {
                            ConsumoParticipanteEvaluado consumo = new ConsumoParticipanteEvaluado();
                            consumo.CodigoParticipanteEnComprobante = candidato.Participantes[0].Clave;
                            consumo.ParticipantesEnComprobante = colaboradores;
                            consumo.ParticipantesRestantes = restantes;
                            consumo.IdParticipanteEnRegla = candidato.PartPromo.Id;
                            consumo.Satisfecho = candidato.Requerido - requerimientoDeRegla;
                            consumo.Requerido = candidato.Requerido;
                            consumo.Atributos = candidato.PartPromo.Reglas.Select( x => x.Atributo ).Distinct().ToList();
                            retorno.Add( consumo );
                        }
                    }
                }
                //resultadoCantidadDelParticipante.ToList().ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.PartPromo.Id + "\t" + x.Regla.Id + "\t" + x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\r\n" + String.Join( "\r\n", x.Participantes.Select( s => s.Clave + s.Id + "\t" ).ToArray() ) + "\r\n(" + x.Regla.Atributo + ")\r\n" ) );
            }

            // En caso que existan reglas de cantidad con operadores > o >= intenta afectar todo lo que pueda de lo que quedó sin consumir
            foreach (ConsumoParticipanteEvaluado itemRetorno in retorno)
            {
                int idRegla = ( from x in ordenNivelReglas where x.parti == itemRetorno.IdParticipanteEnRegla select x.regla ).First();
                String atributoEnRegla = participantesEnPromo.Where( x => x.Id == itemRetorno.IdParticipanteEnRegla ).First().Reglas.Where( z => z.Id == idRegla ).First().Atributo;

                if ( atributoEnRegla == ( this.configuracionComportamiento.ConfiguracionesPorParticipante[itemRetorno.CodigoParticipanteEnComprobante].Cantidad ) 
                        || atributoEnRegla == ( this.configuracionComportamiento.ConfiguracionesPorParticipante[itemRetorno.CodigoParticipanteEnComprobante].CantidadMonto ) )
                {
                    Factor operadorRegla = participantesEnPromo.Where( x => x.Id == itemRetorno.IdParticipanteEnRegla).First().Reglas.Where( z => z.Id == idRegla ).First().Comparacion;
                    if ( ( operadorRegla == Factor.DebeSerMayorIgualA ) || ( operadorRegla == Factor.DebeSerMayorA ) )
                    {
                        // PorArticulo: cada iteración recursiva debe consumir solo el mínimo requerido
                        // para que el do-while pueda seguir aplicando en el siguiente ciclo
                        if (promocion.AplicacionProductosIguales == AplicacionProductosIgualesType.PorArticulo)
                            continue;

                        // PorArticuloYCombinacion: ídem PorArticulo, cada ciclo aplica solo el mínimo
                        // para que el do-while pueda continuar con la misma u otra combinación.
                        if (promocion.AplicacionProductosIguales == AplicacionProductosIgualesType.PorArticuloYCombinacion)
                            continue;

                        ResultadoReglas candidatoOriginal = resultadoreglas.Find(x => x.PartPromo.Id == itemRetorno.IdParticipanteEnRegla && x.Regla.Id == idRegla);
                        foreach (IParticipante itemParticipante in candidatoOriginal.Participantes)
                        {
                            ItemUsabilidad itemUsabilidadLoca = usabilidadLoca.Find(x => x.Candidato == itemRetorno.CodigoParticipanteEnComprobante + itemParticipante.Id && x.IdRegla == idRegla);
                            if (itemUsabilidadLoca.CantidadEnItem > 0)
                            {
                                if ( ! itemRetorno.ParticipantesEnComprobante.Exists(x => x == itemParticipante.Id))
                                {
                                    itemRetorno.ParticipantesEnComprobante.Add(itemParticipante.Id);
                                }

                                if ( atributoEnRegla == ( this.configuracionComportamiento.ConfiguracionesPorParticipante[itemRetorno.CodigoParticipanteEnComprobante].Cantidad ) )
                                {
                                    // CANTIDAD
                                    if (promocion.UtilizaCosumoPorCombinacion())
                                    {
                                        itemRetorno.Requerido = itemUsabilidadLoca.CantidadEnItem + itemRetorno.Satisfecho;
                                        itemRetorno.Satisfecho = itemRetorno.Requerido;
                                        itemParticipante.ConsumoPorCombinacion = itemRetorno.Requerido;
                                        itemUsabilidadLoca.CantidadEnItem = 0;
                                    }
                                    else
                                    {
                                        itemRetorno.Requerido += itemUsabilidadLoca.CantidadEnItem;
                                        itemRetorno.Satisfecho += itemUsabilidadLoca.CantidadEnItem;
                                        itemUsabilidadLoca.CantidadEnItem = 0;
                                    }
                                        
                                }
                                else 
                                {
                                    // MONTO
                                    itemRetorno.Requerido += itemUsabilidadLoca.CantidadEnItem * itemUsabilidadLoca.Precio;
                                    itemRetorno.Satisfecho += itemUsabilidadLoca.CantidadEnItem * itemUsabilidadLoca.Precio;
                                    itemUsabilidadLoca.CantidadEnItem = 0;
                                }
                            }
                        }
                    }
                }

            }

            return retorno;
        }

        private void ModificarCantidadParticipantes(List<ResultadoReglas> resultadoReglas)
        {            
            foreach (ResultadoReglas regla in resultadoReglas)
            {
                foreach (IParticipante participante in regla.Participantes)
                {
                    if (participante.Clave == "COMPROBANTE.VALORESDETALLE.ITEM")
                    {
                        try
                        {
                            decimal consumido = participante.Consumido;
                            if (consumido < participante.Cantidad)
                            {
                                participante.ModificarCantidadSegunMonto();
                            }
                        }
                        catch
                        {
                            participante.ModificarCantidadSegunMonto();
                        }                     
                    }                 
                }
            }
        }

        #endregion

        private void ResolverCantidadesUsadas( ref Decimal requerimientoDeRegla, ref Decimal nuevaCantidad, Decimal cantidadEnItem, String formatoDelRequerimiento )
        {
            if (formatoDelRequerimiento == "xMonto")
                requerimientoDeRegla = Math.Ceiling(requerimientoDeRegla);
            else
            {
                decimal auxCantidad = requerimientoDeRegla * 100;
                auxCantidad = Math.Ceiling(auxCantidad);
                requerimientoDeRegla = auxCantidad / 100;
            }

            if ( cantidadEnItem > requerimientoDeRegla )
            {
                nuevaCantidad = cantidadEnItem - requerimientoDeRegla;
                requerimientoDeRegla = 0;
            }
            else if ( cantidadEnItem < requerimientoDeRegla )
            {
                nuevaCantidad = 0;
                requerimientoDeRegla = requerimientoDeRegla - cantidadEnItem;
            }
            else if ( cantidadEnItem == requerimientoDeRegla )
            {
                nuevaCantidad = 0;
                requerimientoDeRegla = 0;
            }
        }

        /// <summary>
        /// Construye la clave de contenido de combinación para un ítem del comprobante.
        /// Concatena con '|' el valor del atributo de código de artículo y los valores de
        /// cada atributo de combinación configurado (ej: "Art0|Rojo|L").
        /// Si no hay atributos configurados o todos sus valores son nulos/vacíos, retorna
        /// participante.Id para mantener la compatibilidad hacia atrás (comportamiento fila-a-fila).
        /// </summary>
        private string ConstruirClaveCombinacion( IParticipante participante, string atributoCodigoArticulo, string[] atribsCombinacion )
        {
            var partes = new List<string>();

            if ( !string.IsNullOrEmpty( atributoCodigoArticulo ) )
            {
                IAtributo atrib = participante.ObtenerAtributo( atributoCodigoArticulo );
                partes.Add( atrib != null && atrib.Valor != null ? atrib.Valor.ToString() : string.Empty );
            }

            if ( atribsCombinacion != null )
            {
                foreach ( string atribComb in atribsCombinacion )
                {
                    IAtributo atrib = participante.ObtenerAtributo( atribComb );
                    partes.Add( atrib != null && atrib.Valor != null ? atrib.Valor.ToString() : string.Empty );
                }
            }

            if ( partes.Count == 0 || partes.All( v => string.IsNullOrEmpty( v ) ) )
                return participante.Id;

            return string.Join( "|", partes );
        }
    
    }
}
