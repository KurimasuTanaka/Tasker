using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application;

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

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("User ID is required.");

            var result = await _userService.GetUserById(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value.ToDto());
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
