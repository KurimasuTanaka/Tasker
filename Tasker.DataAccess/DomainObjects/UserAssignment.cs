using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class UserAssignment
{
    public string UserId { get; set; } = String.Empty;
    public User? User { get; set; }
    public long AssignmentId { get; set; }
    public Assignment? Assignment { get; set; } = null;

    public UserAssignment(UserAssignmentModel model)
    {
        UserId = model.UserId;
        AssignmentId = model.AssignmentId;
    }

    public UserAssignment() { }

}
