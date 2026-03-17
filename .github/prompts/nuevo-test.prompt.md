---
description: "Generar tests MSTest para una clase del proyecto Motor de Promociones. Sigue los patrones existentes: MSTest, FactoriaRecursosAdicionalesParaTest, nombres en español."
argument-hint: "Nombre de la clase a testear y descripción de los escenarios a cubrir"
agent: "agent"
---

# Generar tests para una clase

Generar tests MSTest para la siguiente clase del Motor de Promociones:

## Clase a testear
{{nombre de la clase y escenarios a cubrir}}

## Pasos

### 1. Leer la clase
Leer el código fuente de la clase indicada para entender sus métodos públicos, casos felices y casos de error.

### 2. Revisar tests existentes similares
Buscar si ya existe un archivo `*Test.cs` relacionado. Si existe, usarlo como referencia de estilo y estructura.

### 3. Crear o actualizar el archivo de tests
El archivo debe estar en `ZooLogicSA.Promociones.Tests/` con nombre `NombreClaseTest.cs`.

Reglas obligatorias:
- Namespace: `ZooLogicSA.Promociones.Tests`
- Atributos: `[TestClass]` en la clase, `[TestMethod]` en cada test
- Nombres de métodos: descriptivos en español, formato `VerificaQue[escenario]`
- Usar `FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento()` para la config
- Para tests del motor: construir `Promocion` → `ParticipanteRegla` → `Regla` con `RelaReglas`
- Incluir mensaje descriptivo en los `Assert`

### 4. Cubrir como mínimo
- Caso exitoso (happy path)
- Caso de fallo esperado
- Caso con datos límite o borde si aplica

### 5. Verificar que compilan y pasan
```powershell
cd "c:\ZooGit\Organic\Components.PromocionesV2"
dotnet test ZooLogicSA.Promociones.Tests\ZooLogicSA.Promociones.Tests.csproj
```

Consultar [tests.instructions.md](.github/instructions/tests.instructions.md) para los patrones de testing.
