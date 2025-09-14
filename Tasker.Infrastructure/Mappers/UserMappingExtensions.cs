using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class UserMappingExtensions
{
    public static User? ToDomain(this UserModel entity)
    {
        if (entity == null) return null;

        return new User
        {
            UserIdentity = entity.UserIdentity,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
        };
    }

    public static UserModel? ToModel(this User domain)
    {
        if (domain == null) return null;

        return new UserModel
        {
            UserIdentity = domain.UserIdentity,
            FirstName = domain.FirstName,
            LastName = domain.LastName,
        };
    }
}
