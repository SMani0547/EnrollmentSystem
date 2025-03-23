using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data.Queries;

public static class VerifyCourses
{
    public static async Task<List<Course>> GetAllCourses(ApplicationDbContext context)
    {
        return await context.Courses
            .Include(c => c.SubjectArea)
            .OrderBy(c => c.Code)
            .ToListAsync();
    }
} 