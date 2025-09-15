using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class UserAssignmentMappingExtensions
{
    public static UserAssignment? ToDomainObject(this UserAssignmentDTO dto)
    {
        if (dto == null) return new UserAssignment();

        return new UserAssignment
        {
            UserId = dto.UserId,
            AssignmentId = dto.AssignmentId
        };
    }

    public static UserAssignmentDTO? ToDto(this UserAssignment domain)
    {
        if (domain == null) return new UserAssignmentDTO();

        return new UserAssignmentDTO
        {
            UserId = domain.UserId,
            AssignmentId = domain.AssignmentId,
            
        };
    }
}
