using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Group : GroupModel
{

    public Group(GroupModel model)
    {
        this.GroupId = model.GroupId;
        this.Name = model.Name;
        this.Participants = model.Participants;
        this.Assignments = model.Assignments;
    }

    public Group(GroupDTO groupDTO)
    {

        this.GroupId = groupDTO.GroupId;
        this.Name = groupDTO.Name;
        this.Participants = groupDTO.Participants.Select(p => new UserParticipationModel
        {
            UserParticipationId = p.UserParticipationId,
            Role = p.Role,
            User = p.User,
            UserId = p.UserId,
            GroupId = p.GroupId,
            Group = p.Group
        }).ToList();
        this.Assignments = groupDTO.Assignments.Select(a => new AssignmentModel
        {
            AssignmentId = a.AssignmentId,
            Title = a.Title,
            GroupId = a.GroupId,
            Description = a.Description,
            IsCompleted = a.IsCompleted
        }).ToList();
    }

    public GroupDTO ToDTO()
    {
        return new GroupDTO
        {
            GroupId = this.GroupId,
            Name = this.Name,
            Participants = this.Participants.Select(p => new UserParticipation(p)).ToList(),
            Assignments = this.Assignments.Select(a => new Assignment(a)).ToList()
        };
    }

    public Group()
    {
    }
}
