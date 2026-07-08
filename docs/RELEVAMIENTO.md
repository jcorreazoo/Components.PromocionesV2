# Relevamiento — Motor de Promociones (ZooLogicSA.Promociones)

> Documento de ingeniería inversa del sistema legacy, insumo para el rediseño con metodología SDD (Spec Kit).
> Estado: **relevamiento cerrado y verificado**. Fecha: 2026-07-08.

## 1. Propósito

Este documento releva el motor de promociones actual (`ZooLogicSA.Promociones`) para:
1. Preservar la **esencia** (el contrato de entrada/salida de la DLL, del que depende el sistema de facturación).
2. Documentar el **modelo de dominio** y su ciclo de vida.
3. Registrar los **problemas de diseño** que hoy generan dolor y la **visión** del sistema nuevo.

El proyecto nuevo se reescribe desde cero; este relevamiento es el puente hacia la constitución y las especificaciones de Spec Kit.

## 2. Stack y entorno

- **Lenguaje:** C# sobre **.NET Framework 4.7.2** (`net472`), compilado a **x86**, `OutputType=Library`.
- **Estilo de proyecto:** `.csproj` clásico (no SDK-style) + `packages.config`.
- **Dependencias clave:** Antlr (evaluación de expresiones), StructureMap (IoC), log4net, Ionic.Zip, y paquetes internos `ZooLogicSA.Core/.Common/.ZED/.Redondeos`. **NHibernate** está referenciado pero **sin uso real** (vestigial, viene arrastrado por `Zoo.nHibernate`; descartable).
- **Decisión de framework para el proyecto nuevo:** se mantiene **.NET Framework 4.7.2** (decisión de la empresa; si a futuro la empresa actualiza, se cambia acá también).

## 3. Arquitectura de la solución

5 proyectos:

| Proyecto | Rol |
|---|---|
| `ZooLogicSA.Promociones` | **Núcleo / motor** de evaluación (la DLL). Cálculo puro. |
| `ZooLogicSA.Promociones.Negocio` | Metadata de tipos de promo (`TipoPromocion` + subclases), beneficios, validaciones. |
| `ZooLogicSA.Promociones.UI` | UserControls WinForms + **DevExpress** para autoría de promos. |
| `ZooLogicSA.Promociones.Asistente` | Asistente WinForms. |
| `ZooLogicSA.Promociones.Tests` | Tests (Rhino Mocks). |

## 4. Interop con el sistema de facturación (FoxPro)

- El consumidor es un **sistema de facturación en Visual FoxPro 9 con SQL Server**.
- El interop **no es COM registrado clásico** (todos los assemblies son `ComVisible(false)`), sino un **bridge propio de ZooLogic que hostea el CLR**: FoxPro instancia clases .NET por nombre de tipo con
  `_screen.Zoo.Crearobjeto("ZooLogicSA.Promociones.MotorPromociones", "", ...args)`.
- Es **bidireccional**: el .NET recibe el objeto comprobante de FoxPro como `object` y lo lee por late binding (`ZooLogicSA.Core.COM.HerramientasCom`); un servicio COM del lado FoxPro (`serializarpromociones`) convierte el comprobante en el **XML string** que alimenta el motor.
- La `ConfiguracionComportamiento` (claves de participante, `Precio`, `CantidadMonto`, etc.) se configura **en runtime desde FoxPro**.

## 5. Contrato de la DLL (la "esencia" a preservar)

**Entrada:**
- Comprobante como **XML string** → `IMotorPromociones.AgregarComprobanteParaEvaluacion(id, xml)` → se carga en un `XmlDocument` y se navega **por XPath** (jerarquía `COMPROBANTE/FACTURADETALLE/ITEM`, atributos tipados `@Valor`/`@TipoDato`/`@Promo`/`@Beneficio`/`@Consumido`, nodos como `ESRETIROEFECTIVO`).
- XML separado de **precios adicionales** (los precios reales los provee FoxPro; la DLL no los tiene).
- Promociones como **XML deserializado** (`Serializador.DeserializarPromocion` con `XmlSerializer`) → `List<Promocion>` inyectada con `EstablecerLibreriaPromociones(...)`.

**Salida:**
- `List<InformacionPromocion>`, cada una con: `IdPromocion`, `Promocion`, `MontoBeneficio`, `Afectaciones`, `Demora`, `DetalleBeneficiado: List<ParticipanteBeneficiado>`, `DetalleAfectado: List<ParticipanteAfectado>`, e `infoIncumplida: InformacionPromocionIncumplida` (por qué NO se cumplió: reglas faltantes/cumplidas, total faltante, etc.).

## 6. Ciclo de vida de una promoción

```
ALTA/EDICIÓN                                   EVALUACIÓN
UserControl .NET (DevExpress)                  FoxPro lee promos (XML) desde SQL
embebido en form FoxPro                        → Deserializar → EstablecerLibreriaPromociones
  → arma un Promocion                          + comprobante (obj FoxPro → serializado a XML)
  → XmlSerializer → XML                        → MOTOR: ¿cuál(es) aplican?
  → FoxPro graba XML como TEXTO en SQL  ──────▶ → List<InformacionPromocion>
```

