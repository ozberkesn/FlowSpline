---
title: MemoryEntry
type: entity
tags: [aggregate, memory, redis]
bounded-context: Memory
sources: [wiki/raw/articles/03-domain-model.md]
updated: 2026-05-09
---

# MemoryEntry

**Type:** Aggregate Root  
**Bounded Context:** Memory

## Identity

MemoryEntry is a per-agent, per-session key-value memory record. In Phase 1, storage is
backed by Redis. The storage implementation detail lives in the Infrastructure layer —
the domain model is storage-agnostic.

## Behaviors

| Method | Description |
|---|---|
| `UpdateValue(string)` | Replaces the current value |
| `Expire()` | Sets ExpiresAt to current time (soft expiry — does not delete the record) |

## Invariants

- `AgentId` and `SessionId` are required
- `Key` and `Value` cannot be empty
- If `ExpiresAt` is null, the entry is permanent (no TTL)

## Domain Events

- `MemoryEntryCreatedEvent`
- `MemoryEntryUpdatedEvent`
- `MemoryEntryExpiredEvent`

## Storage Note

Phase 1: Redis (fast, TTL-based session memory).  
Phase 2 (planned): pgvector for long-term semantic/vector memory (RAG).  
The two storage types are **complementary**, not alternatives.

## Persistence Notes (Redis — Phase 1)

- **Key pattern:** `memory:{agentId}:{sessionId}:{key}`
- **Value:** JSON-serialized `MemoryData` record (AgentId, SessionId, Key, Value, CreatedAt, ExpiresAt)
- **TTL:** If `ExpiresAt` is set, Redis TTL is computed as `ExpiresAt - UtcNow`; null ExpiresAt = no TTL (permanent)
- **GetBySessionAsync** uses Redis SCAN via `IServer.Keys("memory:{agentId}:{sessionId}:*")` — single-node only; acceptable for Phase 1
- **Implementation:** `src/FlowSpline.Persistence/Memory/MemoryEntryRepository.cs`

## Open questions

See [OQ-003 — pgvector vs Redis for Memory](../open-questions.md#oq-003--pgvector-vs-redis-for-memory)

## Cross-references

- [ADR-003: Redis as Runtime Memory](../decisions/adr-003-redis-runtime-memory.md)
- [ADR-002: PostgreSQL over MongoDB](../decisions/adr-002-postgresql-over-mongo.md)
- [DDD Bounded Contexts (concept)](../concepts/ddd-bounded-contexts.md)
