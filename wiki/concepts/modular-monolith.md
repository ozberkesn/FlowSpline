---
title: Modular Monolith
type: concept
tags: [architecture, deployment, monolith, modules]
sources: [wiki/raw/articles/01-system-architecture.md, wiki/raw/articles/adr/001-modular-monolith-first.md]
updated: 2026-05-03
---

# Modular Monolith

## Definition

A modular monolith is a single deployable process with strong internal module boundaries.
Each module is a cohesive unit with explicit interfaces. Unlike a "big ball of mud"
monolith, the structure is enforced by design — modules do not call each other freely.

## How FlowSpline Uses It

- One ASP.NET Core application (single process, single deployment)
- One PostgreSQL database; one Redis instance
- Bounded contexts map to modules — each has its own aggregate roots, events, and interfaces
- Dependency enforcement via Clean Architecture layer rules (not network isolation)
- Worker pattern: API creates runs, Worker executes them — async separation within the monolith

## Migration Path to Microservices

The modular structure is intentional preparation for future decomposition:
- Each bounded context boundary is a potential service boundary
- Context-crossing shortcuts (shared tables, direct class references across contexts)
  are prohibited now so they don't block extraction later

## Trade-offs

| Pro | Con |
|---|---|
| Simpler operations (one deployment unit) | All modules share the same failure domain |
| Easier cross-context refactoring early on | Horizontal scaling requires stateless design + distributed locks |
| No network latency between modules | Module coupling enforced by convention, not by the runtime |

## Cross-references

- [ADR-001: Modular Monolith First](../decisions/adr-001-modular-monolith-first.md)
- [DDD Bounded Contexts (concept)](ddd-bounded-contexts.md)
- [Clean Architecture (concept)](clean-architecture.md)
