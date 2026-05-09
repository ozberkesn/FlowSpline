---
title: "ADR-001: Modular Monolith First"
type: decision
status: Accepted
tags: [architecture, deployment]
sources: [wiki/raw/articles/adr/001-modular-monolith-first.md]
updated: 2026-05-03
---

# ADR-001: Modular Monolith First

**Status:** Accepted

## Decision

Start with a modular monolith rather than microservices.

## Context

FlowSpline is an early-stage product. The team needs to iterate quickly while keeping
the option to decompose into microservices if needed. The risk of premature decomposition
(distributed systems complexity, network overhead, data consistency challenges) outweighs
the theoretical scalability benefits at this stage.

## Rationale

- Faster development iteration — no inter-service contracts to manage upfront
- Lower operational complexity — single deployment unit, single database
- Easier refactoring — changing module boundaries is a code change, not a service negotiation
- Clean module boundaries now mean migration to microservices is feasible later

## Trade-offs

- Single process: horizontal scaling requires careful state management
- All modules share the same runtime failure domain
- Module coupling must be enforced by convention (Clean Architecture rules), not by
  network isolation

## Cross-references

- [Modular Monolith (concept)](../concepts/modular-monolith.md)
- [DDD Bounded Contexts (concept)](../concepts/ddd-bounded-contexts.md)
- [Clean Architecture (concept)](../concepts/clean-architecture.md)
