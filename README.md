# Workout Tracker

A full-stack app for logging workouts and recipes — an ASP.NET Core + EF Core + SQLite backend, paired with a vanilla HTML/CSS/JavaScript frontend that talks to it over `fetch()`. Started as a first backend project, then extended into a full-stack project as a bridge toward React.

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
- Loads the current list of workouts and recipes from the real database automatically when the page opens (no hardcoded placeholder data)
- Displays them as styled cards, laid out with flexbox
- Lets a user add a new workout or recipe through a form, without reloading the page
- Sends new entries to the backend API via `fetch()`, persisting them to the real database, and immediately reflects the addition in the UI

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
- CORS — explicitly allowing a different-origin frontend to call the API (`AddCors`, `UseCors`, `AllowAnyMethod`, `AllowAnyHeader`)

**Frontend**
- HTML structure, semantic lists/forms
- CSS: box model, flexbox (including nested flex containers with different `flex-direction` values), CSS variables for a shared color palette
- JavaScript fundamentals: variables (`let`/`const`), functions (including arrow functions), arrays, objects, `.map()`/`.filter()` as JS's equivalent to LINQ's `.Select()`/`.Where()`
- DOM manipulation: `querySelector`, `createElement`, `appendChild`, `innerHTML` (used to clear a list before repopulating it), event listeners
- Preventing default form submission (`event.preventDefault()`) to build a single-page-style form instead of a full page reload
- `async`/`await` with `fetch()` for both `GET` (loading existing data) and `POST` (creating new data) requests to the backend API
- Populating the UI from the database on page load, replacing hardcoded placeholder HTML with real fetched data
- Debugging real cross-origin request issues (CORS) end to end, from browser error message to backend fix

## Bugs hit and fixed along the way

**Backend**
- Missing `using` directives, a stale running process serving old code, unsaved file edits, a route typo, an accessibility mismatch between a `DbSet` and its non-public model class, and a validation bug that checked the existing entity instead of the incoming update data
- `dotnet-ef` global tool not installed on a second machine (Windows) after working fine on the original machine (Mac) — a reminder that global tool installs are per-machine, not part of the project itself; reinstalled with `dotnet tool install --global dotnet-ef`

**Frontend**
- `{ }` vs `[ ]` confusion when first building an array of objects (curly braces create an object in JS, not an array)
- Used `foreach`, a C# keyword that doesn't exist in JavaScript — replaced with `for...of`
- Attempted to redeclare a `const` variable with the same name twice in the same scope — fixed by giving each a distinct name
- Called the wrong constructor function (`createWorkout` instead of `createRecipe`) when building a recipe object
- Placed the `<script>` tag between `<head>` and `<body>`, causing it to run before the DOM elements it queried existed — moved it to just before `</body>`
- Forgot `event.preventDefault()` on form submit buttons, causing the page to silently reload and discard newly added items before they were visible
- Misplaced `async` on a variable declaration instead of on the function passed to `addEventListener`, and used `await` inside a non-`async` function
- Hit a real CORS error connecting the frontend to the backend for the first time — resolved by adding an explicit CORS policy in `Program.cs`, including a follow-up fix after initially forgetting `.AllowAnyMethod()` on the policy
- Attempted to send a `body` with a `GET` request, which isn't valid — `GET` requests can't carry a request body
- **The trickiest one:** after fixing CORS and the request logic, new items would appear in the list for a split second and then vanish, even though `event.preventDefault()` was firing correctly and the network request was succeeding. Root cause: VS Code's Live Server was watching the entire repository, including the `backend` folder — every time a `POST` request caused EF Core to write to `workouts.db`, Live Server detected the file change and auto-reloaded the page, wiping the freshly-added DOM item a moment after it appeared. Fixed by opening only the `frontend` folder as its own workspace when running Live Server, so it no longer watches backend files it has no reason to reload on

## Planned / future work

- React frontend, replacing the vanilla JS version with a component-based approach
- Expand the workout form to collect sets, reps, and weight from real inputs instead of relying on hardcoded test values
- Wire up delete (and possibly update) buttons on the frontend to the existing `DELETE`/`PUT` endpoints, which currently only have been tested via `curl`
- Possible migration from SQLite to PostgreSQL as a deliberate learning exercise
- Resolve two outstanding `NU1903` package vulnerability warnings (transitive dependencies of EF Core's SQLite provider and the OpenAPI tooling)
