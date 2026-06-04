using System.ComponentModel.DataAnnotations;

namespace workoutapp_API.DTOs
{
    public class WorkoutDTO
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ExerciseDTO> Exercises { get; set; } = [];
    }

    public class ExerciseDTO
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public string PrimaryMuscle { get; set; } = string.Empty;
    }

    public class CreateWorkoutDTO
    {
        [Required]
        public List<int> ExerciseIds { get; set; } = [];
    }
}