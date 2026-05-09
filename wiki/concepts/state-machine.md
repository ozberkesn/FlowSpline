---
title: State Machine (ExecutionRun)
type: concept
tags: [state-machine, execution-engine, pattern, domain]
sources: [wiki/raw/articles/03-domain-model.md]
updated: 2026-05-03
---

# State Machine — ExecutionRun

## Definition

A state machine models an entity's lifecycle as an explicit set of states and allowed
transitions. In DDD, the state machine lives inside the aggregate and enforces that only
valid transitions are executed. Invalid transitions are rejected at the domain level.

## ExecutionRun States

| State | Meaning |
|---|---|
| `Created` | Run initialized, not yet started |
| `Running` | Actively executing |
| `WaitingApproval` | Paused; awaiting human approval to continue |
| `Completed` | Finished successfully |
| `Failed` | Terminated with an error |
| `Retrying` | Preparing to re-run after a failure |

## Transition Diagram

```
Created ──► Running ──► Completed
                │
                ├──► WaitingApproval ──► Running
                │
                └──► Failed ──► Retrying ──► Running
```

## Domain Enforcement

- Each behavior method validates the current state before transitioning
- Invalid transitions throw `InvalidOperationException`
- `Retry()` can only be called when `RetryCount < 3`
- Transitions use behavior-driven methods: `run.Complete()` not `run.Status = "Completed"`

## Human-in-the-Loop Pattern

The `WaitingApproval` state supports a human approval gate: the execution pauses and
waits for an external `Approve()` call before resuming. This pattern is relevant for
high-stakes agent actions that require human oversight.

## Cross-references

- [ExecutionRun (entity)](../entities/execution-run.md)
- [DDD Bounded Contexts (concept)](ddd-bounded-contexts.md)
