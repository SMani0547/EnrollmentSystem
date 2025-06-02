using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using USPFinance.Data;
using USPFinance.Models;
using System.Net.Http;
using System.Text.Json;

namespace USPFinance.Services
{
    public class StudentFinanceUpdateService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public StudentFinanceUpdateService(
            AppDbContext context,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
            
            // Set up the base URL for the USPSystem API
            var baseUrl = _configuration["USPSystem:BaseUrl"] ?? "https://localhost:7234/";
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task UpdateStudentFinanceAsync(string studentId)
        {
            try
            {
                // Get all active enrollments for the student
                var enrollmentsResponse = await _httpClient.GetAsync($"api/StudentEnrollment/active/{studentId}");
                if (!enrollmentsResponse.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to get enrollments for student {studentId}");
                }

                var enrollments = await enrollmentsResponse.Content.ReadFromJsonAsync<List<StudentEnrollmentDto>>();
                if (enrollments == null)
                {
                    throw new Exception($"No enrollments found for student {studentId}");
                }

                // Calculate total fees
                decimal totalFees = 0;
                foreach (var enrollment in enrollments)
                {
                    // Get course details to get the fee
                    var courseResponse = await _httpClient.GetAsync($"api/Course/{enrollment.CourseId}");
                    if (!courseResponse.IsSuccessStatusCode)
                    {
                        continue; // Skip this course if we can't get its details
                    }

                    var course = await courseResponse.Content.ReadFromJsonAsync<CourseDto>();
                    if (course?.Fees != null)
                    {
                        totalFees += course.Fees.Value;
                    }
                }

                // Update or create student finance record
                var studentFinance = await _context.StudentFinances
                    .FirstOrDefaultAsync(sf => sf.StudentID == studentId);

                if (studentFinance == null)
                {
                    studentFinance = new StudentFinance
                    {
                        StudentID = studentId,
                        TotalFees = totalFees,
                        AmountPaid = 0,
                        LastUpdated = DateTime.UtcNow
                    };
                    _context.StudentFinances.Add(studentFinance);
                }
                else
                {
                    studentFinance.TotalFees = totalFees;
                    studentFinance.LastUpdated = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                throw new Exception($"Error updating student finance for {studentId}: {ex.Message}", ex);
            }
        }
    }

    // DTOs for deserializing the response from USPSystem
    public class StudentEnrollmentDto
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public string? Grade { get; set; }
        public int Status { get; set; }
        public bool IsActive { get; set; }
    }

    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal? Fees { get; set; }
    }
} 