using IncidentTrackerApi.Dtos;
using IncidentTrackerApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//json options para que los enums se serialicen como strings en lugar de números
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


var incidents = new List<Incident>();

app.MapGet("/api/incidents", () =>
{
    return incidents.Select(i => new IncidentResponse
    {
        Id = i.Id,
        Title = i.Title,
        Description = i.Description,
        Severity = i.Severity,
        Status = i.Status,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt
    });
});

app.MapGet("/api/incidents/{id:guid}", (Guid id) =>
{
    var incident = incidents.FirstOrDefault(i => i.Id == id);
    return incident is null
        ? Results.NotFound(new { message = "Incident not found" })
        : Results.Ok(new IncidentResponse
        {
            Id = incident.Id,
            Title = incident.Title,
            Description = incident.Description,
            Severity = incident.Severity,
            Status = incident.Status,
            CreatedAt = incident.CreatedAt,
            UpdatedAt = incident.UpdatedAt
        });
});


app.MapPost("/api/incidents", (CreateIncidentRequest req) =>
{
    var now = DateTimeOffset.UtcNow;

    var incident = new Incident
    {
        Id = Guid.NewGuid(),
        Title = req.Title.Trim(),
        Description = req.Description?.Trim(),
        Severity = req.Severity,
        Status = Status.OPEN,
        CreatedAt = now,
        UpdatedAt = now
    };

    incidents.Add(incident);

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
.Accepts<CreateIncidentRequest>("application/json")
.Produces<IncidentResponse>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest);



app.MapPatch("/api/incidents/{id:guid}", (Guid id, UpdateIncidentRequest req) =>
{
    var incident = incidents.FirstOrDefault(i => i.Id == id);
    if (incident is null) return Results.NotFound(new { message = "Incident not found" });

    if (req.Title is not null) incident.Title = req.Title.Trim();
    if (req.Description is not null) incident.Description = req.Description.Trim();
    if (req.Severity.HasValue) incident.Severity = req.Severity.Value;
    if (req.Status.HasValue) incident.Status = req.Status.Value;

    incident.UpdatedAt = DateTimeOffset.UtcNow;

    return Results.Ok(new IncidentResponse
    {
        Id = incident.Id,
        Title = incident.Title,
        Description = incident.Description,
        Severity = incident.Severity,
        Status = incident.Status,
        CreatedAt = incident.CreatedAt,
        UpdatedAt = incident.UpdatedAt
    });
})
.Accepts<UpdateIncidentRequest>("application/json")
.Produces<IncidentResponse>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status400BadRequest);

app.MapDelete("/api/incidents/{id:guid}", (Guid id) =>
{
    var removed = incidents.RemoveAll(i => i.Id == id);
    return removed == 0 ? Results.NotFound(new { message = "Incident not found" }) : Results.NoContent();
});

// Por ahora lo quitamos para no pelear con puertos https en Mac
// app.UseHttpsRedirection();


app.Run();