using Microsoft.EntityFrameworkCore;
using USPSystem.Models;
using USPSystem.Data;

namespace USPSystem.Data.Queries;

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

