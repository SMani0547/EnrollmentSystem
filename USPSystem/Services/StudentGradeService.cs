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
                    var gradeViewModel = new GradeViewModel
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
                    };
                    
                    // Check if recheck has been applied and get the status
                    try
                    {
                        var requestUrl = $"http://localhost:5240/api/grades/recheck/status?studentId={studentId}&courseCode={grade.CourseId}&year={gradeViewModel.Year}&semester={gradeViewModel.Semester}";
                        var response = await _httpClient.GetAsync(requestUrl);
                        
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var recheckStatus = JsonSerializer.Deserialize<RecheckStatus>(content, options);
                            
                            if (recheckStatus != null)
                            {
                                gradeViewModel.HasAppliedForRecheck = recheckStatus.HasApplied;
                                gradeViewModel.RecheckStatus = recheckStatus.GetStatusText();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting recheck status: {ex.Message}");
                        // Continue processing other grades
                    }
                    
                    gradeViewModels.Add(gradeViewModel);
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
            var requestUrl = $"http://localhost:5240/api/grades/recheck/status?studentId={studentId}&courseCode={courseCode}&year={year}&semester={semester}";
            Console.WriteLine($"Checking recheck status: {requestUrl}");
            
            var response = await _httpClient.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error checking recheck status: {response.StatusCode}, {errorContent}");
                
                // Try alternate endpoint
                var altResponse = await _httpClient.GetAsync($"http://localhost:5240/api/grades/recheck-status?studentId={studentId}&courseId={courseCode}");
                if (altResponse.IsSuccessStatusCode)
                {
                    var altContent = await altResponse.Content.ReadAsStringAsync();
                    var altStatus = JsonSerializer.Deserialize<Dictionary<string, bool>>(altContent);
                    return altStatus != null && altStatus.TryGetValue("hasApplied", out bool hasApplied) && hasApplied;
                }
                
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Recheck status response: {content}");
            
            try {
                // Use case-insensitive deserialization
                var options = new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true 
                };
                var status = JsonSerializer.Deserialize<RecheckStatus>(content, options);
                return status?.HasApplied ?? false;
            }
            catch (JsonException ex) {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                // Fallback: manually parse the JSON response
                if (content.Contains("hasApplied") && content.Contains("true")) {
                    return true;
                }
                return false;
            }
        }
        catch (Exception ex)
        {
            // Log the error in production
            Console.WriteLine($"Exception checking recheck status: {ex.Message}");
            return false;
        }
    }

    public async Task<List<RecheckRequestViewModel>> GetAllRecheckRequestsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("http://localhost:5240/api/grades/recheck");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error getting recheck requests: {response.StatusCode}");
                return new List<RecheckRequestViewModel>();
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Recheck requests response: {content}");
            
            var options = new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true 
            };
            
            var recheckRequests = JsonSerializer.Deserialize<List<RecheckApplication>>(content, options);
            if (recheckRequests == null)
                return new List<RecheckRequestViewModel>();

            var viewModels = new List<RecheckRequestViewModel>();
            foreach (var request in recheckRequests)
            {
                // Get student name if possible
                string studentName = "Unknown";
                var student = await _context.Users.FirstOrDefaultAsync(u => u.StudentId == request.StudentId);
                if (student != null)
                {
                    studentName = $"{student.FirstName} {student.LastName}";
                }

                viewModels.Add(new RecheckRequestViewModel
                {
                    Id = request.Id,
                    StudentId = request.StudentId,
                    StudentName = studentName,
                    CourseCode = request.CourseCode,
                    CourseName = request.CourseName,
                    CurrentGrade = request.CurrentGrade,
                    Year = request.Year,
                    Semester = request.Semester,
                    ApplicationDate = request.ApplicationDate,
                    Status = request.Status.ToString(),
                    Reason = request.Reason,
                    Email = request.Email,
                    PaymentReceiptNumber = request.PaymentReceiptNumber
                });
            }

            return viewModels;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception getting recheck requests: {ex.Message}");
            return new List<RecheckRequestViewModel>();
        }
    }

    public async Task<bool> UpdateRecheckStatusAsync(int recheckId, string newStatus)
    {
        try
        {
            if (!Enum.TryParse<USPGradeSystem.Models.RecheckStatus>(newStatus, out var status))
            {
                Console.WriteLine($"Invalid status: {newStatus}");
                return false;
            }

            var statusUpdate = new { Status = (int)status };
            var json = JsonSerializer.Serialize(statusUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"http://localhost:5240/api/grades/recheck/{recheckId}/status", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception updating recheck status: {ex.Message}");
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
    [System.Text.Json.Serialization.JsonPropertyName("hasApplied")]
    public bool HasApplied { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("requestDate")]
    public DateTime? RequestDate { get; set; }
    
    [System.Text.Json.Serialization.JsonPropertyName("status")]
    public string? Status { get; set; }
    
    public string GetStatusText()
    {
        if (Status == null) return "Pending";
        
        if (int.TryParse(Status, out var statusCode))
        {
            return statusCode switch
            {
                0 => "Pending",
                1 => "InProgress",
                2 => "Completed",
                3 => "Rejected",
                _ => "Unknown"
            };
        }
        
        return Status;
    }
} 

