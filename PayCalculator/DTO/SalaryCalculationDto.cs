namespace PayCalculator.DTO
{
    public class SalaryCalculationDto
    {
        public int EmployeeId { get; set; }
        public decimal HourlyPay { get; set; }
        public decimal TotalWorkingHours { get; set; }
        public decimal CalculatedSalary { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

    }
}
