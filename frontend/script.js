//creates a workout object
function createWorkout(exerciseName, sets, reps, weight) {
    return { exerciseName, sets, reps, weight };
}

//creates a recipe object, add other properties later
function createRecipe(recipeName) {
    return { recipeName };
}

async function addWorkoutToServer(workout) {
    const workoutResponse = await fetch("http://localhost:5000/workouts", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(workout)
    });
    const data = await workoutResponse.json();
    console.log(data);
}

async function addRecipeToServer(recipe)
{
    const recipeResponse = await fetch("http://localhost:5000/recipes", 
    {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(recipe)
    });
    const data = await recipeResponse.json();
}


//workouts and recipes array
let workouts = [];
let recipes = [];

//grab the two lists on the page
const workoutList = document.querySelector("ul.workout-card");
const recipeList = document.querySelector("ul.recipe-card");

//#region buttons
const workoutButton = document.querySelector("#add-workout-btn");
workoutButton.addEventListener("click", async (event) => {
    event.preventDefault();

    console.log("workout button was clicked!");
    const exerciseName = document.querySelector("#workout-name").value;

    const newWorkout = createWorkout(exerciseName, 3, 10, 150);
    workouts.push(newWorkout);

    await addWorkoutToServer(newWorkout);

    const newItem = document.createElement("li");
    newItem.textContent = newWorkout.exerciseName;
    workoutList.appendChild(newItem);
});

const recipeButton = document.querySelector("#add-recipe-btn");
recipeButton.addEventListener("click", async (event) => {
    event.preventDefault();

    console.log("recipe button was clicked!");
    const recipeName = document.querySelector("#recipe-name").value;


    const newRecipe = createRecipe(recipeName);
    recipes.push(newRecipe);

    await addRecipeToServer(newRecipe);

    const newItem = document.createElement("li");
    newItem.textContent = newRecipe.recipeName;
    recipeList.appendChild(newItem);
});
//#endregion
