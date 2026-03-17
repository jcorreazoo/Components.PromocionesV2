---
description: "Usar cuando se trabaja con conceptos del dominio: Promocion, Regla, Beneficio, ParticipanteRegla, Comprobante, ConfiguracionComportamiento, MotorPromociones, InformacionPromocion. Incluir para entender el modelo de datos y el flujo de evaluación."
---

# Modelo de dominio — Motor de Promociones

## Flujo principal

```
Comprobante (XML)
      │
      ▼
MotorPromociones.AgregarComprobanteParaEvaluacion(id, xml)
      │
      ▼
MotorPromociones.AplicarPromociones(id, listaDeIds)
      │
      ▼
List<InformacionPromocion>   ← resultado con beneficios aplicados
```

## Entidades clave

### `Promocion` (`ZooLogicSA.Promociones.FormatoPromociones`)
Representa una promoción comercial configurada.

| Propiedad | Tipo | Descripción |
|-----------|------|-------------|
| `Id` | string | Identificador único |
| `Participantes` | `List<ParticipanteRegla>` | Nodos del XML que deben cumplirse |
| `Beneficios` | `List<Beneficio>` | Qué se aplica cuando se cumple la promo |
| `RelaReglas` | string | Expresión lógica ej. `{1} and ({2} or {3})` |
| `AplicaAutomaticamente` | bool | Si se evalúa sin que el operador la seleccione |
| `TopeBeneficio` | decimal | Monto máximo del beneficio |
| `CuotasSinRecargo` | uint16 | Cuotas sin recargo habilitadas |

### `ParticipanteRegla`
Un nodo del comprobante XML que participa en la promoción.

| Propiedad | Descripción |
|-----------|-------------|
| `Codigo` | Ruta XPath del nodo (ej: `Comprobante.Facturadetalle.Item`) |
| `Reglas` | Lista de condiciones que debe cumplir el nodo |
| `RelaReglas` | Cómo se combinan las reglas: `{1} and {2}`, `{1} or {2}` |
| `Beneficiario` | Si este participante recibe el beneficio |

### `Regla`
Una condición sobre un atributo de un participante.

| Propiedad | Descripción |
|-----------|-------------|
| `Atributo` | Nombre del atributo XML a evaluar (ej: `Precio`, `Articulo.Codigo`) |
| `Comparacion` | Factor: `DebeSerIgualA`, `DebeSerMayorA`, `DebeSerMenorA`, `ComienzaCon`, `Contiene`, etc. |
| `Valor` | Valor contra el que se compara |

### `Beneficio`
Qué cambio se aplica al comprobante cuando la promoción se cumple.

| Propiedad | Descripción |
|-----------|-------------|
| `Atributo` | Qué atributo se modifica (`DESCUENTO`, `MONTOFINAL`, `MONTO`) |
| `Cambio` | `Alteracion`: `CambiarValor` o `DisminuirEnPorcentaje` |
| `Valor` | Valor a aplicar |
| `Destinos` | `List<DestinoBeneficio>` — a qué participantes aplica |

### `ConfiguracionComportamiento`
Mapea los nombres de atributos del XML a los conceptos del motor. Se configura una sola vez por instancia.

```csharp
config.NombreComprobante = "Comprobante";
config.AtributoMontoFinal = "MONTOFINAL";
config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Precio = "Precio";
config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Cantidad = "Cantidad";
config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].Descuento = "Descuento";
config.ConfiguracionesPorParticipante["Comprobante.Facturadetalle.Item"].FormulaCalculoPrecio =
    "( <<PRECIO>> * <<CANTIDAD>> ) * ( 1 - <<DESCUENTO>>/100 ) - <<MONTODESCUENTO>>";
```

### `InformacionPromocion`
Resultado de aplicar una promoción exitosamente.

| Propiedad | Descripción |
|-----------|-------------|
| `IdPromocion` | Id de la promoción aplicada |
| `DetalleBeneficiado` | Participantes que recibieron beneficio |
| `DetalleAfectado` | Participantes que cumplieron la condición |
| `MontoBeneficio` | Monto total del beneficio aplicado |
| `Afectaciones` | Cantidad de veces que aplicó |

## Notas de diseño

- **Nunca acceder al XML directamente**: siempre usar `IComprobante` y sus métodos `ObtenerParticipantesSegunClave`, `ObtenerNodoParticipante`.
- **`RelaReglas` es obligatoria** si hay más de una regla en un participante.
- Los participantes se identifican por su ruta XPath: `Comprobante`, `Comprobante.Facturadetalle.Item`, `Comprobante.Valoresdetalle.Item`.
- `ServicioEvaluacionPromociones` envuelve al `MotorPromociones` con evaluación en hilo separado para uso en UI.
