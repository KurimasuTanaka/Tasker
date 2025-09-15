using System;
using Tasker.Domain;
using Tasker.Enums;

namespace Tasker.Application;

public class UserDTO
{
    public string UserIdentity { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public GroupRole Role { get; set; } = GroupRole.User;
}
