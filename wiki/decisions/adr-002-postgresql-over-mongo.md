---
title: "ADR-002: PostgreSQL over MongoDB"
type: decision
status: Accepted
tags: [data, storage, database, postgresql, pgvector]
sources: [wiki/raw/articles/adr/002-postgresql-over-mongo.md]
updated: 2026-05-03
---

# ADR-002: PostgreSQL over MongoDB

**Status:** Accepted

## Decision

Use PostgreSQL with the pgvector extension as the primary data store. MongoDB was rejected.

## Context

The domain model is relational: agents belong to teams, executions reference agents, tools
are bound to agents. The Memory bounded context also requires vector similarity search for
retrieval-augmented generation workflows.

## Rationale

- Transactional consistency — domain aggregates need ACID guarantees
- Relational model — foreign keys, joins, and constraints fit the domain well
- pgvector — vector similarity search without a separate vector database
- Single data store — avoids the operational cost of polyglot persistence in early phases

## Trade-offs

- pgvector performance at scale is unproven compared to dedicated vector databases
  (Pinecone, Weaviate, Qdrant)
- Schema migrations via EF Core add friction compared to MongoDB's schema-less approach
- PostgreSQL requires more upfront schema design

## Open questions

See [OQ-003 — pgvector vs Redis for Memory](../open-questions.md#oq-003--pgvector-vs-redis-for-memory)

## Cross-references

- [MemoryEntry (entity)](../entities/memory-entry.md)
- [ADR-003: Redis as Runtime Memory](adr-003-redis-runtime-memory.md)
