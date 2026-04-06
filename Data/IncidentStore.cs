using IncidentTrackerApi.Models;

namespace IncidentTrackerApi.Data;

public class IncidentStore
{
    public List<Incident> Incidents { get; } = new();
}