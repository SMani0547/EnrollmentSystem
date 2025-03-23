using System.Net.Http.Json;
using USPSystem.Data;
using USPSystem.Models;

namespace USPSystem.Services;

public class StudentGradeService : IStudentGradeService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    
    private readonly ApplicationDbContext _context;

    public StudentGradeService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context )
    {
        _httpClient = httpClient;
        _context = context;
        _configuration = configuration;
    }

    public async Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId)
    {
        try
        {
            var grades = await _httpClient.GetFromJsonAsync<List<Grade>>($"http://localhost:5240/api/grades/student/{studentId}");
            foreach(var grade in grades){
                var course = _context.Courses.Where(i=>i.Code == grade.CourseId).FirstOrDefault();
                grade.CourseId = course.Id.ToString();
            }

            return grades?.Where(g => g.GradeLetter != "F" && !string.IsNullOrEmpty(g.GradeLetter))
                        .Select(g => int.Parse(g.CourseId))
                        .ToHashSet() ?? new HashSet<int>();
        }
        catch (Exception ex)
        {
            // Log the error in production
            return new HashSet<int>();
        }
    }
} 

