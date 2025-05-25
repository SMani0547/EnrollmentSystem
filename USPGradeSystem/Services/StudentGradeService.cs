using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using USPGradeSystem.Models;

namespace USPEducation.Services
{
    public class StudentGradeService : IStudentGradeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public StudentGradeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ExternalAPIs:GradeAPIBaseUrl"] ?? throw new InvalidOperationException("Grade API Base URL is not configured.");
        }

        public async Task<bool> SendGradeDataAsync(int studentId, int courseId, string grade)
        {
            var gradeData = new
            {
                StudentId = studentId,
                CourseId = courseId,
                Grade = grade
            };

            var content = new StringContent(JsonSerializer.Serialize(gradeData), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/grades", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<HashSet<int>> GetCompletedCourseIdsAsync(string studentId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/grades/student/{studentId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch grades for student {studentId}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var grades = JsonSerializer.Deserialize<List<Grade>>(content);

            // Return course IDs where grade is not F and not null
            return grades?
                .Where(g => g.GradeLetter != "F" && !string.IsNullOrEmpty(g.GradeLetter))
                .Select(g => int.Parse(g.CourseId))
                .ToHashSet() ?? new HashSet<int>();
        }

        public async Task<bool> ApplyForRecheckAsync(string studentId, string courseCode, int year, int semester, string reason, string? additionalComments, string email, string paymentReceiptNumber)
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
                    Email = email,
                    PaymentReceiptNumber = paymentReceiptNumber,
                    RequestDate = DateTime.UtcNow
                };

                var content = new StringContent(JsonSerializer.Serialize(recheckRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/grades/recheck", content);

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
                    $"{_apiBaseUrl}/grades/recheck/status?studentId={studentId}&courseCode={courseCode}&year={year}&semester={semester}");

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
    }

    public class RecheckStatus
    {
        public bool HasApplied { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Status { get; set; }
    }
}

