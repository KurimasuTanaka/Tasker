using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.AssignmentsService;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

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
            var result = await _assignmentsService.AssignTaskToUser(new UserAssignment(userAssignment));
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(AssignTaskToUser), result.Value.ToDTO());
            }
            return BadRequest(result.ErrorMessage);
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpDelete]
        public async Task<IActionResult> UnassignTaskFromUser([FromBody] UserAssignmentDTO userAssignment)
        {
            var result = await _assignmentsService.UnassignTaskFromUser(new UserAssignment(userAssignment));
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
