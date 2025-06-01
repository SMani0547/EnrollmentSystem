using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USPSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialConsiderationApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraduationApplications_AspNetUsers_StudentId",
                table: "GraduationApplications");

            migrationBuilder.DropIndex(
                name: "IX_GraduationApplications_StudentId",
                table: "GraduationApplications");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "GraduationApplications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "SpecialConsiderationApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Campus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SemesterYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate1 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate2 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate3 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate4 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate5 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExamDate6 = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamTime6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationType = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportingDocuments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialConsiderationApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialConsiderationApplications_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialConsiderationApplications_StudentId",
                table: "SpecialConsiderationApplications",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialConsiderationApplications");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "GraduationApplications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_GraduationApplications_StudentId",
                table: "GraduationApplications",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GraduationApplications_AspNetUsers_StudentId",
                table: "GraduationApplications",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
