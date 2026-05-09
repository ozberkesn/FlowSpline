# System Architecture

## Architecture Style
Modular Monolith First.

## Stack
Backend:
- .NET 10
- ASP.NET Core
- EF Core

Data:
- PostgreSQL
- pgvector
- Redis

Infra:
- Docker
- Kubernetes

Frontend:
- React
- Next.js

## High-Level Architecture

Frontend
↓
API
↓
Modules
- Agent Management
- Workflow Engine
- Execution Engine
- Tool Runtime
- Memory

Infrastructure
- Postgres
- Redis
- Workers

## Worker Pattern
API creates run.
Worker executes run.
UI tracks progress.