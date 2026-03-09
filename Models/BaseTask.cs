namespace TaskTrackerAPI.Models;

public delegate void TaskCompletedEventHandler(BaseTask task);

public abstract class BaseTask
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }

    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; private set; }

    public event TaskCompletedEventHandler? OnTaskCompleted;

    protected BaseTask()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public void CompleteTask()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            OnTaskCompleted?.Invoke(this);
        }
    }
}
