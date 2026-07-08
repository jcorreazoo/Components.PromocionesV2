---
name: rewrite-promociones-goal
description: Goal and scope for rewriting the Promociones DLL from scratch using Spec-Driven Development (Spec Kit)
metadata:
  type: project
---

The user is rewriting the ZooLogicSA.Promociones project from scratch. The original was built in .NET with Visual Studio; the new one will use VS Code (so UI and NuGet handling differ and the `.UI` layer/structure may change substantially). The original design didn't contemplate several needs, and after years of use the current structure makes them impossible to apply — hence the full rewrite.

**Essence to preserve (the contract, not the internals):**
- **Input:** a serialized comprobante (voucher/receipt) that enters the DLL.
- **Output:** an object with complete information for the billing system to consume.
- The language it's written in.

**Why:** The user wants freedom to change much of the structure while keeping the input→output contract intact, because that contract is what the consuming billing system depends on.

**How to apply:** Focus the relevamiento (survey) on: language/stack, the input contract (serialized comprobante), the output object contract, and the interop boundary. Treat structure/UI/NuGets as reference-only, contemplating change. See [[promociones-consumer-vfp9]] for the integration constraint. Plan: relevar → volcar a doc durable → init Spec Kit (needs `uv`, Python NOT installed) → **crear y pushear una rama/tag `legacy`** con todo el código viejo (NO se borra; consultable con `git checkout legacy`) → borrar código viejo conservando .claude/, CLAUDE.md, Spec Kit dirs, docs/, .gitignore, .autorc, .teamcity → historial limpio en master (orphan + force-push).
