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
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(string userId)
        {
            _logger.LogTrace($"Requested user details for user ID {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Get user by ID failed: user ID is null or empty");
                return BadRequest("User ID is required.");
            }

            var result = await _userService.GetUserById(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value.ToDto());
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
