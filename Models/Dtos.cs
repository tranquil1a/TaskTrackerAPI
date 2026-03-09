namespace TaskTrackerAPI.Models;

public record CreateBugReportDto(string Title, SeverityLevel SeverityLevel);

public record CreateFeatureRequestDto(string Title, double EstimatedHours);
