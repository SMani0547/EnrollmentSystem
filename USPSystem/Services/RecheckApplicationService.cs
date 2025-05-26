using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.Services;

public class RecheckApplicationService
{
    private readonly ApplicationDbContext _context;

    public RecheckApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecheckApplication> CreateRecheckApplicationAsync(RecheckApplicationModel model, string studentId)
    {
        var recheckApplication = new RecheckApplication
        {
            StudentId = studentId,
            CourseCode = model.CourseCode,
            CourseName = model.CourseName,
            CurrentGrade = model.CurrentGrade,
            Year = model.Year,
            Semester = model.Semester,
            Email = model.Email,
            PaymentReceiptNumber = model.PaymentReceiptNumber,
            Reason = model.Reason,
            AdditionalComments = model.AdditionalComments,
            ApplicationDate = DateTime.UtcNow,
            Status = Models.RecheckStatus.Pending
        };

        _context.RecheckApplications.Add(recheckApplication);
        await _context.SaveChangesAsync();
        
        return recheckApplication;
    }

    public async Task<List<RecheckApplication>> GetStudentRecheckApplicationsAsync(string studentId)
    {
        return await _context.RecheckApplications
            .Where(r => r.StudentId == studentId)
            .OrderByDescending(r => r.ApplicationDate)
            .ToListAsync();
    }

    public async Task<RecheckApplication?> GetRecheckApplicationByIdAsync(int id)
    {
        return await _context.RecheckApplications
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> UpdateRecheckApplicationStatusAsync(int id, Models.RecheckStatus status)
    {
        var application = await _context.RecheckApplications.FindAsync(id);
        if (application == null)
            return false;

        application.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
} 