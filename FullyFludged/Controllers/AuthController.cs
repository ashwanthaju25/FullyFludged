using FullyFludged.DTOs;
using FullyFludged.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullyFludged.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var response = await _authService.Authenticate(loginRequest);
            if (response == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto requestDto)
        {
            try
            {
                var user = await _userRepository.RegisterAsync(requestDto);
                return Ok(new { user.Username, user.Role });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
