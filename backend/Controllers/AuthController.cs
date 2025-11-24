using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Backend.Dtos;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UsernameOrEmail)
                       ?? await _userManager.FindByEmailAsync(dto.UsernameOrEmail);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Username, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            return Ok(new { Token = token });
        }
    }
}
