using System;
using Tasker.Domain;

namespace Tasker.Application;

public class UserAssignmentDTO
{
    public string UserId { get; set; } = string.Empty;
    public long AssignmentId { get; set; }

    public UserAssignmentDTO() {}
}
