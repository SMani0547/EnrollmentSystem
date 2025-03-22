using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data.Seeders;

public static class CoursePrerequisiteSeeder
{
    public static async Task SeedPrerequisites(ApplicationDbContext context)
    {
        // Check if prerequisites are already set
        var hasPrerequisites = await context.Courses.AnyAsync(c => c.Prerequisites.Any());
        if (hasPrerequisites)
            return;

        // Get all courses grouped by subject area
        var coursesByArea = await context.Courses
            .GroupBy(c => c.SubjectAreaId)
            .ToDictionaryAsync(g => g.Key, g => g.OrderBy(c => c.Code).ToList());

        foreach (var area in coursesByArea)
        {
            var courses = area.Value;
            
            // For each subject area, set up prerequisites based on course levels
            foreach (var course in courses)
            {
                switch (course.Level)
                {
                    case CourseLevel.Level200:
                        // Level 200 courses require both Level 100 courses
                        var level100Courses = courses
                            .Where(c => c.Level == CourseLevel.Level100)
                            .ToList();
                        course.Prerequisites = level100Courses;
                        break;

                    case CourseLevel.Level300:
                        // Level 300 courses require both Level 200 courses
                        var level200Courses = courses
                            .Where(c => c.Level == CourseLevel.Level200)
                            .ToList();
                        course.Prerequisites = level200Courses;
                        break;

                    case CourseLevel.Level100:
                    default:
                        // Level 100 courses have no prerequisites
                        break;
                }
            }
        }

        await context.SaveChangesAsync();
    }
} 