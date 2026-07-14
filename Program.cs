using WorkoutTracker;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=workouts.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//GET Methods
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

app.Run();