---
description: "Analizar cómo está configurada una promoción específica: qué participantes evalúa, qué reglas aplica, qué beneficio otorga y qué casos cubre. Usar para entender o documentar una promoción existente."
argument-hint: "Id de la promoción o descripción del comportamiento a analizar"
agent: "agent"
---

# Analizar configuración de una promoción

Analizar la configuración de la siguiente promoción del Motor de Promociones:

## Promoción a analizar
{{id de la promoción o descripción del comportamiento}}

## Qué se debe explicar

### 1. Estructura de la promoción
- Qué participantes (`ParticipanteRegla`) intervienen y cuál es su ruta XPath (`Codigo`)
- Qué reglas tiene cada participante (atributo, comparador, valor)
- Cómo se combinan las reglas con `RelaReglas` (and/or)
- Cuál es el participante beneficiario (`Beneficiario = true`)

### 2. Beneficio aplicado
- Qué atributo se modifica (`DESCUENTO`, `MONTOFINAL`, `MONTO`, `MONTODESCUENTO`)
- Qué tipo de alteración se aplica (`CambiarValor` o `DisminuirEnPorcentaje`)
- A qué destinos aplica el beneficio

### 3. Tipo de promoción asociado
- Qué clase `TipoPromocion` corresponde
- Qué `BeneficioType` usa
- Qué validaciones se aplican al configurarla

### 4. Casos cubiertos y no cubiertos
- Escenarios donde la promoción aplica correctamente
- Escenarios donde no aplica (y por qué)
- Casos borde a tener en cuenta

### 5. Tests relacionados
Buscar en `ZooLogicSA.Promociones.Tests/` los tests que verifican esta promoción o comportamiento similar.

Consultar [dominio.instructions.md](../instructions/dominio.instructions.md) para el modelo de datos.
