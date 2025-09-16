using System;
using Tasker.Domain;
using Tasker.Enums;

namespace Tasker.Application;

public static partial class GroupMappingExtensions
{
    public static Group? ToDomainObject(this GroupDTO dto)
    {
        if (dto == null) return null;

        Group groupToReturn = new();

        groupToReturn.GroupId = dto.GroupId;
        groupToReturn.Name = dto.Name;
        //Convert the list of participants to the many-to-many relationship UserParticipations
        groupToReturn.UserParticipations = dto.Participants.Select(p => new UserParticipation
        {
            GroupId = dto.GroupId,
            User = p.ToDomainObject(),
            UserId = p.UserIdentity,
            Role = p.Role
        }).ToList();
        groupToReturn.Assignments = dto.Assignments.
            Select(a => a.ToDomainObject()).
            Where(a => a != null).Select(a => a!).
            ToList();

        return groupToReturn;
    }

    public static GroupDTO? ToDto(this Group domain)
    {
        if (domain == null) return null;

        return new GroupDTO
        {
            GroupId = domain.GroupId,
            Name = domain.Name,
            //Remove the many-to-many relationship and return only the list of participants with their roles
            Participants = domain.UserParticipations.Select(up =>
                {
                    UserDTO? userDTO = up.User!.ToDto();
                    if (userDTO != null) userDTO.Role = up.Role;
                    return userDTO;
                }).
                Where(u => u != null).Select(u => u!).
                ToList(),
            Assignments = domain.Assignments.Select(a => a.ToDto()).
                Where(a => a != null).Select(a => a!).
                ToList()
        };
    }
}
