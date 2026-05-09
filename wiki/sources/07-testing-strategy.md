---
title: Testing Strategy
type: source
tags: [testing, xunit, integration, unit]
sources: [wiki/raw/articles/07-testing-strategy.md]
updated: 2026-05-05
---

# Testing Strategy

## Summary

Two-tier testing strategy: unit tests for domain rules, commands, and retry logic (xUnit + FluentAssertions + Moq); integration tests for PostgreSQL, Redis, and Worker flows. Testcontainers is listed as a future addition for integration test environment setup.

## Key Claims

- **Unit test scope**: Domain rules, commands, retry logic
- **Unit test tooling**: xUnit, FluentAssertions, Moq
- **Integration test scope**: PostgreSQL, Redis, Worker flows
- **Future**: Testcontainers (not yet adopted)

## Wiki Pages Updated

None — testing strategy is operational; no new architectural pages created.

## Open Questions Raised

- [OQ-009](../open-questions.md#oq-009--testcontainers-timeline) — Current integration test environment: manual Docker? Fixed connection string? Testcontainers timeline?
