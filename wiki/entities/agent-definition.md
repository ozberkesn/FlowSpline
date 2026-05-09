---
title: AgentDefinition
type: entity
tags: [aggregate, agent-management]
bounded-context: AgentManagement
sources: [wiki/raw/articles/03-domain-model.md, wiki/raw/articles/09-application-layer.md]
updated: 2026-05-09
---

# AgentDefinition

**Type:** Aggregate Root  
**Bounded Context:** AgentManagement

## Identity

AgentDefinition is the root aggregate for configuring an AI agent. It owns everything
needed to describe an agent: its system prompt, its model settings, and its bound tools.

## Value Objects Owned

| Value Object | Fields |
|---|---|
| `ModelSettings` | provider, model, temperature, maxTokens |
| `Tool` | name only (binding reference; full definition lives in ToolRuntime) |

## Behaviors

| Method | Description | Guard |
|---|---|---|
| `BindTool(Tool)` | Adds tool to binding list | Max 10 tools; duplicate names rejected |
| `RemoveTool(Tool)` | Removes tool from binding list | — |
| `ChangePrompt(string)` | Updates the system prompt | — |
| `Activate()` | Marks agent active | — |
| `Deactivate()` | Marks agent inactive | — |

## Invariants

- Name must be at least 3 characters
- System prompt cannot be empty
- Model is required
- Maximum 10 tools bound; duplicate tool names are rejected

## Domain Events

- `AgentCreatedEvent`
- `ToolBoundEvent`
- `ToolRemovedEvent`
- `PromptChangedEvent`
- `AgentActivatedEvent`
- `AgentDeactivatedEvent`

## Application Layer Notes

`UpdateAgentCommand` covers `ChangePrompt`, `Activate`, and `Deactivate` via nullable
optional fields (`SystemPrompt?`, `IsActive?`). Only non-null fields are applied.
Name and model are not updatable — no corresponding domain behavior exists.

## Important Distinction

`AgentManagement.Tool` is a **name-only binding value object**. It references a tool by
name. The full tool specification lives in `ToolRuntime.ToolDefinition`. The binding does
not embed the schema — it only records the name. Validation against the ToolRuntime
catalog is an Application-layer concern.

See [OQ-007 — Tool binding validation against ToolRuntime catalog](../open-questions.md#oq-007--tool-binding-validation-against-toolruntime-catalog)

## Persistence Notes

- **Table:** `agents`
- **ModelSettings** → Owned entity; columns: `provider`, `model`, `temperature`, `max_tokens` (inlined into `agents` table)
- **Tools collection** → Separate table `agent_tools(AgentId uuid, Name varchar(100))` with composite PK `(AgentId, Name)` and cascade-delete FK to `agents`
- **Backing field:** `_tools` (List<Tool>); EF Core maps via field access mode
- **Implementation:** `src/FlowSpline.Persistence/AgentManagement/AgentRepository.cs`
- **Configuration:** `src/FlowSpline.Persistence/AgentManagement/Configurations/AgentDefinitionConfiguration.cs`

## Cross-references

- [AgentTeam (entity)](agent-team.md)
- [ToolDefinition (entity)](tool-definition.md)
- [DDD Bounded Contexts (concept)](../concepts/ddd-bounded-contexts.md)
- [CQRS (concept)](../concepts/cqrs.md)
