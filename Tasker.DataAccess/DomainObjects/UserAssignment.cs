using System;
using Tasker.Database;

namespace Tasker.DataAccess.DomainObjects;

public class UserAssignment : UserAssignmentModel
{

    public UserAssignment(UserAssignmentModel model)
    {
        UserId = model.UserId;
        AssignmentId = model.AssignmentId;
    }

}
