using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class AssignmentMappingExtensions
{
    public static Assignment? ToDomainObject(this AssignmentDTO dto)
    {
        if (dto == null) return new Assignment();
        return new Assignment
        {
            AssignmentId = dto.AssignmentId,
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            GroupId = dto.GroupId,
            UserAssignments = dto.UserAssignments.Select(ua => ua.ToDomainObject()).ToList()
        };
    }

    public static AssignmentDTO? ToDto(this Assignment domain)
    {
        if (domain == null) return new AssignmentDTO();
        return new AssignmentDTO
        {
            AssignmentId = domain.AssignmentId,
            Title = domain.Title,
            Description = domain.Description,
            IsCompleted = domain.IsCompleted,
            GroupId = domain.GroupId,
            UserAssignments = domain.UserAssignments.Select(ua => ua.ToDto()).ToList()
        };
    }
}
