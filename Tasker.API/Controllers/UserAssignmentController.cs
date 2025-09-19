using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<NotificationHub> _notificationHub;

        public UserAssignmentController(IAssignmentsService assignmentsService, ILogger<UserAssignmentController> logger, IHubContext<NotificationHub> notificationHub)
        {
            _assignmentsService = assignmentsService;
            _logger = logger;
            _notificationHub = notificationHub;
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpPost]
        public async Task<ActionResult<AssignmentDTO>> AssignTaskToUser(string userId, long groupId, long assignmentId, [FromBody] object body)
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

                await _notificationHub.Clients.User(userId).SendAsync("ReceiveNotification", "You have been assigned a new task!");
                return CreatedAtAction(nameof(AssignTaskToUser), result.Value.ToDto());
            }

            return BadRequest(result.ErrorMessage);
        }

        [GroupAuthorize(GroupRole.Manager)]
        [HttpDelete]
        public async Task<ActionResult<AssignmentDTO>> UnassignTaskFromUser(string userId, long groupId, long assignmentId)
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
                await _notificationHub.Clients.User(userId).SendAsync("ReceiveNotification", "A task has been unassigned from you.");
                return Ok(result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
