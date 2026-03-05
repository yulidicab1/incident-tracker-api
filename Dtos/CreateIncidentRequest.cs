using System.ComponentModel.DataAnnotations;
using IncidentTrackerApi.Models;

namespace IncidentTrackerApi.Dtos;

public class CreateIncidentRequest
{
    [Required, MinLength(3), MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public Severity Severity { get; set; }
}