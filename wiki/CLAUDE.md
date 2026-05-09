# Wiki CLAUDE.md

This is the schema document for the FlowSpline development wiki.
The wiki is a persistent, LLM-maintained knowledge base for the FlowSpline project.

## Purpose

This wiki accumulates knowledge about FlowSpline: architecture decisions, domain model,
concepts, design rationale, open questions, and synthesis. The LLM writes and maintains
it; the developer reads and queries it. Knowledge compounds across sessions.

## Directory Structure

```
wiki/
├── CLAUDE.md          — this file; wiki conventions and workflows
├── index.md           — catalog of all pages with one-line summaries
├── log.md             — append-only chronological record of wiki operations
├── open-questions.md  — unresolved design questions identified during ingest
├── raw/
│   ├── articles/      — immutable source documents (project docs + external articles); never edited by LLM
│   └── assets/        — images and binary assets referenced by sources
├── decisions/         — ADR summaries with rationale and trade-offs
├── concepts/          — technical concept pages (patterns, architecture styles)
├── entities/          — domain entity and aggregate pages
└── sources/           — one page per ingested source document
```

## Page Format

All wiki pages use YAML frontmatter followed by a markdown body:

```yaml
---
title: <page title>
type: decision | concept | entity | source
tags: [tag1, tag2]
sources: [path/to/raw/source.md]
updated: YYYY-MM-DD
---
```

### Body structure by type

**entity/** pages:
- Identity (what is it, which aggregate/bounded context)
- Value objects it owns
- Behaviors (domain methods)
- Invariants
- Domain events it raises
- Cross-references to related entities and concepts

**concept/** pages:
- Definition
- How it applies in FlowSpline specifically
- Trade-offs and consequences
- Cross-references

**decision/** pages:
- Status (Accepted / Superseded / Proposed)
- Decision (the choice made)
- Context (why this decision was needed)
- Rationale (why this option was chosen)
- Trade-offs (what was given up)
- Cross-references

**source/** pages:
- Summary of the source document
- Key claims extracted
- Which wiki pages were updated during ingest
- Contradictions or open questions raised

## Ingest Workflow

When a new source document is added to `raw/articles/`:

1. Read the source document fully.
2. Discuss key takeaways with the developer if the source is non-trivial.
3. Create a page in `wiki/sources/` summarizing the source.
4. Update or create pages in `entities/`, `concepts/`, `decisions/` as appropriate.
   A single source may touch 10+ pages.
5. Flag any contradictions with existing wiki pages.
6. Update `wiki/index.md` with links to all new or significantly updated pages.
7. Append an entry to `wiki/log.md`:
   `## [YYYY-MM-DD] ingest | <source title>`

## Query Workflow

When answering a question about FlowSpline:

1. Read `wiki/index.md` to identify relevant pages.
2. Read those pages in full.
3. Synthesize an answer with citations (link to wiki pages, not raw sources).
4. If the answer is significant (a comparison, analysis, discovered connection),
   ask the developer whether to file it back as a new wiki page.

## Lint Workflow

Periodically health-check the wiki:

1. Scan all pages for: contradictions, stale claims, orphan pages (no inbound links),
   concepts mentioned but lacking their own page, missing cross-references.
2. Suggest new questions to investigate and new sources to find.
3. Append a lint entry to `wiki/log.md`:
   `## [YYYY-MM-DD] lint | <summary of findings>`

## Prohibitions

- **Never write to `raw/`** — sources are immutable; the LLM only reads from them.
- **Never make claims without a source** — cite wiki pages or raw documents.
- **Never delete pages** — deprecate with a `status: deprecated` frontmatter field instead.
- **Never overwrite `log.md`** — only append new entries.
- **Never invent domain facts** — if something is uncertain, add it to `open-questions.md`.
