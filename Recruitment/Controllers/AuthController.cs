using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Recruitment.API.Authentication;
using Recruitment.API.DTOs.Auth;
using Recruitment.API.Services;
using Recruitment.Infrastructure.Identity;

namespace Recruitment.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtOptions _jwtOptions;

        public AuthController(UserManager<User> userManager, IJwtTokenService jwtTokenService, IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _jwtOptions = jwtOptions.Value;
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return Conflict("Korisnik sa tom email adresom vec postoji.");

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                DateOfBirth = request.DateOfBirth,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => new { e.Code, e.Description }));

            await _userManager.AddToRoleAsync(user, "Candidate");

            return Created($"/auth/users/{user.Id}", new { user.Id, user.Email });
        }

     
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Pogresan email ili lozinka.");

            var token = await _jwtTokenService.CreateTokenAsync(user);

            return Ok(new AuthResponse
            {
                Token = token,
                ExpiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiresInMinutes)
            });
        }
    }
}
