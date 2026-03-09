using TaskTrackerAPI.Models;

namespace TaskTrackerAPI.Services;

public static class TaskFilterService
{
    public static IEnumerable<BugReportTask> GetHighSeverityIncompleteBugs(
        IEnumerable<BaseTask> tasks) =>
        tasks
            .OfType<BugReportTask>()
            .Where(t => !t.IsCompleted &&
                        t.SeverityLevel is SeverityLevel.High or SeverityLevel.Critical)
            .OrderByDescending(t => t.CreatedAt);

    public static double GetTotalEstimatedHoursForIncompleteFeatures(
        IEnumerable<BaseTask> tasks) =>
        tasks
            .OfType<FeatureRequestTask>()
            .Where(t => !t.IsCompleted)
            .Sum(t => t.EstimatedHours);
}
