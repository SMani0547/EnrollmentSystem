using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using USPSystem.Data;
using USPSystem.Models;
// Using fully qualified names instead of a direct import to avoid ambiguity
// using USPGradeSystem.Models;

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
            var grades = await _httpClient.GetFromJsonAsync<List<USPSystem.Models.Grade>>($"http://localhost:5240/api/grades/student/{studentId}");
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
            var grades = await _httpClient.GetFromJsonAsync<List<USPSystem.Models.Grade>>($"http://localhost:5240/api/grades/student/{studentId}");
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
                        Id = grade.Id,
                        StudentId = grade.StudentId,
                        CourseCode = grade.CourseId,
                        CourseName = course.Name,
                        Grade = grade.GradeLetter,
                        Marks = grade.Marks != null ? (decimal?)grade.Marks : null,
                        Year = DateTime.Now.Year,
                        Semester = (int)GetCurrentSemester(),
                        GradedDate = DateTime.Now
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

    public async Task<bool> ApplyForRecheckAsync(string studentId, string courseCode, int year, int semester, string reason, string? additionalComments, string email, string CourseName, string CurrentGrade, string paymentReceiptNumber)
    {
        try
        {
            // Create a completely simplified JSON object with all fields properly defined
            var json = $@"{{
  ""studentId"": ""{studentId}"",
  ""courseCode"": ""{courseCode}"",
  ""courseName"": ""{CourseName}"",
  ""currentGrade"": ""{CurrentGrade}"",
  ""year"": {year},
  ""semester"": {semester},
  ""reason"": ""{EscapeJsonString(reason)}"",
  ""additionalComments"": ""{EscapeJsonString(additionalComments ?? "")}"",
  ""email"": ""{email}"",
  ""paymentReceiptNumber"": ""{paymentReceiptNumber}"",
  ""applicationDate"": ""2025-07-01T12:00:00.000Z"",
  ""status"": 0
}}";

            Console.WriteLine($"Request JSON: {json}");

            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            
            var response = await _httpClient.PostAsync($"http://localhost:5240/api/grades/recheck", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorContent}");
                Console.WriteLine($"Status Code: {response.StatusCode}");
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            // Log the error in production
            Console.WriteLine($"Exception: {ex.Message}");
            return false;
        }
    }

    private string EscapeJsonString(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;
            
        return str
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
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

