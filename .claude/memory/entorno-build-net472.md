---
name: entorno-build-net472
description: The dev machine CAN build and run .NET Framework 4.7.2, plus packaging/UI migration points for the new project
metadata:
  type: reference
---

**Entorno verificado (2026-07-22) — se puede compilar y probar net472 desde acá:**
- **`dotnet build` de un proyecto SDK-style con `<TargetFramework>net472</TargetFramework>` compila OK** (probado empíricamente en scratchpad). WinForms funciona agregando `<Reference Include="System.Windows.Forms" />` (en SDK-style net472 las refs del framework NO vienen solas).
- **Targeting pack .NET Framework 4.7.2 PRESENTE**; runtime .NET Framework **4.8** instalado (Release 533509) → corre apps 4.7.2.
- **Visual Studio Professional 2019** instalado → **MSBuild 16.11** en `C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe` (permite también build estilo viejo si hiciera falta).
- **.NET SDKs** 9.0.301 y 10.0.201; runtimes WindowsDesktop 9 y 10. `msbuild`/`nuget` NO están en PATH, pero no hacen falta (`dotnet` maneja restore/build de SDK-style; MSBuild de VS disponible por ruta completa).

**Decidido: NO compilar el proyecto legacy acá** — fallaría por DevExpress (licenciado, no instalado) y el feed NuGet privado `ZooLogicSA.*` (sin acceso); los errores serían de entorno, no de diseño.

**Puntos de packaging/UI para el proyecto nuevo (insumo para `/plan` de Spec Kit):**
1. Proyecto **SDK-style + `<PackageReference>`** (no `packages.config`).
2. **Referencias del framework explícitas** en SDK-style net472.
3. **DevExpress**: licenciado/pago; la UI se rehace → decidir si se mantiene o se reemplaza.
4. **Feed NuGet privado** para `ZooLogicSA.*` (bridge Core, Redondeos, etc.) → configurar `nuget.config` si se usan.
5. **Plataforma x86** (por el bridge/COM en el legacy) → definir la del nuevo.

Related: [[decision-framework-destino]], [[decision-alcance-v1]], [[promociones-dll-contract]].
