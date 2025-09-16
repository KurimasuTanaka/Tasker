using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class AssignmentMappingExtensions
{
    public static Assignment? ToDomain(this AssignmentModel entity)
    {
        if (entity == null) return null;

        return new Assignment
        {
            AssignmentId = entity.AssignmentId,
            Title = entity.Title,
            Description = entity.Description,
            IsCompleted = entity.IsCompleted,
            GroupId = entity.GroupId,
            UserAssignments = entity.Participants.
                Select(p => p.ToDomain()).
                Where(x => x != null).Select(x => x!).
                ToList()
        };
    }

    public static AssignmentModel? ToModel(this Assignment domain)
    {
        if (domain == null) return null;

        return new AssignmentModel
        {
            AssignmentId = domain.AssignmentId,
            Title = domain.Title,
            Description = domain.Description,
            IsCompleted = domain.IsCompleted,
            GroupId = domain.GroupId,
            Participants = domain.UserAssignments.
                Select(p => p.ToModel()).
                Where(p => p != null).
                Select(x => x!).ToList()
        };
    }
}
