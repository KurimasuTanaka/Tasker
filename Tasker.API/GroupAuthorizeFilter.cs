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

    public GroupAuthorizeFilter(GroupRole groupRole, IAuthorizationService authorizationService)
    {
        _requiredRole = groupRole;
        _authorizationService = authorizationService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("groupId", out var groupIdObj) ||
            !long.TryParse(groupIdObj?.ToString(), out var groupId))
        {
            context.Result = new BadRequestObjectResult("Missing or invalid groupId in route.");
            return;
        }

        var authResult = await _authorizationService.AuthorizeAsync(context.HttpContext.User, groupId, new PermisionRequirement(_requiredRole));
        if (!authResult.Succeeded)
        {
            context.Result = new ForbidResult();
        }

        await next();
    }
}
