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
    }
}

