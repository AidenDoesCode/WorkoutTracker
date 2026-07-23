# Workout Tracker

A full-stack app for logging workouts and recipes — an ASP.NET Core + EF Core + SQLite backend, with two frontends: an original vanilla HTML/CSS/JavaScript version, and a newer React version built as the next step in learning frontend development.

## Project structure

```
WorkoutTracker/
  backend/
    Program.cs
    Workout.cs
    Recipe.cs
    AppDbContext.cs
    workoutApp.csproj
  frontend/                (vanilla HTML/CSS/JS version)
    index.html
    style.css
    script.js
  frontend-react/           (React version, in progress)
    src/
      App.jsx
      main.jsx
    package.json
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

**Vanilla JS frontend:**
- Loads workouts and recipes from the database on page load
- Displays them as styled cards, laid out with flexbox
- Lets a user add a new workout or recipe through a form, sent to the backend via `fetch()`, without reloading the page

**React frontend (in progress):**
- Fetches and displays workouts and recipes from the backend on component mount, using `useEffect` and `useState`
- Renders each list with `.map()` over the fetched data
- Add-workout/add-recipe forms not yet built

## Tech stack

- **Backend:** ASP.NET Core (minimal API style), Entity Framework Core, SQLite
- **Vanilla frontend:** plain HTML, CSS (flexbox, CSS custom properties), vanilla JavaScript (DOM manipulation, `fetch()`)
- **React frontend:** React + Vite, function components, hooks (`useState`, `useEffect`)
- CORS configured on the backend to allow requests from both frontend origins during local development

## Concepts practiced

**Backend**
- Minimal API routing, dependency injection, EF Core (`DbContext`, `DbSet<T>`, migrations)
- REST conventions: status codes, input validation
- CORS configuration for cross-origin frontend requests

**Vanilla JS frontend**
- DOM manipulation, event listeners, preventing default form submission
- `async`/`await` with `fetch()` for GET and POST requests

**React frontend**
- Function components and JSX
- `useState` for managing state, including the array-destructuring pattern (`const [value, setValue] = useState(...)`)
- `useEffect` for running code once when a component mounts, with a separate inner `async` function since the effect callback itself can't be `async`
- Rendering lists with `.map()`, including the `key` prop React requires for list items
- The distinction between plain text and JavaScript expressions in JSX (`{ }` required to evaluate a value rather than print it literally)

## Planned / future work

- Build add-workout and add-recipe forms in React using controlled inputs
- Wire up POST requests from the React form to update state and the backend together
- Add update/delete functionality to the React frontend
- Decide whether to keep both frontends long-term or retire the vanilla JS version once React is feature-complete
- Possible migration from SQLite to PostgreSQL
- Resolve two outstanding `NU1903` package vulnerability warnings