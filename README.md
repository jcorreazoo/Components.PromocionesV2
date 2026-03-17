# Motor de Promociones — ZooLogicSA

Motor de evaluación de promociones comerciales para el sistema de facturación de ZooLogicSA. Recibe un comprobante (XML) y una librería de promociones, evalúa cuáles aplican y retorna los beneficios a aplicar.

## Proyectos

| Proyecto | Descripción |
|----------|-------------|
| `ZooLogicSA.Promociones` | DLL principal: motor, evaluador de reglas, parsing XML |
| `ZooLogicSA.Promociones.Negocio` | Tipos de promoción, tipos de beneficio, validaciones |
| `ZooLogicSA.Promociones.UI` | Controles WinForms para mostrar/configurar promociones |
| `ZooLogicSA.Promociones.Asistente` | App standalone de estado de promociones en tiempo real |
| `ZooLogicSA.Promociones.Tests` | Tests MSTest — más de 50 clases de test |

## Build y tests

```powershell
# Build completo
dotnet build ZooLogicSA.Promociones.sln

# Todos los tests
dotnet test ZooLogicSA.Promociones.sln

# Solo los tests
dotnet test ZooLogicSA.Promociones.Tests\ZooLogicSA.Promociones.Tests.csproj
```

---

Para información sobre los prompts y la asistencia con GitHub Copilot, ver [.github/README.md](.github/README.md).
