using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PayCalculator.Data;
using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public AdminService(AppDbContext context, IEmployeeService employeeService, IMapper mapper)
        {
            _context = context;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<SalaryCalculationDto> CalculateBiWeeklySalaryAsync(int employeeId, DateTime periodStart, DateTime periodEnd)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if(employee == null) return new SalaryCalculationDto { EmployeeId = employeeId, CalculatedSalary = 0, HourlyPay = 0, TotalWorkingHours = 0, PeriodStart = periodStart, PeriodEnd = periodEnd };

            var workingHours = await _employeeService.CalculateWorkingHoursAsync(employeeId, periodStart, periodEnd);
            var salary = employee.HourlyPay * workingHours;

            var result = new SalaryCalculationDto
            {
                EmployeeId = employee.EmployeeId,
                HourlyPay = employee.HourlyPay,
                TotalWorkingHours = workingHours,
                CalculatedSalary = salary,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            };

            return result;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task SetHourlyPayAsync(int employeeId, decimal hourlyPay)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                employee.HourlyPay = hourlyPay;
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
