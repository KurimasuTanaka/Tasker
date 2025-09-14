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
            Assignments = entity.Assignments.Select(a => a.ToDomain()).ToList(),
            UserParticipations = entity.UserParticipations.Select(ug => ug.ToDomain()).ToList()
        };
    }

    public static GroupModel? ToModel(this Group domain)
    {
        if (domain == null) return null;

        return new GroupModel
        {
            GroupId = domain.GroupId,
            Name = domain.Name,
            Assignments = domain.Assignments.Select(a => a.ToModel()).ToList(),
            UserParticipations = domain.UserParticipations.Select(ug => ug.ToModel()).ToList()
        };
    }
}
