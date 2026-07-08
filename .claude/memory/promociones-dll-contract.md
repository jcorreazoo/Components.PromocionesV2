---
name: promociones-dll-contract
description: Confirmed input/output contract, interop model and language of the legacy Promociones DLL (from the relevamiento)
metadata:
  type: reference
---

Relevamiento of the legacy ZooLogicSA.Promociones engine (core project `ZooLogicSA.Promociones`, entry interface `IMotorPromociones` / class `MotorPromociones`):

- **Language/stack:** C# on **.NET Framework 4.7.2** (`net472`), `OutputType=Library`, `PlatformTarget=x86`. Old-style .csproj + packages.config. Deps: NHibernate (Zoo.nHibernate), Antlr (expression grammar), StructureMap (IoC), log4net, Ionic.Zip, ZooLogicSA.Core/.Common/.ZED/.Redondeos.
- **Interop with the VFP9 billing system is via a custom ZooLogic CLR-hosting bridge, NOT classic registered COM** (CONFIRMED). FoxPro instantiates .NET classes by full type name with `_screen.Zoo.Crearobjeto("ZooLogicSA.Promociones.MotorPromociones", "", ...ctorArgs)` (the `_screen.Zoo` bridge lives in ZooLogicSA.Core and hosts the CLR) — this is why all assemblies are `ComVisible(false)` (no regasm/.tlb needed). It's bidirectional: .NET calls back into the FoxPro comprobante object via late binding (`ZooLogicSA.Core.COM.HerramientasCom`), and a FoxPro-side serialization service (`serializarpromociones`) turns the comprobante into the **XML string** that feeds the engine. `ConfiguracionComportamiento` (participant keys like `COMPROBANTE.FACTURADETALLE.ITEM`, `.Precio`, `.CantidadMonto`) is configured at runtime from FoxPro. Signatures use `string`/`ArrayList` for FOX compatibility.
- **Input contract:** comprobante as XML string via `AgregarComprobanteParaEvaluacion(id, xml)` → `IComprobante.Cargar` → `XmlDocument` navigated by **XPath** (hierarchy `COMPROBANTE/VALORESDETALLE/ITEM`, typed attributes `@Valor`/`@TipoDato`/`@Promo`/`@Beneficio`/`@Consumido`, node `ESRETIROEFECTIVO`). Separate "PreciosAdicionales" XML. Promotions enter as XML deserialized via `XmlSerializer` into `Promocion`, or injected as `List<Promocion>` via `EstablecerLibreriaPromociones`.
- **Output contract:** `List<InformacionPromocion>` = { IdPromocion, Promocion, Demora, DetalleBeneficiado: List<ParticipanteBeneficiado>, DetalleAfectado: List<ParticipanteAfectado>, Afectaciones, MontoBeneficio, infoIncumplida: InformacionPromocionIncumplida (reglas faltantes/cumplidas, TotalFaltante, etc.) }.
- **DB:** the engine is **pure computation — it does NOT touch SQL Server at all** (CONFIRMED: zero `using NHibernate`/ISession/SessionFactory/.hbm.xml/connection strings anywhere in .cs). Promotions and redondeos are loaded FoxPro-side from SQL Server (`CargarPromociones`/`CargarRedondeos`, with change detection via last-modified date) and injected into the engine via `EstablecerLibreriaPromociones(List<Promocion>)` / XML `DeserializarPromocion`. The **NHibernate reference in the .csproj is vestigial** — it comes bundled inside the `Zoo.nHibernate` package (which also ships the Antlr/Iesi/Castle DLLs the expression evaluator actually uses); safe to drop in the rewrite.

Related: [[rewrite-promociones-goal]], [[promociones-consumer-vfp9]].
