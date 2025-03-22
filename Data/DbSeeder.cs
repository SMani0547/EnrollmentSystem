using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Seed Roles
        await SeedRoles(roleManager);

        // Seed Manager
        await SeedManager(userManager);

        // Seed Sample Students
        await SeedSampleStudents(userManager, context);

        // Seed Sample Courses
        await SeedSampleCourses(context);
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

    private static async Task SeedSampleStudents(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        var sampleStudents = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                StudentId = "S12345",
                UserName = "S12345",
                Email = "s12345@student.usp.ac.fj",
                FirstName = "Avishek",
                MiddleName = "Anishkar",
                LastName = "Ram",
                DateOfBirth = new DateTime(1997, 9, 30),
                Gender = "Male",
                Citizenship = "Fiji",
                Program = "MSCCS",
                StudentLevel = "Postgraduate",
                StudentCampus = "Laucala",
                ExamSite = "Laucala Campus",
                MajorI = "Computer Science",
                ElsaTestResult = "Pass",
                ElsaTestDate = DateTime.Now.AddMonths(-6),
                GsdTestResult = "Pass",
                GsdTestDate = DateTime.Now.AddMonths(-6),
                PassportNumber = "FJ123456",
                PassportExpiryDate = DateTime.Now.AddYears(5),
                VisaNumber = "V123456",
                VisaExpiryDate = DateTime.Now.AddYears(2),
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                StudentId = "S12346",
                UserName = "S12346",
                Email = "s12346@student.usp.ac.fj",
                FirstName = "John",
                MiddleName = "David",
                LastName = "Smith",
                DateOfBirth = new DateTime(1998, 5, 15),
                Gender = "Male",
                Citizenship = "Fiji",
                Program = "BSCS",
                StudentLevel = "Undergraduate",
                StudentCampus = "Laucala",
                ExamSite = "Laucala Campus",
                MajorI = "Computer Science",
                ElsaTestResult = "Pass",
                ElsaTestDate = DateTime.Now.AddMonths(-3),
                EmailConfirmed = true
            },
            new ApplicationUser
            {
                StudentId = "S12347",
                UserName = "S12347",
                Email = "s12347@student.usp.ac.fj",
                FirstName = "Sarah",
                MiddleName = "Jane",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1999, 3, 20),
                Gender = "Female",
                Citizenship = "Samoa",
                Program = "BCOM",
                StudentLevel = "Undergraduate",
                StudentCampus = "Alafua",
                ExamSite = "Alafua Campus",
                MajorI = "Accounting",
                MajorII = "Economics",
                ElsaTestResult = "Pass",
                ElsaTestDate = DateTime.Now.AddMonths(-2),
                EmailConfirmed = true
            }
        };

        foreach (var student in sampleStudents)
        {
            var existingStudent = await userManager.FindByEmailAsync(student.Email);
            if (existingStudent == null)
            {
                var result = await userManager.CreateAsync(student, "Student@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(student, "Student");

                    // Add address
                    var address = new StudentAddress
                    {
                        UserId = student.Id,
                        StreetAddress = "123 Main Street",
                        City = student.StudentCampus,
                        State = "Central",
                        PostalCode = "0000",
                        Country = student.Citizenship,
                        PhoneNumber = "+679 999 9999"
                    };
                    context.StudentAddresses.Add(address);

                    // Add emergency contact
                    var emergencyContact = new EmergencyContact
                    {
                        UserId = student.Id,
                        Name = "Emergency Contact",
                        Relationship = "Parent",
                        PhoneNumber = "+679 888 8888",
                        Address = $"456 Family Street, {student.StudentCampus}, {student.Citizenship}",
                        Email = "emergency@example.com"
                    };
                    context.EmergencyContacts.Add(emergencyContact);
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedSampleCourses(ApplicationDbContext context)
    {
        if (!context.Courses.Any())
        {
            var courses = new List<Course>
            {
                new Course
                {
                    CourseCode = "CS101",
                    Title = "Introduction to Programming",
                    Description = "Basic concepts of programming using Python",
                    Credits = 15,
                    Fee = 500
                },
                new Course
                {
                    CourseCode = "CS102",
                    Title = "Data Structures",
                    Description = "Fundamental data structures and algorithms",
                    Credits = 15,
                    Fee = 200
                },
                new Course
                {
                    CourseCode = "CS103",
                    Title = "Database Systems",
                    Description = "Introduction to database design and SQL",
                    Credits = 15,
                    Fee = 300
                }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();
        }
    }
} 