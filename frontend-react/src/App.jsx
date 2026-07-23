import { useState, useEffect } from "react"

function App() {
  const [workouts, setWorkouts] = useState([]);
  const [recipes, setRecipes] = useState([]);

  useEffect(() => {
    // fetch workouts and recipes here, then call setWorkouts/setRecipes
    async function getAllWorkouts() 
    {
        const workoutsResponse = await fetch("http://localhost:5000/workouts");
        const data = await workoutsResponse.json();
        setWorkouts(data);
    }

    async function getAllRecipes() 
    {
        const recipesResponse = await fetch("http://localhost:5000/recipes");
        const data = await recipesResponse.json();
        setRecipes(data);
    }

    getAllWorkouts();
    getAllRecipes();

  }, []);

return (
  <div>
    <h1>My Workouts &amp; Recipes</h1>
    <ul>
      {workouts.map(workout => (
        <li key={workout.id}>{workout.exerciseName}</li>
      ))}
    </ul>
    <ul>
      {recipes.map(recipe => (
        <li key={recipe.id}>{recipe.recipeName}</li>
      ))}
    </ul>
  </div>
)
}

export default App