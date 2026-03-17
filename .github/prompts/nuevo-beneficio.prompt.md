---
description: "Guía paso a paso para agregar un nuevo tipo de beneficio/promoción al motor. Crea la clase TipoPromocion, actualiza el enum BeneficioType, actualiza FactoryBeneficio y genera los tests."
argument-hint: "Nombre y descripción del nuevo tipo de promoción (ej: 'DescuentoPorVolumen: descuento por comprar más de N unidades')"
agent: "agent"
---

# Nuevo tipo de promoción

Voy a agregar un nuevo tipo de promoción al Motor de Promociones.

## Descripción del nuevo tipo
{{mensaje del usuario con nombre y descripción}}

## Pasos a seguir

### 1. Agregar al enum `BeneficioType`
Abrir `ZooLogicSA.Promociones.Negocio/Clases/Beneficios/BeneficioType.cs` y agregar el nuevo valor al final del enum, con el siguiente número correlativo.

### 2. Crear la clase `TipoPromocion`
Crear el archivo `ZooLogicSA.Promociones.Negocio/Clases/Promociones/NuevaTipoPromocion.cs` siguiendo exactamente la estructura de `LLevaXpagaY.cs` o `MontoAplicaDescuento.cs` como referencia. Implementar todos los métodos abstractos.

### 3. Actualizar `FactoryBeneficio`
En `ZooLogicSA.Promociones.Negocio/Clases/Beneficios/FactoryBeneficio.cs`, agregar el nuevo `case` en el `switch` de `ObtenerBeneficio()`, configurando `Cambio`, `Atributo` y `Valor` apropiados.

### 4. Actualizar `FactoryPromociones`
Buscar en `ZooLogicSA.Promociones/FactoriaPromociones.cs` dónde se registran los tipos de promoción y agregar el nuevo.

### 5. Generar tests
Crear `ZooLogicSA.Promociones.Tests/NuevaTipoPromocionTest.cs` con al menos:
- Test que verifica que un participante válido aplica al tipo
- Test que verifica que un participante inválido no aplica
- Test de integración con `MotorPromociones` usando `FactoriaRecursosAdicionalesParaTest`

### 6. Verificar build
```powershell
cd "c:\ZooGit\Organic\Components.PromocionesV2"
dotnet build ZooLogicSA.Promociones.sln
dotnet test ZooLogicSA.Promociones.Tests\ZooLogicSA.Promociones.Tests.csproj
```

Consultar [beneficios.instructions.md](../instructions/beneficios.instructions.md) para los patrones exactos.
