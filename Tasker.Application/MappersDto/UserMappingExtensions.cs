using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class UserMappingExtensions
{
    public static User ToDomainObject(this UserDTO dto)
    {
        if (dto == null) return new User();

        return new User
        {
            UserIdentity = dto.UserIdentity,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
    }
    public static UserDTO ToDto(this User domain)
    {
        if (domain == null) return new UserDTO();

        return new UserDTO
        {
            UserIdentity = domain.UserIdentity,
            FirstName = domain.FirstName,
            LastName = domain.LastName
        };
    }
}
