using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasker.DataAccess;

namespace Tasker.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroups(CancellationToken cancellationToken)
        {
           // var groups = await _groupsManager.GetAllGroups(cancellationToken);
            return Ok();
        }
    }
}
