# Integration Pattern for NotificationService
## Block 3 Answer

### Question
If we need a NotificationService that sends an email when the OnTaskCompleted
event fires in Task Service — should we use synchronous or asynchronous integration?

---

### Chosen Pattern: ASYNCHRONOUS via Message Broker (RabbitMQ)

**Recommendation:** Publish a message to a RabbitMQ queue when a task completes.
A separate NotificationService subscribes to that queue and sends emails independently.

### Why Asynchronous?

1. **Decoupling** — Task Service doesn't need to know NotificationService exists.
   If the email service is down, tasks can still be completed without errors.

2. **Reliability** — RabbitMQ persists messages. If NotificationService crashes,
   it picks up queued events on restart (no lost notifications).

3. **Performance** — Sending an email (SMTP) is slow (100–500ms+). Doing it
   synchronously inside the HTTP request would degrade API response times.

4. **Scalability** — NotificationService can scale independently; multiple
   consumer instances can drain the queue in parallel.

### How It Would Work

```
TasksController
    │
    └─► CompleteTask() fires OnTaskCompleted event
            │
            └─► TaskEventPublisher.PublishAsync("task.completed", { taskId, title, ... })
                        │  (RabbitMQ / Azure Service Bus)
                        ▼
                NotificationService (separate process/container)
                        │
                        └─► Sends email via SendGrid / SMTP
```

### Technologies

| Component         | Technology                              |
|-------------------|-----------------------------------------|
| Message broker    | RabbitMQ (or Azure Service Bus)         |
| .NET client       | MassTransit or RabbitMQ.Client NuGet    |
| Email delivery    | SendGrid SDK / MailKit (SMTP)           |

### When Would Synchronous (HTTP/REST) Make Sense?
Only if the caller MUST know the email was sent before responding to the user
(e.g., OTP delivery). For task completion notifications, that constraint doesn't
apply, so async is the right choice.
