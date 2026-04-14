using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutApp.API.Models;

public class Workout
{
    [Key]
    public int WorkoutId { get; set; }

    public int UserId { get; set; }

    public int ExerciseId { get; set; }

    [ForeignKey(nameof(ExerciseId))]
    public virtual Exercise Exercise { get; set; } = null!;
}