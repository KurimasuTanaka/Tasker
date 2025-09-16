using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class AssignmentMappingExtensions
{
    public static Assignment? ToDomainObject(this AssignmentDTO dto)
    {
        if (dto == null) return null;
        return new Assignment
        {
            AssignmentId = dto.AssignmentId,
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            GroupId = dto.GroupId,
            //Convert the list of users assigned to the assignment to the many-to-many relationship UserAssignments
            UserAssignments = dto.Users.Select(u => new UserAssignment
            {
                UserId = u.UserIdentity,
                User = u.ToDomainObject()
            }).ToList()
        };
    }

    public static AssignmentDTO? ToDto(this Assignment domain)
    {
        if (domain == null) return null;
        return new AssignmentDTO
        {
            AssignmentId = domain.AssignmentId,
            Title = domain.Title,
            Description = domain.Description,
            IsCompleted = domain.IsCompleted,
            GroupId = domain.GroupId,
            //Remove the many-to-many relationship and return only the list of users assigned to the assignment
            Users = domain.UserAssignments.
                Where(ua => ua.User != null).
                Select(ua => ua.User!.ToDto()).
                Where(u => u != null).Select(u => u!).
                ToList()
        };
    }
}
