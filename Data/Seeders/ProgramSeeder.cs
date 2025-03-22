using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data.Seeders;

public static class ProgramSeeder
{
    public static async Task SeedPrograms2024(ApplicationDbContext context)
    {
        // Check if data already exists for 2024
        if (await context.Programs.AnyAsync(p => p.OfferingYear == 2024))
        {
            return;
        }

        // 1. Create Subject Areas (Majors/Minors)
        var subjectAreas = new List<SubjectArea>
        {
            // BCom Subject Areas
            new SubjectArea 
            { 
                Code = "ACC", 
                Name = "Accounting",
                Description = "Study of accounting principles, financial analysis, and business reporting",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "ECO", 
                Name = "Economics",
                Description = "Study of economic theories, market analysis, and policy development",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "MGT", 
                Name = "Management",
                Description = "Study of organizational management, leadership, and business strategy",
                CanBeMajor = true,
                CanBeMinor = true
            },

            // BSc Subject Areas
            new SubjectArea 
            { 
                Code = "BIO", 
                Name = "Biology",
                Description = "Study of living organisms, their structure, function, and evolution",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "CHE", 
                Name = "Chemistry",
                Description = "Study of matter, its properties, structure, and transformations",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "PHY", 
                Name = "Physics",
                Description = "Study of matter, energy, and their interactions",
                CanBeMajor = true,
                CanBeMinor = true
            },

            // BA Subject Areas
            new SubjectArea 
            { 
                Code = "HIS", 
                Name = "History",
                Description = "Study of past events, societies, and civilizations",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "PSY", 
                Name = "Psychology",
                Description = "Study of human behavior and mental processes",
                CanBeMajor = true,
                CanBeMinor = true
            },
            new SubjectArea 
            { 
                Code = "SOC", 
                Name = "Sociology",
                Description = "Study of human society, social relationships, and institutions",
                CanBeMajor = true,
                CanBeMinor = true
            }
        };

        await context.SubjectAreas.AddRangeAsync(subjectAreas);
        await context.SaveChangesAsync();

        // 2. Create Programs for 2024
        var programs = new List<AcademicProgram>
        {
            new AcademicProgram
            {
                Code = "BCOM",
                Name = "Bachelor of Commerce",
                Description = "A comprehensive business degree focusing on commerce, finance, and management",
                CreditPoints = 360,
                Duration = 3,
                Level = ProgramLevel.BCom,
                OfferingYear = 2024,
                MajorCreditsRequired = 48,
                MinorCreditsRequired = 24,
                IsActive = true
            },
            new AcademicProgram
            {
                Code = "BSC",
                Name = "Bachelor of Science",
                Description = "A degree program focusing on scientific principles and research",
                CreditPoints = 360,
                Duration = 3,
                Level = ProgramLevel.BSc,
                OfferingYear = 2024,
                MajorCreditsRequired = 48,
                MinorCreditsRequired = 24,
                IsActive = true
            },
            new AcademicProgram
            {
                Code = "BA",
                Name = "Bachelor of Arts",
                Description = "A flexible degree program in humanities and social sciences",
                CreditPoints = 360,
                Duration = 3,
                Level = ProgramLevel.BA,
                OfferingYear = 2024,
                MajorCreditsRequired = 48,
                MinorCreditsRequired = 24,
                IsActive = true
            }
        };

        await context.Programs.AddRangeAsync(programs);
        await context.SaveChangesAsync();

        // 3. Create Courses for each Subject Area
        foreach (var area in subjectAreas)
        {
            var courses = new List<Course>();
            
            // Level 1 Courses (First Year)
            courses.Add(new Course
            {
                Code = $"{area.Code}101",
                Name = $"Introduction to {area.Name}",
                Description = $"First-year introduction to {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level100,
                Semester = Semester.Semester1,
                SubjectAreaId = area.Id
            });
            
            courses.Add(new Course
            {
                Code = $"{area.Code}102",
                Name = $"Foundations of {area.Name}",
                Description = $"First-year foundations of {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level100,
                Semester = Semester.Semester2,
                SubjectAreaId = area.Id
            });

            // Level 2 Courses (Second Year)
            courses.Add(new Course
            {
                Code = $"{area.Code}201",
                Name = $"Intermediate {area.Name} I",
                Description = $"Second-year intermediate studies in {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level200,
                Semester = Semester.Semester1,
                SubjectAreaId = area.Id
            });

            courses.Add(new Course
            {
                Code = $"{area.Code}202",
                Name = $"Intermediate {area.Name} II",
                Description = $"Second-year advanced studies in {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level200,
                Semester = Semester.Semester2,
                SubjectAreaId = area.Id
            });

            // Level 3 Courses (Third Year)
            courses.Add(new Course
            {
                Code = $"{area.Code}301",
                Name = $"Advanced {area.Name} I",
                Description = $"Third-year advanced studies in {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level300,
                Semester = Semester.Semester1,
                SubjectAreaId = area.Id
            });

            courses.Add(new Course
            {
                Code = $"{area.Code}302",
                Name = $"Advanced {area.Name} II",
                Description = $"Third-year specialized studies in {area.Name.ToLower()}",
                CreditPoints = 12,
                Level = CourseLevel.Level300,
                Semester = Semester.Semester2,
                SubjectAreaId = area.Id
            });

            await context.Courses.AddRangeAsync(courses);
        }
        await context.SaveChangesAsync();

        // 4. Create Program Requirements
        foreach (var program in programs)
        {
            var requirements = new List<ProgramRequirement>();
            var relevantSubjectAreas = program.Code switch
            {
                "BCOM" => subjectAreas.Where(a => new[] { "ACC", "ECO", "MGT" }.Contains(a.Code)),
                "BSC" => subjectAreas.Where(a => new[] { "BIO", "CHE", "PHY" }.Contains(a.Code)),
                "BA" => subjectAreas.Where(a => new[] { "HIS", "PSY", "SOC" }.Contains(a.Code)),
                _ => Enumerable.Empty<SubjectArea>()
            };

            foreach (var area in relevantSubjectAreas)
            {
                // Add Major Core Requirements
                var majorCore = new ProgramRequirement
                {
                    Program = program,
                    SubjectArea = area,
                    Type = RequirementType.MajorCore,
                    Year = 2024,
                    CreditPointsRequired = 48,
                    Description = $"Core courses required for {area.Name} major",
                    IsActive = true,
                    RequiredCourses = await context.Courses
                        .Where(c => c.SubjectAreaId == area.Id)
                        .ToListAsync()
                };

                // Add Minor Requirements
                var minor = new ProgramRequirement
                {
                    Program = program,
                    SubjectArea = area,
                    Type = RequirementType.MinorCore,
                    Year = 2024,
                    CreditPointsRequired = 24,
                    Description = $"Courses required for {area.Name} minor",
                    IsActive = true,
                    RequiredCourses = await context.Courses
                        .Where(c => c.SubjectAreaId == area.Id && c.Level == CourseLevel.Level100)
                        .ToListAsync()
                };

                requirements.Add(majorCore);
                requirements.Add(minor);
            }

            await context.ProgramRequirements.AddRangeAsync(requirements);
        }
        await context.SaveChangesAsync();
    }
} 