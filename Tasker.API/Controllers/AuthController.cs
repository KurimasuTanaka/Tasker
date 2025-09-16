using Microsoft.AspNetCore.Mvc;
using Tasker.Application;

namespace Tasker.API.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _logger.LogTrace($"Requested registration for {model.Email}");

            if (model.Email == null || model.Password == null || model.FirstName == null || model.LastName == null)
            {
                _logger.LogWarning("User registration failed: all fields are required.");
                return BadRequest("All fields are required.");
            }

            try
            {
                var result = await _authService.Register(model);
                if (result.IsSuccess)
                {
                    return Ok(new { Token = result.Value });
                }
                return BadRequest(result.ErrorMessage);

            }
            catch (Exception)
            {
                return BadRequest("Invalid request payload.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogDebug($"Requested login for {model.Email}");

            if (model.Email == null || model.Password == null)
            {
                _logger.LogWarning("User login failed: email and password are required.");
                return BadRequest("Email and Password are required.");
            }

            try
            {
                var result = await _authService.Login(model);
                if (result.IsSuccess)
                {
                    return Ok(new { Token = result.Value });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception)
            {
                return BadRequest("Invalid request payload.");
            }
        }
    }
}