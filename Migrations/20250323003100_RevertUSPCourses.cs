using Microsoft.EntityFrameworkCore.Migrations;

namespace USPEducation.Migrations;

public partial class RevertUSPCourses : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // First remove the courses
        migrationBuilder.Sql(@"
            DELETE FROM Courses WHERE Code IN ('CS101', 'CS102', 'MATH101', 'ACC101', 'FIN101', 'MGT101', 'ECO101', 'BIO101', 'CHE101', 'PHY101');
            DELETE FROM SubjectAreas WHERE Code IN ('CS', 'MATH', 'ACC', 'FIN', 'MGT', 'ECO', 'BIO', 'CHE', 'PHY');
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Re-apply the original script
        var sql = File.ReadAllText("Data/Scripts/SeedCourses.sql");
        migrationBuilder.Sql(sql);
    }
} 