using Microsoft.AspNetCore.Mvc;
using Tasker.Application;
using Tasker.Enums;

namespace Tasker.API.Controllers
{
    [Route("api/groups/{groupId:long}/userassignments")]
    [ApiController]
    public class UserAssignmentController : ControllerBase
    {
        private readonly IAssignmentsService _assignmentsService;

        public UserAssignmentController(IAssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpPost]
        public async Task<ActionResult<UserAssignmentDTO>> AssignTaskToUser([FromBody] UserAssignmentDTO userAssignment)
        {
            if (userAssignment is null) return BadRequest("User assignment is required.");
            var result = await _assignmentsService.AssignTaskToUser(userAssignment.ToDomainObject()!);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(AssignTaskToUser), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpDelete]
        public async Task<ActionResult<UserAssignmentDTO>> UnassignTaskFromUser([FromBody] UserAssignmentDTO userAssignment)
        {
            if (userAssignment is null) return BadRequest("User assignment is required.");

            var result = await _assignmentsService.UnassignTaskFromUser(userAssignment.ToDomainObject()!);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(UnassignTaskFromUser), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
