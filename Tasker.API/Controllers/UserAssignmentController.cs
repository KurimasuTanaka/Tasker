using Microsoft.AspNetCore.Mvc;
using Tasker.Application;
using Tasker.Enums;

namespace Tasker.API.Controllers
{
    [Route("api/groups/{groupId:long}/user/{userId}/assignments/{assignmentId:long}")]
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
        public async Task<ActionResult<UserAssignmentDTO>> AssignTaskToUser(string userId, long groupId, long assignmentId, [FromBody] object body)
        {
            _logger.LogTrace($"Requested assignment of task {assignmentId} to user {userId ?? "ID WAS NULL"}");

            if (userId == null)
            {
                _logger.LogWarning("User unassignment failed: user ID is null");
                return BadRequest("User ID cannot be null");
            }

            var result = await _assignmentsService.AssignTaskToUser(userId, assignmentId);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(AssignTaskToUser), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpDelete]
        public async Task<IActionResult> UnassignTaskFromUser(string userId, long groupId, long assignmentId)
        {
            _logger.LogTrace($"Requested unassignment of task {assignmentId} from user {userId ?? "ID WAS NULL"}");

            if (userId == null)
            {
                _logger.LogWarning("User unassignment failed: user ID is null");
                return BadRequest("User ID cannot be null");
            }

            var result = await _assignmentsService.UnassignTaskFromUser(userId, assignmentId);
            if (result.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
