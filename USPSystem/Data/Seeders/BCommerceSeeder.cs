using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.Data.Seeders;

public static class BCommerceSeeder
{
    public static async Task SeedBCommerceProgram(ApplicationDbContext context)
    {
        // Check if program already exists
        if (await context.Programs.AnyAsync())
        {
            return;
        }

        // Create Bachelor of Commerce program
        var bcom = new AcademicProgram
        {
            Code = "BCOM",
            Name = "Bachelor of Commerce",
            Description = "The Bachelor of Commerce program provides a comprehensive foundation in business disciplines including accounting, economics, management, and finance. Students can specialize in various majors and develop practical skills for successful business careers.",
            CreditPoints = 360,
            Duration = 3,
            Level = ProgramLevel.BCom,
            MajorCreditsRequired = 120,
            MinorCreditsRequired = 60
        };

        // Add and save program first
        context.Programs.Add(bcom);
        await context.SaveChangesAsync();

        // Get subject areas
        var accounting = await context.SubjectAreas.FirstOrDefaultAsync(s => s.Code == "ACC");
        var economics = await context.SubjectAreas.FirstOrDefaultAsync(s => s.Code == "ECO");
        var management = await context.SubjectAreas.FirstOrDefaultAsync(s => s.Code == "MGT");

        if (accounting == null || economics == null || management == null)
        {
            throw new InvalidOperationException("Required subject areas not found. Please ensure subject areas are seeded first.");
        }

        // Get courses for each subject area
        var accountingCourses = await context.Courses
            .Where(c => c.SubjectAreaId == accounting.Id)
            .ToListAsync();

        var economicsCourses = await context.Courses
            .Where(c => c.SubjectAreaId == economics.Id)
            .ToListAsync();

        var managementCourses = await context.Courses
            .Where(c => c.SubjectAreaId == management.Id)
            .ToListAsync();

        // Create requirements
        var requirements = new List<ProgramRequirement>();

        // Add major core requirements for Accounting
        requirements.Add(new ProgramRequirement
        {
            Program = bcom,
            SubjectArea = accounting,
            Type = RequirementType.MajorCore,
            Year = 1,
            CreditPointsRequired = 48,
            Description = "Core courses for Accounting major",
            RequiredCourses = new List<Course>(accountingCourses.Take(2))
        });

        // Add major core requirements for Economics
        requirements.Add(new ProgramRequirement
        {
            Program = bcom,
            SubjectArea = economics,
            Type = RequirementType.MajorCore,
            Year = 1,
            CreditPointsRequired = 48,
            Description = "Core courses for Economics major",
            RequiredCourses = new List<Course>(economicsCourses.Take(2))
        });

        // Add major core requirements for Management
        requirements.Add(new ProgramRequirement
        {
            Program = bcom,
            SubjectArea = management,
            Type = RequirementType.MajorCore,
            Year = 1,
            CreditPointsRequired = 48,
            Description = "Core courses for Management major",
            RequiredCourses = new List<Course>(managementCourses.Take(2))
        });

        // Add progression requirement
        requirements.Add(new ProgramRequirement
        {
            Program = bcom,
            SubjectArea = accounting, // Using accounting as default subject area
            Type = RequirementType.ProgressionRequirement,
            Year = 1,
            CreditPointsRequired = 120,
            Description = "Must complete 120 credit points to progress to Year 2",
            MinimumGrade = "C",
            RequiredCourses = new List<Course>()
        });

        // Add requirements and save
        await context.ProgramRequirements.AddRangeAsync(requirements);
        await context.SaveChangesAsync();
    }
} 

