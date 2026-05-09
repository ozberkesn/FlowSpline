# Application Layer Design

## Overview

The Application layer implements CQRS (Command Query Responsibility Segregation) using
MediatR 12.4.1 and FluentValidation 11.11.0 targeting .NET 10.

This layer mediates between the API and the Domain. It owns repository interfaces,
commands, queries, handlers, validators, and the MediatR pipeline.

## Packages

| Package | Version | Purpose |
|---------|---------|---------|
| MediatR | 12.4.1 | Command/query dispatching and pipeline behaviors |
| FluentValidation | 11.11.0 | Input validation |
| FluentValidation.DependencyInjectionExtensions | 11.11.0 | Auto-registration of validators |

## Folder Structure

Vertical slices — one folder per feature within its bounded context.
No `Common/` or `Shared/` dump folders.

```
src/FlowSpline.Application/
├── ValidationBehavior.cs          — MediatR pipeline: runs validators before handlers
├── DependencyInjection.cs         — IServiceCollection extension: AddApplication()
│
├── AgentManagement/
│   ├── Repositories/
│   │   ├── IAgentRepository.cs
│   │   └── IAgentTeamRepository.cs
│   ├── DTOs/
│   │   └── AgentDto.cs
│   ├── CreateAgent/               — CreateAgentCommand + Handler + Validator
│   ├── GetAgent/                  — GetAgentQuery + Handler
│   ├── GetAgents/                 — GetAgentsQuery + Handler
│   ├── UpdateAgent/               — UpdateAgentCommand + Handler + Validator
│   └── DeleteAgent/               — DeleteAgentCommand + Handler
│
├── ExecutionEngine/
│   ├── Repositories/
│   │   └── IExecutionRunRepository.cs
│   ├── DTOs/
│   │   └── ExecutionRunDto.cs
│   ├── CreateExecution/           — CreateExecutionCommand + Handler + Validator
│   └── GetExecution/              — GetExecutionQuery + Handler
│
├── ToolRuntime/
│   ├── Repositories/
│   │   └── IToolDefinitionRepository.cs
│   ├── DTOs/
│   │   └── ToolDefinitionDto.cs
│   ├── RegisterTool/              — RegisterToolCommand + Handler + Validator
│   └── GetTools/                  — GetToolsQuery + Handler
│
└── Memory/
    └── Repositories/
        └── IMemoryEntryRepository.cs
```

## Repository Interfaces

Repository interfaces are defined in the Application layer and implemented in Persistence.
This keeps Application dependent on Domain only (no Persistence reference).

### IAgentRepository

```csharp
Task<AgentDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default);
Task<IReadOnlyList<AgentDefinition>> GetAllAsync(CancellationToken ct = default);
Task AddAsync(AgentDefinition agent, CancellationToken ct = default);
Task UpdateAsync(AgentDefinition agent, CancellationToken ct = default);
Task DeleteAsync(Guid id, CancellationToken ct = default);
Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
```

### IExecutionRunRepository

```csharp
Task<ExecutionRun?> GetByIdAsync(Guid id, CancellationToken ct = default);
Task AddAsync(ExecutionRun run, CancellationToken ct = default);
Task UpdateAsync(ExecutionRun run, CancellationToken ct = default);
```

### IToolDefinitionRepository

```csharp
Task<ToolDefinition?> GetByIdAsync(Guid id, CancellationToken ct = default);
Task<ToolDefinition?> GetByNameAsync(string name, CancellationToken ct = default);
Task<IReadOnlyList<ToolDefinition>> GetAllAsync(CancellationToken ct = default);
Task AddAsync(ToolDefinition tool, CancellationToken ct = default);
Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
```

`GetByNameAsync` and `ExistsByNameAsync` exist because `ToolDefinition` uses a slug-format
unique name as a natural key. The handler must validate uniqueness before delegating to domain.

## Commands and Queries

| Artifact | Type | Return | Key business rule |
|----------|------|--------|-------------------|
| CreateAgentCommand | IRequest\<Guid\> | new agent Id | ModelSettings constructed from flat fields |
| GetAgentQuery | IRequest\<AgentDto?\> | agent or null | null → 404 in API |
| GetAgentsQuery | IRequest\<IReadOnlyList\<AgentDto\>\> | all agents | empty list valid |
| UpdateAgentCommand | IRequest | void | nullable fields; only set fields are applied |
| DeleteAgentCommand | IRequest | void | existence check before delete |
| CreateExecutionCommand | IRequest\<Guid\> | new run Id | agent must exist and be active |
| GetExecutionQuery | IRequest\<ExecutionRunDto?\> | run or null | — |
| RegisterToolCommand | IRequest\<Guid\> | new tool Id | name uniqueness enforced before domain call |
| GetToolsQuery | IRequest\<IReadOnlyList\<ToolDefinitionDto\>\> | all tools | — |

## Key Design Decisions

### CreateExecution does not call Start()

`CreateExecutionCommandHandler` creates an `ExecutionRun` in `Created` status and
persists it. It does NOT call `run.Start()`.

Starting is the Worker's responsibility. The Worker watches for runs in `Created` state
and calls `Start()` before dispatching to the LLM. If the handler called `Start()` directly,
the run would be in `Running` state with no Worker actually executing it.

### UpdateAgent uses nullable optional fields

`UpdateAgentCommand` has `string? SystemPrompt` and `bool? IsActive`. The handler
applies only the non-null fields. This allows callers to update a single property
without knowing the current value of others. Name and model are not updatable via
this command because no corresponding domain behavior exists on `AgentDefinition`.

### Tool uniqueness is enforced in the Application layer

`ToolDefinition` enforces the slug format in its constructor, but cannot enforce
global uniqueness (that would require a repository query, which Domain cannot do).
`RegisterToolCommandHandler` calls `ExistsByNameAsync` before constructing the domain
object. This is the Application-layer boundary for cross-aggregate uniqueness rules.

### ValidationBehavior at Application root

`ValidationBehavior<TRequest, TResponse>` is placed at `FlowSpline.Application/`
root — not inside a bounded context folder. It is the only cross-cutting file in the
Application layer. It is an MediatR pipeline behavior, not a business concern.
Creating a `Common/` folder for one file violates the no-shared-dump-folder rule.

### No AutoMapper

DTO mapping is done inline in query handlers via a `static internal ToDto()` method.
`GetAgentsQueryHandler` reuses `GetAgentQueryHandler.ToDto()` to avoid duplication.
AutoMapper adds complexity and indirection that is not justified at this scale.

### Domain events are internal — consequence for Phase 2

Domain event classes are declared `internal` within their namespace. MediatR
`INotificationHandler<T>` cannot consume them without making them `public`.
Phase 1 handlers do not need to publish domain events. When event-driven side effects
are needed (Phase 2), domain event classes should be changed to `public`.

## DI Registration

```csharp
// src/FlowSpline.Api/Program.cs
builder.Services.AddApplication();

// src/FlowSpline.Application/DependencyInjection.cs
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    var assembly = typeof(DependencyInjection).Assembly;
    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(assembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
    services.AddValidatorsFromAssembly(assembly);
    return services;
}
```

## Validation Rules

| Validator | Key rules |
|-----------|-----------|
| CreateAgentCommandValidator | Name ≥ 3 chars; Temperature ∈ [0,2]; MaxTokens > 0 |
| UpdateAgentCommandValidator | Id not empty; SystemPrompt not empty when set |
| CreateExecutionCommandValidator | AgentId, SessionId not empty; Input not empty |
| RegisterToolCommandValidator | Name matches `^[a-z0-9_-]+$` |
