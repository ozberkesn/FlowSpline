# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Start required infrastructure (PostgreSQL + Redis)
docker compose up

# Run the API
dotnet run --project src/FlowSpline.Api

# Build entire solution
dotnet build

# Run all tests
dotnet test

# Run a specific test project
dotnet test tests/FlowSpline.UnitTests
dotnet test tests/FlowSpline.IntegrationTests

# Apply EF Core migrations
dotnet ef database update --project src/FlowSpline.Persistence --startup-project src/FlowSpline.Api
```

## Architecture

FlowSpline is an AI agent orchestration platform built as a **modular monolith** (ADR-001) using **Clean Architecture** with **DDD bounded contexts**.

### Project Layout

```
src/
  FlowSpline.Api/           — ASP.NET Core 10 entry point, controllers, OpenAPI
  FlowSpline.Application/   — CQRS commands/queries and handlers (ADR-004)
  FlowSpline.Domain/        — Aggregates, value objects, domain events; no external deps
  FlowSpline.Infrastructure/ — LLM/tool integrations, external services
  FlowSpline.Persistence/   — EF Core, PostgreSQL, pgvector, Redis
  FlowSpline.Worker/        — Background worker service
tests/
  FlowSpline.UnitTests/     — Domain rules, commands, retry logic
  FlowSpline.IntegrationTests/ — PostgreSQL, Redis, worker flows
```

### Dependency Rules

Only these directions are allowed; violations are architectural errors:

| Layer | May depend on |
|-------|---------------|
| Domain | *(nothing)* |
| Application | Domain |
| Infrastructure | Application, Domain |
| Persistence | Application, Domain |
| Api | Application, Infrastructure, Persistence |
| Worker | Application, Infrastructure |

**Never** allow: Domain → anything, Application → Infrastructure/Persistence.

### Bounded Contexts

- **AgentManagement** — agent creation and configuration
- **WorkflowEngine** — workflow orchestration
- **ExecutionEngine** — run state machine (Created → Running → WaitingApproval → Completed/Failed/Retrying)
- **ToolRuntime** — tool integration layer
- **Memory** — vector storage and retrieval (pgvector)
- **Governance** — access control and policy enforcement

### Key Domain Types

- `AgentDefinition` — root aggregate for agents; owns `PromptConfig`, `ToolBindings`, `ModelSettings`, `Policies`
- `AgentTeam` — supervisor required; circular delegation is a domain invariant violation
- `ExecutionRun` — root aggregate tracking a single execution run through its state machine
- `AggregateRoot` — base class with domain event collection
- `ModelSettings` — value object: provider, model, temperature, maxTokens
- `Tool` — name-based value object

## Coding Standards

### Naming

| Artifact | Pattern | Example |
|----------|---------|---------|
| Command | `{Action}{Entity}Command` | `CreateAgentCommand` |
| Handler | `{Action}{Entity}CommandHandler` | `CreateAgentCommandHandler` |
| Test method | `{Method}_When{Condition}_Should{Result}` | `CreateAgent_WhenInvalid_ShouldFail` |

### Style

- Use **behavior-driven** domain methods: `execution.Complete()` not `execution.Status = "Done"`.
- Nullable reference types and implicit usings are **enabled** across all projects.
- No `Common`/`Shared` dump projects — keep code in its bounded context.
- Vertical slices inside Application; one folder per feature/command.

## Docs

The `docs/` directory contains authoritative design documents:

- `01-system-architecture.md` — full system architecture
- `02-dependency-rules.md` — detailed dependency enforcement rules
- `03-domain-model.md` — aggregate definitions and invariants
- `04-engineering-standards.md` — code style and git strategy
- `06-api-spec.md` — REST API contract
- `07-testing-strategy.md` — testing approach and tooling
- `adr/` — architecture decision records
