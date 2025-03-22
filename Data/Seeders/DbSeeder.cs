using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using USPEducation.Models;
using USPEducation.Data.Seeders;

namespace USPEducation.Data.Seeders;

public static class DbSeeder
{
    private static async Task CreateStudent(
        UserManager<ApplicationUser> userManager,
        string email,
        string firstName,
        string lastName,
        string majorI,
        int admissionYear,
        MajorType majorType = MajorType.SingleMajor,
        string? majorII = null,
        string? minorI = null)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var student = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                AdmissionYear = admissionYear,
                MajorType = majorType,
                MajorI = majorI,
                MajorII = majorII,
                MinorI = minorI
            };

            var result = await userManager.CreateAsync(student, "Student123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(student, "Student");
            }
        }
    }

    public static async Task SeedData(ApplicationDbContext context, IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Create roles
        string[] roles = { "Admin", "Manager", "Student" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create admin user
        var adminEmail = "admin@usp.ac.fj";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                AdmissionYear = DateTime.Now.Year,
                MajorType = MajorType.SingleMajor,
                MajorI = "ADM"
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // Create manager user
        var managerEmail = "manager@usp.ac.fj";
        if (await userManager.FindByEmailAsync(managerEmail) == null)
        {
            var manager = new ApplicationUser
            {
                UserName = managerEmail,
                Email = managerEmail,
                FirstName = "Program",
                LastName = "Manager",
                EmailConfirmed = true,
                AdmissionYear = DateTime.Now.Year,
                MajorType = MajorType.SingleMajor,
                MajorI = "MGT"
            };

            var result = await userManager.CreateAsync(manager, "Manager123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(manager, "Manager");
            }
        }

        // Create multiple student users
        var currentYear = DateTime.Now.Year;
        
        // IT Students
        await CreateStudent(userManager, "john.doe@usp.ac.fj", "John", "Doe", "ITC", currentYear);
        await CreateStudent(userManager, "jane.smith@usp.ac.fj", "Jane", "Smith", "ITC", currentYear - 1);
        
        // Business Students
        await CreateStudent(userManager, "bob.wilson@usp.ac.fj", "Bob", "Wilson", "ACC", currentYear, 
            MajorType.DoubleMajor, "FIN");
        await CreateStudent(userManager, "mary.jones@usp.ac.fj", "Mary", "Jones", "MGT", currentYear,
            MajorType.SingleMajor, null, "ACC");
            
        // Science Students
        await CreateStudent(userManager, "alex.brown@usp.ac.fj", "Alex", "Brown", "BIO", currentYear - 1);
        await CreateStudent(userManager, "sarah.davis@usp.ac.fj", "Sarah", "Davis", "CHE", currentYear);
        
        // Economics Students
        await CreateStudent(userManager, "mike.taylor@usp.ac.fj", "Mike", "Taylor", "ECO", currentYear,
            MajorType.DoubleMajor, "MGT");
        await CreateStudent(userManager, "lisa.anderson@usp.ac.fj", "Lisa", "Anderson", "ECO", currentYear - 2);

        // Seed 2024 programs
        await ProgramSeeder.SeedPrograms2024(context);

        // Seed course prerequisites
        await CoursePrerequisiteSeeder.SeedPrerequisites(context);
    }
} 