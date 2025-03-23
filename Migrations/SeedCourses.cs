using Microsoft.EntityFrameworkCore.Migrations;
using USPEducation.Models;

namespace USPEducation.Migrations;

public partial class SeedCourses : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // First, ensure we have the necessary subject areas
        migrationBuilder.InsertData(
            table: "SubjectAreas",
            columns: new[] { "Id", "Name", "Description" },
            values: new object[,]
            {
                { 1, "Computer Science", "Computer Science courses" },
                { 2, "Mathematics", "Mathematics courses" },
                { 3, "Accounting", "Accounting courses" },
                { 4, "Finance", "Finance courses" },
                { 5, "Management", "Management courses" },
                { 6, "Economics", "Economics courses" },
                { 7, "Biology", "Biology courses" },
                { 8, "Chemistry", "Chemistry courses" },
                { 9, "Physics", "Physics courses" }
            });

        // Insert courses
        migrationBuilder.InsertData(
            table: "Courses",
            columns: new[] { "Id", "Code", "Name", "Description", "CreditPoints", "Level", "Semester", "SubjectAreaId", "IsActive" },
            values: new object[,]
            {
                { 1, "CS101", "Introduction to Computer Science", "Basic concepts of computer science", 3, CourseLevel.Level100, Semester.Semester1, 1, true },
                { 2, "CS102", "Programming Fundamentals", "Introduction to programming concepts", 3, CourseLevel.Level100, Semester.Semester2, 1, true },
                { 3, "MATH101", "Calculus I", "Introduction to calculus", 4, CourseLevel.Level100, Semester.Semester1, 2, true },
                { 4, "ACC101", "Financial Accounting", "Basic accounting principles", 3, CourseLevel.Level100, Semester.Semester1, 3, true },
                { 5, "FIN101", "Introduction to Finance", "Basic financial concepts", 3, CourseLevel.Level100, Semester.Semester2, 4, true },
                { 6, "MGT101", "Principles of Management", "Basic management concepts", 3, CourseLevel.Level100, Semester.Semester1, 5, true },
                { 7, "ECO101", "Microeconomics", "Introduction to microeconomics", 3, CourseLevel.Level100, Semester.Semester1, 6, true },
                { 8, "BIO101", "General Biology", "Introduction to biology", 4, CourseLevel.Level100, Semester.Semester1, 7, true },
                { 9, "CHE101", "General Chemistry", "Introduction to chemistry", 4, CourseLevel.Level100, Semester.Semester1, 8, true },
                { 10, "PHY101", "General Physics", "Introduction to physics", 4, CourseLevel.Level100, Semester.Semester1, 9, true }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "Courses",
            keyColumn: "Id",
            keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

        migrationBuilder.DeleteData(
            table: "SubjectAreas",
            keyColumn: "Id",
            keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
    }
} 