using System;

namespace PayCalculator.DTO
{
    public class TimeEntryWithEmployeeDto
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
    }
}
