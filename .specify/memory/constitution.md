<!--
Sync Impact Report
- Version change: (inicial) → 1.0.0
- Tipo de cambio: creación inicial (ratificación)
- Principios (nuevos):
    I.   Núcleo Aislado (Arquitectura Hexagonal)
    II.  Motor Data-Driven con Mecánicas Extensibles
    III. Dominio Correcto: Magnitudes y Dimensiones
    IV.  Orquestación Explícita
    V.   Separación Autoría / Evaluación
    VI.  Verificable y Testeable (SDD)
    VII. Diseñar Contra la Visión (Evolución Aditiva) — Principio Rector
- Secciones añadidas: Restricciones Técnicas; Flujo de Desarrollo; Governance
- Secciones removidas: ninguna (creación inicial)
- Templates dependientes (revisados):
    ✅ .specify/templates/plan-template.md  — alineado (genérico; su "Constitution Check" referencia la constitución en runtime)
    ✅ .specify/templates/spec-template.md  — alineado (genérico; sin referencias a principios específicos)
    ✅ .specify/templates/tasks-template.md — alineado (genérico; nota: por Principio VI, nuestros specs SIEMPRE piden tests aunque el template los marque opcionales)
- Follow-up TODOs:
    - Ninguno pendiente. Fecha de ratificación = 2026-07-24 (fecha de creación; ajustable a pedido).
-->

# Constitución del Motor de Promociones ZooLogicSA (V2)

## Core Principles

### I. Núcleo Aislado (Arquitectura Hexagonal)
El motor de promociones es un **núcleo de cálculo puro** con un contrato propio, limpio y neutral
(puertos = interfaces + DTOs). Toda integración con sistemas externos —hoy, la facturación en
Visual FoxPro 9 mediante comprobante XML y el bridge `_screen.Zoo`— DEBE vivir en **adaptadores
internos**, en el mismo proceso, reemplazables sin tocar el núcleo. Nada externo "en el medio"
(sin servicios/middleware aparte). El núcleo NO conoce a sus consumidores ni depende de tecnologías
externas; cualquier nuevo consumidor se integra escribiendo su propio adaptador contra el contrato
del núcleo.
**Rationale:** el diseño original no contempló cambios de frontera; aislar el núcleo permite
evolucionar o reemplazar la integración (incluso salir de FoxPro) sin reescribir la lógica.

### II. Motor Data-Driven con Mecánicas Extensibles
Se distinguen **dos cosas** que NO deben confundirse:
- **Mecánica** (o *tipo* de promoción): la pieza que programa el **desarrollador** —«% de descuento»,
  «2x1», «precio fijo», «cumpleaños del cliente», …—. Cada mecánica DEBE vivir en su **propia clase**
  (patrón Strategy). Agregar un tipo nuevo = **agregar una clase**, sin tocar las existentes, para
  cubrir pedidos futuros de clientes que hoy no imaginamos.
- **Promoción configurada** (instancia): lo que el **negocio arma en el formulario** eligiendo una
  mecánica y definiendo **cuándo y cómo** se aplica (ej.: «3x2 en gaseosas del 1 al 10»). Esto es
  **datos/configuración, NO código**.

Queda PROHIBIDO crear una clase de código por cada **promoción configurada** (esas son datos); lo que
sí se crea por clase son las **mecánicas/tipos**. El modelo es **componible** —`(condición) →
(beneficio)`— en lugar de un catálogo cerrado de tipos.
**Rationale:** el catálogo fijo de 8 tipos "quedó chico"; mecánicas como clases (extensibles por el
dev) + promociones como datos (autorables por el negocio) permite sumar tipos nuevos sin tocar el
motor y mantiene la autoría en manos del negocio.

### III. Dominio Correcto: Magnitudes y Dimensiones
La cantidad DEBE representarse como **magnitud real** (decimales + unidad de medida), nunca como
entero de unidades con "1" por defecto. **"Monto" y "cantidad de unidades" son dimensiones distintas**
y NO deben conflacionarse. Queda PROHIBIDO el round-trip importe→cantidad→importe; cada dimensión se
calcula en su propia unidad con **precisión decimal**, de modo que las comparaciones exactas (`=`)
sean confiables. La **unidad de medida** y la **divisibilidad** (si el artículo se fracciona o no)
son atributos **explícitos y OBLIGATORIOS del participante/artículo** (la carne se fracciona; la
lámpara no); si faltan, es **error de configuración**, nunca un "1" por defecto oculto. Separar monto
de cantidad NO implica que no se relacionen: una condición de **monto** se satisface **a través de
los ítems** (precio × cantidad); lo prohibido es **conflacionarlas**.
**Rationale:** estos atajos del diseño original son la raíz de bugs reales (250 g con unidad en kg;
cargar cantidad=2 para cubrir monto+artículo; diferencias por redondeo en comparaciones exactas).

### IV. Orquestación Explícita
La coexistencia de varias promociones en una misma compra DEBE resolverse en una **capa de
orquestación explícita** que gobierne: acumulación (stacking), exclusividad, **prioridad
configurable**, exclusiones y consumo compartido de participantes. Los "beneficiarios" son un
**subconjunto de los participantes de la condición** (no una lista separada). El **alcance carrito
vs línea** es un concepto de primera clase. Cuando una promo puede cumplirse **varias veces** en la
misma compra, **repetir vs aplicar una sola vez** —y **cómo se agrupa el consumo**— es
**configurable por promoción** (con tope opcional) y lo resuelve la orquestación; NO se deriva
implícitamente del operador de comparación (como en el legacy).
**Rationale:** lo difícil no es el paralelismo sino cómo conviven las promos; hoy la prioridad está
hardcodeada en varios niveles, y el comportamiento de repetición/consumo se deriva **implícitamente**
del operador (`=` corta en la cantidad y repite por defecto, no configurable; `>`/`>=` da por
cumplida al alcanzar el valor y asigna a la promo todo lo que cumple hacia arriba, con errores en
casos puntuales).

