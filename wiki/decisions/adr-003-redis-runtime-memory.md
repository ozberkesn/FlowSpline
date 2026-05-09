---
title: "ADR-003: Redis as Runtime Memory"
type: decision
status: Accepted
tags: [data, storage, cache, redis, runtime]
sources: [wiki/raw/articles/adr/003-redis-runtime-memory.md]
updated: 2026-05-03
---

# ADR-003: Redis as Runtime Memory

**Status:** Accepted

## Decision

Use Redis for: runtime state, distributed locks, and caches.

## Context

The execution engine needs fast, ephemeral storage for in-flight run state. The worker
service needs distributed locking to prevent duplicate execution. The API needs caching
to reduce database load for frequently-read data.

## Rationale

- Low-latency reads and writes suit runtime execution state
- Built-in TTL support suits session memory (MemoryEntry Phase 1)
- Pub/sub capabilities available for future worker-API coordination
- Atomic operations enable reliable distributed locking

## Trade-offs

- In-memory: data lost on Redis restart unless persistence (AOF/RDB) is configured
- Adds operational complexity — a second stateful service alongside PostgreSQL
- Redis Cluster adds further complexity if horizontal scaling is needed

## Open questions

See [OQ-003 — pgvector vs Redis for Memory](../open-questions.md#oq-003--pgvector-vs-redis-for-memory)  
See [OQ-006 — Worker-API coordination mechanism](../open-questions.md#oq-006--worker-api-coordination-mechanism)

## Cross-references

- [MemoryEntry (entity)](../entities/memory-entry.md)
- [ExecutionRun (entity)](../entities/execution-run.md)
- [ADR-002: PostgreSQL over MongoDB](adr-002-postgresql-over-mongo.md)
