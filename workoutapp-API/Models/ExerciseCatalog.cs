using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkoutApp.API.Models;

[Table("Exercise")]
public class ExerciseCatalog
{
    [Key]
    [Column("id")]
    [MaxLength(100)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [Column("name")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [Column("force")]
    [MaxLength(50)]
    public string? Force { get; set; }

    [Column("level")]
    [MaxLength(50)]
    public string? Level { get; set; }

    [Column("mechanic")]
    [MaxLength(50)]
    public string? Mechanic { get; set; }

    [Column("equipment")]
    [MaxLength(100)]
    public string? Equipment { get; set; }

    [Column("category")]
    [MaxLength(100)]
    public string? Category { get; set; }

    [Required]
    [Column("primary_muscles")]
    public List<string> PrimaryMuscles { get; set; } = new();

    [Column("secondary_muscles")]
    public List<string>? SecondaryMuscles { get; set; }

    [Column("instructions")]
    public List<string>? Instructions { get; set; }

    [Column("images")]
    public List<string>? Images { get; set; }

    public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
