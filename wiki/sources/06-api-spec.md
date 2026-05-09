---
title: API Spec
type: source
tags: [api, rest, endpoints]
sources: [wiki/raw/articles/06-api-spec.md]
updated: 2026-05-05
---

# API Spec

## Summary

REST API contract for FlowSpline. Defines endpoints for Agents (full CRUD), Executions (create + get), and Tools (create + list). No authentication or authorization details are specified in this document.

## Key Claims

- **Agents**: `POST /api/agents`, `GET /api/agents`, `GET /api/agents/{id}`, `PUT /api/agents/{id}`, `DELETE /api/agents/{id}`
- **Executions**: `POST /api/executions`, `GET /api/executions/{id}`
- **Tools**: `POST /api/tools`, `GET /api/tools`
- No auth scheme documented

## Wiki Pages Updated

None — no dedicated API spec wiki pages created in initial ingest; Phase 1 implementation will drive these.

## Open Questions Raised

- [OQ-005](../open-questions.md#oq-005--api-authentication) — No auth/authorization details: deferred to Phase 2 Governance? JWT, API keys, or OAuth2?
