using WorkoutTracker;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=workouts.db"));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

#region Workout CRUD Operations

//Workout GET Methods
app.MapGet("/workouts", async (AppDbContext db) =>
{
    return await db.Workouts.ToListAsync();
});

app.MapGet("/workouts/{id}", async (int id, AppDbContext db) =>
{
    var workout = await db.Workouts.FindAsync(id);
    return workout is not null ? Results.Ok(workout) : Results.NotFound();
});

//POST METHODS
app.MapPost("/workouts", async (Workout workout, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(workout.ExerciseName) || workout.Sets <= 0 || workout.Reps <= 0)
    {
        return Results.BadRequest("Exercise name is required, and sets/reps must be greater than 0.");
    }

    db.Workouts.Add(workout);
    await db.SaveChangesAsync();
    return Results.Created($"/workouts/{workout.Id}", workout);
});

//PUT methods
app.MapPut("/workouts/{id}", async (int id, Workout updatedWorkout, AppDbContext db) =>
{
    var workout = await db.Workouts.FindAsync(id);
    
    //If the value exists
    if (workout is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(updatedWorkout.ExerciseName) ||updatedWorkout.Sets <= 0 || updatedWorkout.Reps <= 0)
    {
        return Results.BadRequest("Exercise name is required, and sets/reps must be greater than 0.");
    }

    //Update to new workout changes
    db.Entry(workout).CurrentValues.SetValues(updatedWorkout);
    await db.SaveChangesAsync();

    return Results.Ok(workout);
    
});

//DELETE methods

app.MapDelete("/workouts/{id}", async (int id, AppDbContext db) =>

{
    var workout = await db.Workouts.FindAsync(id);

    //Validating it exists
    if (workout is null)
    {
        return Results.NotFound();
    }

    db.Workouts.Remove(workout);
    
    await db.SaveChangesAsync();

    return Results.NoContent();

});

#endregion Workout CRUD Operations

#region Recipes Crud Operations

//Recipes Recieve/GET
app.MapGet("/recipes", async ( AppDbContext db)=>
{
    return await db.Recipes.ToListAsync();
});

app.MapGet("/recipes/{id}", async (int id, AppDbContext db) =>
{
    var recipe = await db.Recipes.FindAsync(id);
    return recipe is not null ? Results.Ok(recipe) : Results.NotFound();
});

//Recipes Create/POST
app.MapPost("/recipes", async (Recipe recipe, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(recipe.Name) || recipe.Ingredients.Count == 0)
    {
        return Results.BadRequest("Please enter a name for the recipe or ingredients required.");
    }
    db.Recipes.Add(recipe);
    await db.SaveChangesAsync();
    return Results.Created($"/recipes/{recipe.Id}", recipe);

});

//Recipes Update/PUT
app.MapPut("/recipes/{id}", async (int id, AppDbContext db, Recipe newRecipe) =>
{
    var recipe = await db.Recipes.FindAsync(id);
    if (recipe is null)
    {
        return Results.NotFound();
    }

    if (string.IsNullOrWhiteSpace(newRecipe.Name) || newRecipe.Ingredients.Count == 0)
    {
        return Results.BadRequest("Please enter a name for the new recipe or ingredients required.");
    }

    db.Entry(recipe).CurrentValues.SetValues(newRecipe);
    await db.SaveChangesAsync();

    return Results.Ok(recipe);
});

//Recipes Delete/DELETE
app.MapDelete("/recipes/{id}", async(int id, AppDbContext db) =>
{
    var recipe = await db.Recipes.FindAsync(id);
    if (recipe is null)
    {
        return Results.NotFound();
    }

    db.Recipes.Remove(recipe);
    await db.SaveChangesAsync();

    return Results.NoContent();

});

#endregion

app.Run();