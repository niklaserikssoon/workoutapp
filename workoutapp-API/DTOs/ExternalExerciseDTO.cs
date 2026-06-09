namespace workoutapp_API.DTOs
{
    public class ExternalExerciseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Force { get; set; }
        public string? Level { get; set; }
        public string? Mechanic { get; set; }
        public string? Equipment { get; set; }
        public string? Category { get; set; }
        public List<string> PrimaryMuscles { get; set; } = new();
        public List<string>? SecondaryMuscles { get; set; }
        public List<string>? Instructions { get; set; }
        public List<string>? Images { get; set; }
    }
}
