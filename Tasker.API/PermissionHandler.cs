using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Tasker.DataAccess;
using Tasker.DataAccess.Repositories;

namespace Tasker.API;

public class PermissionHandler : AuthorizationHandler<PermisionRequirement, long>
{
    IUserParticipationRepository _userParticipationRepository;

    public PermissionHandler(IUserParticipationRepository userParticipationRepository)
    {
        _userParticipationRepository = userParticipationRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisionRequirement requirement, long groupId)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            context.Fail();
            return;
        }

        // Ищем запись о членстве
        UserParticipation? userParticipation = await _userParticipationRepository.GetUserParticipationAsyc(userId, groupId);

        if (userParticipation == null)
        {
            context.Fail();
            return;
        }

        if (userParticipation.Role >= requirement.minimumRole)
        {
            context.Succeed(requirement);
        }
    }
}
