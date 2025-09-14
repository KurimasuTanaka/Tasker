using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static class UserAssignmentMappingExtensions
{
    public static UserAssignment ToDomain(this UserAssignmentModel entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new UserAssignment
        {
            UserId = entity.UserId,
            AssignmentId = entity.AssignmentId,
            User = entity.User?.ToDomain(),
        };
    }

    public static UserAssignmentModel ToModel(this UserAssignment domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        return new UserAssignmentModel
        {
            UserId = domain.UserId,
            AssignmentId = domain.AssignmentId,
            User = domain.User?.ToModel(),
        };
    }
}
