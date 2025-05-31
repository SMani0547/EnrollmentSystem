using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USPSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddGraduationApplicationCommentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraduationApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Programme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MajorI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MajorII = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Minor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GraduationCeremony = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WillAttend = table.Column<bool>(type: "bit", nullable: false),
                    DeclarationConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraduationApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraduationApplications_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraduationApplications_StudentId",
                table: "GraduationApplications",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraduationApplications");
        }
    }
}
