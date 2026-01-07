using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PayCalculator.DTO;
using PayCalculator.Services;

namespace PayCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var employee = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                if (employee == null)
                    return Unauthorized("Invalid credentials");

                var token = _authService.GenerateJwtToken(employee);
                return Ok(new { token, employeeId = employee.EmployeeId, isAdmin = employee.IsAdmin });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var employee = await _authService.RegisterAsync(registerDto);
                if (employee == null)
                    return BadRequest("Email already in use");

                var token = _authService.GenerateJwtToken(employee);
                return Ok(new { token, employeeId = employee.EmployeeId, isAdmin = employee.IsAdmin });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
