# Workout Tracker API

A backend API for logging workouts and recipes — built with ASP.NET Core and EF Core, backed by SQLite. Started as a first full-stack-style project, moving beyond console apps into a real HTTP API with database persistence.

## What it does

Full CRUD for two resources: workouts and recipes.

**Workouts**

| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/workouts` | Get all workouts |
| GET | `/workouts/{id}` | Get a single workout by ID |
| POST | `/workouts` | Create a new workout |
| PUT | `/workouts/{id}` | Update an existing workout |
| DELETE | `/workouts/{id}` | Delete a workout |

Each workout tracks an exercise name, sets, reps, weight, and date.

**Recipes**

| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/recipes` | Get all recipes |
| GET | `/recipes/{id}` | Get a single recipe by ID |
| POST | `/recipes` | Create a new recipe |
| PUT | `/recipes/{id}` | Update an existing recipe |
| DELETE | `/recipes/{id}` | Delete a recipe |

Each recipe tracks a name, ingredients, calories, macros, cook method, cook time, and oven temperature.

## Tech stack

- **ASP.NET Core** (minimal API style)
- **Entity Framework Core** as the ORM
- **SQLite** for the database
- Tested manually via `curl`

## Concepts practiced

- Minimal API routing (`MapGet`, `MapPost`, `MapPut`, `MapDelete`)
- Dependency injection — `AppDbContext` is automatically supplied to each endpoint by ASP.NET, registered once in `Program.cs`
- EF Core fundamentals: `DbContext`, multiple `DbSet<T>` properties in one context, migrations (`dotnet ef migrations add` / `dotnet ef database update`)
- `async`/`await` for database calls
- REST conventions: proper status codes (`200`, `201`, `204`, `404`, `400`) instead of always returning success
- Input validation on both create and update for both resources (required fields, positive numeric values, non-empty lists)
- Using `db.Entry(entity).CurrentValues.SetValues(...)` to update all fields on an entity in one call instead of assigning each property manually
- Organizing a growing `Program.cs` with `#region` blocks to keep Workouts and Recipes endpoints visually separated
- Extending an existing EF Core setup with a second model/table, reusing the same patterns rather than relearning them from scratch

## Bugs hit and fixed along the way

- **Missing `using` directives:** `Program.cs` initially couldn't find `AppDbContext` or `UseSqlite` — both needed explicit `using` statements (`using WorkoutTracker;` and `using Microsoft.EntityFrameworkCore;`) since C# doesn't auto-import namespaces
- **Stale running process:** after adding new endpoints, `dotnet run` kept serving an old build because a previous instance was still bound to the port, causing requests to return `405 Method Not Allowed` for methods that should have existed. Fixed by finding and killing the process (`lsof -i :5129`, `kill -9 <PID>`) before restarting
- **Unsaved file edits:** the most confusing one — new endpoints appeared to "not exist" even after rebuilding, traced back to the code never actually being saved to disk in the first place. Confirmed with `cat Program.cs` to compare what was actually on disk versus what was visible in the editor
- **Typo in a route pattern:** `MapPut("/workouts{id}", ...)` was missing the `/` before `{id}`, so the route didn't match the expected URL shape
- **Inconsistent accessibility error:** a `public DbSet<Recipe>` property failed to compile because the `Recipe` class itself wasn't marked `public`, making the property's type less accessible than the property — a reminder that this rule applies to any type, not just the class you happen to be looking at
- **Validating the wrong object on update:** the `PUT /recipes/{id}` validation initially checked the *existing* database entity instead of the *incoming* update data, so bad input would always pass validation since the existing record was already known-good. Fixed by validating the incoming parameter instead of the one just pulled from the database

## Planned / future work

- **React frontend** — a simple UI to list, add, edit, and delete both workouts and recipes by calling this API, to make the full-stack story concrete
- Possible migration from SQLite to PostgreSQL as a deliberate learning exercise once the frontend is in place
- Resolve two outstanding `NU1903` package vulnerability warnings (`Microsoft.OpenApi`, `SQLitePCLRaw.lib.e_sqlite3`) by checking for updated package versions