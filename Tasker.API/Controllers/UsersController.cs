using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services;

namespace Tasker.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUserById(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