### V. Separación de Responsabilidades (Motor / Autoría / Presentación)
El **motor de evaluación** (cálculo puro) NO DEBE contener detalles de **presentación** (máscaras,
etiquetas, controles) ni de **persistencia** (SQL). La **autoría de promociones (UI)** es un
**proyecto separado dentro de la misma solución**, consumidor del núcleo; la UX de **asesor en tiempo
real** (Asistente: semáforo verde/amarillo/gris + "qué falta" + aplicar) es otro consumidor.
Aclaración: el problema de hoy NO es que el motor "tenga pegada la UI" (los proyectos ya están
separados), sino que clases de **dominio** (p. ej. `TipoPromocion` en `Negocio`) están
**contaminadas con presentación (máscaras/labels) y SQL crudo** (`ObtenerWhereAdicional`); eso se
elimina.
**Rationale:** la separación física de proyectos ya existe, pero la separación de *responsabilidades*
está filtrada; limpiarla permite rehacer la UI (salir de DevExpress) sin tocar el dominio.

### VI. Verificable y Testeable, sin Depender de FoxPro (SDD)
El desarrollo es **dirigido por especificaciones** (SDD). El motor DEBE ser **independientemente
testeable** y compilable/ejecutable desde el entorno de desarrollo (net472 verificado). Además, DEBE
poder **ejercitarse end-to-end SIN pasar por FoxPro**, mediante: (a) una **suite de tests
automatizados** y (b) un **banco de pruebas / arnés** (proyecto aparte en la misma solución) que
cargue comprobantes de ejemplo y muestre el resultado. **Nunca** debe ser necesario "compilar y pegar
en la app FOX" para verificar comportamiento. La **paridad con el sistema actual** DEBE demostrarse
con pruebas automatizadas antes de considerar completa la v1.
**Rationale:** cerrar el loop de prueba localmente acelera el desarrollo y es la única forma de
reescribir con confianza sin perder la esencia.

### VII. Diseñar Contra la Visión (Evolución Aditiva) — Principio Rector
La v1 entrega **paridad con el sistema actual + orquestación robusta**. Pero las abstracciones DEBEN
diseñarse **contra la visión completa** (motor estilo supermercado: exclusiones, segmentación,
condiciones temporales, cupón, referido). Toda decisión de arquitectura se valida con la pregunta:
*"¿esto permite llegar a la visión AGREGANDO, sin rehacer?"*. Se construye el **esqueleto completo**
y se implementan solo los **músculos de la v1**.
**Rationale:** evita el error original de diseñar corto de miras y garantiza que crecer sea aditivo,
no un nuevo rewrite.

## Restricciones Técnicas

- **Framework destino:** .NET Framework **4.7.2** (`net472`) — decisión de empresa. Si la empresa
  actualiza el framework, se actualiza también aquí.
- **Formato de proyecto:** SDK-style + `<PackageReference>` (no `packages.config`); referencias de
  framework explícitas; plataforma según requiera el bridge (el legacy es x86).
- **El motor NO accede a SQL Server** ni a ninguna base directamente: es cálculo puro; promociones,
  redondeos y precios se le **inyectan** desde afuera.
- **Frontera FoxPro:** el contrato actual (comprobante **XML** de entrada; objeto de resultado tipo
  `InformacionPromocion` de salida) se preserva a través del **adaptador**; el núcleo mantiene su
  propio contrato limpio y neutral.
- **Repositorio de producción INTOCABLE:** el sistema productivo vive en OTRO repositorio que NO se
  toca por ningún motivo. Este repositorio es el **sandbox** del rediseño.

## Flujo de Desarrollo

- **Metodología:** SDD con Spec Kit — `/speckit-constitution` → `/speckit-specify` → `/speckit-plan`
  → `/speckit-tasks` → `/speckit-implement` (opcionales: `clarify` / `analyze` / `checklist`).
- **Base de conocimiento:** `docs/RELEVAMIENTO.md` + la memoria del proyecto en `.claude/memory/`
  son insumo de todas las especificaciones.
- **Archivo del legacy:** el código previo a la reescritura se conserva en el tag `snapshot-inicial`
  (consultable con git; nunca se toca el repositorio de producción).
- **Compilar y probar localmente:** todo cambio de producto DEBE poder compilarse y ejercitarse
  desde este entorno (net472 verificado).

## Governance

Esta constitución **prevalece** sobre otras prácticas del proyecto. Las enmiendas DEBEN documentarse
y versionarse (SemVer): **MAJOR** para cambios incompatibles de principios/gobernanza, **MINOR** para
principios o secciones nuevas, **PATCH** para aclaraciones. Toda spec y plan DEBE validarse contra
estos principios (la "Constitution Check" de `/speckit-plan`). Toda complejidad añadida DEBE
justificarse contra el **Principio VII** (evolución aditiva): si una decisión obliga a un futuro
rewrite para alcanzar la visión, se rechaza o se rediseña.

**Version**: 1.0.0 | **Ratified**: 2026-07-24 | **Last Amended**: 2026-07-24
