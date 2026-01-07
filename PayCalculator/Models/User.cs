namespace PayCalculator.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
