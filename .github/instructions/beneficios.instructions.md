---
description: "Usar cuando se agregan o modifican tipos de beneficio, tipos de promoción (TipoPromocion), o se trabaja con BeneficioType y FactoryBeneficio. Cubre cómo extender correctamente el sistema de beneficios."
applyTo: "ZooLogicSA.Promociones.Negocio/**"
---

# Sistema de beneficios y tipos de promoción

## Arquitectura de extensión

Para agregar un nuevo tipo de promoción se necesita modificar **4 lugares**:

```
1. BeneficioType (enum)           ← agregar nuevo valor
2. TipoPromocion (nueva subclase) ← implementar comportamiento UI
3. FactoryBeneficio               ← mapear BeneficioType → Beneficio
4. FactoryPromociones             ← registrar el nuevo tipo
```

## 1. `BeneficioType` enum

Ubicación: `ZooLogicSA.Promociones.Negocio/Clases/Beneficios/BeneficioType.cs`

```csharp
public enum BeneficioType
{
    None = 0,
    LlevaUnoPagaConDescuentoOtro = 1,   // MontoAplicaDescuento, DescuentoXcaracteristica
    LLevaXPagaY = 2,                     // LLevaXpagaY
    MontoFijoDeDescuento = 3,            // MontoAplicaMontoFijo
    PorcentajeFijoDeDescuento = 4,       // RebajaXcaracteristica
    PorcentajeFijoDeDescuentoBancario = 5, // DescuentoBancarioConTope, PromocionesBancarias
    ValorDeOtraListaDePrecios = 6        // LlevaAValorDeOtraListaDePrecios
    // NuevoTipo = 7  ← agregar acá
}
```

## 2. Subclase de `TipoPromocion`

Ubicación: `ZooLogicSA.Promociones.Negocio/Clases/Promociones/`

Plantilla base:

```csharp
using ZooLogicSA.Promociones.Negocio.Clases.Validaciones;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.Negocio.Clases.Promociones
{
    public class NuevoTipoPromocion : TipoPromocion
    {
        public NuevoTipoPromocion()
        {
            // Configurar qué controles UI se muestran
            this._tieneBeneficionMask = true;        // campo de máscara para beneficio
            this._tieneCondicionMask = true;          // campo de máscara para condición
            this._tieneCondicionLabel = true;         // label descriptivo de condición
            this._tieneCondicionFiltro = true;        // filtro de participantes condición
            this._tieneTipoPrecio = true;             // selector de tipo de precio
            this._tieneTopeDeDescuento = false;       // tope de descuento
            this._tieneRedondeo = false;              // regla de redondeo
            this._tieneListaDePrecios = false;        // lista de precios alternativa
            this._tieneCuotasSinRecargo = false;      // cuotas sin recargo

            this._typeValidacion = ValidacionPromoType.ValidarCantidadParticipantes; // o None
            this.beneficioType = BeneficioType.NuevoTipo; // el enum nuevo
        }

        public override string ObtenerDescripcion() =>
            "Descripción visible al usuario del tipo de promoción";

        public override string ObtenerMascaraCondicion() =>
            @"\Llev\ando 999";  // máscara de entrada numérica

        public override string ObtenerMascaraBeneficio() =>
            @"P\ag\a 999";

        public override string ObtenerLabelBeneficio() => "";
        public override string ObtenerLabelCondicion() => "Condición:";

        public override string ObtenerReglaCondicionSegunMask(string valorMask) =>
            "[CANTIDAD] = '" + valorMask.Trim() + "'";

        public override bool DebeAgregarReglaCondicionMask(List<string> listaCondiciones) =>
            false;

        public override string ObtenerMensajeErrorCondicionMask() =>
            "Debe cargar el campo condición.";

        public override string ObtenerMensajeErrorBeneficioMask() =>
            "Debe cargar el campo beneficio.";

        public override string ObtenerMensajeDeAyudaMascaraBeneficio() =>
            "Descripción del valor esperado en el campo beneficio.";

        public override string ObtenerMascaraTopeDescuento() =>
            throw new System.NotImplementedException();

        public override string ObtenerLabelTopeDescuento() =>
            throw new System.NotImplementedException();
    }
}
```

## 3. `FactoryBeneficio` — agregar el nuevo case

Ubicación: `ZooLogicSA.Promociones.Negocio/Clases/Beneficios/FactoryBeneficio.cs`

Atributos disponibles para `beneficio.Atributo`:
- `"DESCUENTO"` — porcentaje de descuento sobre el item
- `"MONTOFINAL"` — monto final del comprobante
- `"MONTO"` — monto del item
- `"MONTODESCUENTO"` — descuento en monto fijo

Alteraciones disponibles (`beneficio.Cambio`):
- `Alteracion.CambiarValor` — reemplaza el valor
- `Alteracion.DisminuirEnPorcentaje` — descuenta un porcentaje

```csharp
case BeneficioType.NuevoTipo:
{
    beneficio.Cambio = Alteracion.CambiarValor;
    beneficio.Atributo = cNombreAtributoDescuento;
    beneficio.Destinos = valoresBeneficio.Destinos;
    beneficio.Valor = valoresBeneficio.Valor;
    break;
}
```

## 4. Tipos de promoción existentes (referencia)

| Clase | Descripción | BeneficioType |
|-------|-------------|---------------|
| `LLevaXpagaY` | Lleva X paga Y (cantidad) | `LLevaXPagaY` |
| `MontoAplicaDescuento` | Llevando un monto, descuento en producto | `LlevaUnoPagaConDescuentoOtro` |
| `MontoAplicaMontoFijo` | Llevando un monto, descuento fijo | `MontoFijoDeDescuento` |
| `DescuentoXcaracteristica` | Descuento por característica del artículo | `LlevaUnoPagaConDescuentoOtro` |
| `RebajaXcaracteristica` | Rebaja porcentual por característica | `PorcentajeFijoDeDescuento` |
| `DescuentoBancarioConTope` | Descuento bancario con tope de monto | `PorcentajeFijoDeDescuentoBancario` |
| `LlevaAValorDeOtraListaDePrecios` | Precio de otra lista | `ValorDeOtraListaDePrecios` |
