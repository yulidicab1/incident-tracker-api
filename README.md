# Incident Tracker API

REST API built with **ASP.NET Core (.NET 8)** to create, track, update and delete incidents (CRUD).  
Portfolio-ready backend project with clean endpoints, proper HTTP status codes, and interactive API docs via Swagger UI.

## What you can do
- Create incidents with **severity**: `LOW`, `MEDIUM`, `HIGH`
- Update lifecycle status: `OPEN → IN_PROGRESS → RESOLVED`
- Retrieve incidents (list + by id)
- Delete incidents
- Explore and test everything through **Swagger UI**

## Tech stack
- C# / ASP.NET Core Minimal API
- Swagger UI (Swashbuckle)
- JSON serialization (enums as strings)

## Run locally
```bash
dotnet restore
dotnet run
```

You’ll see a URL like http://localhost:XXXX in the terminal.
Open Swagger UI at:
http://localhost:XXXX/swagger

## API overview
Base path: /api/incidents
### Create incident (POST)
Example request:
```JSON
{
  "title": "API down in staging",
  "description": "502 from gateway when calling /payments",
  "severity": "HIGH"
}
```

Expected behavior:
- Returns 201 Created
- Default status is OPEN
- createdAt and updatedAt are set automatically

### Update incident (PATCH)
Example request:
```JSON
{
  "status": "IN_PROGRESS"
}
```

## Screenshots
- Swagger endpoints
- POST example (201 Created)

## Notes
Current version uses in-memory storage (data resets when the app restarts).
This is intentional for the MVP and will be upgraded in the roadmap.

## Roadmap
- Persist data with PostgreSQL + Entity Framework Core
- Add automated tests (unit + integration)
- Dockerize API + DB with docker-compose
- Add structured error responses (ProblemDetails)

## Author
Yulicenia Díaz Cabrera
- GitHub: https://github.com/yulidicab1