La promo **nace** en el UserControl, se serializa a **XML y se guarda como campo de texto en SQL** (del lado FoxPro), y **vuelve** al motor para decidir cuáles aplican (todas, o una puntual). El `Serializador` (XmlSerializer de `Promocion`) es la bisagra de ida y vuelta.

## 7. Modelo de dominio

Namespace `ZooLogicSA.Promociones.FormatoPromociones`. Idea central: **CONDICIONES → BENEFICIOS**.

```
Promocion
├── Participantes : List<ParticipanteRegla>   (CONDICIONES)
│     ├── Codigo       → parte del comprobante (ej. "COMPROBANTE.FACTURADETALLE.ITEM")
│     ├── Reglas       : List<Regla>          → Atributo + Comparacion(Factor) + Valor
│     ├── RelaReglas   → fórmula lógica: "{1} and ({2} or {3})"
│     └── Beneficiario → true = está en la grilla de beneficio (recibe)
└── Beneficios   : List<Beneficio>            (QUÉ se otorga)
      ├── Atributo → qué modifica (ej. PRECIO)
      ├── Cambio   (Alteracion) → cómo (CambiarValor / Inc-Dism EnCantidad / EnPorcentaje)
      ├── Valor    → cuánto
      └── Destinos : List<DestinoBeneficio>   → a qué participante y Cuantos
```

**Perillas de la `Promocion`:** `TopeBeneficio` (tope máx. de descuento, con saldo que se gasta), `Recursiva` (re-aplica mientras califique y haya saldo de tope), `CuotasSinRecargo` (cuotas sin interés; viaja a facturación), `ListaDePrecios` (viaja a facturación), `Redondeo`/`ObjetoRedondeo` (regla de redondeo programable, cargada de FoxPro), `EleccionParticipante`, `AplicacionProductosIguales`, `Tipo`, `ConMedioDePago`.

**Comparadores (`Factor`):** IgualA, MayorA, MenorA, DistintoA, MayorIgualA, MenorIgualA, ContieneA, ComienzaCon, TerminaCon, IgualADiaDeLaSemana. (`Compuesta`+`ReglaAsociada` = Between.)

### 7.1 Familias de participantes
Tres grandes familias (prefijo `COMPROBANTE.`), **combinables entre sí**:
- **`COMPROBANTE`** → cabecera/pie (cliente, sucursal, totales).
- **`FACTURADETALLE`** → ítems con los artículos vendidos.
- **`VALORESDETALLE`** → medios de pago.

El catálogo de participantes es **abierto/configurable** (un `Dictionary` en `ConfiguracionComportamiento`), no un enum fijo. Ej. de combinación: "artículo X **y** tarjeta Y".

### 7.2 "¿A quién se aplica el beneficio?" — 3 mecanismos (crecieron por capas)
1. **Por defecto:** proporcionalmente a todos los participantes de la condición que sean `EsConsumible`.
2. **Filtro menor/mayor precio** (`EleccionParticipanteType`): los `Seleccionador...PorPrecio` ordenan candidatos por precio.
3. **Grilla de beneficiarios** (`ParticipanteRegla.Beneficiario`): acota el conjunto que recibe. Hoy es una lista separada, pero el motor exige que **ambas grillas** (condición y beneficio) cumplan condiciones.

## 8. Catálogo de tipos de promoción

Definido en `FactoryPromociones.LlenarListaTipoPromociones()` (proyecto `Negocio`). Cada tipo es una subclase de `TipoPromocion` con **flags de capacidad** (`_tieneTopeDeDescuento`, `_tieneRedondeo`, `_tieneListaDePrecios`, `_tieneCuotasSinRecargo`, `_tieneParticipantesBeneficiarios`, `_typeValidacion`, `BeneficioType`, `EleccionParticipanteDefault`).

| Clase | Nombre en UI | Condición → Beneficio |
|---|---|---|
| `LLevaXpagaY` | Lleva una cantidad, paga otra cantidad (2x1) | cantidad → cantidad gratis |
| `LlevaXtienedescuentoY` | Lleva un producto, paga con descuento otro | producto → % desc. otro |
| `DescuentoXcaracteristica` | % de descuento por característica | característica → % desc. |
| `RebajaXcaracteristica` | Monto fijo por característica | característica → monto fijo |
| `DescuentoBancarioConTope` | Bancaria: % con tope | medio de pago → % + tope |
| `MontoAplicaDescuento` | Lleva un monto, paga con descuento un producto | monto → % desc. producto |
| `MontoAplicaMontoFijo` | Lleva un monto, producto a precio fijo | monto → precio fijo |
| `LlevaAValorDeOtraListaDePrecios` | Producto a valor de otra lista | producto → precio otra lista |

