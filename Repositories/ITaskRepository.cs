using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Repositories;

public interface ITaskRepository
{
    IEnumerable<BaseTask> GetAll();
    BaseTask? GetById(Guid id);
    void Add(BaseTask task);
}
