using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Services
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<TimeEntryDto>> GetTimeEntriesAsync(int employeeId);
        Task AddTimeEntryAsync(TimeEntry timeEntry);
        Task AddBreakEntryAsync(BreakEntry breakEntry);
        Task<decimal> CalculateWorkingHoursAsync(int employeeId, DateTime periodStart, DateTime periodEnd);
        Task<IEnumerable<TimeEntryWithEmployeeDto>> GetTimeEntriesForPeriodAsync(DateTime periodStart, DateTime periodEnd);
    }
}
