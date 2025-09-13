using System;

namespace Tasker.DataAccess.DataTransferObjects;

public class UserAssignmentDTO
{
    public string UserId { get; set; } = string.Empty;
    public long AssignmentId { get; set; }

    public UserAssignmentDTO(UserAssignment userAssignment)
    {
        UserId = userAssignment.UserId;
        AssignmentId = userAssignment.AssignmentId;
    }

    public UserAssignment ToDomainObject()
    {
        UserAssignment userAssignment = new();
        userAssignment.AssignmentId = AssignmentId;
        userAssignment.UserId = UserId;

        return userAssignment;
    }
}
