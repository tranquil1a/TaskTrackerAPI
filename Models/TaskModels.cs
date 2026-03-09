namespace TaskTrackerAPI.Models;

public enum SeverityLevel { Low, Medium, High, Critical }

public class BugReportTask : BaseTask
{
    public SeverityLevel SeverityLevel { get; set; }
}

public class FeatureRequestTask : BaseTask
{
    public double EstimatedHours { get; set; }
}
