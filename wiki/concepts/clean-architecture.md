---
title: Clean Architecture
type: concept
tags: [architecture, layers, dependency-rules]
sources: [wiki/raw/articles/01-system-architecture.md, wiki/raw/articles/02-dependency-rules.md]
updated: 2026-05-03
---

# Clean Architecture

## Definition

Clean Architecture organizes code into concentric layers where dependencies only point
inward (toward the domain). Each layer knows only about the layers inside it. Business
rules are independent of frameworks, databases, and delivery mechanisms.

## FlowSpline Layer Map

| Layer | Project | May depend on |
|---|---|---|
| Domain | `FlowSpline.Domain` | nothing |
| Application | `FlowSpline.Application` | Domain |
| Infrastructure | `FlowSpline.Infrastructure` | Application, Domain |
| Persistence | `FlowSpline.Persistence` | Application, Domain |
| Api | `FlowSpline.Api` | Application, Infrastructure, Persistence |
| Worker | `FlowSpline.Worker` | Application, Infrastructure |

## Forbidden Dependencies

- **Domain → anything** — Domain is the innermost layer; it has zero external dependencies
- **Application → Infrastructure** — Application defines interfaces; Infrastructure implements them
- **Application → Persistence** — same reason; Application cannot import EF Core directly

## Consequences in Practice

- Domain layer: pure C#, no NuGet packages, easiest to unit-test
- Application layer: cannot import EF Core, Redis, or HTTP clients; depends only on Domain interfaces
- Infrastructure layer: implements Application interfaces; depends on specific LLM SDKs, etc.
- Persistence layer: implements repository interfaces; owns EF Core `DbContext`, migrations
- A dependency violation is an **architectural error**, not a style issue

## Cross-references

- [DDD Bounded Contexts (concept)](ddd-bounded-contexts.md)
- [CQRS (concept)](cqrs.md)
- [ADR-001: Modular Monolith First](../decisions/adr-001-modular-monolith-first.md)
- [ADR-004: CQRS in Application Layer](../decisions/adr-004-cqrs-application-layer.md)
