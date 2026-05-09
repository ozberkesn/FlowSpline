# Wiki Log

Append-only record of wiki operations. Format: `## [YYYY-MM-DD] <operation> | <title>`

Parse recent entries: `grep "^## \[" wiki/log.md | tail -5`

---

## [2026-05-03] ingest | Initial wiki setup from docs/ and adr/

Processed 13 source documents from `docs/` and `docs/adr/`.

Pages created:
- decisions/adr-001-modular-monolith-first.md
- decisions/adr-002-postgresql-over-mongo.md
- decisions/adr-003-redis-runtime-memory.md
- decisions/adr-004-cqrs-application-layer.md
- entities/agent-definition.md
- entities/agent-team.md
- entities/execution-run.md
- entities/tool-definition.md
- entities/memory-entry.md
- concepts/clean-architecture.md
- concepts/ddd-bounded-contexts.md
- concepts/cqrs.md
- concepts/modular-monolith.md
- concepts/state-machine.md
- open-questions.md
- index.md

Sources ingested:
- docs/00-product-vision.md
- docs/01-system-architecture.md
- docs/02-dependency-rules.md
- docs/03-domain-model.md
- docs/04-engineering-standards.md
- docs/05-dev-setup.md
- docs/06-api-spec.md
- docs/07-testing-strategy.md
- docs/08-roadmap.md
- docs/adr/001-modular-monolith-first.md
- docs/adr/002-postgresql-over-mongo.md
- docs/adr/003-redis-runtime-memory.md
- docs/adr/004-cqrs-application-layer.md

Open questions raised: 10 (see open-questions.md)

---

## [2026-05-05] migration | docs/ → wiki/raw/articles/; wiki/sources/ pages created

Aligned wiki with LLM Wiki pattern (three-layer architecture: raw sources / wiki / schema).

Changes:
- Moved `docs/` (13 files: 9 root + 4 in adr/) to `wiki/raw/articles/` via git mv; `docs/` directory removed
- Created `wiki/sources/` with 13 summary pages (one per ingested source document)
- Updated root `CLAUDE.md`: `## Docs` section now references `wiki/raw/articles/`
- Updated `wiki/CLAUDE.md`: `raw/articles/` description clarified to cover both project docs and external articles
- Updated `sources:` frontmatter in all 14 existing wiki pages (entities/, decisions/, concepts/)
- Added `## Sources` section to `wiki/index.md` with links to all 13 source pages

---

## [2026-05-05] sync | Entity pages corrected against actual code

Cross-checked all 5 entity wiki pages against the implemented .cs files in src/.

Corrections applied:
- `entities/execution-run.md` — added missing `ExecutionApprovedEvent` (raised by `Approve()`)
- `entities/tool-definition.md` — added missing `ToolDescriptionUpdatedEvent` and `ToolSchemaUpdatedEvent` (raised by `UpdateDescription()` and `UpdateSchema()`)

Root cause: initial ingest was based on design documents; these events were present in the
implementation but absent from the source docs.

---

## [2026-05-07] ingest | 09-application-layer — CQRS Application layer design

New source document: `wiki/raw/articles/09-application-layer.md`

Pages created:
- `wiki/sources/09-application-layer.md`

Pages updated:
- `wiki/concepts/cqrs.md` — Added "Implementation" section with Phase 1 command/query inventory, MediatR 12.4.1 details, ValidationBehavior notes. Removed stale "No MediatR currently" claim.
- `wiki/entities/execution-run.md` — Added Application Layer Notes: CreateExecution does not call Start(); Worker is responsible.
- `wiki/entities/agent-definition.md` — Added Application Layer Notes: UpdateAgent covers ChangePrompt/Activate/Deactivate via nullable fields.
- `wiki/index.md` — Added Application Layer row in Sources table.

Open questions resolved:
- OQ-008 (RetryCount storage) — confirmed: `public int RetryCount { get; private set; }` on ExecutionRun aggregate, incremented in `Retry()`, checked against `MaxRetries = 3`.

---

## [2026-05-09] implement | Persistence katmanı — EF Core 10 + Redis

Implemented `FlowSpline.Persistence` project from scratch.

Files created:
- `docker-compose.yml` — pgvector/pgvector:pg17 + redis:7-alpine
- `src/FlowSpline.Persistence/FlowSplineDbContext.cs`
- `src/FlowSpline.Persistence/FlowSplineDbContextFactory.cs` (design-time factory)
- `src/FlowSpline.Persistence/DependencyInjection.cs`
- `src/FlowSpline.Persistence/AgentManagement/AgentRepository.cs`
- `src/FlowSpline.Persistence/AgentManagement/AgentTeamRepository.cs`
- `src/FlowSpline.Persistence/AgentManagement/Configurations/AgentDefinitionConfiguration.cs`
- `src/FlowSpline.Persistence/AgentManagement/Configurations/AgentTeamConfiguration.cs`
- `src/FlowSpline.Persistence/ExecutionEngine/ExecutionRunRepository.cs`
- `src/FlowSpline.Persistence/ExecutionEngine/Configurations/ExecutionRunConfiguration.cs`
- `src/FlowSpline.Persistence/ToolRuntime/ToolDefinitionRepository.cs`
- `src/FlowSpline.Persistence/ToolRuntime/Configurations/ToolDefinitionConfiguration.cs`
- `src/FlowSpline.Persistence/Memory/MemoryEntryRepository.cs` (Redis)
- `src/FlowSpline.Persistence/Migrations/20260509105408_InitialCreate.cs`

