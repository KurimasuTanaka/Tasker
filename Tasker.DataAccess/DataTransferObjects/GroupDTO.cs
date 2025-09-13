using System;

namespace Tasker.DataAccess.DataTransferObjects;

public class GroupDTO
{
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<UserDTO> Participants { get; set; } = new();
    public List<AssignmentDTO> Assignments { get; set; } = new();

    public GroupDTO(Group group)
    {
        Assignments = group.Assignments.Select(a => new AssignmentDTO(a)).ToList();
        Participants = group.UserParticipations.Select(up => new UserDTO(up.User)).ToList();
    }

    public Group ToDomainObject()
    {
        Group groupToReturn = new();

        groupToReturn.GroupId = this.GroupId;
        groupToReturn.UserParticipations = this.Participants.Select(p => new UserParticipation
        {
            GroupId = this.GroupId,
            User = p.ToDomainObject(),
            UserId = p.UserIdentity,
        }).ToList();
        groupToReturn.Assignments = this.Assignments.Select(a => a.ToDomainObject()).ToList();

        return groupToReturn;
    }
}

