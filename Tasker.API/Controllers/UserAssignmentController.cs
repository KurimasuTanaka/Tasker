using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.AssignmentsService;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;

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

        [HttpPost]
        public async Task<IActionResult> AssignTaskToUser([FromBody] UserAssignmentDTO userAssignment)
        {
            var result = await _assignmentsService.AssignTaskToUser(userAssignment);
            if (result.IsSuccess)
            {
                return CreatedAtAction("GetUserAssignment", result.Value);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete]
        public async Task<IActionResult> UnassignTaskFromUser([FromBody] UserAssignmentDTO userAssignment)
        {
            var result = await _assignmentsService.UnassignTaskFromUser(userAssignment);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
