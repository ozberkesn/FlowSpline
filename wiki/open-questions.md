---
title: Open Questions
updated: 2026-05-03
---

# Open Questions

Unresolved or ambiguous design questions identified during wiki ingest.
Each entry links to the relevant wiki page and the source doc that raised it.

---

## OQ-001 — Circular delegation enforcement layer

**Source:** [docs/03-domain-model.md](../docs/03-domain-model.md)  
**Related:** [AgentTeam](entities/agent-team.md)

The domain doc states: "Circular delegation yasak — domain tek aggregate'in kendi
sınırını kontrol eder; circular kontrol Application katmanında yapılır."

This means the cross-aggregate circular delegation check lives in Application, not Domain.
Is this the right placement? Circular delegation is a domain invariant, but it requires
querying the repository (which the Domain layer cannot do). The Application layer placement
is pragmatic but means the invariant can be bypassed if someone calls the domain method
directly. Is there a domain service or specification pattern that would be more appropriate?

---

## OQ-002 — Phase 2 trigger conditions

**Source:** [docs/08-roadmap.md](../docs/08-roadmap.md)  
**Related:** [DDD Bounded Contexts](concepts/ddd-bounded-contexts.md)

WorkflowEngine and Governance are "Phase 2". There is no documented trigger for when
Phase 2 begins — is it milestone-based, revenue-based, or customer-demand-based?
What is the minimum Phase 1 completion criterion?

---

## ~~OQ-003 — pgvector vs Redis for Memory~~ RESOLVED

**Source:** [docs/03-domain-model.md](../docs/03-domain-model.md), [docs/adr/002-postgresql-over-mongo.md](../docs/adr/002-postgresql-over-mongo.md)  
**Related:** [MemoryEntry](entities/memory-entry.md), [ADR-002](decisions/adr-002-postgresql-over-mongo.md), [ADR-003](decisions/adr-003-redis-runtime-memory.md)  
**Resolved:** 2026-05-09

**Decision:** The split is **complementary**:
1. **Short-term session memory → Redis** (Phase 1, implemented): `MemoryEntry` aggregate, key pattern `memory:{agentId}:{sessionId}:{key}`, TTL-based
2. **Long-term semantic memory → pgvector** (Phase 2, planned): will require a second aggregate or storage adapter for vector similarity search (RAG use case)

Phase 2 pgvector usage is deferred. The `FlowSplineDbContext` has `HasPostgresExtension("vector")` registered so the extension is created in the initial migration.

---

## OQ-004 — Multi-tenancy design

**Source:** [docs/08-roadmap.md](../docs/08-roadmap.md)  
**Related:** [DDD Bounded Contexts](concepts/ddd-bounded-contexts.md)

Phase 3 mentions multi-tenancy but there is no existing design for tenant isolation.
Key questions:
- Shared schema with tenant ID column, or separate schemas per tenant?
- How does tenant context propagate through the layers (HTTP header, DI scope, claim)?
- Which bounded contexts are most affected (likely all of them)?

---

## OQ-005 — API authentication

**Source:** [docs/06-api-spec.md](../docs/06-api-spec.md)  
**Related:** [Governance bounded context](concepts/ddd-bounded-contexts.md)

The API spec has no authentication or authorization details. Questions:
- Is auth deferred entirely to Phase 2 (Governance)?
- Is there a temporary development-mode bypass in Phase 1?
- What auth scheme is planned (JWT, API keys, OAuth2)?

---

## OQ-006 — Worker-API coordination mechanism

**Source:** [docs/01-system-architecture.md](../docs/01-system-architecture.md)  
**Related:** [ExecutionRun](entities/execution-run.md)

The architecture says "API creates run, Worker executes run, UI tracks progress."
The coordination mechanism is unspecified:
- Does the API poll ExecutionRun status from the database?
- Is there a pub/sub channel (Redis pub/sub, Redis Streams)?
- Is there a real-time push to the UI (SignalR, SSE)?

---

## OQ-007 — Tool binding validation against ToolRuntime catalog

**Source:** [docs/03-domain-model.md](../docs/03-domain-model.md)  
**Related:** [AgentDefinition](entities/agent-definition.md), [ToolDefinition](entities/tool-definition.md)

`AgentDefinition.BindTool(Tool)` takes a name-only value object. It enforces max-10 and
no-duplicates, but does it validate that the tool name exists in `ToolRuntime`? If not:
- An agent can bind a non-existent tool
- The error would surface only at execution time

Should the Application layer's `BindToolCommandHandler` query ToolRuntime before
delegating to the domain? Is there a domain event or specification for this?

---

## ~~OQ-008 — RetryCount storage location~~ RESOLVED

**Source:** [docs/03-domain-model.md](../docs/03-domain-model.md)  
**Related:** [ExecutionRun](entities/execution-run.md)  
**Resolved:** 2026-05-07

`RetryCount` is a `public int RetryCount { get; private set; }` property on `ExecutionRun`.
It is incremented inside `Retry()` and checked against `MaxRetries = 3`.
It is not a value object — it is a primitive field on the aggregate root itself.
Confirmed by reading `src/FlowSpline.Domain/ExecutionEngine/Aggregates/ExecutionRun.cs`.

---

## OQ-009 — Testcontainers timeline

**Source:** [docs/07-testing-strategy.md](../docs/07-testing-strategy.md)

Integration tests are listed as "Future: Testcontainers." Currently, what is the
integration test environment setup? Manual Docker? Fixed connection string?
When is Testcontainers adoption planned?

---

## OQ-010 — Disable tool effect on existing bindings

**Source:** [docs/03-domain-model.md](../docs/03-domain-model.md)  
**Related:** [ToolDefinition](entities/tool-definition.md), [AgentDefinition](entities/agent-definition.md)

`ToolDefinition.Disable()` disables a tool. What happens to agents that already have this
tool bound via `AgentManagement.Tool`? Options:
1. Nothing — agents retain the binding; execution fails at runtime
2. Domain event triggers cascading unbind in Application layer
3. Disabled tools cause execution to skip/reject the tool call
