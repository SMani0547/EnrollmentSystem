namespace USPEducation.Services;

public interface IStudentGradeService
{
    Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId);
} 