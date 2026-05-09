---
title: "ADR-003: Redis as Runtime Memory"
type: source
tags: [adr, redis, runtime, cache]
sources: [wiki/raw/articles/adr/003-redis-runtime-memory.md]
updated: 2026-05-05
---

# ADR-003: Redis as Runtime Memory

## Summary

Architecture Decision Record establishing Redis as the runtime memory store. Used for runtime state tracking, distributed locks, and caching.

## Key Claims

- **Decision**: Use Redis for runtime state, distributed locks, and caches
- **Status**: Implied Accepted (no explicit status field in source)

## Wiki Pages Updated

- [decisions/adr-003-redis-runtime-memory](../decisions/adr-003-redis-runtime-memory.md)

## Open Questions Raised

None directly from this ADR. See [OQ-003](../open-questions.md#oq-003--pgvector-vs-redis-for-memory) (raised by the combination of ADR-002 and ADR-003).
