using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Models;
using TaskTrackerAPI.Repositories;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskRepository repository, ILogger<TasksController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var tasks = _repository.GetAll();
        return Ok(tasks.Select(t => t switch
        {
            BugReportTask bug => (object)new
            {
                bug.Id, bug.Title, bug.CreatedAt, bug.IsCompleted,
                Type = "BugReport", bug.SeverityLevel
            },
            FeatureRequestTask feat => new
            {
                feat.Id, feat.Title, feat.CreatedAt, feat.IsCompleted,
                Type = "FeatureRequest", feat.EstimatedHours
            },
            _ => new { t.Id, t.Title, t.CreatedAt, t.IsCompleted, Type = "Unknown" }
        }));
    }

    [HttpPost("bug")]
    public IActionResult CreateBug([FromBody] CreateBugReportDto dto)
    {
        var task = new BugReportTask
        {
            Title = dto.Title,
            SeverityLevel = dto.SeverityLevel
        };

        task.OnTaskCompleted += OnTaskCompleted;
        _repository.Add(task);

        return CreatedAtAction(nameof(GetAll), new { id = task.Id }, new
        {
            task.Id, task.Title, task.CreatedAt, task.IsCompleted,
            Type = "BugReport", task.SeverityLevel
        });
    }

    [HttpPost("feature")]
    public IActionResult CreateFeature([FromBody] CreateFeatureRequestDto dto)
    {
        var task = new FeatureRequestTask
        {
            Title = dto.Title,
            EstimatedHours = dto.EstimatedHours
        };

        task.OnTaskCompleted += OnTaskCompleted;
        _repository.Add(task);

        return CreatedAtAction(nameof(GetAll), new { id = task.Id }, new
        {
            task.Id, task.Title, task.CreatedAt, task.IsCompleted,
            Type = "FeatureRequest", task.EstimatedHours
        });
    }

    [HttpPut("{id:guid}/complete")]
    public IActionResult CompleteTask(Guid id)
    {
        var task = _repository.GetById(id);
        if (task is null)
            return NotFound(new { message = $"Task {id} not found." });

        task.OnTaskCompleted += OnTaskCompleted;
        task.CompleteTask();

        return Ok(new { message = $"Task '{task.Title}' marked as complete." });
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        var all = _repository.GetAll();
        var highBugs = TaskFilterService.GetHighSeverityIncompleteBugs(all);
        var totalHours = TaskFilterService.GetTotalEstimatedHoursForIncompleteFeatures(all);

        return Ok(new
        {
            HighSeverityIncompleteBugs = highBugs.Select(b => new
            {
                b.Id, b.Title, b.CreatedAt, b.SeverityLevel
            }),
            TotalIncompleteFeatureHours = totalHours
        });
    }

    private void OnTaskCompleted(BaseTask task)
    {
        _logger.LogInformation("Task completed event fired: [{Id}] {Title} at {Time}",
            task.Id, task.Title, DateTime.UtcNow);
    }
}
