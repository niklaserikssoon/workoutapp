using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.API.Models;

public class Workout
{
    [Key]
    public int WorkoutId { get; set; }

    [Required]
    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}