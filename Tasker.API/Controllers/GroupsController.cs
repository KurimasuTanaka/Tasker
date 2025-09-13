using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.GroupsService;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;
using Tasker.Database;

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
        public async Task<ActionResult<List<GroupDTO>>> GetAllGroups(CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return BadRequest("Invalid user");

            var groups = await _groupService.GetAllGroups(userId, cancellationToken);

            return Ok(groups.Value.Select(g => new Group(g)).ToList());
        }

        [HttpPost]
        public async Task<ActionResult<GroupDTO>> CreateGroup(Group group)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return BadRequest("Invalid user");

            Result<Group> createdGroup = await _groupService.CreateGroup(group, userId);

            if (!createdGroup.IsSuccess) return BadRequest(createdGroup.ErrorMessage);
            return CreatedAtAction("CreateGroup", createdGroup.Value.ToDTO());
        }

        [HttpGet("{groupId:long}")]
        [GroupAuthorize(GroupRole.User)]
        public async Task<ActionResult<GroupDTO>> GetGroupById(long groupId, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetGroupById(groupId, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            return Ok(result.Value.ToDTO());
        }


        [HttpPost("{groupId:long}/members")]
        [GroupAuthorize(GroupRole.Manager)]
        public async Task<IActionResult> AddMember(long groupId, [FromBody] string userId)
        {
            var result = await _groupService.AddGroupMember(groupId, userId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [GroupAuthorize(GroupRole.Admin)]
        [HttpDelete("{groupId:long}")]
        public async Task<IActionResult> DeleteGroup(long groupId)
        {
            var result = await _groupService.DeleteGroup(groupId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [GroupAuthorize(GroupRole.Admin)]
        [HttpPut("{groupId:long}")]
        public async Task<ActionResult<GroupDTO>> UpdateGroup(long groupId,[FromBody] GroupDTO groupDTO)
        {
            var result = await _groupService.UpdateGroup(new Group(groupDTO));
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return NoContent();
        }
    }
}
