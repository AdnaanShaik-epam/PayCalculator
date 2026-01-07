using PayCalculator.Models;
using PayCalculator.DTO;

namespace PayCalculator.Services
{
    public interface IAuthService
    {
        Task<Employee> AuthenticateAsync(string email, string password);
        string GenerateJwtToken(Employee employee);
        Task<Employee> RegisterAsync(RegisterDto dto);
    }
}
