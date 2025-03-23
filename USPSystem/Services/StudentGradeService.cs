using System.Net.Http.Json;
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
                        Semester = (int)GetCurrentSemester() // Cast Semester enum to int
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

