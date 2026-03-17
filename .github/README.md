# Asistencia con GitHub Copilot

Este proyecto tiene archivos de customización para Copilot que permiten trabajar con el motor de manera más eficiente.

## ⚠️ Límite de tamaño de archivos de customización

Los archivos `.instructions.md`, `.prompt.md` y `copilot-instructions.md` **no deben superar las 500 líneas**. A partir de ese punto el contenido puede compactarse y perderse información al cargarse en el contexto.

**Regla práctica:**
- Si un archivo se acerca a las **400 líneas**, es momento de refactorizarlo.
- Extraer secciones en un skill separado (`.github/skills/<nombre>/SKILL.md`) o dividir en múltiples `.instructions.md` más específicos con `applyTo` acotado.
- Esta regla está también en `copilot-instructions.md` para que Copilot la respete automáticamente.

## Prompts disponibles

En el chat de Copilot (modo **Agent**), escribí `/` para ver los prompts disponibles:

| Prompt | Cuándo usarlo |
|--------|---------------|
| `/nuevo-beneficio` | Agregar un nuevo tipo de promoción/beneficio completo (enum + clase + factory + tests) |
| `/nuevo-test` | Generar tests MSTest para cualquier clase del proyecto |
| `/analizar-promocion` | Entender cómo está configurada una promoción: participantes, reglas y beneficio |
| `/nueva-validacion` | Agregar una nueva validación de promoción implementando `IValidacionPromocion` |

**Ejemplo de uso:**
1. Abrir GitHub Copilot Chat en VS Code
2. Seleccionar modo **Agent**
3. Escribir `/nuevo-beneficio` y presionar Enter
4. Describir el tipo de promoción que querés agregar

## Instrucciones de contexto

Las siguientes instrucciones se cargan automáticamente cuando son relevantes:

| Archivo | Se activa cuando... |
|---------|---------------------|
| `instructions/dominio.instructions.md` | Se trabaja con el modelo de datos (Promocion, Regla, Beneficio, etc.) |
| `instructions/tests.instructions.md` | Se edita o crea un archivo `*Test.cs` |
| `instructions/beneficios.instructions.md` | Se trabaja en `ZooLogicSA.Promociones.Negocio/` |

## Estructura de archivos

```
.github/
├── copilot-instructions.md          ← contexto global, siempre activo
├── README.md                        ← este archivo
├── instructions/
│   ├── dominio.instructions.md
│   ├── tests.instructions.md
│   └── beneficios.instructions.md
└── prompts/
    ├── nuevo-beneficio.prompt.md
    ├── nuevo-test.prompt.md
    ├── analizar-promocion.prompt.md
    └── nueva-validacion.prompt.md
```
