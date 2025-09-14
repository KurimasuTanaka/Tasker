using System;
using Microsoft.AspNetCore.Mvc;
using Tasker.Enums;

namespace Tasker.API;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class GroupAuthorizeAttribute : TypeFilterAttribute
{
    public GroupAuthorizeAttribute(GroupRole requiredRole) : base(typeof(GroupAuthorizeFilter))
    {
        Arguments = new object[] { requiredRole };
    }
}
