using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.API.Models;

public class Workout
{
    [Key]
    public int WorkoutId { get; set; }

    [Required]
    public int UserId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    public virtual ICollection<ExerciseCatalog> CatalogExercises { get; set; } = new List<ExerciseCatalog>();
}