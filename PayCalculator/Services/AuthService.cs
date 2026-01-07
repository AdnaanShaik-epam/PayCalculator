using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PayCalculator.Data;
using PayCalculator.Models;
using PayCalculator.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayCalculator.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Employee> AuthenticateAsync(string email, string password)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.PasswordHash == password);
        }

        public async Task<Employee> RegisterAsync(RegisterDto dto)
        {

            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.Email == dto.Email);
            if (existing != null) return null;

            var employee = new Employee
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = dto.Password, 
                HourlyPay = 0,
                IsAdmin = false
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public string GenerateJwtToken(Employee employee)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, employee.Email),
            new Claim("id", employee.EmployeeId.ToString()),
            new Claim("isAdmin", employee.IsAdmin.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
