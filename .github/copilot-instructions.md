# Motor de Promociones — Instrucciones del Workspace

## Regla crítica — Lectura obligatoria al inicio de cada sesión

**Al abrir este workspace, SIEMPRE leer los siguientes archivos antes de responder cualquier consulta:**

1. `.github/copilot-instructions.md` ← este archivo (contexto global)
2. `.github/instructions/dominio.instructions.md` ← modelo de datos y flujo de evaluación
3. `.github/instructions/beneficios.instructions.md` ← cómo extender tipos de beneficio
4. `.github/instructions/tests.instructions.md` ← patrones de testing
5. `.github/README.md` ← prompts disponibles y reglas de mantenimiento

No esperar a que el usuario los mencione. Leerlos proactivamente en cada nueva sesión.

## Regla crítica — Límite de tamaño de archivos de customización

**Al inicio de cada sesión, y antes de crear o editar cualquier archivo `.instructions.md`, `.prompt.md` o `copilot-instructions.md`:**

1. Verificar el tamaño de cada archivo de customización en `.github/`.
2. Si algún archivo supera las **500 líneas**, el contenido empieza a compactarse y se pierde información.
3. Si se detecta un archivo cercano o mayor a 500 líneas: **refactorizarlo** extrayendo secciones en un skill (`.github/skills/<nombre>/SKILL.md`) o dividiéndolo en múltiples `.instructions.md` más específicos con `applyTo` más acotados.
4. Nunca agregar contenido a un archivo que ya supere las 400 líneas sin antes refactorizar.

## ¿Qué hace este proyecto?
Motor de evaluación de promociones comerciales para un sistema de facturación. Recibe un comprobante (XML) y una librería de promociones, evalúa cuáles aplican y retorna los beneficios a aplicar.

## Estructura de proyectos

| Proyecto | Rol |
|----------|-----|
| `ZooLogicSA.Promociones` | DLL principal: `MotorPromociones`, `ServicioEvaluacionPromociones`, evaluación de reglas, parsing XML |
| `ZooLogicSA.Promociones.Negocio` | Tipos de promoción (`TipoPromocion`), tipos de beneficio (`BeneficioType`), validaciones |
| `ZooLogicSA.Promociones.UI` | Controles WinForms: `ControlPromociones`, `MaskPromocion` |
| `ZooLogicSA.Promociones.Asistente` | Aplicación standalone que muestra estado de promociones en tiempo real |
| `ZooLogicSA.Promociones.Tests` | Tests MSTest (.NET) — más de 50 clases de test |

## Convenciones de código

- **Idioma**: todo en español (nombres de clases, métodos, variables, mensajes)
- **Namespaces**: `ZooLogicSA.Promociones.*`
- **Interfaces**: prefijo `I` (ej: `IMotorPromociones`, `IComprobante`)
- **Factories**: se llaman `Factoria*` o `Factory*` según el contexto
- **Tests**: clase de test = nombre de la clase + `Test` (ej: `LLevaXpagaYTest`)
- **Campos privados**: prefijo `_` o sin prefijo con `this.`

## Build y tests

```powershell
# Build de la solución completa
cd "c:\ZooGit\Organic\Components.PromocionesV2"
dotnet build ZooLogicSA.Promociones.sln

# Ejecutar todos los tests
dotnet test ZooLogicSA.Promociones.sln

# Ejecutar tests de un proyecto específico
dotnet test ZooLogicSA.Promociones.Tests\ZooLogicSA.Promociones.Tests.csproj
```

## Decisiones de diseño clave

- `ConfiguracionComportamiento` controla qué atributos del XML mapean a qué conceptos (precio, cantidad, descuento). Se configura una vez y se pasa a toda la cadena.
- `IComprobante` abstrae el XML de la factura — nunca acceder al XML directamente, siempre a través de la interfaz.
- Los participantes en una promoción se identifican por su ruta XPath en el XML (ej: `Comprobante.Facturadetalle.Item`).
- Las reglas se vinculan entre sí mediante `RelaReglas` que usa notación `{id}` con operadores `and`/`or`.
- `TipoPromocion` (abstract) es el punto de extensión principal para agregar nuevos tipos de promoción en `Negocio`.