⚠️ **Codificación duplicada:** el `BeneficioType` de `Negocio` (enum 0–6) **no coincide** con el `Tipo` string del motor (`"3"`/`"6"` = consumo por combinación, `"4"` = caso especial de cantidad). Hay dos sistemas de numeración inconsistentes conviviendo.

## 9. Base de datos / persistencia

- **El motor NO toca SQL Server** (0 uso de NHibernate/ISession/SqlConnection). Es **cálculo puro**.
- FoxPro carga promociones y redondeos desde SQL (`CargarPromociones`/`CargarRedondeos`, con detección de cambios por fecha de modificación) y los **inyecta** al motor.
- Las promos se guardan en SQL como **texto XML**. Los precios reales de producto también viven en FoxPro (llegan como "precios adicionales").

## 10. Problemas de diseño detectados (dolor actual)

1. **Cantidad como entero de unidades con "1" por defecto**: rompe con decimales / unidades de medida (ej. 250g cuando la unidad es kg). Hay parche `"0.01"/"1"` en `ComprobanteXML`.
2. **Monto conflacionado con cantidad** (`CantidadMonto="MONTO"`): combinar "monto + artículo" obliga a cargar cantidad=2 ("una aberración").
3. **Round-trip importe→cantidad→importe**: genera diferencias por redondeo, críticas con comparaciones `=`. Ya tiene curitas (`CorreccionPorAjuste`).
4. **UI no permite paréntesis** para agrupar `and`/`or` (el motor sí lo soporta).
5. **Codificación de tipos duplicada/inconsistente** (`BeneficioType` vs `Tipo`).
6. **`TipoPromocion` mezcla dominio con UI (máscaras/labels) y con SQL** (`ObtenerWhereAdicional` devuelve SQL crudo).
7. **Grilla de beneficio como lista separada** en vez de subconjunto de la condición.

## 11. Visión del sistema nuevo

Motor de reglas/promociones **estilo supermercado**: beneficios y agrupamientos variados, con:
- **Exclusiones** (ej. no aplica a carnes; o segmentos con su propio grupo de promos que conviven en la misma compra).
- **Segmentación** (cliente/comunidad), **condiciones temporales/contextuales** (días, cumpleaños), **combinadas** (local + medio de pago), y disparadores como **cupón** y **referido**.
- **Stacking**: varias promos coexisten en la misma compra.

**Postura de arquitectura (acordada):**
- Mecánicas como **clases extensibles (patrón Strategy)** → un dev agrega una clase para una mecánica nueva.
- Promociones individuales como **datos/config** autorables por el negocio desde la UI (**data-driven**). NO una clase por promo.
- Ya existe multi-hilo en el legacy; lo difícil no es el paralelismo sino la **capa de orquestación** (stacking/exclusividad/prioridad/exclusiones/consumo compartido), porque las promos apiladas son interdependientes (el consumo cambia lo disponible). El legacy evalúa en paralelo pero **aplica en secuencia**.

## 12. Observaciones / decisiones de diseño

1. Separar **autoría (UI)** del **motor** de evaluación.
2. Unificar los **3 mecanismos** de "a quién beneficio".
3. **Beneficiarios = subconjunto de la condición**, no lista aparte.
4. Catálogo de participantes **abierto/configurable**.
5. **UI con paréntesis** para agrupar `and`/`or`.
6. **Cantidad como magnitud real** (decimales + unidad de medida).
7. **Separar "monto" de "cantidad de unidades"** (dimensiones distintas).
8. **Importe como condición multinivel** (comprobante / ítem / familia).
9. **No hacer round-trip importe↔cantidad**: calcular cada dimensión en su unidad con precisión decimal; cuidado con `=` sobre dinero.
10. Reemplazar el **catálogo rígido de tipos** por **`(condición) → (beneficio)` componible**.
11. **Motor data-driven**: mecánicas = clases; promos = datos autorables.
12. **Capa de orquestación** explícita.
13. **Alcance carrito vs línea** como concepto de primera clase.
14. Unificar la **codificación de tipos**.
15. `TipoPromocion` es la base para "clase por mecánica", pero **desacoplado de UI y SQL**.

## 13. Puntos asumidos / a validar con negocio

- `Redondeo`: confirmado estructuralmente (regla programable aplicada por el motor), pero el usuario **nunca lo usó** — validar su uso real.
- Prioridad exacta cuando se combinan grilla de beneficiarios + filtro menor/mayor: la lectura de código respalda "elección opera sobre el conjunto que la grilla acota", pero conviene validar casos límite.

## 14. Decisiones tomadas

- **Framework destino:** .NET Framework 4.7.2 (decisión de empresa).
- **Metodología:** SDD con Spec Kit (requiere instalar `uv`; Python no está instalado; sí hay .NET 10 y git).
- **Limpieza del repo:** al iniciar el proyecto nuevo se conservan `.claude/`, `CLAUDE.md`, dirs de Spec Kit, `docs/`, `.gitignore`, `.autorc`, `.teamcity`; se borra el resto y se hace historial limpio (rama huérfana + force-push).
