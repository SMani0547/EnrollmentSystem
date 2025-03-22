using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace USPGradeSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGradesWithActualStudentIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1,
                column: "StudentId",
                value: "S12345678");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2,
                column: "StudentId",
                value: "S12345678");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3,
                column: "StudentId",
                value: "S12345678");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 4,
                column: "StudentId",
                value: "S12345679");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "A", 88f, "S12345679" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "B", 75f, "S12345679" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CourseId", "Marks", "StudentId" },
                values: new object[] { "ACC101", 90f, "S12345680" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CourseId", "Marks", "StudentId" },
                values: new object[] { "FIN101", 85f, "S12345680" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CourseId", "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "MGT101", "A", 82f, "S12345680" });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseId", "GradeLetter", "Marks", "StudentId" },
                values: new object[,]
                {
                    { 10, "MGT101", "B+", 78f, "S12345681" },
                    { 11, "ACC101", "B", 72f, "S12345681" },
                    { 12, "ECO101", "C+", 68f, "S12345681" },
                    { 13, "BIO101", "A+", 95f, "S12345682" },
                    { 14, "CHE101", "A", 88f, "S12345682" },
                    { 15, "MATH101", "A", 82f, "S12345682" },
                    { 16, "CHE101", "A+", 92f, "S12345683" },
                    { 17, "PHY101", "A", 85f, "S12345683" },
                    { 18, "MATH101", "B+", 78f, "S12345683" },
                    { 19, "ECO101", "A", 88f, "S12345684" },
                    { 20, "MGT101", "A", 85f, "S12345684" },
                    { 21, "MATH101", "B", 75f, "S12345684" },
                    { 22, "ECO101", "A+", 92f, "S12345685" },
                    { 23, "FIN101", "A", 85f, "S12345685" },
                    { 24, "MATH101", "B+", 78f, "S12345685" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1,
                column: "StudentId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2,
                column: "StudentId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3,
                column: "StudentId",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 4,
                column: "StudentId",
                value: "2");

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "F", 45f, "2" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "A", 88f, "2" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CourseId", "Marks", "StudentId" },
                values: new object[] { "CS101", 95f, "3" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CourseId", "Marks", "StudentId" },
                values: new object[] { "CS102", 82f, "3" });

            migrationBuilder.UpdateData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CourseId", "GradeLetter", "Marks", "StudentId" },
                values: new object[] { "MATH101", "B", 75f, "3" });
        }
    }
}
