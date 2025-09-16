using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class GroupMappingExtensions
{
    public static Group? ToDomain(this GroupModel entity)
    {
        if (entity == null) return null;

        return new Group
        {
            GroupId = entity.GroupId,
            Name = entity.Name,
            Assignments = entity.Assignments
                .Select(a => a.ToDomain())
                .Where(a => a != null).Select(a => a!)
                .ToList(),
            UserParticipations = entity.UserParticipations
                .Select(up => up.ToDomain())
                .Where(up => up != null).Select(up => up!)
                .ToList()
        };
    }

    public static GroupModel? ToModel(this Group domain)
    {
        if (domain == null) return null;

        return new GroupModel
        {
            GroupId = domain.GroupId,
            Name = domain.Name,
            Assignments = domain.Assignments.
                Select(a => a.ToModel()).
                Where(a => a != null).Select(x => x!).
                ToList(),
            UserParticipations = domain.UserParticipations.
                Select(ug => ug.ToModel()).
                Where(ug => ug != null).Select(x => x!).
                ToList()
        };
    }
}
