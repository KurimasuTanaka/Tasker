using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Tasker.Database;

namespace Tasker.API;

public class PermisionRequirement : IAuthorizationRequirement
{
    public GroupRole minimumRole;

    public PermisionRequirement(GroupRole minimumRole)
    {
        this.minimumRole = minimumRole;
    }

}
