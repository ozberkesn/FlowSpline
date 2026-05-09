---
title: Dependency Rules
type: source
tags: [architecture, dependencies, clean-architecture]
sources: [wiki/raw/articles/02-dependency-rules.md]
updated: 2026-05-05
---

# Dependency Rules

## Summary

Defines the strict inward-only dependency flow for Clean Architecture layers. Domain has no dependencies. Each outer layer may only depend on layers closer to the center. Four specific cross-layer dependencies are explicitly forbidden.

## Key Claims

- **Rule**: Outer layers depend inward only
- **Allowed**: Domain (none), Application → Domain, Infrastructure → Application+Domain, Persistence → Application+Domain, Api → Application+Infrastructure+Persistence, Worker → Application+Infrastructure
- **Forbidden**: Domain → Infrastructure, Domain → Persistence, Application → Infrastructure, Application → Persistence
- This document is the authoritative reference for dependency violation checks

## Wiki Pages Updated

- [Clean Architecture](../concepts/clean-architecture.md) — full dependency table

## Open Questions Raised

None.
