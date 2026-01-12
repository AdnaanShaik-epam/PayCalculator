using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayCalculator.DTO;
using PayCalculator.Services;

namespace PayCalculator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IEmployeeService _employeeService;

        public AdminController(IAdminService adminService, IEmployeeService employeeService)
        {
            _adminService = adminService;
            _employeeService = employeeService;
        }

        // Only allow admins
        private bool IsAdmin() =>
            User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value == "True";

        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            if (!IsAdmin()) return Forbid();
            var employees = await _adminService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost("set-hourly-pay")]
        public async Task<IActionResult> SetHourlyPay([FromBody] SetHourlyPayDto dto)
        {
            if (!IsAdmin()) return Forbid();
            await _adminService.SetHourlyPayAsync(dto.EmployeeId, dto.HourlyPay);
            return Ok();
        }

        [HttpGet("calculate-salary")]
        public async Task<IActionResult> CalculateSalary([FromQuery] int employeeId, [FromQuery] DateTime periodStart, [FromQuery] DateTime periodEnd)
        {
            if (!IsAdmin()) return Forbid();
            var salary = await _adminService.CalculateBiWeeklySalaryAsync(employeeId, periodStart, periodEnd);
            return Ok(salary);
        }

        [HttpGet("time-entries")]
        public async Task<IActionResult> GetTimeEntriesForPeriod([FromQuery] DateTime periodStart, [FromQuery] DateTime periodEnd, [FromQuery] int? employeeId)
        {
            if (!IsAdmin()) return Forbid();
            var entries = await _employeeService.GetTimeEntriesForPeriodAsync(periodStart, periodEnd, employeeId);
            return Ok(entries);
        }
    }
}
