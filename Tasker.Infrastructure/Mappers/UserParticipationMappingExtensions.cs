using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class UserParticipationMappingExtensions
{
    public static UserParticipation ToDomain(this UserParticipationModel entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new UserParticipation
        {
            UserParticipationId = entity.UserParticipationId,
            UserId = entity.UserId,
            GroupId = entity.GroupId,
            Role = entity.Role,
            User = entity.User?.ToDomain(),
        };
    }

    public static UserParticipationModel ToModel(this UserParticipation domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        return new UserParticipationModel
        {
            UserId = domain.UserId,
            GroupId = domain.GroupId,
            Role = domain.Role,
            User = domain.User?.ToModel()
        };
    }
}
