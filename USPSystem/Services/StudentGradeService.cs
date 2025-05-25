using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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
            if (grades == null) return new HashSet<int>();
            
            foreach(var grade in grades){
                var course = await _context.Courses.FirstOrDefaultAsync(i => i.Code == grade.CourseId);
                if (course != null)
                {
                    grade.CourseId = course.Id.ToString();
                }
            }

            return grades.Where(g => g.GradeLetter != "F" && !string.IsNullOrEmpty(g.GradeLetter))
                        .Select(g => int.Parse(g.CourseId))
                        .ToHashSet();
        }
        catch (Exception)
        {
            // Log the error in production
            return new HashSet<int>();
        }
    }

    public async Task<List<GradeViewModel>> GetGradesAsync(string studentId)
    {
        try
        {
            var grades = await _httpClient.GetFromJsonAsync<List<Grade>>($"http://localhost:5240/api/grades/student/{studentId}");
            if (grades == null)
                return new List<GradeViewModel>();

            var gradeViewModels = new List<GradeViewModel>();
            foreach (var grade in grades)
            {
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.Code == grade.CourseId);
                if (course != null)
                {
                    gradeViewModels.Add(new GradeViewModel
                    {
                        StudentId = grade.StudentId,
                        CourseCode = grade.CourseId,
                        CourseName = course.Name,
                        Grade = grade.GradeLetter,
                        Marks = grade.Marks,
                        Year = DateTime.Now.Year, // You might want to store this in the Grade model
                        Semester = (int)GetCurrentSemester(), // Cast Semester enum to int
                        GradedDate = DateTime.Now // You might want to store this in the Grade model
                    });
                }
            }

            return gradeViewModels;
        }
        catch (Exception)
        {
            // Log the error in production
            return new List<GradeViewModel>();
        }
    }

    public async Task<bool> ApplyForRecheckAsync(string studentId, string courseCode, int year, int semester, string reason, string? additionalComments)
    {
        try
        {
            var recheckRequest = new
            {
                StudentId = studentId,
                CourseCode = courseCode,
                Year = year,
                Semester = semester,
                Reason = reason,
                AdditionalComments = additionalComments,
                RequestDate = DateTime.UtcNow
            };

            var content = new StringContent(JsonSerializer.Serialize(recheckRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"http://localhost:5240/api/grades/recheck", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            // Log the error in production
            return false;
        }
    }

    public async Task<bool> HasAppliedForRecheckAsync(string studentId, string courseCode, int year, int semester)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"http://localhost:5240/api/grades/recheck/status?studentId={studentId}&courseCode={courseCode}&year={year}&semester={semester}");

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();
            var status = JsonSerializer.Deserialize<RecheckStatus>(content);
            return status?.HasApplied ?? false;
        }
        catch (Exception)
        {
            // Log the error in production
            return false;
        }
    }

    private static Semester GetCurrentSemester()
    {
        var month = DateTime.Now.Month;
        return month switch
        {
            >= 1 and <= 6 => Semester.Semester1,
            >= 7 and <= 11 => Semester.Semester2,
            _ => Semester.Semester1
        };
    }
}

public class RecheckStatus
{
    public bool HasApplied { get; set; }
    public DateTime? RequestDate { get; set; }
    public string? Status { get; set; }
} 

