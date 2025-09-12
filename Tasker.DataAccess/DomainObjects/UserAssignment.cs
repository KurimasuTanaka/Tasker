using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class UserAssignment : UserAssignmentModel
{

    public UserAssignment(UserAssignmentModel model)
    {
        UserId = model.UserId;
        AssignmentId = model.AssignmentId;
        User = model.User;
        Assignment = model.Assignment;
    }

    public UserAssignment() { }

}
