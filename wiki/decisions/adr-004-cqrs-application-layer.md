---
title: "ADR-004: CQRS in Application Layer"
type: decision
status: Accepted
tags: [architecture, cqrs, application-layer, pattern]
sources: [wiki/raw/articles/adr/004-cqrs-application-layer.md]
updated: 2026-05-03
---

# ADR-004: CQRS in Application Layer

**Status:** Accepted

## Decision

Apply CQRS (Command Query Responsibility Segregation) in the Application layer. All
mutations go through Commands; all reads go through Queries. Commands and Queries are
never mixed in the same handler.

## Context

The Application layer mediates between the API and the Domain. Without a clear pattern,
handlers tend to accumulate mixed concerns: reading state, mutating state, and returning
results all in one method. This becomes harder to test and reason about as the system grows.

## Rationale

- Commands and Queries have different performance characteristics; separating them enables
  independent optimization (e.g., query-side can bypass the domain model and read directly
  from the persistence layer)
- Easier to test: Commands test domain mutation; Queries test read paths independently
- Enforces vertical slices: one folder per feature/command keeps the codebase navigable
- Aligns with Clean Architecture: Application layer has no upward dependencies

## Trade-offs

- More files: each feature has a separate Command, Handler, Query, and QueryHandler
- Overhead for simple CRUD: a thin read-only endpoint still needs a Query + QueryHandler
- No MediatR or similar dispatch library currently listed — handlers wired via DI

## Cross-references

- [CQRS (concept)](../concepts/cqrs.md)
- [Clean Architecture (concept)](../concepts/clean-architecture.md)
