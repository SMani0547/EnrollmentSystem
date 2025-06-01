using System.Net.Http.Json;
using USPFinance.Models;
using Microsoft.Extensions.Logging;

namespace USPSystem.Services;

public class StudentFinanceService : IStudentFinanceService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<StudentFinanceService> _logger;

    public StudentFinanceService(
        HttpClient httpClient, 
        IConfiguration configuration,
        ILogger<StudentFinanceService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        var baseUrl = _configuration["USPFinance:BaseUrl"] ?? "https://localhost:7235/";
        _logger.LogInformation("Initializing StudentFinanceService with base URL: {BaseUrl}", baseUrl);
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<StudentFinance?> GetStudentFinanceAsync(string studentId)
    {
        try
        {
            _logger.LogInformation("Attempting to get student finance for student {StudentId}", studentId);
            var response = await _httpClient.GetAsync($"api/StudentFinance/{studentId}");
            _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<StudentFinance>();
                _logger.LogInformation("Successfully retrieved finance data for student {StudentId}", studentId);
                return result;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to get student finance data. Status: {StatusCode}, Content: {Content}, StudentId: {StudentId}", 
                response.StatusCode, errorContent, studentId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student finance data for student {StudentId}", studentId);
            return null;
        }
    }

    public async Task<StudentFinanceDetails?> GetStudentFinanceWithDetailsAsync(string studentId)
    {
        try
        {
            _logger.LogInformation("Attempting to get student finance details for student {StudentId}", studentId);
            var response = await _httpClient.GetAsync($"api/StudentFinance/{studentId}/details");
            _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<StudentFinanceDetails>();
                _logger.LogInformation("Successfully retrieved finance details for student {StudentId}", studentId);
                return result;
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogWarning("Failed to get student finance details. Status: {StatusCode}, Content: {Content}, StudentId: {StudentId}", 
                response.StatusCode, errorContent, studentId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student finance details for student {StudentId}", studentId);
            return null;
        }
    }
} 