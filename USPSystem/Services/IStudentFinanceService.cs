using USPFinance.Models;

namespace USPSystem.Services;

public interface IStudentFinanceService
{
    Task<StudentFinance?> GetStudentFinanceAsync(string studentId);
    Task<StudentFinanceDetails?> GetStudentFinanceWithDetailsAsync(string studentId);
} 