using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class UserParticipation : UserParticipationModel
{
    public UserParticipation(UserParticipationModel model)
    {
        this.UserParticipationId = model.UserParticipationId;
        this.Role = model.Role;
        this.User = model.User;
        this.UserId = model.UserId;
        this.GroupId = model.GroupId;
        this.Group = model.Group;
    }

    public UserParticipation(UserParticipationDTO userParticipationDTO)
    {
        this.UserParticipationId = userParticipationDTO.UserParticipationId;
        this.Role = userParticipationDTO.Role;
        this.User = userParticipationDTO.User;
        this.UserId = userParticipationDTO.UserId;
        this.GroupId = userParticipationDTO.GroupId;
        this.Group = userParticipationDTO.Group;
    }

    public UserParticipation()
    {
    }


    public UserParticipationDTO ToDTO()
    {
        return new UserParticipationDTO
        {
            UserParticipationId = this.UserParticipationId,
            Role = Role,
            User = new User(User),
            UserId = UserId,
            GroupId = GroupId,
            Group = new Group(Group)
        };
    }
}
