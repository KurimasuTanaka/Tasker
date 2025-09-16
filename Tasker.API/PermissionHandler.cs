using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Tasker.Domain;

namespace Tasker.API;

public class PermissionHandler : AuthorizationHandler<PermisionRequirement, long>
{
    private readonly IUserParticipationRepository _userParticipationRepository;
    private readonly ILogger<PermissionHandler> _logger;

    public PermissionHandler(IUserParticipationRepository userParticipationRepository, ILogger<PermissionHandler> logger)
    {
        _userParticipationRepository = userParticipationRepository;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisionRequirement requirement, long groupId)
    {
        _logger.LogDebug($"Handling permission requirement for group {groupId} with minimum role {requirement.minimumRole}");

        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogDebug("User ID claim not found or empty");
            context.Fail();
            return;
        }

        // Ищем запись о членстве
        UserParticipation? userParticipation = await _userParticipationRepository.GetAsync((userId, groupId));

        if (userParticipation == null)
        {
            _logger.LogDebug($"User {userId} is not a member of group {groupId}");
            context.Fail();
            return;
        }

        if (userParticipation.Role >= requirement.minimumRole)
        {
            _logger.LogDebug($"User {userId} has sufficient role {userParticipation.Role} for group {groupId}");
            context.Succeed(requirement);
        }
    }
}
