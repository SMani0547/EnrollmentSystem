namespace USPSystem.Services;

public interface IStudentGradeService
{
    Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId);
} 

