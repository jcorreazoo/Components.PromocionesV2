---
description: "Usar cuando se escriben, corrigen o amplían tests. Patrones del proyecto para MSTest, construcción de promociones de prueba, uso de FactoriaRecursosAdicionalesParaTest y RhinoMocks."
applyTo: "**/*Test.cs"
---

# Patrones de testing — Motor de Promociones

## Framework y estructura

- **Framework**: MSTest (`Microsoft.VisualStudio.TestTools.UnitTesting`)
- **Mocks**: RhinoMocks (`Rhino.Mocks`)
- **Clase de test**: nombre de la clase bajo test + `Test` (ej: clase `LLevaXpagaY` → `LLevaXpagaYTest`)
- **Ubicación**: `ZooLogicSA.Promociones.Tests/`
- **Namespace**: `ZooLogicSA.Promociones.Tests`

## Estructura mínima de un test

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class NombreClaseTest
    {
        [TestMethod]
        public void DescripcionEnEspañolaQueExplicaElEscenario()
        {
            // Arrange
            // ...

            // Act
            // ...

            // Assert
            Assert.IsTrue(resultado, "Mensaje descriptivo del fallo.");
        }
    }
}
```

## Construir una Promocion de prueba

```csharp
// Patrón estándar para armar una Promoción en tests
Promocion promocion = new Promocion() { Id = "1" };

ParticipanteRegla participante = new ParticipanteRegla();
participante.Id = "1";
participante.Codigo = "Comprobante.Facturadetalle.Item";   // ruta XPath

Regla regla = new Regla();
regla.Id = 1;
regla.Atributo = "Articulo.Codigo";
regla.Comparacion = Factor.DebeSerIgualA;
regla.Valor = "00100101";
participante.Reglas.Add(regla);

participante.RelaReglas = "{1}";   // obligatorio, incluso con una sola regla
promocion.Participantes.Add(participante);
```

## Obtener ConfiguracionComportamiento

Usar siempre `FactoriaRecursosAdicionalesParaTest` en vez de construir la config a mano:

```csharp
ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
```

Esta config ya incluye los participantes standard:
- `Comprobante`
- `Comprobante.Facturadetalle.Item`
- `Comprobante.Valoresdetalle.Item`

## Construir y ejecutar el motor en un test

```csharp
// 1. Obtener config y motor
ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
FactoriaPromociones factoria = new FactoriaPromociones();
IMotorPromociones motor = new MotorPromociones(config, factoria);

// 2. Cargar librería de promociones
motor.EstablecerLibreriaPromociones(new List<Promocion> { promocion });

// 3. Cargar el comprobante XML
motor.AgregarComprobanteParaEvaluacion("proceso1", xmlDeFactura);

// 4. Evaluar
List<InformacionPromocion> resultado = motor.AplicarPromociones("proceso1", new List<string> { "1" });

// 5. Verificar
Assert.AreEqual(1, resultado.Count);
```

## Convenciones de nombres de métodos de test

Los nombres deben ser descriptivos, en español, indicando el escenario y el resultado esperado:

```
VerificaQue[sujeto][verbo][condicion]()
Cuando[condicion]_Entonces[resultado]()
```

Ejemplos del proyecto:
- `VerificaQueCuponNoParticipaEnLLevaXpagaY`
- `ValidarPromocionUsandoCasoReal`
- `VerificaQueCualquierOtroParticipaEnLLevaXpagaY`

## Archivos de referencia en el proyecto

- `FactoriaRecursosAdicionalesParaTest.cs` — factory central de recursos para tests
- `DebugTools.cs` — utilidades de diagnóstico en tests
- `CasosBugs.cs` — casos de prueba para bugs específicos documentados
- `CasosDePrueba_AFU.cs` — casos de prueba de escenarios AFU
