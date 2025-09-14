using Microsoft.AspNetCore.Mvc;
using Tasker.Application;

namespace Tasker.API.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);
            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Value });
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.Login(model);
            if (result.IsSuccess)
            {
                return Ok(new { Token = result.Value });
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}