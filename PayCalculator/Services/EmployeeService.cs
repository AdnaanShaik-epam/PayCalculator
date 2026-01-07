using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PayCalculator.Data;
using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public EmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddBreakEntryAsync(BreakEntry breakEntry)
        {
            await _context.BreakEntries.AddAsync(breakEntry);
            await _context.SaveChangesAsync();
        }

        public async Task AddTimeEntryAsync(TimeEntry timeEntry)
        {
            await _context.TimeEntries.AddAsync(timeEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculateWorkingHoursAsync(int employeeId, DateTime periodStart, DateTime periodEnd)
        {
            var timeEntries = await _context.TimeEntries
                .Where(te => te.EmployeeId == employeeId && te.LoginTime >= periodStart && te.LogoutTime <= periodEnd)
                .Include(te => te.BreakEntries)
                .ToListAsync();

            decimal totalMinutes = 0;
            foreach(var entry in timeEntries)
            {
                var workDuration = (decimal)(entry.LogoutTime - entry.LoginTime).TotalMinutes;
                var breakDuration = entry.BreakEntries.Sum(be => (decimal)(be.BreakEnd - be.BreakStart).TotalMinutes);
                totalMinutes += (workDuration - breakDuration);
            }

            return totalMinutes / 60;
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _context.Employees.Include(e => e.TimeEntries).FirstOrDefaultAsync(e => e.EmployeeId == id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<IEnumerable<TimeEntryDto>> GetTimeEntriesAsync(int employeeId)
        {
            var entries = await _context.TimeEntries.Where(te => te.EmployeeId == employeeId).Include(te => te.BreakEntries).ToListAsync();
            return _mapper.Map<IEnumerable<TimeEntryDto>>(entries);
        }

        public async Task<IEnumerable<TimeEntryWithEmployeeDto>> GetTimeEntriesForPeriodAsync(DateTime periodStart, DateTime periodEnd)
        {
            var entries = await _context.TimeEntries
                .Where(te => te.LoginTime >= periodStart && te.LogoutTime <= periodEnd)
                .Include(te => te.Employee)
                .ToListAsync();

            var result = entries.Select(te => new TimeEntryWithEmployeeDto
            {
                TimeEntryId = te.TimeEntryId,
                EmployeeId = te.EmployeeId,
                EmployeeName = te.Employee?.FullName ?? string.Empty,
                LoginTime = te.LoginTime,
                LogoutTime = te.LogoutTime
            });

            return result;
        }
    }
}
