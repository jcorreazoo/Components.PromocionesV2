---
description: "Agregar una nueva validación de promoción implementando IValidacionPromocion. Crea la clase, la registra en ValidacionPromoType y FactoryPromociones, y genera los tests."
argument-hint: "Nombre y descripción de la nueva validación (ej: 'ValidarFechaVigencia: verificar que la promo esté dentro del rango de fechas')"
agent: "agent"
---

# Nueva validación de promoción

Agregar una nueva validación al Motor de Promociones que se ejecuta al activar una promoción.

## Validación a implementar
{{nombre y descripción de la nueva validación}}

## Pasos

### 1. Revisar validaciones existentes
Leer las clases en `ZooLogicSA.Promociones.Negocio/Clases/Validaciones/` para entender el patrón:
- `IValidacionPromocion` — interfaz que deben implementar
- `ValidacionPromoType` — enum con los tipos registrados
- `ValidarCantidadParticipantes.cs` — ejemplo de validación existente
- `ValidarPromocion.cs` — orquestador que ejecuta todas las validaciones

### 2. Agregar al enum `ValidacionPromoType`
Abrir `ValidacionPromoType.cs` y agregar el nuevo valor.

### 3. Implementar `IValidacionPromocion`
Crear `ZooLogicSA.Promociones.Negocio/Clases/Validaciones/ValidarNuevaRegla.cs`:

```csharp
namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarNuevaRegla : IValidacionPromocion
    {
        public bool Validar(/* parámetros según interfaz */)
        {
            // lógica de validación
        }

        public string ObtenerMensajeDeError()
        {
            return "Mensaje descriptivo del error de validación.";
        }
    }
}
```

### 4. Registrar en `ManagerValidaciones` o `ValidarPromocion`
Buscar dónde se instancian las validaciones según el `ValidacionPromoType` y agregar el nuevo caso.

### 5. Asociar a uno o más `TipoPromocion`
En las clases de `TipoPromocion` que deban usar esta validación, asignar:
```csharp
this._typeValidacion = ValidacionPromoType.NuevaValidacion;
```

### 6. Crear los tests
Crear `ZooLogicSA.Promociones.Tests/ValidarNuevaReglaTest.cs` con casos:
- Caso donde la validación pasa
- Caso donde la validación falla
- Caso con datos límite

### 7. Verificar build y tests
```powershell
cd "c:\ZooGit\Organic\Components.PromocionesV2"
dotnet build ZooLogicSA.Promociones.sln
dotnet test ZooLogicSA.Promociones.Tests\ZooLogicSA.Promociones.Tests.csproj
```
