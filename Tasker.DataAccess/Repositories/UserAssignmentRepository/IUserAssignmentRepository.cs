using System;

namespace Tasker.DataAccess.Repositories;

public interface IUserAssignmentRepository : IRepository<UserAssignment, (string UserId, long AssignmentId)>
{

}
