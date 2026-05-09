---
title: System Architecture
type: source
tags: [architecture, stack, worker]
sources: [wiki/raw/articles/01-system-architecture.md]
updated: 2026-05-05
---

# System Architecture

## Summary

Defines the technology stack and high-level architecture of FlowSpline. The system follows a Modular Monolith-first style on .NET 10 + ASP.NET Core, backed by PostgreSQL + pgvector and Redis. A background Worker service handles execution runs independently from the API.

## Key Claims

- **Backend**: .NET 10, ASP.NET Core, EF Core
- **Data**: PostgreSQL + pgvector (primary store), Redis (runtime/cache)
- **Infra**: Docker (dev), Kubernetes (future)
- **Frontend**: React + Next.js
- **Module list**: Agent Management, Workflow Engine, Execution Engine, Tool Runtime, Memory
- **Worker pattern**: API creates run → Worker executes run → UI tracks progress

## Wiki Pages Updated

- [Clean Architecture](../concepts/clean-architecture.md) — layer definitions
- [Modular Monolith](../concepts/modular-monolith.md) — confirmed stack choice
- [DDD Bounded Contexts](../concepts/ddd-bounded-contexts.md) — module list
- [State Machine](../concepts/state-machine.md) — worker pattern implies execution state tracking

## Open Questions Raised

- [OQ-006](../open-questions.md#oq-006--worker-api-coordination-mechanism) — Worker-API coordination mechanism unspecified (poll? pub/sub? SignalR?)
