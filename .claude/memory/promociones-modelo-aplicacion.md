---
name: promociones-modelo-aplicacion
description: How promotions get applied at runtime — the 3 entry points, the multi-level (hardcoded) priority orderings, and incremental vs global recalculation
metadata:
  type: reference
---

Runtime application model of the legacy engine:

**Three ways to apply a promo:**
1. **Automatic** — promos flagged "automáticas" (`Promocion.AplicaAutomaticamente`, a checkbox in the authoring UI) apply **as articles are entered** into the comprobante (`ServicioEvaluacionPromociones.AplicarPromocionesAutomaticas`).
2. **Manual by name** — the user types the promo name in the invoice; evaluated via `EvaluarPromocionesIndividualmente` / `EvaluarYAplicarPromocion`; if it can't apply, it's reported (exception / `InformacionPromocionIncumplida`).
3. **Assistant** — double-click in the Asistente (see [[promociones-asistente]]).

**"Priority" is NOT a single thing** — it's several hardcoded orderings at different levels (this is why the user could never fully decipher it):
- Which **automatic promo wins**: `OrderByDescending(MontoBeneficio).ThenByDescending(FechaModificacion).ThenByDescending(HoraModificacion)` in `ServicioEvaluacionPromociones`.
- **Batch apply order**: by `AplicacionProductosIguales` (most restrictive first) in `MotorPromociones.AplicarPromociones`.
- **Which items get consumed first** to satisfy a promo: by computed `dificultad` in `ArmadorDeCoincidenciasPorUsabilidad` — THIS is the "usabilidad priority" the user remembered.
- **Which participant receives the benefit**: by mayor/menor precio in `TransformadorComprobante`.

**Two application strategies:** **incremental application is automatic** — automatic promos apply on their own as articles are loaded. **Global recalculation is a user decision** (the system does NOT do it on its own): via a menu option in the invoice, the user chooses to remove all promos from the comprobante and recompute to prioritize from an overall view. Both are orchestration concerns.

Design implications: make priority **explicit and configurable** (unify the scattered hardcoded orderings), and support both application strategies as explicit orchestration modes. Related: [[promociones-vision-nueva]], [[promociones-modelo-dominio]], [[promociones-asistente]].
