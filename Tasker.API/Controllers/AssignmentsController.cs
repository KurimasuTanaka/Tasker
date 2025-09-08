using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.DataAccess;

namespace Tasker.API.Controllers
{
    [Route("api/[controller]/groups/{groupId:long}/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask(long groupId, [FromBody] Assignment userId)
        {
            // Implementation for adding a member to a group
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken)
        {
            // Implementation for retrieving all group members
            return Ok(new List<Assignment>());
        }

        [HttpGet("{assignmentId:long}")]
        public async Task<ActionResult<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken)
        {
            // Implementation for retrieving a specific assignment
            return Ok();
        }

        [HttpDelete("{assignmentId:long}")]
        public async Task<IActionResult> DeleteAssignment(long groupId, long assignmentId)
        {
            // Implementation for removing a member from a group
            return NoContent();
        }

        [HttpPut("{assignmentId:long}")]
        public async Task<IActionResult> UpdateAssignment(long groupId, long assignmentId, [FromBody] Assignment updatedAssignment)
        {
            // Implementation for updating a member's role in a group
            return NoContent();
        }

    }
}
