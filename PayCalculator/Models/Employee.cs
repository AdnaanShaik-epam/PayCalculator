using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayCalculator.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public decimal HourlyPay { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<TimeEntry> TimeEntries { get; set; }
    }
}
