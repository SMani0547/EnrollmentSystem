using System.Net.Http.Json;
using USPSystem.Models;

namespace USPSystem.Services;

public class StudentFinanceService : IStudentFinanceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public StudentFinanceService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _httpClient.BaseAddress = new Uri(_configuration["USPFinance:BaseUrl"] ?? "https://localhost:7235/");
    }

    public async Task<StudentFinance?> GetStudentFinanceAsync(string studentId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StudentFinance>($"api/StudentFinance/{studentId}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<StudentFinance?> GetStudentFinanceWithDetailsAsync(string studentId)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StudentFinance>($"api/StudentFinance/{studentId}/details");
        }
        catch
        {
            return null;
        }
    }
} 