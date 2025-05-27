using System.Collections.Generic;
using System.Threading.Tasks;
using USPSystem.Models;

namespace USPSystem.Services;

public interface IStudentGradeService
{
    Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId);
    Task<List<GradeViewModel>> GetGradesAsync(string studentId);
    Task<bool> ApplyForRecheckAsync(string studentId, string courseCode, int year, int semester, string reason, string? additionalComments, string email, string CourseName,string CurrentGrade, string paymentReceiptNumber);
    Task<bool> HasAppliedForRecheckAsync(string studentId, string courseCode, int year, int semester);
} 

