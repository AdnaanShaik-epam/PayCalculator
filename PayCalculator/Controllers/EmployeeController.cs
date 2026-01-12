using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayCalculator.DTO;
using PayCalculator.Models;
using PayCalculator.Services;

namespace PayCalculator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var dto = await _employeeService.GetEmployeeByIdAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        [HttpGet("{employeeId}/time-entries")]
        public async Task<IActionResult> GetTimeEntries(int employeeId)
        {
            var dtos = await _employeeService.GetTimeEntriesAsync(employeeId);
            return Ok(dtos);
        }

        [HttpPost("time-entry")]
        public async Task<IActionResult> AddTimeEntry([FromBody] TimeEntryDto dto)
        {
            // determine if caller is admin
            var isAdmin = User.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value == "True";
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            int.TryParse(userIdClaim, out var callerId);

            if (!isAdmin)
            {
                // Override employee id to caller to prevent forging
                dto.EmployeeId = callerId;

                // non-admins can only add entries for themselves (now enforced)
                if (dto.EmployeeId != callerId)
                    return Forbid();

                // disallow entries not on current day (server local date)
                var now = DateTime.Now;
                if (dto.LoginTime.Date != now.Date || dto.LogoutTime.Date != now.Date)
                    return BadRequest("Employees can only add time entries for the current day.");

                // disallow future times (both must be <= now)
                if (dto.LoginTime > now || dto.LogoutTime > now)
                    return BadRequest("Cannot add entries with future times.");

                if (dto.LogoutTime <= dto.LoginTime)
                    return BadRequest("Logout must be after login.");
            }

            var timeEntry = _mapper.Map<TimeEntry>(dto);
            await _employeeService.AddTimeEntryAsync(timeEntry);
            return Ok();
        }

        [HttpPost("break-entry")]
        public async Task<IActionResult> AddBreakEntry([FromBody] BreakEntryDto dto)
        {
            var breakEntry = _mapper.Map<BreakEntry>(dto);
            await _employeeService.AddBreakEntryAsync(breakEntry);
            return Ok();
        }

        [HttpGet("{employeeId}/working-hours")]
        public async Task<IActionResult> GetWorkingHours(int employeeId, [FromQuery] DateTime periodStart, [FromQuery] DateTime periodEnd)
        {
            var hours = await _employeeService.CalculateWorkingHoursAsync(employeeId, periodStart, periodEnd);
            return Ok(hours);
        }
    }
}
