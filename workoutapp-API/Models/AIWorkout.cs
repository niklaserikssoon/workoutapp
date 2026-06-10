using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.API.Models;

public class AIWorkout
{
    [Key]
    public int AIWorkoutId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    [StringLength(200)]
    public string Goal { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string Plan { get; set; } = string.Empty;
}
