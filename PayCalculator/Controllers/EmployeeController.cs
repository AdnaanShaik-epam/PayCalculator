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
