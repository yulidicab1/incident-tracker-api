# Incident Tracker API

A lightweight incident management API built with **ASP.NET Core Minimal API (.NET 8)**.

This project provides a clean RESTful interface to create, retrieve, update, and delete incidents. It was developed as a portfolio project to showcase solid backend fundamentals, including endpoint design, request and response modeling, HTTP status code handling, enum serialization, Swagger documentation, and simple frontend integration.

## Features

- Create incidents with title, description, and severity
- Retrieve all incidents or a specific incident by ID
- Update incident fields such as title, description, severity, or status
- Delete incidents by ID
- Automatically generate creation and update timestamps
- Serialize enum values as strings for cleaner API responses
- Explore and test the API through Swagger UI
- Interact with the API through a lightweight frontend

## Tech Stack

- **C#**
- **ASP.NET Core 8**
- **Minimal API**
- **Swagger / Swashbuckle**
- **HTML, CSS, JavaScript**
- **In-memory data storage** for the current MVP version

## Project Structure

```text
incident-tracker-api/
├── Data/
├── docs/
│   └── images/
├── Dtos/
├── Frontend/
├── Models/
├── Properties/
├── Program.cs
├── incident-tracker-api.csproj
└── README.md
```
## Running the Project Locally

Restore dependencies and run the API:
```bash
dotnet restore
dotnet run
```

Once the application starts, the terminal will show a local URL similar to:
```bash 
http://localhost:5283
```
Then open Swagger UI at:
```bash 
http://localhost:5283/swagger
```
If the local port is different on your machine, use the one shown in the terminal output.

## API Overview
Base route:
```bash 
/api/incidents
```

Available endpoints:
- GET / — root endpoint
- GET /api/incidents — retrieve all incidents
- GET /api/incidents/{id} — retrieve an incident by ID
- POST /api/incidents — create a new incident
- PATCH /api/incidents/{id} — update an existing incident
- DELETE /api/incidents/{id} — delete an incident

## Example Request
Create Incident
```bash
{
  "title": "Login service unavailable",
  "description": "Users cannot authenticate into the platform.",
  "severity": "HIGH"
}
```

## Expected Behavior
When a new incident is created:
- the API returns 201 Created
- the initial status is set to OPEN
- createdAt and updatedAt are generated automatically

## Update Incident
```bash
{
  "status": "IN_PROGRESS"
}
```

## Screenshots
- Swagger UI
- Example POST Request Returning 201 Created

## Current Limitations
This version uses in-memory storage, which means data is reset every time the application restarts.

This is intentional for the current scope of the project, where the focus is on API design, backend structure, and developer-facing usability. Persistent storage is planned for a future iteration.

## Why This Project
This project was built to present practical backend development skills in a compact but polished format. It emphasizes clean API behavior, readable structure, and clear technical communication without adding unnecessary complexity.

## Possible Next Steps
- Add persistent storage with PostgreSQL and Entity Framework Core
- Add unit and integration tests
- Improve validation and standardized error handling
- Containerize the project with Docker
- Extend the frontend to support editing and deleting incidents visually
- Add CI workflows for build and test automation

## Author
Yulicenia Díaz Cabrera
GitHub: [yulidicab1](https://github.com/yulidicab1)
