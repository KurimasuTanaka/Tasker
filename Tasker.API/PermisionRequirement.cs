using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Tasker.Enums;

namespace Tasker.API;

//Minimal required role to get access to resource.
//Admin > Manager > User
public class PermisionRequirement : IAuthorizationRequirement
{
    public GroupRole minimumRole;

    public PermisionRequirement(GroupRole minimumRole)
    {
        this.minimumRole = minimumRole;
    }

}
