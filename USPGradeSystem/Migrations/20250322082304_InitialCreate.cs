using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace USPGradeSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marks = table.Column<float>(type: "real", nullable: false),
                    GradeLetter = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseId", "GradeLetter", "Marks", "StudentId" },
                values: new object[,]
                {
                    { 1, "CS101", "A", 85f, "1" },
                    { 2, "CS102", "A+", 92f, "1" },
                    { 3, "MATH101", "B+", 78f, "1" },
                    { 4, "CS101", "C", 65f, "2" },
                    { 5, "CS102", "F", 45f, "2" },
                    { 6, "MATH101", "A", 88f, "2" },
                    { 7, "CS101", "A+", 95f, "3" },
                    { 8, "CS102", "A", 82f, "3" },
                    { 9, "MATH101", "B", 75f, "3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}
