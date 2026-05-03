# Domain Model

## Bounded Contexts

| Context | Status | Sorumlu Alan |
|---|---|---|
| AgentManagement | Implemented | Agent ve team tanımları |
| ExecutionEngine | Implemented | Execution run state machine |
| ToolRuntime | Implemented | Araç kayıt ve yönetimi |
| Memory | Implemented | Agent session belleği |
| WorkflowEngine | Planned (Phase 2) | Çok-agent iş akışı |
| Governance | Planned (Phase 2) | Erişim kontrolü ve policy |

---

## Klasör Yapısı Kuralı

Domain dosyaları bounded context alt klasörlerine göre organize edilir:

```
FlowSpline.Domain/
├── Common/
│   └── AggregateRoot.cs
├── AgentManagement/
│   ├── Aggregates/
│   ├── Events/
│   └── ValueObjects/
├── ExecutionEngine/
│   ├── Aggregates/
│   ├── Enums/
│   ├── Events/
│   └── ValueObjects/
├── ToolRuntime/
│   ├── Aggregates/
│   ├── Events/
│   └── ValueObjects/
└── Memory/
    ├── Aggregates/
    └── Events/
```

---

## AgentManagement

### AgentDefinition (Aggregate Root)

**Value Objects:** `ModelSettings` (provider, model, temperature, maxTokens), `Tool` (name)

**Davranışlar:**
- `BindTool(Tool)` — max 10 tool, duplicate yasak
- `RemoveTool(Tool)`
- `ChangePrompt(string)`
- `Activate()` / `Deactivate()`

**Domain Events:** `AgentCreatedEvent`, `ToolBoundEvent`, `ToolRemovedEvent`, `PromptChangedEvent`, `AgentActivatedEvent`, `AgentDeactivatedEvent`

**Invariantlar:**
- Agent name minimum 3 karakter
- System prompt boş olamaz
- Model zorunlu

---

### AgentTeam (Aggregate Root)

**Davranışlar:**
- `AddMember(Guid agentId)` — supervisor member olarak eklenemez
- `RemoveMember(Guid agentId)` — supervisor çıkarılamaz
- `ChangeSupervisor(Guid agentId)` — yeni supervisor mevcut member olmalı

**Domain Events:** `AgentTeamCreatedEvent`, `MemberAddedEvent`, `MemberRemovedEvent`, `SupervisorChangedEvent`

**Invariantlar:**
- Supervisor zorunlu (constructor'da validate edilir)
- Circular delegation yasak — domain tek aggregate'in kendi sınırını kontrol eder; circular kontrol Application katmanında yapılır

---

## ExecutionEngine

### ExecutionRun (Aggregate Root)

**Value Objects:** `RunContext` (agentId, input, sessionId)

> Not: `ExecutionContext` adı `System.Threading.ExecutionContext` ile çakıştığı için `RunContext` olarak adlandırıldı.

**State Machine:**

```
Created ──► Running ──► Completed
                │
                ├──► WaitingApproval ──► Running
                │
                └──► Failed ──► Retrying ──► Running
```

**Davranışlar:**
- `Start()` — Created veya Retrying → Running
- `Complete()` — Running → Completed
- `Fail(reason)` — Running → Failed
- `RequestApproval()` — Running → WaitingApproval
- `Approve()` — WaitingApproval → Running
- `Retry()` — Failed → Retrying (max 3)

**Domain Events:** `ExecutionCreatedEvent`, `ExecutionStartedEvent`, `ExecutionCompletedEvent`, `ExecutionFailedEvent`, `ApprovalRequestedEvent`, `ExecutionRetriedEvent`

**Invariantlar:**
- Geçersiz state transition → `InvalidOperationException`
- Max retry sayısı: 3

---

## ToolRuntime

### ToolDefinition (Aggregate Root)

**Value Objects:** `ToolSchema` (inputSchema, outputSchema — her ikisi de nullable JSON Schema string)

**Davranışlar:**
- `Enable()` / `Disable()`
- `UpdateDescription(string)`
- `UpdateSchema(ToolSchema)`

**Domain Events:** `ToolRegisteredEvent`, `ToolEnabledEvent`, `ToolDisabledEvent`

**Invariantlar:**
- Tool name slug formatında olmalı: `[a-z0-9_-]`
- Description zorunlu

> Not: `AgentManagement.Tool` (yalnızca isim içeren binding value object) ile `ToolRuntime.ToolDefinition` (sistemdeki tam kayıt) birbirinden bağımsızdır.

---

## Memory

### MemoryEntry (Aggregate Root)

Agent session başına key-value bellek kaydı. Phase 1'de Redis üzerinde çalışır; storage detayı Infrastructure katmanındadır.

**Davranışlar:**
- `UpdateValue(string)`
- `Expire()` — ExpiresAt'i şimdiki zaman olarak set eder (soft expire)

**Domain Events:** `MemoryEntryCreatedEvent`, `MemoryEntryUpdatedEvent`, `MemoryEntryExpiredEvent`

**Invariantlar:**
- AgentId ve SessionId zorunlu
- Key ve Value boş olamaz
- ExpiresAt null ise kalıcı kayıt

---

## Henüz Implement Edilmeyenler (Phase 2)

**WorkflowEngine — Workflow aggregate**
- Çok-agent iş akışı tanımı ve orchestration
- Multi-agent handoff ve delegation

**Governance — Policy aggregate**
- Erişim kontrolü ve yetkilendirme
- Tool binding authorization
