---
title: "Source: Application Layer Design"
type: source
tags: [application-layer, cqrs, mediatr, fluent-validation, handlers, repository]
sources: [wiki/raw/articles/09-application-layer.md]
updated: 2026-05-07
---

# Source: Application Layer Design

**Raw document:** `wiki/raw/articles/09-application-layer.md`  
**Scope:** Application layer implementation — commands, queries, handlers, validators, repository interfaces, DI registration.

## Summary

Documents the complete Application layer built for Phase 1 MVP. Covers package choices (MediatR 12.4.1, FluentValidation 11.11.0), folder structure (vertical slices per bounded context), repository interface contracts, command/query inventory, and five key design decisions.

## Key Claims

1. **MediatR 12.4.1** is used for command/query dispatching. `RequestHandlerDelegate<TResponse>` takes no arguments in this version (changed from earlier MediatR releases).
2. **Vertical slices** — one folder per feature inside each bounded context. No `Common/` or `Shared/` folders.
3. **Repository interfaces** live in Application, implementations in Persistence. Application has no Persistence dependency.
4. **`CreateExecutionCommand` does not call `Start()`** — starting is the Worker's responsibility. This is load-bearing: the Worker polls for `Created` runs and transitions them to `Running`.
5. **Tool name uniqueness** is enforced in `RegisterToolCommandHandler` via `IToolDefinitionRepository.ExistsByNameAsync()` — not in the domain, which cannot query.
6. **`ValidationBehavior<TRequest, TResponse>`** is placed at the Application root (not in a bounded context folder) as the only cross-cutting infrastructure piece.
7. **No AutoMapper** — mapping inline via `static internal ToDto()` methods in query handlers.
8. **Domain events are `internal`** — phase 1 handlers do not publish them. When MediatR notification dispatch is needed in Phase 2, domain events must be made `public`.

## Wiki Pages Updated During Ingest

- `wiki/concepts/cqrs.md` — Added "Implementation" section with actual command/query list, MediatR details, and correction of stale "No MediatR currently" note.
- `wiki/entities/execution-run.md` — Added Application layer note: `CreateExecution` does not call `Start()`; Worker is responsible.
- `wiki/entities/agent-definition.md` — Added Application layer note: `UpdateAgent` covers `ChangePrompt`, `Activate`, `Deactivate` via nullable fields.
- `wiki/open-questions.md` — OQ-008 (RetryCount storage) marked **resolved**: confirmed as `public int RetryCount` property on `ExecutionRun`.

## Open Questions Raised

None. OQ-007 (tool binding validation) is partially addressed: the Application layer validates name existence before creating, but does not validate bindings on existing agents when tools are disabled — that remains open per OQ-010.
