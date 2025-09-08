using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.GroupsService;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupService;
        private readonly UserManager<IdentityUser> _userManager;

        public GroupsController(IGroupsService groupService, UserManager<IdentityUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroups(CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return BadRequest("Invalid user");

            var groups = await _groupService.GetAllGroups(userId, cancellationToken);

            return Ok(groups.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(Group group)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return BadRequest("Invalid user");

            Result<Group> createdGroup = await _groupService.CreateGroup(group, userId);

            if (!createdGroup.IsSuccess) return BadRequest(createdGroup.ErrorMessage);
            return CreatedAtAction("CreateGroup", createdGroup.Value);
        }

        [HttpGet("{groupId:long}")]
        public async Task<IActionResult> GetGroupById(long groupId, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetGroupById(groupId, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [Authorize(Policy = "Admin")]
        [Authorize(Policy = "Manager")]
        [HttpPost("{groupId:long}/members")]
        public async Task<IActionResult> AddMember(long groupId, [FromBody] string userId)
        {
            var result = await _groupService.AddGroupMember(groupId, userId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }   
    }
}
