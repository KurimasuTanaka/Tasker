using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.AssignmentsService;
using Tasker.DataAccess;

namespace Tasker.API.Controllers
{
    [Route("api/[controller]/groups/{groupId:long}/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly IAssignmentsService _assignmentsService;

        public AssignmentController(IAssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAssignment(long groupId, [FromBody] Assignment assignment)
        {
            var result = await _assignmentsService.CreateAssignment(groupId, assignment);
            if (result.IsSuccess)
            {
                return CreatedAtAction("CreateAssignment", result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult<List<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken)
        {
            var result = await _assignmentsService.GetAllAssignments(groupId, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("{assignmentId:long}")]
        public async Task<ActionResult<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken)
        {
            var result = await _assignmentsService.GetAssignment(groupId, assignmentId, cancellationToken);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{assignmentId:long}")]
        public async Task<IActionResult> DeleteAssignment(long groupId, long assignmentId)
        {
            var result = await _assignmentsService.DeleteAssignment(groupId, assignmentId);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{assignmentId:long}")]
        public async Task<IActionResult> UpdateAssignment(long groupId, long assignmentId, [FromBody] Assignment updatedAssignment)
        {
            var result = await _assignmentsService.UpdateAssignment(groupId, assignmentId, updatedAssignment);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }

    }
}
