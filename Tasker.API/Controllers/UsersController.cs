using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

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
            var result = await _userService.GetUserById(userId);
            if (result.IsSuccess)
            {
                return Ok(new UserDTO(result.Value));
            }
            return NotFound(result.ErrorMessage);
        }
    }
}
