---
title: FlowSpline Wiki Index
updated: 2026-05-07
---

# FlowSpline Wiki

Project knowledge base for FlowSpline — an AI agent orchestration platform.

## Decisions

| Page | Summary |
|------|---------|
| [ADR-001: Modular Monolith First](decisions/adr-001-modular-monolith-first.md) | Use modular monolith architecture over microservices for initial phase |
| [ADR-002: PostgreSQL over MongoDB](decisions/adr-002-postgresql-over-mongo.md) | PostgreSQL + pgvector as primary data store |
| [ADR-003: Redis as Runtime Memory](decisions/adr-003-redis-runtime-memory.md) | Redis for runtime state, distributed locks, and caches |
| [ADR-004: CQRS in Application Layer](decisions/adr-004-cqrs-application-layer.md) | CQRS pattern for all application-layer operations |

## Entities

| Page | Summary |
|------|---------|
| [AgentDefinition](entities/agent-definition.md) | Root aggregate for agent configuration; owns prompt, tools, model settings |
| [AgentTeam](entities/agent-team.md) | Group of agents with a mandatory supervisor; circular delegation forbidden |
| [ExecutionRun](entities/execution-run.md) | Root aggregate tracking a single execution through its state machine |
| [ToolDefinition](entities/tool-definition.md) | System-wide tool registration with schema and enable/disable lifecycle |
| [MemoryEntry](entities/memory-entry.md) | Per-agent per-session key-value memory record backed by Redis |

## Concepts

| Page | Summary |
|------|---------|
| [Clean Architecture](concepts/clean-architecture.md) | Layered architecture with inward-only dependency flow |
| [DDD Bounded Contexts](concepts/ddd-bounded-contexts.md) | Domain partitioned into 6 isolated contexts; 4 implemented, 2 planned |
| [CQRS](concepts/cqrs.md) | Commands mutate state; queries read state; applied in Application layer |
| [Modular Monolith](concepts/modular-monolith.md) | Single deployable with strong module boundaries; migration path to microservices |
| [State Machine](concepts/state-machine.md) | ExecutionRun lifecycle modeled as an explicit state machine |

## Sources

| Page | Raw Document | Key Topics |
|------|-------------|------------|
| [Product Vision](sources/00-product-vision.md) | 00-product-vision.md | Vision, modules, ICP, business model |
| [System Architecture](sources/01-system-architecture.md) | 01-system-architecture.md | Stack, modules, worker pattern |
| [Dependency Rules](sources/02-dependency-rules.md) | 02-dependency-rules.md | Layer dependency table, forbidden deps |
| [Domain Model](sources/03-domain-model.md) | 03-domain-model.md | All aggregates, behaviors, invariants, events |
| [Engineering Standards](sources/04-engineering-standards.md) | 04-engineering-standards.md | Naming, style, git strategy |
| [Development Setup](sources/05-dev-setup.md) | 05-dev-setup.md | Prerequisites, run commands |
| [API Spec](sources/06-api-spec.md) | 06-api-spec.md | REST endpoints for agents, executions, tools |
| [Testing Strategy](sources/07-testing-strategy.md) | 07-testing-strategy.md | xUnit, FluentAssertions, Moq, Testcontainers |
| [Roadmap](sources/08-roadmap.md) | 08-roadmap.md | Phase 1/2/3 features |
| [Application Layer](sources/09-application-layer.md) | 09-application-layer.md | CQRS implementation, handlers, repository interfaces, DI |
| [ADR-001: Modular Monolith](sources/adr-001-modular-monolith-first.md) | adr/001-modular-monolith-first.md | Architecture style decision |
| [ADR-002: PostgreSQL](sources/adr-002-postgresql-over-mongo.md) | adr/002-postgresql-over-mongo.md | Database choice |
| [ADR-003: Redis](sources/adr-003-redis-runtime-memory.md) | adr/003-redis-runtime-memory.md | Runtime memory store |
| [ADR-004: CQRS](sources/adr-004-cqrs-application-layer.md) | adr/004-cqrs-application-layer.md | Application layer pattern |

## Open Questions

[open-questions.md](open-questions.md) — unresolved design questions identified during initial ingest
