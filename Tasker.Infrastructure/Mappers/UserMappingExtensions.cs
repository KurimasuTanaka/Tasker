using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class UserMappingExtensions
{
    public static User ToDomain(this UserModel entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new User
        {
            UserIdentity = entity.UserIdentity,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
        };
    }

    public static UserModel ToModel(this User domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        return new UserModel
        {
            UserIdentity = domain.UserIdentity,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
        };
    }
}
