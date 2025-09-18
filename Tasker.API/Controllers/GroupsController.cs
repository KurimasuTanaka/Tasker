using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application;
using Tasker.Domain;
using Tasker.Enums;

namespace Tasker.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupsService groupService, UserManager<IdentityUser> userManager, ILogger<GroupsController> logger)
        {
            _groupService = groupService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<GroupDTO>>> GetAllGroups(CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Requested all groups for user {_userManager.GetUserId(User)}");

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                _logger.LogWarning("Get all groups failed: invalid user");
                return BadRequest("Invalid user");
            }

            var groups = await _groupService.GetAllGroups(userId, cancellationToken);

            return Ok(groups.Value.Select(g => g.ToDto()).ToList());
        }

        [HttpPost]
        public async Task<ActionResult<GroupDTO>> CreateGroup(GroupDTO group)
        {
            _logger.LogTrace($"Requested creation of group {group?.Name} for user {_userManager.GetUserId(User)}");

            var userId = _userManager.GetUserId(User);

            if (userId is null)
            {
                _logger.LogWarning("Group creation failed: invalid user");
                return BadRequest("Invalid user");
            }

            if (group is null)
            {
                _logger.LogWarning("Group creation failed: group is null");
                return BadRequest("Group is required.");
            }

            Result<Group> createdGroup = await _groupService.CreateGroup(group.ToDomainObject()!, userId);

            if (!createdGroup.IsSuccess) return BadRequest(createdGroup.ErrorMessage);
            return CreatedAtAction("CreateGroup", createdGroup.Value.ToDto());
        }

        [HttpGet("{groupId:long}")]
        [GroupAuthorize(GroupRole.User)]
        public async Task<ActionResult<GroupDTO>> GetGroupById(long groupId, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Requested group {groupId} for user {_userManager.GetUserId(User)}");

            var result = await _groupService.GetGroupById(groupId, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            return Ok(result.Value.ToDto());
        }


        [HttpPost("{groupId:long}/members")]
        [GroupAuthorize(GroupRole.Manager)]
        public async Task<ActionResult<GroupDTO>> AddMember(long groupId, [FromBody] string userId)
        {
            _logger.LogTrace($"Requested adding user {userId} to group {groupId} by user {_userManager.GetUserId(User)}");

            var result = await _groupService.AddGroupMember(groupId, userId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value.ToDto());
        }

        [HttpPut("{groupId:long}/members/{userId}/role")]
        [GroupAuthorize(GroupRole.Admin)]
        public async Task<IActionResult> ChangeUserRole(long groupId, string userId, [FromBody] GroupRole newRole)
        {
            _logger.LogTrace($"Requested changing role of user {userId} in group {groupId} by user {_userManager.GetUserId(User)}");

            var result = await _groupService.ChangeUserRole(groupId, userId, newRole);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok();
        }

        [GroupAuthorize(GroupRole.Admin)]
        [HttpDelete("{groupId:long}")]
        public async Task<IActionResult> DeleteGroup(long groupId)
        {
            _logger.LogTrace($"Requested deletion of group {groupId} by user {_userManager.GetUserId(User)}");

            var result = await _groupService.DeleteGroup(groupId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return NoContent();
        }

        [GroupAuthorize(GroupRole.Admin)]
        [HttpPut("{groupId:long}")]
        public async Task<ActionResult<GroupDTO>> UpdateGroup(long groupId, [FromBody] GroupDTO groupDTO)
        {
            _logger.LogTrace($"Requested update of group {groupId} by user {_userManager.GetUserId(User)}");

            if (groupDTO is null) return BadRequest("Group is required.");

            var result = await _groupService.UpdateGroup(groupDTO.ToDomainObject()!);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value.ToDto());
        }
    }
}
