using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.GroupService;
using Tasker.DataAccess;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroups(CancellationToken cancellationToken)
        {
            var groups = await _groupService.GetAllGroups(User, cancellationToken);

            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(Group group)
        {
            var createdGroup = await _groupService.CreateGroup(group, User);
            
            return CreatedAtAction(nameof(GetAllGroups), createdGroup);
        }

        [HttpGet("{groupId:long}")]
        public async Task<IActionResult> GetGroupById(long groupId, CancellationToken cancellationToken)
        {
            var result = await _groupService.GetGroupById(groupId, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost("{groupId:long}/members")]
        public async Task<IActionResult> AddMember(long groupId, [FromBody] string userId)
        {
            var result = await _groupService.AddGroupMember(groupId, userId);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }   
    }
}
