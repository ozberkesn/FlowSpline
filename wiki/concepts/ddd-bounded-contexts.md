---
title: DDD Bounded Contexts
type: concept
tags: [ddd, bounded-context, architecture, domain]
sources: [wiki/raw/articles/03-domain-model.md, wiki/raw/articles/01-system-architecture.md]
updated: 2026-05-03
---

# DDD Bounded Contexts

## Definition

A bounded context is an explicit boundary within which a domain model is internally
consistent. Terms, rules, and models inside one context do not bleed into another.
Contexts communicate only via well-defined interfaces or domain events.

## FlowSpline Bounded Contexts

| Context | Status | Responsibility |
|---|---|---|
| AgentManagement | Implemented | Agent and team definition |
| ExecutionEngine | Implemented | Execution run state machine |
| ToolRuntime | Implemented | Tool registration and management |
| Memory | Implemented | Agent session memory |
| WorkflowEngine | Phase 2 | Multi-agent workflow orchestration |
| Governance | Phase 2 | Access control and policy enforcement |

## Folder Convention

Each bounded context gets its own subfolder under `FlowSpline.Domain/`:

```
FlowSpline.Domain/
├── Common/          — AggregateRoot base class only
├── AgentManagement/
│   ├── Aggregates/
│   ├── Events/
│   └── ValueObjects/
├── ExecutionEngine/
├── ToolRuntime/
└── Memory/
```

**Rule:** No `Common`/`Shared` dump projects. Code lives in its bounded context.

## Cross-context Terminology (Ubiquitous Language)

The same word can mean different things in different contexts:

| Term | Context | Meaning |
|---|---|---|
| `Tool` | AgentManagement | Name-only binding value object inside an agent |
| `ToolDefinition` | ToolRuntime | Full registered tool in the system catalog |

This ambiguity is intentional and healthy — do not unify without a clear reason.

## Cross-references

- [AgentDefinition (entity)](../entities/agent-definition.md)
- [AgentTeam (entity)](../entities/agent-team.md)
- [ExecutionRun (entity)](../entities/execution-run.md)
- [ToolDefinition (entity)](../entities/tool-definition.md)
- [MemoryEntry (entity)](../entities/memory-entry.md)
- [Clean Architecture (concept)](clean-architecture.md)
- [ADR-001: Modular Monolith First](../decisions/adr-001-modular-monolith-first.md)
