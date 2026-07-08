---
name: decision-framework-destino
description: Decision — the new Promociones project targets .NET Framework 4.7.2 (net472), a company decision
metadata:
  type: project
---

**Decisión firme (2026-07-08):** el proyecto nuevo (reescritura) apunta a **.NET Framework 4.7.2 (`net472`)**, igual que el legacy — NO a .NET moderno.

**Why:** Es una decisión de la empresa. Aunque la máquina tenga .NET 10 instalado, el estándar corporativo es net472 (entre otras razones, mantiene la compatibilidad con el interop existente con FoxPro / bridge `_screen.Zoo`, que fue pensado para .NET Framework). Si en el futuro la empresa hace un update de framework, se aplicará el cambio también en este proyecto.

**How to apply:** Al armar la constitución de Spec Kit y crear los proyectos nuevos, fijar `TargetFrameworkVersion` = v4.7.2 (proyecto .NET Framework, no SDK-style net8/net10). Tener presente que esto condiciona qué paquetes/NuGets y qué features de C# están disponibles. Related: [[promociones-dll-contract]], [[promociones-consumer-vfp9]], [[rewrite-promociones-goal]].
