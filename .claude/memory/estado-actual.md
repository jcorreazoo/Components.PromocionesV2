---
name: estado-actual
description: Where we left off — current progress in the Promociones rewrite plan and the next step
metadata:
  type: project
---

**Dónde quedamos (última actualización: 2026-07-08):**

Plan acordado (reescritura del motor de promociones con SDD/Spec Kit — ver [[rewrite-promociones-goal]]):
1. **Relevar** el proyecto actual ← ✅ COMPLETO Y VERIFICADO
2. **Volcar el relevamiento a doc durable** ← ✅ COMPLETO → `docs/RELEVAMIENTO.md` + memorias
3. **Inicializar Spec Kit** ← ⏳ PRÓXIMO PASO (instalar `uv`; Python NO está instalado; sí hay dotnet 10 y git)
4. Borrar código viejo conservando: `.claude/`, `CLAUDE.md`, dirs de Spec Kit, `docs/`, `.gitignore`, `.autorc`, `.teamcity`
5. Historial limpio → rama huérfana + force-push a GitHub

**Relevamiento cerrado:** stack, interop (bridge `_screen.Zoo`), contrato entrada/salida, ciclo de vida, modelo de dominio completo, catálogo de 8 tipos, base de datos (motor de cálculo puro), problemas de diseño y visión del sistema nuevo. Todo consolidado en **`docs/RELEVAMIENTO.md`** y distribuido en las memorias: [[promociones-dll-contract]], [[promociones-consumer-vfp9]], [[promociones-lifecycle-ui]], [[promociones-modelo-dominio]], [[promociones-vision-nueva]].

**Próximo paso:** iniciar Spec Kit (paso 3). Antes de eso: instalar `uv` en Windows (`winget install --id=astral-sh.uv` o `irm https://astral.sh/uv/install.ps1 | iex`). Luego `specify init` y armar la constitución tomando `docs/RELEVAMIENTO.md` como insumo, respetando la decisión de framework ([[decision-framework-destino]]).

**Pendientes menores a validar con negocio:** uso real del `Redondeo`; prioridad exacta grilla-beneficiarios + filtro menor/mayor precio.

**Nota de trabajo:** el usuario es programador senior autodidacta — pide que se le expliquen los términos técnicos sin jerga y prefiere confirmar antes de modificaciones. Autorizó escritura sin confirmación previa dentro de `.claude/`.
