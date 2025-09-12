using System;
using Tasker.DataAccess.DataTransferObjects;
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

    public UserAssignment(UserAssignmentDTO dto)
    {
        UserId = dto.UserId;
        AssignmentId = dto.AssignmentId;
    }

    public UserAssignmentDTO ToDTO()
    {
        return new UserAssignmentDTO
        {
            UserId = UserId,
            AssignmentId = AssignmentId
        };
    }

    public UserAssignment() { }

}
