using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tasker.Enums;

namespace Tasker.API;

public class GroupAuthorizeFilter : IAsyncActionFilter
{
    private readonly GroupRole _requiredRole;
    private readonly IAuthorizationService _authorizationService;
    private readonly ILogger<GroupAuthorizeFilter> _logger;

    public GroupAuthorizeFilter(GroupRole groupRole, IAuthorizationService authorizationService, ILogger<GroupAuthorizeFilter> logger)
    {
        _requiredRole = groupRole;
        _authorizationService = authorizationService;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("groupId", out var groupIdObj) ||
            !long.TryParse(groupIdObj?.ToString(), out var groupId))
        {
            context.Result = new BadRequestObjectResult("Missing or invalid groupId in route.");
            return;
        }

        _logger.LogDebug($"Authorizing access to group {groupId} with required role {_requiredRole}");

        var authResult = await _authorizationService.AuthorizeAsync(context.HttpContext.User, groupId, new PermisionRequirement(_requiredRole));
        if (!authResult.Succeeded)
        {
            _logger.LogDebug($"Authorization failed for group {groupId} with required role {_requiredRole}");
            context.Result = new ForbidResult();
        }

        _logger.LogDebug($"Authorization succeeded for group {groupId} with required role {_requiredRole}");
        await next();
    }
}
