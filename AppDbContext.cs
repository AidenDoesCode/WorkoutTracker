using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Workout> Workouts { get; set; } //workout table
    }
}