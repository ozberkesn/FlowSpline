---
title: ToolDefinition
type: entity
tags: [aggregate, tool-runtime]
bounded-context: ToolRuntime
sources: [wiki/raw/articles/03-domain-model.md]
updated: 2026-05-09
---

# ToolDefinition

**Type:** Aggregate Root  
**Bounded Context:** ToolRuntime

## Identity

ToolDefinition is the system-wide registration of a tool. It holds the tool's
description and optional input/output JSON Schema. It is the authoritative catalog entry
for a tool — distinct from `AgentManagement.Tool`, which is only a name-binding reference
inside an agent.

## Value Objects Owned

| Value Object | Fields |
|---|---|
| `ToolSchema` | inputSchema (nullable JSON Schema string), outputSchema (nullable JSON Schema string) |

## Behaviors

| Method | Description |
|---|---|
| `Enable()` | Makes the tool available for agent binding |
| `Disable()` | Marks the tool unavailable; effect on existing bindings unspecified |
| `UpdateDescription(string)` | Updates the human-readable description |
| `UpdateSchema(ToolSchema)` | Updates input/output schema |

## Invariants

- Tool name must match slug format: `[a-z0-9_-]`
- Description is required

## Domain Events

- `ToolRegisteredEvent`
- `ToolEnabledEvent`
- `ToolDisabledEvent`
- `ToolDescriptionUpdatedEvent`
- `ToolSchemaUpdatedEvent`

## Distinction from AgentManagement.Tool

`AgentManagement.Tool` is a **name-only binding value object** inside AgentDefinition.
`ToolRuntime.ToolDefinition` is the **full registered tool** in the system catalog.
These live in separate bounded contexts and do not directly reference each other.

## Persistence Notes

- **Table:** `tool_definitions`
- **Name** → `varchar(200)` with **unique index** `IX_tool_definitions_Name`
- **ToolSchema** → Owned entity; columns: `input_schema` (text, nullable), `output_schema` (text, nullable) (inlined into `tool_definitions` table)
- **Implementation:** `src/FlowSpline.Persistence/ToolRuntime/ToolDefinitionRepository.cs`
- **Configuration:** `src/FlowSpline.Persistence/ToolRuntime/Configurations/ToolDefinitionConfiguration.cs`

## Open questions

See [OQ-007 — Tool binding validation against ToolRuntime catalog](../open-questions.md#oq-007--tool-binding-validation-against-toolruntime-catalog)  
See [OQ-010 — Disable tool effect on existing bindings](../open-questions.md#oq-010--disable-tool-effect-on-existing-bindings)

## Cross-references

- [AgentDefinition (entity)](agent-definition.md)
- [DDD Bounded Contexts (concept)](../concepts/ddd-bounded-contexts.md)
