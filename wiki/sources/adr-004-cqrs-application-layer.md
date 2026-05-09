---
title: "ADR-004: CQRS in Application Layer"
type: source
tags: [adr, cqrs, application-layer, commands, queries]
sources: [wiki/raw/articles/adr/004-cqrs-application-layer.md]
updated: 2026-05-05
---

# ADR-004: CQRS in Application Layer

## Summary

Architecture Decision Record establishing CQRS as the pattern for the Application layer. Commands mutate state; queries read state. This is the foundational pattern for all Application-layer operations in FlowSpline.

## Key Claims

- **Decision**: CQRS in Application layer
- **Commands**: mutate state
- **Queries**: read state

## Wiki Pages Updated

- [decisions/adr-004-cqrs-application-layer](../decisions/adr-004-cqrs-application-layer.md)
- [CQRS](../concepts/cqrs.md)

## Open Questions Raised

None.
