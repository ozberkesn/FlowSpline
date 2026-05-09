---
title: Engineering Standards
type: source
tags: [standards, naming, git, ddd]
sources: [wiki/raw/articles/04-engineering-standards.md]
updated: 2026-05-05
---

# Engineering Standards

## Summary

Defines coding standards for the FlowSpline codebase: Clean Architecture + DDD boundaries + vertical slices, behavior-driven domain method style, naming conventions for commands/handlers/tests, and git branching strategy.

## Key Claims

- **Architecture rules**: Clean Architecture, DDD boundaries, vertical slices, no Shared/Common dump project
- **Behavior-driven style**: `entity.Complete()` not `entity.Status = "Done"`
- **Naming**: `CreateAgentCommand`, `CreateAgentCommandHandler`, `CreateAgent_WhenInvalid_ShouldFail`
- **Git branches**: `main`, `develop`, `feature/*`

## Wiki Pages Updated

- [Clean Architecture](../concepts/clean-architecture.md) — style rules
- [CQRS](../concepts/cqrs.md) — naming conventions for commands/handlers

## Open Questions Raised

None.
