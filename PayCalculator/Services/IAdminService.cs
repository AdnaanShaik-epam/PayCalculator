using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task SetHourlyPayAsync(int employeeId, decimal hourlyPay);
        Task<SalaryCalculationDto> CalculateBiWeeklySalaryAsync(int employeeId, DateTime periodStart, DateTime periodEnd);
    }
}
