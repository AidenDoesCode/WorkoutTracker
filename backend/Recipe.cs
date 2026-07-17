namespace WorkoutTracker
{
    public class Recipe
    {
        public int Id { get; set; }
        public required string Name {get; set;}
        public required List<string> Ingredients { get; set; }
        public int? Calories { get; set; }
        public List<string>? Macros { get; set; }
        public string? CookMethod { get; set; }
        public int? CookTimeMinutes { get; set; }
        public int? DegreesFahrenheit { get; set; }
    }
}