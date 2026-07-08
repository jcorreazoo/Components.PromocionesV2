# Índice de memoria del proyecto

Memoria persistente del proyecto, versionada dentro del repo. Una línea por archivo.

- [Estado actual](estado-actual.md) — ⭐ dónde quedamos y el próximo paso (leer primero)
- [Decisión: framework destino](decision-framework-destino.md) — el proyecto nuevo apunta a .NET Framework 4.7.2 (decisión de la empresa)
- [Rewrite Promociones goal](rewrite-promociones-goal.md) — reescritura completa vía SDD/Spec Kit; preservar el contrato entrada→salida de la DLL, no los internals
- [Promociones consumer VFP9](promociones-consumer-vfp9.md) — la DLL la consume un sistema de facturación en Visual FoxPro 9 + SQL Server; el límite de interop es crítico
- [Promociones DLL contract](promociones-dll-contract.md) — C#/net472 x86, bridge CLR de ZooLogic (no COM registrado), comprobante XML de entrada, List&lt;InformacionPromocion&gt; de salida, motor de cálculo puro
- [Promociones lifecycle & UI](promociones-lifecycle-ui.md) — ciclo de vida de una promo: UserControl .NET (DevExpress) embebido en form FoxPro → se guarda como texto XML en SQL → vuelve al motor
- [Promociones modelo de dominio](promociones-modelo-dominio.md) — Promocion (participantes/reglas + beneficios), familias de participantes, mecanismos de beneficio y catálogo de 8 tipos
- [Promociones visión nueva](promociones-vision-nueva.md) — visión estilo supermercado + las 15 observaciones de diseño para la reescritura
- [Relevamiento completo](../../docs/RELEVAMIENTO.md) — documento consolidado (fuera de .claude, en docs/), insumo para Spec Kit
