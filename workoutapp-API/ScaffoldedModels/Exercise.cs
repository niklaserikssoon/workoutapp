using System;
using System.Collections.Generic;

namespace workoutapp_API.ScaffoldedModels;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public string ExerciseName { get; set; } = null!;

    public string PrimaryMuscle { get; set; } = null!;

    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
