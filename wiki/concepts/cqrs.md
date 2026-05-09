---
title: CQRS
type: concept
tags: [cqrs, application-layer, pattern, commands, queries]
sources: [wiki/raw/articles/adr/004-cqrs-application-layer.md, wiki/raw/articles/04-engineering-standards.md, wiki/raw/articles/09-application-layer.md]
updated: 2026-05-07
---

# CQRS (Command Query Responsibility Segregation)

## Definition

CQRS separates operations that change state (Commands) from operations that read state
(Queries). They are never mixed in the same handler. A Command mutates; a Query reads.

## FlowSpline Application

Applied exclusively at the Application layer. Every feature is a vertical slice:

```
Application/
└── AgentManagement/
    └── CreateAgent/
        ├── CreateAgentCommand.cs
        └── CreateAgentCommandHandler.cs
    └── GetAgent/
        ├── GetAgentQuery.cs
        └── GetAgentQueryHandler.cs
```

**Commands:** mutate domain aggregates, raise domain events, return void or an ID.  
**Queries:** can bypass the domain model and read directly from the persistence layer.

## Naming Conventions

| Artifact | Pattern | Example |
|---|---|---|
| Command | `{Action}{Entity}Command` | `CreateAgentCommand` |
| Command handler | `{Action}{Entity}CommandHandler` | `CreateAgentCommandHandler` |
| Query | `Get{Entity}Query` | `GetAgentQuery` |
| Query handler | `Get{Entity}QueryHandler` | `GetAgentQueryHandler` |

## Implementation

**Packages:** MediatR 12.4.1, FluentValidation 11.11.0.

**Phase 1 command/query inventory:**

| Artifact | Type | Bounded Context |
|----------|------|----------------|
| `CreateAgentCommand` | Command → `Guid` | AgentManagement |
| `UpdateAgentCommand` | Command → void | AgentManagement |
| `DeleteAgentCommand` | Command → void | AgentManagement |
| `GetAgentQuery` | Query → `AgentDto?` | AgentManagement |
| `GetAgentsQuery` | Query → `IReadOnlyList<AgentDto>` | AgentManagement |
| `CreateExecutionCommand` | Command → `Guid` | ExecutionEngine |
| `GetExecutionQuery` | Query → `ExecutionRunDto?` | ExecutionEngine |
| `RegisterToolCommand` | Command → `Guid` | ToolRuntime |
| `GetToolsQuery` | Query → `IReadOnlyList<ToolDefinitionDto>` | ToolRuntime |

**Pipeline:** `ValidationBehavior<TRequest, TResponse>` runs all FluentValidation validators before any handler executes. Placed at Application root (not in a bounded context folder).

**DI registration:** `AddApplication()` extension method on `IServiceCollection`. Called from `Program.cs`.

## Trade-offs

- More files per feature, but each file is small and single-purpose
- Queries can be independently optimized (e.g., raw SQL, projections)
- Overhead for simple CRUD: even a read-only endpoint needs a Query + Handler pair

## Cross-references

- [Clean Architecture (concept)](clean-architecture.md)
- [ADR-004: CQRS in Application Layer](../decisions/adr-004-cqrs-application-layer.md)
