---
name: promociones-lifecycle-ui
description: Full lifecycle of a promotion (authoring UI → SQL storage as XML text → engine evaluation) and the role of the .UI project
metadata:
  type: reference
---

Full lifecycle of a promotion in the legacy system, and the role of `ZooLogicSA.Promociones.UI`:

**Authoring:** `ControlPromociones` (a DevExpress `XtraUserControl`) and `MaskPromocion` (`UserControl`) are embeddable .NET WinForms controls. They are **embedded inside a FoxPro form** (via the same `_screen.Zoo` CLR bridge) and are where users create/edit ("dan de alta") the promotions that exist in the system. The UI builds a `Promocion` object which is serialized to XML via `Serializador` (`XmlSerializer`).

**Storage:** FoxPro saves each promotion into **SQL Server as an XML-like text field** (the serialized `Promocion`). Change detection uses a last-modified date.

**Evaluation:** FoxPro reads the promo XML text back from SQL, the engine deserializes it (`DeserializarPromocion`) and receives it via `EstablecerLibreriaPromociones(List<Promocion>)`, then evaluates against the comprobante to decide **which promotions apply** (all of them, or a specific one the user indicates) → returns `List<InformacionPromocion>`.

**Design implication for the rewrite:** two responsibilities live together today — (A) promotion authoring UI (WinForms + DevExpress, embedded in FoxPro) and (B) the evaluation engine. The user said the visual/UI part and NuGets will be handled differently in VS Code, so the DevExpress UserControl authoring surface is a prime candidate to be redesigned/separated from the engine. The engine's job is purely: (promos as XML) + (comprobante as XML) → results. See [[promociones-dll-contract]], [[promociones-consumer-vfp9]], [[rewrite-promociones-goal]].
