using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace workoutapp_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalExerciseId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExerciseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PrimaryMuscle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exercise__A074AD2FC4ADCE17", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    WorkoutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Workouts__E1C42A0158B09245", x => x.WorkoutId);
                    table.ForeignKey(
                        name: "FK_Workouts_Exercises",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_ExerciseId",
                table: "Workouts",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Exercises");
        }
    }
}
