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

            if (model.Email == null || model.Password == null || model.FirstName == null || model.LastName == null)
            {
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
            if (model.Email == null || model.Password == null)
            {
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