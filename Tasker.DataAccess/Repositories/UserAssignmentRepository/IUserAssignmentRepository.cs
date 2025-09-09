using System;

namespace Tasker.DataAccess.Repositories;

public interface IUserAssignmentRepository : IRepository<UserAssignment, (long UserId, long AssignmentId)>
{

}
