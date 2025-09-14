using System;

namespace Tasker.Domain;

public interface IUserAssignmentRepository : IRepository<UserAssignment, (string UserId, long AssignmentId)>
{

}
