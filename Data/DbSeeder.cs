using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed Roles
        await SeedRoles(roleManager);

        // Seed Manager
        await SeedManager(userManager);

        // Seed Sample Students
        await SeedSampleStudents(userManager);

        // Seed Sample Courses
        await SeedSampleCourses(serviceProvider);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Manager"))
            await roleManager.CreateAsync(new IdentityRole("Manager"));

        if (!await roleManager.RoleExistsAsync("Student"))
            await roleManager.CreateAsync(new IdentityRole("Student"));
    }

    private static async Task SeedManager(UserManager<ApplicationUser> userManager)
    {
        var managerEmail = "manager@usp.ac.fj";
        var manager = await userManager.FindByEmailAsync(managerEmail);

        if (manager == null)
        {
            manager = new ApplicationUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                FirstName = "System",
                LastName = "Manager",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(manager, "Manager@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(manager, "Manager");
            }
        }
    }

    private static async Task SeedSampleStudents(UserManager<ApplicationUser> userManager)
    {
        var sampleStudents = new List<(string StudentId, string FirstName, string LastName)>
        {
            ("S12345", "John", "Smith"),
            ("S12346", "Mary", "Johnson"),
            ("S12347", "David", "Williams")
        };

        foreach (var student in sampleStudents)
        {
            var studentId = student.StudentId;
            var existingStudent = await userManager.Users.FirstOrDefaultAsync(u => u.StudentId == studentId);

            if (existingStudent == null)
            {
                var newStudent = new ApplicationUser
                {
                    UserName = studentId,
                    StudentId = studentId,
                    Email = $"{studentId.ToLower()}@student.usp.ac.fj",
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newStudent, "Student@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newStudent, "Student");
                }
            }
        }
    }

    private static async Task SeedSampleCourses(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Courses.Any())
        {
            var courses = new List<Course>
            {
                new Course
                {
                    CourseCode = "CS101",
                    Title = "Introduction to Programming",
                    Description = "Basic concepts of programming using Python",
                    Credits = 15
                },
                new Course
                {
                    CourseCode = "CS102",
                    Title = "Data Structures",
                    Description = "Fundamental data structures and algorithms",
                    Credits = 15
                },
                new Course
                {
                    CourseCode = "CS103",
                    Title = "Database Systems",
                    Description = "Introduction to database design and SQL",
                    Credits = 15
                }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();
        }
    }
} 