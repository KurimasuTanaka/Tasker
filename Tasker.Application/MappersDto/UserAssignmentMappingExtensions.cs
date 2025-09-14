using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class UserAssignmentMappingExtensions
{
    public static UserAssignment? ToDomainObject(this UserAssignmentDTO dto)
    {
        if (dto == null) return null;

        return new UserAssignment
        {
            UserId = dto.UserId,
            AssignmentId = dto.AssignmentId
        };
    }

    public static UserAssignmentDTO? ToDto(this UserAssignment domain)
    {
        if (domain == null) return null;

        return new UserAssignmentDTO
        {
            UserId = domain.UserId,
            AssignmentId = domain.AssignmentId,
            
        };
    }
}
