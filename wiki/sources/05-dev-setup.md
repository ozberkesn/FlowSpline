---
title: Development Setup
type: source
tags: [setup, dev, docker, dotnet]
sources: [wiki/raw/articles/05-dev-setup.md]
updated: 2026-05-05
---

# Development Setup

## Summary

Operational guide for running the FlowSpline stack locally. Prerequisites are .NET 10, Docker, PostgreSQL, and Redis. Infrastructure starts via `docker compose up`; API via `dotnet run`; migrations via `dotnet ef database update`.

## Key Claims

- **Prerequisites**: .NET 10, Docker, PostgreSQL, Redis
- **Infra start**: `docker compose up`
- **Run API**: `dotnet run --project src/FlowSpline.Api`
- **Migrations**: `dotnet ef database update`

## Wiki Pages Updated

None — operational content, no architectural pages created from this source.

## Open Questions Raised

- [OQ-009](../open-questions.md#oq-009--testcontainers-timeline) — Integration test environment setup: currently manual Docker? When is Testcontainers adoption planned?
