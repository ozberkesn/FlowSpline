---
title: Domain Model
type: source
tags: [domain, aggregates, ddd, bounded-contexts]
sources: [wiki/raw/articles/03-domain-model.md]
updated: 2026-05-05
---

# Domain Model

## Summary

The authoritative domain model document. Describes 6 bounded contexts (4 implemented, 2 planned for Phase 2), all aggregate roots with their value objects, behaviors, invariants, and domain events. Also specifies the folder structure convention for Domain project files.

## Key Claims

- **4 implemented contexts**: AgentManagement, ExecutionEngine, ToolRuntime, Memory
- **2 planned (Phase 2)**: WorkflowEngine, Governance
- **AgentDefinition**: BindTool (max 10, no duplicates), ChangePrompt, Activate/Deactivate; name ≥ 3 chars
- **AgentTeam**: Supervisor required; circular delegation forbidden — enforced in Application layer (not Domain) because it requires repository access
- **ExecutionRun**: Full state machine (Created → Running → WaitingApproval → Completed/Failed/Retrying); max 3 retries
- **RunContext** (not ExecutionContext — name clash with `System.Threading.ExecutionContext`)
- **ToolDefinition**: name must match slug format `[a-z0-9_-]`; `AgentManagement.Tool` (binding) ≠ `ToolRuntime.ToolDefinition` (full record)
- **MemoryEntry**: Phase 1 on Redis; key-value per agent+session; ExpiresAt null = permanent

## Wiki Pages Updated

- [AgentDefinition](../entities/agent-definition.md)
- [AgentTeam](../entities/agent-team.md)
- [ExecutionRun](../entities/execution-run.md)
- [ToolDefinition](../entities/tool-definition.md)
- [MemoryEntry](../entities/memory-entry.md)
- [DDD Bounded Contexts](../concepts/ddd-bounded-contexts.md)
- [State Machine](../concepts/state-machine.md)

## Open Questions Raised

- [OQ-001](../open-questions.md#oq-001--circular-delegation-enforcement-layer) — Circular delegation: Application layer placement vs. domain service
- [OQ-003](../open-questions.md#oq-003--pgvector-vs-redis-for-memory) — pgvector vs Redis split for memory
- [OQ-007](../open-questions.md#oq-007--tool-binding-validation-against-toolruntime-catalog) — Tool binding validation against ToolRuntime catalog
- [OQ-008](../open-questions.md#oq-008--retrycount-storage-location) — RetryCount storage location in ExecutionRun
- [OQ-010](../open-questions.md#oq-010--disable-tool-effect-on-existing-bindings) — ToolDefinition.Disable() effect on existing agent bindings
