using Microsoft.EntityFrameworkCore;
using WorkoutApp.API.Models;

namespace WorkoutApp.API.Data;

public class WorkoutDbContext : DbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<Workout> Workouts => Set<Workout>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD2FC4ADCE17");

            entity.Property(e => e.ExerciseName).HasMaxLength(150);
            entity.Property(e => e.PrimaryMuscle).HasMaxLength(100);
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("PK__Workouts__E1C42A0158B09245");

            entity.HasMany(w => w.Exercises)
                  .WithMany(e => e.Workouts)
                  .UsingEntity(j => j.ToTable("WorkoutExercises"));
        });

        base.OnModelCreating(modelBuilder);
    }
}