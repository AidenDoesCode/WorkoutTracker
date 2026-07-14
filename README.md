# Workout Tracker API

A backend API for logging workouts — built with ASP.NET Core and EF Core, backed by SQLite. Built as a first full-stack-style project, moving beyond console apps into a real HTTP API with database persistence.

## What it does

Full CRUD for workout entries:

| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/workouts` | Get all workouts |
| GET | `/workouts/{id}` | Get a single workout by ID |
| POST | `/workouts` | Create a new workout |
| PUT | `/workouts/{id}` | Update an existing workout |
| DELETE | `/workouts/{id}` | Delete a workout |

Each workout tracks an exercise name, sets, reps, weight, and date.

## Tech stack

- **ASP.NET Core** (minimal API style)
- **Entity Framework Core** as the ORM
- **SQLite** for the database
- Tested manually via `curl`

## Concepts practiced

- Minimal API routing (`MapGet`, `MapPost`, `MapPut`, `MapDelete`)
- Dependency injection — `AppDbContext` is automatically supplied to each endpoint by ASP.NET, registered once in `Program.cs`
- EF Core fundamentals: `DbContext`, `DbSet<T>`, migrations (`dotnet ef migrations add` / `dotnet ef database update`)
- `async`/`await` for database calls
- REST conventions: proper status codes (`200`, `201`, `404`, `400`) instead of always returning success
- Basic input validation on create (rejecting empty names or non-positive sets/reps)
- Using `db.Entry(entity).CurrentValues.SetValues(...)` to update all fields on an entity in one call instead of assigning each property manually

## Bugs hit and fixed along the way

- **Missing `using` directives:** `Program.cs` initially couldn't find `AppDbContext` or `UseSqlite` — both needed explicit `using` statements (`using WorkoutTracker;` and `using Microsoft.EntityFrameworkCore;`) since C# doesn't auto-import namespaces
- **Stale running process:** after adding new endpoints, `dotnet run` kept serving an old build because a previous instance was still bound to the port, causing `POST` requests to return `405 Method Not Allowed`. Fixed by finding and killing the process (`lsof -i :5129`, `kill -9 <PID>`) before restarting
- **Unsaved file edits:** the most confusing one — new endpoints appeared to "not exist" even after rebuilding, traced back to the code never actually being saved to disk in the first place. Confirmed with `cat Program.cs` to compare what was actually on disk versus what was visible in the editor
- **Typo in a route pattern:** `MapPut("/workouts{id}", ...)` was missing the `/` before `{id}`, so the route didn't match the expected URL shape

## Planned / future work

- **React frontend** — a simple UI to list, add, edit, and delete workouts by calling this API, to make the full-stack story concrete
- **Recipes feature** — extending the app beyond workouts to also track recipes, likely as a second model/table (`Recipe`) with its own CRUD endpoints, reusing the same patterns established here
- Possible migration from SQLite to PostgreSQL as a deliberate learning exercise once the above features are in place
