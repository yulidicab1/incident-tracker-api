using IncidentTrackerApi.Models;

namespace IncidentTrackerApi.Dtos;

public class UpdateIncidentRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Severity? Severity { get; set; }
    public Status? Status { get; set; }
}