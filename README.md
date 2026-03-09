# Task Tracker API

A microservice for task management built with ASP.NET Core 8 (.NET 8).

## Project Structure

```
TaskTrackerAPI/
├── Models/
│   ├── BaseTask.cs          # Abstract base class with delegate + event
│   ├── TaskModels.cs        # BugReportTask, FeatureRequestTask
│   └── Dtos.cs              # Record-based DTOs
├── Repositories/
│   ├── ITaskRepository.cs   # Interface (for DI)
│   └── InMemoryTaskRepository.cs
├── Services/
│   └── TaskFilterService.cs # Static class with LINQ queries
├── Controllers/
│   └── TasksController.cs   # Web API endpoints
├── Program.cs               # App entry + DI registration
├── Dockerfile               # Multi-stage build
├── docker-compose.yml
└── INTEGRATION_NOTES.md     # Block 3 answer
```

## Running Locally

```bash
cd TaskTrackerAPI
dotnet run
# API available at https://localhost:5001
# Swagger UI at https://localhost:5001/swagger
```

## Running with Docker

```bash
docker-compose up --build
# API available at http://localhost:8080
```

## API Endpoints

| Method | Route                        | Description                         |
|--------|------------------------------|-------------------------------------|
| GET    | /api/tasks                   | Get all tasks                       |
| POST   | /api/tasks/bug               | Create a bug report                 |
| POST   | /api/tasks/feature           | Create a feature request            |
| PUT    | /api/tasks/{id}/complete     | Complete a task (fires event)       |
| GET    | /api/tasks/stats             | LINQ stats (high bugs, hours total) |

## Example Requests

### Create a bug report
```json
POST /api/tasks/bug
{
  "title": "Crash on login",
  "severityLevel": 2
}
// SeverityLevel: 0=Low, 1=Medium, 2=High, 3=Critical
```

### Create a feature request
```json
POST /api/tasks/feature
{
  "title": "Dark mode",
  "estimatedHours": 16.5
}
```

### Complete a task
```
PUT /api/tasks/{guid}/complete
```

## Key Design Decisions

- **`init`-only properties** on `Id` and `CreatedAt` enforce encapsulation (Block 1.2)
- **`record` types** used for DTOs — idiomatic modern C# (Block 1)
- **Pattern matching** (`switch` expression) in controller for type-safe serialization
- **Singleton** repository registration keeps in-memory state across requests
- **Async/RabbitMQ** chosen for NotificationService integration (see INTEGRATION_NOTES.md)
