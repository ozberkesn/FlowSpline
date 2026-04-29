# Domain Model

## Bounded Contexts
- AgentManagement
- WorkflowEngine
- ExecutionEngine
- ToolRuntime
- Memory
- Governance

## Aggregates

## Agent Aggregate
Root:
AgentDefinition

Contains:
- PromptConfig
- ToolBindings
- ModelSettings
- Policies

Rules:
- Agent must have model
- Unauthorized tool cannot bind

---

## Team Aggregate
Root:
AgentTeam

Rules:
- Supervisor required
- Circular delegation forbidden

---

## Execution Aggregate
Root:
ExecutionRun

States:
Created
Running
WaitingApproval
Completed
Failed
Retrying