using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Data.Queries;

namespace USPEducation.Programs;

public class VerifyCoursesProgram
{
    public static async Task Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=FJ241064;Database=USPEducation;Trusted_Connection=True;TrustServerCertificate=True;");

        using var context = new ApplicationDbContext(optionsBuilder.Options);
        
        var courses = await VerifyCourses.GetAllCourses(context);
        
        Console.WriteLine("Current Courses in Database:");
        Console.WriteLine("----------------------------");
        foreach (var course in courses)
        {
            Console.WriteLine($"Code: {course.Code}");
            Console.WriteLine($"Name: {course.Name}");
            Console.WriteLine($"Subject Area: {course.SubjectArea.Name}");
            Console.WriteLine($"Credit Points: {course.CreditPoints}");
            Console.WriteLine($"Level: {course.Level}");
            Console.WriteLine($"Semester: {course.Semester}");
            Console.WriteLine("----------------------------");
        }
    }
} 

