---
name: decisiones-dominio
description: Domain design decisions for the new engine (units, monto vs cantidad, repetition) and how the legacy handles them implicitly
metadata:
  type: project
---

Decisiones de dominio para el motor nuevo (insumo para `/speckit-specify`; ya reflejadas en la constitución, Principios III y IV):

- **Unidad de medida y divisibilidad OBLIGATORIAS:** atributos explícitos del participante/artículo (carne = fraccionable/decimal; lámpara = entero). Sin "1" por defecto oculto; si faltan, es **error de configuración**, no un valor mágico.
- **Monto vs cantidad:** dimensiones distintas, NO se conflacionan, **pero se relacionan a través de los ítems** (una condición de monto se satisface con precio × cantidad de los ítems que califican). Prohibido el hack legacy de cargar `cantidad=2` para cubrir "artículo + monto".
- **Repetición/consumo CONFIGURABLE por promoción:** "aplicar una sola vez" vs "repetir mientras califique" (con tope opcional), y cómo se agrupa el consumo → lo decide el negocio al armar la promo; lo gobierna la orquestación. NO se deriva del operador de comparación.
- **Comportamiento legacy (a replicar/mejorar bajo paridad):** hoy es **implícito según el operador**: `=` corta en la cantidad y **repite por defecto** (no configurable); `>` / `>=` da por cumplida al alcanzar el valor y **asigna a la promo todo lo que cumple hacia arriba** (con errores en casos puntuales). El rediseño lo hace explícito y configurable.

Related: [[promociones-modelo-aplicacion]], [[promociones-modelo-dominio]], [[decision-alcance-v1]].
