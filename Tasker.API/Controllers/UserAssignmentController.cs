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
        private readonly ILogger<UserAssignmentController> _logger;

        public UserAssignmentController(IAssignmentsService assignmentsService, ILogger<UserAssignmentController> logger)
        {
            _assignmentsService = assignmentsService;
            _logger = logger;
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpPost]
        public async Task<ActionResult<UserAssignmentDTO>> AssignTaskToUser([FromBody] UserAssignmentDTO userAssignment)
        {
            _logger.LogTrace($"Requested assignment of task {userAssignment.AssignmentId} to user {userAssignment.UserId ?? "ID WAS NULL"}");

            if (userAssignment is null)
            {
                _logger.LogWarning("User assignment failed: user assignment is null");
                return BadRequest("User assignment is required.");
            }
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
            _logger.LogTrace($"Requested unassignment of task {userAssignment.AssignmentId} from user {userAssignment.UserId ?? "ID WAS NULL"}");

            if (userAssignment is null)
            {
                _logger.LogWarning("User unassignment failed: user assignment is null");
                return BadRequest("User assignment is required.");
            }

            var result = await _assignmentsService.UnassignTaskFromUser(userAssignment.ToDomainObject()!);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(UnassignTaskFromUser), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
