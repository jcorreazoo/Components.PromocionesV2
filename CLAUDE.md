# CLAUDE.md

## Memoria del proyecto

La memoria persistente de este proyecto vive **dentro del repo**, en [`.claude/memory/`](.claude/memory/), no en la carpeta de memoria por defecto del perfil de usuario.

**Al iniciar una sesión:** leé [`.claude/memory/INDEX.md`](.claude/memory/INDEX.md) y los archivos que sean relevantes a la tarea.

**Al guardar/actualizar memoria del proyecto:** escribí los archivos en `.claude/memory/` (un hecho por archivo, con frontmatter `name`/`description`/`metadata.type`) y agregá su línea en `.claude/memory/INDEX.md`. No uses la carpeta de memoria por defecto del usuario para conocimiento de este proyecto.

## Contexto

Reescritura desde cero del motor de promociones (`ZooLogicSA.Promociones`) usando metodología SDD con Spec Kit. Ver los archivos de memoria para el objetivo, el consumidor (facturación en Visual FoxPro 9 + SQL Server) y el contrato relevado de la DLL legacy.
