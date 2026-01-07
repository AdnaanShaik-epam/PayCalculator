namespace PayCalculator.DTO
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal HourlyPay { get; set; }
        public bool IsAdmin { get; set; }
    }
}
