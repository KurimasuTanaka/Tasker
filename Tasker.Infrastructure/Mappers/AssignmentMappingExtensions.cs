using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class AssignmentMappingExtensions
{
    public static Assignment ToDomain(this AssignmentModel entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new Assignment
        {
            AssignmentId = entity.AssignmentId,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted,
            GroupId = entity.GroupId,
            UserAssignments = entity.Participants.Select(p => p.ToDomain()).ToList()
        };
    }

    public static AssignmentModel ToModel(this Assignment domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        return new AssignmentModel
        {
            AssignmentId = domain.AssignmentId,
            Title = domain.Title,
            Description = domain.Description,
            IsCompleted = domain.IsCompleted,
            GroupId = domain.GroupId,
            Participants = domain.UserAssignments.Select(p => p.ToModel()).ToList()
        };
    }
}
