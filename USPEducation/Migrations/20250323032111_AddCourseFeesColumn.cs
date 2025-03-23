using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USPEducation.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseFeesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fees",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Courses");
        }
    }
}
