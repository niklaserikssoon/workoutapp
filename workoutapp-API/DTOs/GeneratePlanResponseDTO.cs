namespace workoutapp_API.DTOs
{
    public class GeneratePlanResponseDTO
    {
        public int AIWorkoutId { get; set; }
        public string Goal { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<AIPlanDayDTO> Plan { get; set; } = [];
    }

    public class AIPlanDayDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<AIPlanExerciseDTO> Exercises { get; set; } = [];
    }

    public class AIPlanExerciseDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double? Weight { get; set; }
    }

    public class AIPlanJsonDTO
    {
        public List<AIPlanDayDTO> Days { get; set; } = [];
    }

    public class SaveAIPlanDTO
    {
        public string Goal { get; set; } = string.Empty;
        public List<AIPlanDayDTO> Plan { get; set; } = [];
    }
}
