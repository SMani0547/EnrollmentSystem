using Microsoft.AspNetCore.Identity;
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

    private static async Task SeedSampleCourses(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Courses.Any())
        {
            var courses = new List<Course>
            {
                new Course { CourseCode = "CS101", Title = "Introduction to Computing", Description = "Basic concepts of computing", Credits = 15 },
                new Course { CourseCode = "CS102", Title = "Programming Fundamentals", Description = "Introduction to programming concepts", Credits = 15 },
                new Course { CourseCode = "CS201", Title = "Data Structures", Description = "Advanced programming concepts", Credits = 15 }
            };

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
        }
    }
} 