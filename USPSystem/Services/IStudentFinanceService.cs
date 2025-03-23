using USPSystem.Models;

namespace USPSystem.Services;

public interface IStudentFinanceService
{
    Task<StudentFinance?> GetStudentFinanceAsync(string studentId);
    Task<StudentFinance?> GetStudentFinanceWithDetailsAsync(string studentId);
} 