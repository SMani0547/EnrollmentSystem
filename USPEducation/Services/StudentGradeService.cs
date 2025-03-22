using System.Net.Http.Json;
using USPGradeSystem.Models;

namespace USPEducation.Services;

public class StudentGradeService : IStudentGradeService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public StudentGradeService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId)
    {
        try
        {
            var grades = await _httpClient.GetFromJsonAsync<List<Grade>>($"http://localhost:5240/api/grades/student/{studentId}");
            return grades?.Where(g => g.GradeLetter != "F" && !string.IsNullOrEmpty(g.GradeLetter))
                        .Select(g => int.Parse(g.CourseId))
                        .ToHashSet() ?? new HashSet<int>();
        }
        catch (Exception)
        {
            // Log the error in production
            return new HashSet<int>();
        }
    }
} 