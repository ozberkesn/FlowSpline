# Engineering Standards

## Rules
- Clean Architecture
- DDD boundaries
- Vertical slices
- No Shared/Common dump project

## Code
Bad:
entity.Status="Done"

Good:
entity.Complete()

## Naming
Commands:
CreateAgentCommand

Handlers:
CreateAgentCommandHandler

Tests:
CreateAgent_WhenInvalid_ShouldFail

## Git
main
develop
feature/*