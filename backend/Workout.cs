namespace WorkoutTracker
{
    public class Workout
    {
        public int Id { get; set; }
        public required string ExerciseName { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; }
    }
}