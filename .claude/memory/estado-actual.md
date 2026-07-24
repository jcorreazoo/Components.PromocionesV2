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
3. **Inicializar Spec Kit** ← ✅ HECHO (`uv` instalado via winget; `specify init --here --integration claude --script ps --force`; skills en `.claude/skills/`, infra en `.specify/`; commit `6ab455b`)
4. **Archivar el código pre-reescritura:** crear y pushear el **tag `snapshot-inicial`** con el código viejo (NO se borra) ANTES de limpiar. Para consultarlo luego sin ensuciar el proyecto nuevo: `git grep <patrón> snapshot-inicial`, `git show snapshot-inicial:<ruta>`, o `git worktree add ../snapshot-wt snapshot-inicial` (copia aparte donde funcionan Read/Grep/Glob normalmente). (Se evita el nombre `legacy`: se confunde con producción y con el sector "Legacy" de la empresa.)
5. Borrar código viejo conservando: `.claude/`, `CLAUDE.md`, dirs de Spec Kit, `docs/`, `.gitignore`, `.autorc`, `.teamcity`
6. Historial limpio en master → rama huérfana + force-push a GitHub

**Relevamiento cerrado:** stack, interop (bridge `_screen.Zoo`), contrato entrada/salida, ciclo de vida, modelo de dominio completo, catálogo de 8 tipos, base de datos (motor de cálculo puro), problemas de diseño y visión del sistema nuevo. Todo consolidado en **`docs/RELEVAMIENTO.md`** y distribuido en las memorias: [[promociones-dll-contract]], [[promociones-consumer-vfp9]], [[promociones-lifecycle-ui]], [[promociones-modelo-dominio]], [[promociones-vision-nueva]].

**Documentación revisada y decisiones de alcance tomadas** (ver [[decision-alcance-v1]]): rewrite completo (motor+UI+asistente), FoxPro aislado tras adaptador (hexagonal), v1 = paridad + orquestación robusta. El `RELEVAMIENTO.md` está completo y listo como insumo.

**Constitución:** ✅ v1.0.0 en `.specify/memory/constitution.md` (commit `f1c9a31`). 7 principios: I Núcleo Aislado (hexagonal), II Motor Data-Driven (mecánica=clase, promo=dato), III Dominio Correcto (unidad obligatoria, monto≠cantidad), IV Orquestación (repetición configurable), V Separación de Responsabilidades (dominio sin UI/SQL), VI Verificable sin FoxPro (tests + banco de pruebas), VII Diseñar Contra la Visión (aditivo). Ver también [[decisiones-dominio]].

**Próximo paso:** `/speckit-specify` — crear la especificación de la v1 tomando `docs/RELEVAMIENTO.md` + la constitución + [[decisiones-dominio]] como insumo. Después `/speckit-plan` → `/speckit-tasks` → `/speckit-implement` (opcionales: clarify/analyze/checklist). Los comandos son skills `/speckit-*`. Recordar: en `/plan` se define la estructura de proyectos (núcleo, adaptador FoxPro, UI, Asistente, tests, banco de pruebas).

**Nota `uv`/entorno:** `uv` se instaló con winget; en shells nuevos el PATH necesita refresco: `$env:Path = [Environment]::GetEnvironmentVariable('Path','Machine') + ';' + [Environment]::GetEnvironmentVariable('Path','User')`. Enriquecimientos opcionales para el spec: glosario de negocio (característica, familia) y ejemplos reales de promos en XML como fixtures.

**Pendientes menores a validar con negocio:** uso real del `Redondeo`; prioridad exacta grilla-beneficiarios + filtro menor/mayor precio.

**Nota de trabajo:** el usuario es programador senior autodidacta — pide que se le expliquen los términos técnicos sin jerga y prefiere confirmar antes de modificaciones. Autorizó escritura sin confirmación previa dentro de `.claude/`.

**Backup:** el trabajo (relevamiento + memoria + docs) se pusheó a GitHub (commit `7bb98a0`) como respaldo. Se perderá al hacer el historial limpio, pero el contenido queda en el árbol de trabajo. `.autorc`/`.teamcity`/`settings.local.json` NO se versionan/tocan por ahora: tienen que ver con **versionado y CI**, y cobran sentido recién al **migrar el proyecto terminado a producción**.
