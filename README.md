# FlowSpline

**Control Plane for AI Workforces.** FlowSpline is an open-source platform for building, orchestrating, and operating AI agent teams. It fills the gap organizations hit when adopting AI at scale: disconnected agents, unmanaged prompts, ungoverned tools, and zero observability.

Building in public.

---

## What It Does

| Module | Description |
|--------|-------------|
| **Agent Studio** | Create and version AI agent definitions with prompt configs, model settings, and tool bindings |
| **Agent Teams** | Compose agents into supervised teams; circular delegation is a domain invariant violation |
| **Workflow Orchestrator** | Define multi-step, multi-agent workflows with branching and handoff logic |
| **Tool Integration Layer** | Register external tools with schemas; govern which agents can use which tools |
| **Memory Layer** | Per-agent, per-session key-value memory backed by Redis; vector memory via pgvector |
| **Execution Engine** | State machine for single-agent and multi-agent runs (Created → Running → WaitingApproval → Completed/Failed/Retrying) |
| **Observability** | Execution logs, run history, and tracing hooks |
| **Governance** | Policy enforcement and access control across agents and tools |

---

## Architecture

**Modular Monolith** ([ADR-001](wiki/decisions/adr-001-modular-monolith-first.md)) with **Clean Architecture** and **DDD bounded contexts**. Single deployable binary with strong module boundaries and a clear migration path to microservices.

### Tech Stack

| Concern | Choice |
|---------|--------|
| Backend | .NET 10, ASP.NET Core, EF Core |
| Primary store | PostgreSQL + pgvector |
| Runtime/cache | Redis |
| Frontend (planned) | React + Next.js |
| Infra (dev) | Docker Compose |
| Infra (prod) | Kubernetes (Phase 3) |

### Project Layout

```
src/
  FlowSpline.Api/            — ASP.NET Core 10 entry point, controllers, OpenAPI
  FlowSpline.Application/    — CQRS commands/queries and handlers
  FlowSpline.Domain/         — Aggregates, value objects, domain events; no external deps
  FlowSpline.Infrastructure/ — LLM/tool integrations, external services
  FlowSpline.Persistence/    — EF Core, PostgreSQL, pgvector, Redis
  FlowSpline.Worker/         — Background worker service (executes runs)
tests/
  FlowSpline.UnitTests/      — Domain rules, command handlers, retry logic
  FlowSpline.IntegrationTests/ — PostgreSQL, Redis, worker flows
wiki/                        — LLM-maintained project knowledge base (see below)
```

### Dependency Rules

```
Domain ← Application ← Infrastructure
                     ← Persistence
              Api  ← Application, Infrastructure, Persistence
           Worker  ← Application, Infrastructure
```

Domain has zero outward dependencies. Application never depends on Infrastructure or Persistence.

---

## Roadmap

**Phase 1 — MVP** *(in progress)*
- Agent CRUD and Tool CRUD
- Single-agent execution with state machine
- Execution logs and Redis memory

**Phase 2 — Teams & Workflows**
- Agent teams with supervisor enforcement
- Workflow engine and multi-agent handoff

**Phase 3 — Platform**
- Multi-tenancy
- Full observability stack
- Kubernetes-native deployment

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run Locally

```bash
# 1. Start PostgreSQL + Redis
docker compose up

# 2. Apply database migrations
dotnet ef database update --project src/FlowSpline.Persistence --startup-project src/FlowSpline.Api

# 3. Start the API
dotnet run --project src/FlowSpline.Api
```

API is available at `http://localhost:5275`. Interactive API explorer (Scalar) at `http://localhost:5275/scalar/v1`.

### Run Tests

```bash
dotnet test                                    # all tests
dotnet test tests/FlowSpline.UnitTests         # unit tests only
dotnet test tests/FlowSpline.IntegrationTests  # integration tests only
```

---

## Wiki

`wiki/` is a persistent, LLM-maintained knowledge base that accumulates architectural decisions, domain model documentation, concept pages, and open questions across development sessions.

```
wiki/
├── index.md           — catalog of all pages
├── log.md             — append-only record of changes
├── open-questions.md  — unresolved design questions
├── raw/articles/      — immutable source documents (never edited)
├── decisions/         — ADR summaries with rationale and trade-offs
├── concepts/          — architecture patterns (Clean Architecture, CQRS, etc.)
├── entities/          — domain aggregate and entity pages
└── sources/           — one synthesis page per source document
```

Key starting points:
- [Wiki Index](wiki/index.md)
- [Open Questions](wiki/open-questions.md)
- [ADR-001: Modular Monolith](wiki/decisions/adr-001-modular-monolith-first.md)
- [Domain Model](wiki/entities/)

---

## Development with Claude

FlowSpline is developed with [Claude Code](https://claude.ai/code) (Anthropic's AI coding assistant) as a first-class development tool.

### How It's Used

- **Architecture decisions** — Claude reads the wiki and ADRs before proposing changes, keeping suggestions grounded in documented rationale.
- **Feature implementation** — Commands, handlers, and domain types are implemented following the coding standards in [CLAUDE.md](CLAUDE.md).
- **Wiki maintenance** — After significant architectural changes, Claude updates the relevant wiki pages and appends an entry to `wiki/log.md`.
- **Domain modeling** — Claude enforces domain invariants (e.g., no circular delegation in `AgentTeam`, behavior-driven methods on aggregates).
- **Code review** — `/review` and `/security-review` skills run multi-agent reviews on open branches.

### Conventions

The [CLAUDE.md](CLAUDE.md) file is the source of truth for how Claude operates in this repo. It covers:
- Which wiki pages to read before starting any task
- Dependency rules Claude must not violate
- Naming conventions for commands, handlers, and tests
- Coding style (behavior-driven domain methods, no `Common`/`Shared` dumps)

The wiki's [CLAUDE.md](wiki/CLAUDE.md) defines the schema and workflows for how Claude reads and writes the knowledge base.

---

## Contributing

This project is in early development. Architecture decisions are documented in `wiki/decisions/`. Read [wiki/open-questions.md](wiki/open-questions.md) to see what's currently unresolved before opening an issue or PR.

## License

TBD