Files modified:
- `src/FlowSpline.Api/Program.cs` — `AddPersistence()` call added
- `src/FlowSpline.Api/appsettings.Development.json` — ConnectionStrings added

Key decisions:
- Single `FlowSplineDbContext` for all 4 PostgreSQL-backed aggregates (modular monolith)
- `ModelSettings`, `RunContext`, `ToolSchema` → EF Core `OwnsOne` (inlined columns)
- `AgentDefinition.Tools` → `OwnsMany` → separate `agent_tools` table
- `AgentTeam.MemberIds` → JSONB column via `ValueConverter<HashSet<Guid>, string>`
- `ExecutionStatus` → stored as `varchar(20)` string (`.HasConversion<string>()`)
- `MemoryEntry` → Redis only; key `memory:{agentId}:{sessionId}:{key}`; TTL from ExpiresAt
- `pgvector` extension enabled in migration via `HasPostgresExtension("vector")`

Open questions resolved:
- OQ-003 (pgvector vs Redis) — Phase 1 = Redis (MemoryEntry), Phase 2 = pgvector (semantic memory)

---

## [2026-05-09] implement | Scalar API Explorer eklendi

API'yi tarayıcıdan keşfetmek için Scalar UI entegrasyonu yapıldı.

Files modified:
- `src/FlowSpline.Api/FlowSpline.Api.csproj` — `Scalar.AspNetCore` 2.14.11 paketi eklendi
- `src/FlowSpline.Api/Program.cs` — `using Scalar.AspNetCore` + `app.MapScalarApiReference()` (IsDevelopment guard içinde)
- `README.md` — API URL ve explorer adresi güncellendi (yanlış `/swagger` → `/scalar/v1`)
- `wiki/sources/05-dev-setup.md` — API base URL, Scalar URL, OpenAPI JSON endpoint bilgileri eklendi

Key decisions:
- ASP.NET 10 built-in `app.MapOpenApi()` + Scalar tercih edildi; Swashbuckle/NSwag eklenmedi
- Scalar yalnızca Development ortamında açık (IsDevelopment guard)
- HTTP: `http://localhost:5275/scalar/v1` · HTTPS: `https://localhost:7197/scalar/v1`

---

## [2026-05-09] implement | API Controllers — 9 endpoint, ValidationExceptionHandler

Implemented `FlowSpline.Api` controllers exposing all Phase 1 endpoints.

Files created:
- `src/FlowSpline.Api/Controllers/AgentsController.cs` (5 endpoints)
- `src/FlowSpline.Api/Controllers/ExecutionsController.cs` (2 endpoints)
- `src/FlowSpline.Api/Controllers/ToolsController.cs` (2 endpoints)
- `src/FlowSpline.Api/Middleware/ValidationExceptionHandler.cs`

Files modified:
- `src/FlowSpline.Api/Program.cs` — `AddExceptionHandler<ValidationExceptionHandler>()`, `AddProblemDetails()`, `UseExceptionHandler()`
- `src/FlowSpline.Api/FlowSpline.Api.csproj` — FluentValidation 11.11.0 package reference added

Files deleted:
- `src/FlowSpline.Api/WeatherForecast.cs` (scaffold template)
- `src/FlowSpline.Api/Controllers/WeatherForecastController.cs` (scaffold template)

Key decisions:
- `CreateAgentCommand`, `CreateExecutionCommand`, `RegisterToolCommand` → bağlanır doğrudan `[FromBody]` olarak (Id içermiyorlar)
- `UpdateAgent` için nested public record `UpdateAgentBody(string? SystemPrompt, bool? IsActive)` tanımlandı; Id route'dan alınır
- `ValidationException` → HTTP 400 `{ errors: { PropertyName: [messages] } }` dönüşümü `IExceptionHandler` implementasyonu ile sağlandı
- `PUT /api/agents/{id}` → önce agent varlığını kontrol eder, yoksa 404 döner
- `DELETE /api/agents/{id}` → agent bulunamazsa handler zaten no-op; 204 döner

Endpoint summary:
- POST   /api/agents         → 201 Created + { id }
- GET    /api/agents         → 200 AgentDto[]
- GET    /api/agents/{id}    → 200 AgentDto / 404
- PUT    /api/agents/{id}    → 204 / 404
- DELETE /api/agents/{id}    → 204
- POST   /api/executions     → 201 Created + { id }
- GET    /api/executions/{id}→ 200 ExecutionRunDto / 404
- POST   /api/tools          → 201 Created + { id }
- GET    /api/tools          → 200 ToolDefinitionDto[]
