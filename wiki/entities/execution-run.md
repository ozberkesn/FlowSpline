---
title: ExecutionRun
type: entity
tags: [aggregate, execution-engine, state-machine]
bounded-context: ExecutionEngine
sources: [wiki/raw/articles/03-domain-model.md, wiki/raw/articles/09-application-layer.md]
updated: 2026-05-09
---

# ExecutionRun

**Type:** Aggregate Root  
**Bounded Context:** ExecutionEngine

## Identity

ExecutionRun tracks a single execution of an agent. It is the authoritative record of
what happened during a run: from creation through completion, failure, or human approval.

## Value Objects Owned

| Value Object | Fields |
|---|---|
| `RunContext` | agentId, input, sessionId |

> Note: Named `RunContext` (not `ExecutionContext`) to avoid collision with
> `System.Threading.ExecutionContext` in .NET.

## State Machine

```
Created ──► Running ──► Completed
                │
                ├──► WaitingApproval ──► Running
                │
                └──► Failed ──► Retrying ──► Running
```

## Behaviors

| Method | Transition | Guard |
|---|---|---|
| `Start()` | Created/Retrying → Running | — |
| `Complete()` | Running → Completed | — |
| `Fail(reason)` | Running → Failed | — |
| `RequestApproval()` | Running → WaitingApproval | — |
| `Approve()` | WaitingApproval → Running | — |
| `Retry()` | Failed → Retrying | RetryCount < 3 |

## Invariants

- Invalid state transitions throw `InvalidOperationException`
- Maximum retry count is 3

## Domain Events

- `ExecutionCreatedEvent`
- `ExecutionStartedEvent`
- `ExecutionCompletedEvent`
- `ExecutionFailedEvent`
- `ApprovalRequestedEvent`
- `ExecutionApprovedEvent`
- `ExecutionRetriedEvent`

## Application Layer Notes

`CreateExecutionCommand` creates an `ExecutionRun` in `Created` status and persists it.
It does **not** call `Start()`. Starting is the Worker's responsibility — the Worker polls
for runs in `Created` state and calls `Start()` before dispatching to the LLM.
See [Application Layer source](../sources/09-application-layer.md).

## Persistence Notes

- **Table:** `execution_runs`
- **RunContext** → Owned entity; columns: `agent_id`, `input`, `session_id` (inlined into `execution_runs` table)
- **Status** → stored as `varchar(20)` string via `.HasConversion<string>()` (human-readable, enum-rename-safe)
- **Indexes:** `IX_execution_runs_Status` (Worker queries by status); `IX_execution_runs_agent_id_session_id` (session filtering)
- **Implementation:** `src/FlowSpline.Persistence/ExecutionEngine/ExecutionRunRepository.cs`
- **Configuration:** `src/FlowSpline.Persistence/ExecutionEngine/Configurations/ExecutionRunConfiguration.cs`

## Cross-references

- [State Machine (concept)](../concepts/state-machine.md)
- [AgentDefinition (entity)](agent-definition.md)
- [ADR-003: Redis as Runtime Memory](../decisions/adr-003-redis-runtime-memory.md)
