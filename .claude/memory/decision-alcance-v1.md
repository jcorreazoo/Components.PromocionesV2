---
name: decision-alcance-v1
description: Scope, FoxPro boundary architecture, and v1 ambition decisions for the Promociones rewrite
metadata:
  type: project
---

Decisions taken (2026-07-08) that shape the Spec Kit constitution/spec:

- **Scope:** rewrite **everything** — engine + authoring UI + assistant — properly designed, scalable, fixing the root problems from the poorly-scoped original. The **FoxPro integration is the only thing kept for now** (unless changing it becomes extremely necessary).
- **FoxPro boundary (architecture):** the current contract (comprobante XML + `_screen.Zoo` bridge) is **fixed for now but isolated behind an adapter** (ports & adapters / hexagonal). The core exposes its own clean, neutral contract (a **port** = interface + DTOs); the FoxPro integration (XML/bridge) is encapsulated in a replaceable adapter. **The communication layer is INTERNAL — nothing external in the middle:** classes inside the same solution, in-process (no separate service/middleware/process). Designed for **multiple consumers**: another system integrates by writing its own adapter (or calling the clean API), without touching the engine. Open detail for the design phase: same assembly vs separate assembly (both internal; separate helps keep the core FoxPro-agnostic).
- **v1 ambition:** **parity with the current system** (the 8 types + the 3 application modes), done right and fixing the root problems (cantidad/monto/decimales/prioridad), **plus a robust orchestration layer from the start** (stacking / exclusivity / configurable priority). The broader vision features (segmentation, temporal, coupon, referral, advanced exclusions) are deferred beyond v1.

- **Guiding principle (north star):** v1 implements parity + robust orchestration (option 2), but the **abstractions are designed against the full vision** (supermarket scheme, option 3). Build the full skeleton, add only v1's muscles. Goal: evolving toward the vision must be **additive** (new mechanics/conditions as new classes, without touching the core), **not a rewrite**. Validate every v1 architecture decision with: "does this let me reach the supermarket scheme by adding, not redoing?". Foundations that must be right from day one: a general condition model (client/date/coupon, not just article/monto/payment), orchestration that accounts for exclusions/exclusivity, cart-vs-line scope, and the quantity/monto/decimals fix.

Related: [[rewrite-promociones-goal]], [[decision-framework-destino]], [[promociones-vision-nueva]], [[promociones-dll-contract]].
