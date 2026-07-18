# Workout Tracker

A full-stack app for logging workouts and recipes — an ASP.NET Core + EF Core + SQLite backend, paired with a vanilla HTML/CSS/JavaScript frontend that talks to it over `fetch()`. Started as a first backend project, then extended into a first full-stack project as a bridge toward React.

## Project structure

```
WorkoutTracker/
  backend/
    Program.cs
    Workout.cs
    Recipe.cs
    AppDbContext.cs
    workoutApp.csproj
  frontend/
    index.html
    style.css
    script.js
```

## What it does

**Backend — full CRUD for two resources: workouts and recipes**

| Method | Endpoint | Description |
|--------|----------|--------------|
| GET | `/workouts` | Get all workouts |
| GET | `/workouts/{id}` | Get a single workout by ID |
| POST | `/workouts` | Create a new workout |
| PUT | `/workouts/{id}` | Update an existing workout |
| DELETE | `/workouts/{id}` | Delete a workout |
| GET | `/recipes` | Get all recipes |
| GET | `/recipes/{id}` | Get a single recipe by ID |
| POST | `/recipes` | Create a new recipe |
| PUT | `/recipes/{id}` | Update an existing recipe |
| DELETE | `/recipes/{id}` | Delete a recipe |

**Frontend — a plain HTML/CSS/JS page that:**
- Displays workouts and recipes as styled cards, laid out with flexbox
- Lets a user add a new workout or recipe through a form, without reloading the page
- Sends new entries to the backend API via `fetch()`, persisting them to the real database

## Tech stack

- **Backend:** ASP.NET Core (minimal API style), Entity Framework Core, SQLite
- **Frontend:** plain HTML, CSS (flexbox layout, CSS custom properties for a shared color palette), vanilla JavaScript (DOM manipulation, `fetch()`)
- Tested manually via `curl` (backend) and the browser console / Live Server (frontend)

## Concepts practiced

**Backend**
- Minimal API routing (`MapGet`, `MapPost`, `MapPut`, `MapDelete`)
- Dependency injection — `AppDbContext` supplied automatically to each endpoint
- EF Core: `DbContext`, multiple `DbSet<T>` properties, migrations
- REST conventions: proper status codes, input validation on create and update
- **CORS** — explicitly allowing a different-origin frontend to call the API (`AddCors`, `UseCors`, `AllowAnyMethod`, `AllowAnyHeader`)

**Frontend**
- HTML structure, semantic lists/forms
- CSS: box model, flexbox (including nested flex containers with different `flex-direction` values), CSS variables for a shared color palette
- JavaScript fundamentals: variables (`let`/`const`), functions (including arrow functions), arrays, objects, `.map()`/`.filter()` as JS's equivalent to LINQ's `.Select()`/`.Where()`
- DOM manipulation: `querySelector`, `createElement`, `appendChild`, event listeners
- Preventing default form submission (`event.preventDefault()`) to build a single-page-style form instead of a full page reload
- `async`/`await` with `fetch()` to send data to and receive data from the backend API
- Debugging real cross-origin request issues (CORS) end to end, from browser error message to backend fix

## Bugs hit and fixed along the way

**Backend**
- Missing `using` directives, a stale running process serving old code, unsaved file edits, a route typo, an accessibility mismatch between a `DbSet` and its non-public model class, and a validation bug that checked the existing entity instead of the incoming update data (see prior project history for details)

**Frontend**
- `{ }` vs `[ ]` confusion when first building an array of objects (curly braces create an object in JS, not an array)
- Used `foreach`, a C# keyword that doesn't exist in JavaScript — replaced with `for...of`
- Attempted to redeclare a `const` variable with the same name twice in the same scope (two `<ul>` references both named `list`) — fixed by giving each a distinct name
- Called the wrong constructor function (`createWorkout` instead of `createRecipe`) when building a recipe object
- Placed the `<script>` tag between `<head>` and `<body>`, causing it to run before the DOM elements it queried existed — moved it to just before `</body>`
- Forgot `event.preventDefault()` on form submit buttons, causing the page to silently reload and discard newly added items before they were visible
- Misplaced `async` on a variable declaration instead of on the function passed to `addEventListener`, and used `await` inside a non-`async` function
- Hit a real CORS error connecting the frontend to the backend for the first time — resolved by adding an explicit CORS policy in `Program.cs` (`AddCors`/`UseCors`), including a follow-up fix after initially forgetting `.AllowAnyMethod()` on the policy

## Planned / future work

- React frontend, replacing the vanilla JS version with a component-based approach
- Populate the list on page load via a `GET` fetch instead of starting from hardcoded HTML items
- Expand the workout form to collect sets, reps, and weight instead of relying on hardcoded test values
- Possible migration from SQLite to PostgreSQL as a deliberate learning exercise
- Resolve two outstanding `NU1903` package vulnerability warnings (transitive dependencies of EF Core's SQLite provider and the OpenAPI tooling)