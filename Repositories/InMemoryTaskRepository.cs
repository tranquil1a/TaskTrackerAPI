using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<BaseTask> _tasks = new()
    {
        new BugReportTask { Title = "Login page crashes on Safari", SeverityLevel = SeverityLevel.High },
        new BugReportTask { Title = "Minor UI misalignment", SeverityLevel = SeverityLevel.Low },
        new FeatureRequestTask { Title = "Dark mode support", EstimatedHours = 12.5 },
        new FeatureRequestTask { Title = "Export to PDF", EstimatedHours = 8.0 },
    };

    public IEnumerable<BaseTask> GetAll() => _tasks.AsReadOnly();

    public BaseTask? GetById(Guid id) =>
        _tasks.FirstOrDefault(t => t.Id == id);

    public void Add(BaseTask task) => _tasks.Add(task);
}
