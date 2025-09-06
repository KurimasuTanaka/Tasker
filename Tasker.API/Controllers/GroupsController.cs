using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.DataAccess;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserParticipationRepository _userRepository;

        private readonly UserManager<IdentityUser> _userManager;

        public GroupsController(IGroupRepository groupRepository, IUserParticipationRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroups(CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return BadRequest("Invalid user");

            var groups = await _groupRepository.GetAllAsync(userId);
            return Ok(groups);
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(Group group, CancellationToken cancellationToken)
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null) return BadRequest("Invalid user");



            Group newGroup = new Group();
            newGroup.Name = group.Name;
            newGroup.Participants.Add(new UserParticipation() { UserId = userId, Role = "Admin" });


            var createdGroup = await _groupRepository.AddAsync(newGroup);
            return CreatedAtAction(nameof(GetAllGroups), createdGroup);
        }
    }
}
