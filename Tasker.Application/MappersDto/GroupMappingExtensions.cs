using System;
using Tasker.Domain;

namespace Tasker.Application;

public static partial class GroupMappingExtensions
{
    public static Group? ToDomainObject(this GroupDTO dto)
    {
        if (dto == null) return null;

        Group groupToReturn = new();

        groupToReturn.GroupId = dto.GroupId;
        groupToReturn.UserParticipations = dto.Participants.Select(p => new UserParticipation
        {
            GroupId = dto.GroupId,
            User = p.ToDomainObject(),
            UserId = p.UserIdentity,
        }).ToList();
        groupToReturn.Assignments = dto.Assignments.Select(a => a.ToDomainObject()).ToList();

        return groupToReturn;
    }

    public static GroupDTO? ToDto(this Group domain)
    {
        if (domain == null) return null;

        return new GroupDTO
        {
            GroupId = domain.GroupId,
            Participants = domain.UserParticipations.Select(up => up.User.ToDto()).ToList(),
            Assignments = domain.Assignments.Select(a => a.ToDto()).ToList()
        };
    }
}
