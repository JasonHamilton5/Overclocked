using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Overclocked.API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Overclocked.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed - email already in use: {Email}", request.Email);
                    return BadRequest(new { message = "Email already in use." });
                }

                var user = new IdentityUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Registration failed for {Email}: {Errors}", request.Email, errors);
                    return BadRequest(new { message = "Registration failed.", errors });
                }

                _logger.LogInformation("New user registered: {Username}", request.Username);
                return Ok(new { message = "Registration successful." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Email}", request.Email);
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    _logger.LogWarning("Failed login attempt for {Email}", request.Email);
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                var token = GenerateToken(user);

                _logger.LogInformation("User logged in: {Username}", user.UserName);

                var response = new AuthResponseDto
                {
                    Token = token,
                    Username = user.UserName!
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", request.Email);
                return StatusCode(500, new { message = "Something went wrong.", error = ex.Message });
            }
        }

        private string GenerateToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToInt32(jwtSettings["ExpiryInDays"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}