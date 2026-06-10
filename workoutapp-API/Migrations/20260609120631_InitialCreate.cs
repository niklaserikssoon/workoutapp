using System;
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
                name: "Exercise",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    force = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    mechanic = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    equipment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    primary_muscles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondary_muscles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    images = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.id);
                });

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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Workouts__E1C42A0158B09245", x => x.WorkoutId);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutCatalogExercises",
                columns: table => new
                {
                    CatalogExercisesId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    WorkoutsWorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCatalogExercises", x => new { x.CatalogExercisesId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_WorkoutCatalogExercises_Exercise_CatalogExercisesId",
                        column: x => x.CatalogExercisesId,
                        principalTable: "Exercise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutCatalogExercises_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExercises",
                columns: table => new
                {
                    ExercisesExerciseId = table.Column<int>(type: "int", nullable: false),
                    WorkoutsWorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExercises", x => new { x.ExercisesExerciseId, x.WorkoutsWorkoutId });
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Exercises_ExercisesExerciseId",
                        column: x => x.ExercisesExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExercises_Workouts_WorkoutsWorkoutId",
                        column: x => x.WorkoutsWorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCatalogExercises_WorkoutsWorkoutId",
                table: "WorkoutCatalogExercises",
                column: "WorkoutsWorkoutId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_WorkoutsWorkoutId",
                table: "WorkoutExercises",
                column: "WorkoutsWorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutCatalogExercises");

            migrationBuilder.DropTable(
                name: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Workouts");
        }
    }
}
