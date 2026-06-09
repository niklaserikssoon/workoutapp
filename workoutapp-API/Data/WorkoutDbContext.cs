using System.Text.Json;
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
    public DbSet<ExerciseCatalog> ExerciseCatalog => Set<ExerciseCatalog>();

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

            entity.HasMany(w => w.CatalogExercises)
                  .WithMany(e => e.Workouts)
                  .UsingEntity(j => j.ToTable("WorkoutCatalogExercises"));
        });

        modelBuilder.Entity<ExerciseCatalog>(entity =>
        {
            entity.ToTable("Exercise");

            entity.HasKey(e => e.Id);

            var jsonOptions = new JsonSerializerOptions();

            entity.Property(e => e.PrimaryMuscles)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, jsonOptions),
                    v => JsonSerializer.Deserialize<List<string>>(v, jsonOptions) ?? new List<string>()
                )
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.SecondaryMuscles)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)
                )
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Instructions)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)
                )
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Images)
                .HasConversion(
                    v => v == null ? null : JsonSerializer.Serialize(v, jsonOptions),
                    v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)
                )
                .HasColumnType("nvarchar(max)");
        });

        base.OnModelCreating(modelBuilder);
    }
}