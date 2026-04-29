# Dependency Rules

## Rule
Outer layers depend inward.

## References

Domain
- none

Application
- Domain

Infrastructure
- Application
- Domain

Persistence
- Application
- Domain

Api
- Application
- Infrastructure
- Persistence

Worker
- Application
- Infrastructure

## Forbidden
- Domain -> Infrastructure
- Domain -> Persistence
- Application -> Infrastructure
- Application -> Persistence