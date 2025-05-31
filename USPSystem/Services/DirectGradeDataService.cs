using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.Services
{
    // This service provides a direct database connection to the USPGradeSystemDB
    // It can be used alongside the API-based StudentGradeService when direct DB access is needed
    public class DirectGradeDataService
    {
        private readonly GradeSystemDbContext _dbContext;

        public DirectGradeDataService(GradeSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Sample method - Get a grade by ID
        public async Task<Grade?> GetGradeByIdAsync(int id)
        {
            return await _dbContext.Grades.FindAsync(id);
        }

        // Sample method - Get all grades for a student
        public async Task<List<Grade>> GetGradesByStudentIdAsync(string studentId)
        {
            return await _dbContext.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();
        }

        // Sample method - Get all recheck applications for a student
        public async Task<List<GradeRecheckApplication>> GetRecheckApplicationsByStudentIdAsync(string studentId)
        {
            return await _dbContext.RecheckApplications
                .Where(r => r.StudentId == studentId)
                .ToListAsync();
        }

        // Sample method - Get a recheck application by ID
        public async Task<GradeRecheckApplication?> GetRecheckApplicationByIdAsync(int id)
        {
            return await _dbContext.RecheckApplications.FindAsync(id);
        }

        // Add more methods as needed for direct database access
    }
} 