---
name: promociones-asistente
description: The Asistente project — a real-time promotions advisor UI with a traffic-light state model
metadata:
  type: reference
---

`ZooLogicSA.Promociones.Asistente` is a **real-time promotions advisor**: a floating WinForms panel (positionable on screen), launched from the FoxPro menu, that lists **all promotions** with a **traffic-light state** updated live as the comprobante is built.

- **States** (`enum estado`): `Cumplida` (0) → 🟢 green, applicable now (`Afectaciones > 0`); `Parcial` (1) → 🟡 yellow, feasible but missing something; `Incumplida` (2) → 🔘 gray (LightGray), not met / default. Colors set in `FrmAsistente`.
- **State computation** (`ObservadorPromocionSingleThread` / `KontrolerAsistente.ObtenerEstado`): `Cumplida` if `Afectaciones>0`; else `Parcial` if `InformacionPromocionIncumplida.SatisfechoEfectivo>0` or some non-COMPROBANTE `Resultados.Satisfecho>0`; else `Incumplida`.
- **Real-time push via observer pattern:** engine → `NotificadorServicioPromociones` → `IObservadorServicioPromociones` → `ObservadorPromocionSingleThread.PresentarPromocionesAplicables` refreshes the form.
- **Guidance:** in yellow/gray, the `ArmadoDeLeyenda` subsystem builds the "what's missing" message from `InformacionPromocionIncumplida`.
- **Apply on double-click:** selects the promo, computes the benefit, transforms the document (`TransformadorComprobante`) and returns the info to apply (enabled only when green / `estado.Cumplida`).

Design note: this real-time advisor UX (traffic light + "what's missing" guidance + one-click apply) is a valuable feature to preserve; today it's coupled to WinForms/DevExpress and the FoxPro bridge. Related: [[promociones-dll-contract]], [[promociones-lifecycle-ui]], [[promociones-modelo-dominio]], [[promociones-vision-nueva]].
