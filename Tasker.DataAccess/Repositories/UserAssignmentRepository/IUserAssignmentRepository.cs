using System;
using Tasker.DataAccess.DomainObjects;

namespace Tasker.DataAccess.Repositories.UserAssignmentRepository;

public interface IUserAssignmentRepository : IRepository<UserAssignment, (long UserId, long AssignmentId)>
{

}
