---
name: repo-produccion-intocable
description: CRITICAL — there is a SEPARATE production repo for Promociones that must NEVER be touched; this repo (PromocionesV2) is the rewrite sandbox
metadata:
  type: project
---

**Regla de seguridad crítica:**

- **Este repo** (`Components.PromocionesV2`, en la cuenta personal de GitHub del usuario `jcorreazoo`, pero es trabajo de la empresa) es el **entorno de la reescritura/rediseño** (sandbox). Es una copia/versión sobre la que trabajamos libremente (relevar, borrar, force-push, etc.).
- **En OTRO repositorio aparte vive la versión de PRODUCCIÓN** del sistema de promociones. Ese repo **NO se toca JAMÁS, por ningún motivo**. Por eso este se llama **PromocionesV2** (la V2 / rediseño, no el sistema productivo).

**How to apply:** trabajar únicamente en este repo. Nunca acceder, clonar para modificar, ni tocar el repo de producción. El tag de archivo que crearemos acá (**`snapshot-inicial`**) es solo un snapshot del código pre-reescritura DE ESTE repo, NO es producción. Se evitó a propósito el nombre `legacy` porque se confunde con la versión de producción Y con el sector "Legacy" de la empresa. Ver [[estado-actual]].
