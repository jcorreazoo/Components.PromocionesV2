---
name: promociones-modelo-dominio
description: Domain model of the legacy Promociones engine — Promocion aggregate, rules, benefits, participant families and the type catalog
metadata:
  type: reference
---

Domain model of the legacy engine (namespace `ZooLogicSA.Promociones.FormatoPromociones`):

- **`Promocion`** (aggregate root): `Id`, `Participantes: List<ParticipanteRegla>` (the CONDITIONS), `Beneficios: List<Beneficio>` (what is granted). Config knobs: `TopeBeneficio` (max discount cap, engine tracks a running `saldoTope`), `Recursiva` (re-apply while it keeps qualifying and cap not exhausted), `CuotasSinRecargo` (interest-free installments — travels back to billing), `ListaDePrecios` (which price list; DLL does NOT hold product prices — they arrive as "precios adicionales" XML), `Redondeo`/`ObjetoRedondeo` (programmable rounding rule loaded from FoxPro), `EleccionParticipante` (`EleccionParticipanteType`), `AplicacionProductosIguales`, `Tipo` (string), `ConMedioDePago`.
- **`ParticipanteRegla`**: `Codigo` (which part of the comprobante it looks at), `Reglas: List<Regla>`, `RelaReglas` (boolean formula combining rules with and/or + parentheses/nesting, each `{id}` = a Regla; supports nesting, engine injects synthetic ids), `Beneficiario` (true = loaded in the "grilla de beneficio", i.e. receives the benefit).
- **`Regla`**: `Atributo` + `Comparacion` (`Factor` enum: IgualA/Mayor/Menor/Distinto/Mayor-Menor-Igual/Contiene/ComienzaCon/TerminaCon/IgualADiaSemana) + `Valor`. `Compuesta`+`ReglaAsociada` = Between.
- **`Beneficio`**: `Atributo` (what it modifies), `Cambio` (`Alteracion` enum: CambiarValor / Incrementar-Disminuir EnCantidad / EnPorcentaje), `Valor`, `Destinos: List<DestinoBeneficio>` (a qué participante y `Cuantos`).

**3 participant families** (prefixed `COMPROBANTE.`): `COMPROBANTE` (header/footer: cliente, sucursal, totales), `FACTURADETALLE.ITEM` (sold articles), `VALORESDETALLE.ITEM` (payment methods). Combinable across families (e.g. article X + credit card Y). The catalog is **open/configurable** — a `Dictionary` in `ConfiguracionComportamiento` (`ConfiguracionesPorParticipante`), NOT a fixed enum; configured at runtime from FoxPro.

**"A quién se aplica el beneficio" — 3 mechanisms:** (1) default: proportionally to all `EsConsumible` participants in the condition; (2) `EleccionParticipanteType` (AplicarAlDeMenorPrecio/MayorPrecio/ATodos) via `Seleccionador...PorPrecio` (orders candidates by price); (3) "grilla de beneficiarios" (`ParticipanteRegla.Beneficiario`) narrows the candidate set.

**Type catalog (8):** defined in `FactoryPromociones.LlenarListaTipoPromociones()` (project `Negocio`), each a `TipoPromocion` subclass with capability flags (`_tieneTopeDeDescuento`, `_tieneRedondeo`, `_tieneListaDePrecios`, `_tieneCuotasSinRecargo`, `_tieneParticipantesBeneficiarios`, `_typeValidacion`, `BeneficioType`, `EleccionParticipanteDefault`). Types: LLevaXpagaY (2x1), LlevaXtienedescuentoY, DescuentoXcaracteristica, RebajaXcaracteristica, DescuentoBancarioConTope, MontoAplicaDescuento, MontoAplicaMontoFijo, LlevaAValorDeOtraListaDePrecios. **`BeneficioType` enum (0–6) in Negocio is a DIFFERENT numbering from the engine's `Tipo` string ("3"/"6"=consumo por combinación, "4"=caso cantidad) — duplicated/inconsistent codification.** `TipoPromocion` is mixed with UI (masks/labels) and coupled to SQL (`ObtenerWhereAdicional` returns raw SQL). Related: [[promociones-dll-contract]], [[promociones-lifecycle-ui]], [[promociones-vision-nueva]].
