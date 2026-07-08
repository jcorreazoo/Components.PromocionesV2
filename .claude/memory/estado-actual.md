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
4. **Archivar legacy:** crear y pushear rama/tag `legacy` con el código viejo (NO se borra) ANTES de limpiar
5. Borrar código viejo conservando: `.claude/`, `CLAUDE.md`, dirs de Spec Kit, `docs/`, `.gitignore`, `.autorc`, `.teamcity`
6. Historial limpio en master → rama huérfana + force-push a GitHub

**Relevamiento cerrado:** stack, interop (bridge `_screen.Zoo`), contrato entrada/salida, ciclo de vida, modelo de dominio completo, catálogo de 8 tipos, base de datos (motor de cálculo puro), problemas de diseño y visión del sistema nuevo. Todo consolidado en **`docs/RELEVAMIENTO.md`** y distribuido en las memorias: [[promociones-dll-contract]], [[promociones-consumer-vfp9]], [[promociones-lifecycle-ui]], [[promociones-modelo-dominio]], [[promociones-vision-nueva]].

**Documentación revisada y decisiones de alcance tomadas** (ver [[decision-alcance-v1]]): rewrite completo (motor+UI+asistente), FoxPro aislado tras adaptador (hexagonal), v1 = paridad + orquestación robusta. El `RELEVAMIENTO.md` está completo y listo como insumo.

**Próximo paso:** iniciar Spec Kit (paso 3). Antes de eso: instalar `uv` en Windows (`winget install --id=astral-sh.uv` o `irm https://astral.sh/uv/install.ps1 | iex`). Luego `specify init` y armar la constitución tomando `docs/RELEVAMIENTO.md` + las decisiones ([[decision-framework-destino]], [[decision-alcance-v1]]) como insumo. Enriquecimientos opcionales pendientes para el spec: glosario de términos de negocio (característica, familia) y ejemplos reales de promos en XML como fixtures.

**Pendientes menores a validar con negocio:** uso real del `Redondeo`; prioridad exacta grilla-beneficiarios + filtro menor/mayor precio.

**Nota de trabajo:** el usuario es programador senior autodidacta — pide que se le expliquen los términos técnicos sin jerga y prefiere confirmar antes de modificaciones. Autorizó escritura sin confirmación previa dentro de `.claude/`.

**Backup:** el trabajo (relevamiento + memoria + docs) se pusheó a GitHub (commit `7bb98a0`) como respaldo. Se perderá al hacer el historial limpio, pero el contenido queda en el árbol de trabajo. `.autorc`/`.teamcity`/`settings.local.json` NO se versionan/tocan por ahora: tienen que ver con **versionado y CI**, y cobran sentido recién al **migrar el proyecto terminado a producción**.
