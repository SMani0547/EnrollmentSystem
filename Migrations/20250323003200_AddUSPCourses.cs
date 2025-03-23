using Microsoft.EntityFrameworkCore.Migrations;

namespace USPEducation.Migrations;

public partial class AddUSPCourses : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Execute the updated SQL script
        var sql = File.ReadAllText("Data/Scripts/SeedCourses.sql");
        migrationBuilder.Sql(sql);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Remove the seeded data
        migrationBuilder.Sql(@"
            DELETE FROM Courses WHERE Code IN ('CS101', 'CS102', 'MATH101', 'ACC101', 'FIN101', 'MGT101', 'ECO101', 'BIO101', 'CHE101', 'PHY101');
            DELETE FROM SubjectAreas WHERE Code IN ('CS', 'MATH', 'ACC', 'FIN', 'MGT', 'ECO', 'BIO', 'CHE', 'PHY');
        ");
    }
} 