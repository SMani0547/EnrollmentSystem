using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace USPGradeSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddRecheckApplicationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 9);

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

            migrationBuilder.CreateTable(
                name: "RecheckApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentReceiptNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecheckApplications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecheckApplications");

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "CourseId", "GradeLetter", "Marks", "StudentId" },
                values: new object[,]
                {
                    { 1, "CS101", "A", 85f, "S12345678" },
                    { 2, "CS102", "A+", 92f, "S12345678" },
                    { 3, "MATH101", "B+", 78f, "S12345678" },
                    { 4, "CS101", "C", 65f, "S12345679" },
                    { 5, "CS102", "A", 88f, "S12345679" },
                    { 6, "MATH101", "B", 75f, "S12345679" },
                    { 7, "ACC101", "A+", 90f, "S12345680" },
                    { 8, "FIN101", "A", 85f, "S12345680" },
                    { 9, "MGT101", "A", 82f, "S12345680" },
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
    }
}
