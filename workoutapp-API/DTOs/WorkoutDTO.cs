using System.ComponentModel.DataAnnotations;

namespace workoutapp_API.DTOs
{
    public class WorkoutDTO
    {
        public int WorkoutId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ExerciseDTO> Exercises { get; set; } = [];
        public List<CatalogExerciseDTO> CatalogExercises { get; set; } = [];
    }

    public class ExerciseDTO
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public string PrimaryMuscle { get; set; } = string.Empty;
    }

    public class CatalogExerciseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Level { get; set; }
        public string? Equipment { get; set; }
        public string? Category { get; set; }
        public List<string> PrimaryMuscles { get; set; } = [];
    }

    public class CreateWorkoutDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<int> ExerciseIds { get; set; } = [];
        public List<string> CatalogExerciseIds { get; set; } = [];
    }
}