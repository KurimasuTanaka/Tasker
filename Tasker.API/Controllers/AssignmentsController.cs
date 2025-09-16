using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.Application;
using Tasker.Domain;
using Tasker.Enums;
using Tasker.Infrastructure;

namespace Tasker.API.Controllers
{
    [Route("api/groups/{groupId:long}/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly ILogger<AssignmentController> _logger;

        public AssignmentController(IAssignmentsService assignmentsService, ILogger<AssignmentController> logger)
        {
            _assignmentsService = assignmentsService;
            _logger = logger;
        }

        [HttpPost]
        [GroupAuthorize(GroupRole.Manager)]
        public async Task<ActionResult<AssignmentDTO>> CreateAssignment(long groupId, [FromBody] AssignmentDTO assignment)
        {
            _logger.LogTrace($"Requested creation of assignment in group {groupId}");

            if (assignment is null)
            {
                _logger.LogWarning("Assignment creation failed: assignment is null");
                return BadRequest("Assignment is required.");
            }   
                

            var result = await _assignmentsService.CreateAssignment(groupId, assignment.ToDomainObject()!);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(CreateAssignment), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        [GroupAuthorize(GroupRole.User)]
        public async Task<ActionResult<List<AssignmentDTO>>> GetAllAssignments(long groupId, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Requested all assignments in group {groupId}");

            var result = await _assignmentsService.GetAllAssignments(groupId, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value.Select(a => a.ToDto()).ToList());
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{assignmentId:long}")]
        [GroupAuthorize(GroupRole.User)]
        public async Task<ActionResult<AssignmentDTO>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken)
        {
            _logger.LogTrace($"Requested assignment {assignmentId} in group {groupId}");

            var result = await _assignmentsService.GetAssignment(groupId, assignmentId, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{assignmentId:long}")]
        [GroupAuthorize(GroupRole.Manager)]
        public async Task<IActionResult> DeleteAssignment(long groupId, long assignmentId)
        {
            _logger.LogTrace($"Requested deletion of assignment {assignmentId} in group {groupId}");

            var result = await _assignmentsService.DeleteAssignment(groupId, assignmentId);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{assignmentId:long}")]
        [GroupAuthorize(GroupRole.User)]
        public async Task<ActionResult<AssignmentDTO>> UpdateAssignment(long groupId, long assignmentId, [FromBody] AssignmentDTO updatedAssignment)
        {
            _logger.LogTrace($"Requested update of assignment {assignmentId} in group {groupId}");

            if (updatedAssignment is null)
            {
                _logger.LogWarning("Assignment update failed: assignment is null");
                return BadRequest("Assignment is required.");
            }

            var result = await _assignmentsService.UpdateAssignment(groupId, assignmentId, updatedAssignment.ToDomainObject()!);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetAssignment), result.Value.ToDto());
            }
            return BadRequest(result.ErrorMessage);
        }

    }
}
