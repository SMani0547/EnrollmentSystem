using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace USPEducation.Services
{
    public class StudentGradeService : IStudentGradeService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public StudentGradeService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ExternalAPIs:StudentSystemBaseUrl"] ?? throw new InvalidOperationException("API Base URL is not configured.");
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
    }
}

