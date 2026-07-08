---
name: promociones-vision-nueva
description: Vision for the new Promociones system and the accumulated design observations from the relevamiento
metadata:
  type: project
---

**Vision for the rewrite** (user's own words): build a supermarket-style promotions engine — very varied benefits and groupings, with **exclusions** (e.g. not for meat, or for members of a community that has its own promo group but all apply within the same purchase), **segmentation** (client/community), **temporal/contextual** conditions (certain days, birthday), **combined** promos (store + payment method), and triggers like coupon codes and referrals. Promos must be able to apply individually or **stacked** in the same purchase.

**Architecture stance (agreed):** mechanics as extensible code **classes (Strategy pattern)** — a dev adds a class for a new mechanic; **individual promotions are data/config** authored by business users from the UI (data-driven). NOT a class per individual promo. Multithreading already exists in the legacy engine (ThreadPool) for evaluation, but the hard part is the **orchestration layer** (stacking / exclusivity / priority / exclusions / shared-consumption), because stacked promos are interdependent (consumption changes what's available) — legacy evaluates in parallel but applies sequentially.

**Design observations to address in the new design (accumulated):**
1. Separate promotion **authoring (UI)** from the **evaluation engine**.
2. Unify the **3 mechanisms** of "who gets the benefit" into one model.
3. **Beneficiarios = a subset of the condition participants**, not a separate list (today both grids must satisfy conditions).
4. Participant catalog **open/configurable**, not fixed.
5. UI must allow **parenthesized grouping** of and/or logic (engine supports it; the form doesn't).
6. **Quantity as a real magnitude** (decimals + unit of measure), not integer-of-units defaulting to "1" (breaks e.g. 250g when unit is kg).
7. **Separate "monto" from "cantidad de unidades"** — today conflated (`CantidadMonto="MONTO"`), forcing hacks like loading cantidad=2 to cover article+monto ("una aberración").
8. **Importe as a multi-level condition** (comprobante / item / family).
9. **No round-trip importe→cantidad→importe** — compute each dimension in its own unit with decimal precision; special care with `=` comparisons on money (legacy has band-aids: `CorreccionPorAjuste`, "0.01"/"1" hack).
10. Replace the **rigid named-type catalog** with **composable `(condición) → (beneficio)`** dimensions (the catalog "quedó chico").
11. **Data-driven engine**: mechanics as classes, promos as user-authorable data.
12. Explicit **orchestration layer** (stacking/exclusivity/priority/exclusions/segmentation).
13. **Scope carrito vs línea** as a first-class concept.
14. Unify the **type codification** (today duplicated: `BeneficioType` 0–6 vs engine `Tipo` string).
15. `TipoPromocion` (Negocio) is the base for "class per mechanic", but must be **decoupled from UI (masks/labels) and from SQL** (`ObtenerWhereAdicional`).
16. Preserve the **real-time advisor UX** (the Asistente): traffic-light states (green/yellow/gray) + "what's missing" guidance + one-click apply, decoupled from WinForms/DevExpress and the FoxPro bridge. See [[promociones-asistente]].
17. **Explicit, configurable application priority**: today several hardcoded orderings at different levels are conflated (see [[promociones-modelo-aplicacion]]); unify into one clear, configurable priority model.
18. **Support both application strategies** — incremental (as articles load) and global recalculation — as explicit orchestration modes. There are 3 apply entry points to preserve: automatic, manual-by-name, assistant.

Related: [[promociones-modelo-dominio]], [[rewrite-promociones-goal]], [[decision-framework-destino]].
