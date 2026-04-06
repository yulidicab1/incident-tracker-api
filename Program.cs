using IncidentTrackerApi.Data;
using IncidentTrackerApi.Dtos;
using IncidentTrackerApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// In-memory store
builder.Services.AddSingleton<IncidentStore>();

// JSON options: serialize enums as strings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// CORS: allow frontend requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Seed sample data
var store = app.Services.GetRequiredService<IncidentStore>();

if (!store.Incidents.Any())
{
    store.Incidents.AddRange(new[]
    {
        new Incident
        {
            Id = Guid.NewGuid(),
            Title = "Login service unavailable",
            Description = "Users cannot authenticate into the platform.",
            Severity = Severity.HIGH,
            Status = Status.OPEN,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        },
        new Incident
        {
            Id = Guid.NewGuid(),
            Title = "Dashboard loading slowly",
            Description = "Analytics dashboard response time is above expected.",
            Severity = Severity.MEDIUM,
            Status = Status.IN_PROGRESS,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        }
    });
}

// Root endpoint
app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        message = "Incident Tracker API is running",
        swagger = "/swagger"
    });
});

// Get all incidents
app.MapGet("/api/incidents", (IncidentStore incidentStore) =>
{
    var response = incidentStore.Incidents.Select(i => new IncidentResponse
    {
        Id = i.Id,
        Title = i.Title,
        Description = i.Description,
        Severity = i.Severity,
        Status = i.Status,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt
    });

    return Results.Ok(response);
})
.WithName("GetAllIncidents")
.Produces<IEnumerable<IncidentResponse>>(StatusCodes.Status200OK);

// Get incident by id
app.MapGet("/api/incidents/{id:guid}", (Guid id, IncidentStore incidentStore) =>
{
    var incident = incidentStore.Incidents.FirstOrDefault(i => i.Id == id);

    if (incident is null)
    {
        return Results.NotFound(new { message = "Incident not found" });
    }

    var response = new IncidentResponse
    {
        Id = incident.Id,
        Title = incident.Title,
        Description = incident.Description,
        Severity = incident.Severity,
        Status = incident.Status,
        CreatedAt = incident.CreatedAt,
        UpdatedAt = incident.UpdatedAt
    };

    return Results.Ok(response);
})
.WithName("GetIncidentById")
.Produces<IncidentResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// Create incident
app.MapPost("/api/incidents", (CreateIncidentRequest req, IncidentStore incidentStore) =>
{
    if (string.IsNullOrWhiteSpace(req.Title))
    {
        return Results.BadRequest(new { message = "Title is required" });
    }

    var now = DateTimeOffset.UtcNow;

    var incident = new Incident
    {
        Id = Guid.NewGuid(),
        Title = req.Title.Trim(),
        Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
        Severity = req.Severity,
        Status = Status.OPEN,
        CreatedAt = now,
        UpdatedAt = now
    };

    incidentStore.Incidents.Add(incident);

    var response = new IncidentResponse
    {
        Id = incident.Id,
        Title = incident.Title,
        Description = incident.Description,
        Severity = incident.Severity,
        Status = incident.Status,
        CreatedAt = incident.CreatedAt,
        UpdatedAt = incident.UpdatedAt
    };

    return Results.Created($"/api/incidents/{incident.Id}", response);
})
.WithName("CreateIncident")
.Accepts<CreateIncidentRequest>("application/json")
.Produces<IncidentResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);

// Update incident
app.MapPatch("/api/incidents/{id:guid}", (Guid id, UpdateIncidentRequest req, IncidentStore incidentStore) =>
{
    var incident = incidentStore.Incidents.FirstOrDefault(i => i.Id == id);

    if (incident is null)
    {
        return Results.NotFound(new { message = "Incident not found" });
    }

    if (req.Title is not null)
    {
        if (string.IsNullOrWhiteSpace(req.Title))
        {
            return Results.BadRequest(new { message = "Title cannot be empty" });
        }

        incident.Title = req.Title.Trim();
    }

    if (req.Description is not null)
    {
        incident.Description = string.IsNullOrWhiteSpace(req.Description)
            ? null
            : req.Description.Trim();
    }

    if (req.Severity.HasValue)
    {
        incident.Severity = req.Severity.Value;
    }

    if (req.Status.HasValue)
    {
        incident.Status = req.Status.Value;
    }

    incident.UpdatedAt = DateTimeOffset.UtcNow;

    var response = new IncidentResponse
    {
        Id = incident.Id,
        Title = incident.Title,
        Description = incident.Description,
        Severity = incident.Severity,
        Status = incident.Status,
        CreatedAt = incident.CreatedAt,
        UpdatedAt = incident.UpdatedAt
    };

    return Results.Ok(response);
})
.WithName("UpdateIncident")
.Accepts<UpdateIncidentRequest>("application/json")
.Produces<IncidentResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest);

// Delete incident
app.MapDelete("/api/incidents/{id:guid}", (Guid id, IncidentStore incidentStore) =>
{
    var removed = incidentStore.Incidents.RemoveAll(i => i.Id == id);

    return removed == 0
        ? Results.NotFound(new { message = "Incident not found" })
        : Results.NoContent();
})
.WithName("DeleteIncident")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();