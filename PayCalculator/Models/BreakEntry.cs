namespace PayCalculator.Models
{
    public class BreakEntry
    {
        public int BreakEntryId { get; set; }
        public int TimeEntryId { get; set; }
        public DateTime BreakStart { get; set; }
        public DateTime BreakEnd { get; set; }

        public TimeEntry TimeEntry { get; set; }
    }
}
