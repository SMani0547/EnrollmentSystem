using System.Net.Http.Json;

namespace USPFinance.Services;

public class StudentFinanceService : IStudentFinanceService
{
    private readonly HttpClient _httpClient;

    public StudentFinanceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Implement your service methods here
    // Example: public async Task<StudentFinanceInfo> GetStudentFinanceInfoAsync(string studentId) { ... }
} 