---
title: "ADR-002: PostgreSQL over MongoDB"
type: source
tags: [adr, database, postgresql, pgvector]
sources: [wiki/raw/articles/adr/002-postgresql-over-mongo.md]
updated: 2026-05-05
---

# ADR-002: PostgreSQL over MongoDB

## Summary

Architecture Decision Record accepting PostgreSQL + pgvector as the primary data store over MongoDB. Chosen for transactional consistency, relational domain fit, and built-in vector similarity search via pgvector extension.

## Key Claims

- **Status**: Accepted
- **Decision**: PostgreSQL + pgvector
- **Reasons**: Transactional consistency, relational domain model, vector support (pgvector) for semantic memory

## Wiki Pages Updated

- [decisions/adr-002-postgresql-over-mongo](../decisions/adr-002-postgresql-over-mongo.md)

## Open Questions Raised

- [OQ-003](../open-questions.md#oq-003--pgvector-vs-redis-for-memory) — Relationship between pgvector (long-term semantic memory) and Redis (short-term session memory) not clarified in this ADR
