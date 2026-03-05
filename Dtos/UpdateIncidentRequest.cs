using System.ComponentModel.DataAnnotations;
using IncidentTrackerApi.Models;

namespace IncidentTrackerApi.Dtos;

public class UpdateIncidentRequest
{
    [MinLength(3), MaxLength(120)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public Severity? Severity { get; set; }
    public Status? Status { get; set; }
}
