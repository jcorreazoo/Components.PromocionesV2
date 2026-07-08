---
name: promociones-consumer-vfp9
description: The Promociones DLL is consumed by a billing system written in Visual FoxPro 9 with SQL Server databases
metadata:
  type: project
---

The billing system ("sistema de facturación") that consumes the ZooLogicSA.Promociones DLL is written in **Visual FoxPro 9** with **SQL Server** databases.

**Why:** This is a hard external constraint on the rewrite. A VFP9 host cannot call a modern .NET assembly the way .NET code can — it typically needs COM interop (a COM-visible assembly registered via regasm) or a file/serialized-data exchange. This shapes the entire integration boundary of the new design and the choice of target framework.

**How to apply:** During the relevamiento, determine exactly HOW VFP9 invokes the DLL today (COM interop vs serialized file exchange vs wrapper), and whether the DLL itself touches SQL Server or only receives the serialized comprobante and returns an object. Preserve that boundary in the new design. Related: [[rewrite-promociones-goal]], [[promociones-dll-contract]].
