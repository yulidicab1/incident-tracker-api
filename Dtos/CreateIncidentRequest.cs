using IncidentTrackerApi.Models;

namespace IncidentTrackerApi.Dtos;

public class CreateIncidentRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Severity Severity { get; set; }
}