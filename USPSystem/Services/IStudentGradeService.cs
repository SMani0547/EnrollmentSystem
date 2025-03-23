using System.Collections.Generic;
using System.Threading.Tasks;
using USPSystem.Models;

namespace USPSystem.Services;

public interface IStudentGradeService
{
    Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId);
    Task<List<GradeViewModel>> GetGradesAsync(string studentId);
} 

