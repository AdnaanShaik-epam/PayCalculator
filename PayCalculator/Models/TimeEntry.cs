namespace PayCalculator.Models
{
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }

        public Employee Employee { get; set; }
        public ICollection<BreakEntry> BreakEntries { get; set; }
    }
}
