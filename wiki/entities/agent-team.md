---
title: AgentTeam
type: entity
tags: [aggregate, agent-management, delegation]
bounded-context: AgentManagement
sources: [wiki/raw/articles/03-domain-model.md]
updated: 2026-05-09
---

# AgentTeam

**Type:** Aggregate Root  
**Bounded Context:** AgentManagement

## Identity

AgentTeam groups a set of agents under a designated supervisor. A team enables
multi-agent delegation patterns: the supervisor can hand off work to member agents.

## Behaviors

| Method | Description | Guard |
|---|---|---|
| `AddMember(Guid agentId)` | Adds agent to team | Supervisor cannot be added as a member |
| `RemoveMember(Guid agentId)` | Removes agent from team | Supervisor cannot be removed |
| `ChangeSupervisor(Guid agentId)` | Designates a new supervisor | New supervisor must already be a member |

## Invariants

- A supervisor is **required** — validated at construction time
- The supervisor cannot be a regular member simultaneously
- Circular delegation is **forbidden** — see note below

## Circular Delegation Note

The domain enforces only intra-aggregate invariants: "supervisor cannot also be a member."
Cross-aggregate circular delegation (Agent A supervises Team B which contains Agent A)
cannot be checked in the domain alone because it requires a repository query. This check
lives in the **Application layer**.

See [OQ-001 — Circular delegation enforcement layer](../open-questions.md#oq-001--circular-delegation-enforcement-layer)

## Domain Events

- `AgentTeamCreatedEvent`
- `MemberAddedEvent`
- `MemberRemovedEvent`
- `SupervisorChangedEvent`

## Persistence Notes

- **Table:** `agent_teams`
- **MemberIds** → `HashSet<Guid>` backing field `_memberIds` stored as `jsonb` column `member_ids` via a `ValueConverter<HashSet<Guid>, string>` (JSON serialized Guid array)
- **SupervisorId** → plain `uuid` column, not a FK (agents are in a separate bounded context)
- **Implementation:** `src/FlowSpline.Persistence/AgentManagement/AgentTeamRepository.cs`
- **Configuration:** `src/FlowSpline.Persistence/AgentManagement/Configurations/AgentTeamConfiguration.cs`

## Cross-references

- [AgentDefinition (entity)](agent-definition.md)
- [DDD Bounded Contexts (concept)](../concepts/ddd-bounded-contexts.md)
