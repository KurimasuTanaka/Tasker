using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class User : UserModel
{
    public User() { }

    public User(UserModel model)
    {
        this.FirstName = model.FirstName;
        this.LastName = model.LastName;
        this.UserIdentity = model.UserIdentity;
        this.Participations = model.Participations;
    }

    public User(UserDTO dto)
    {
        this.FirstName = dto.FirstName;
        this.LastName = dto.LastName;
        this.UserIdentity = dto.UserIdentity;
        this.Participations = dto.Participations.Select(up => new UserParticipationModel
        {
            UserParticipationId = up.UserParticipationId,
            Role = up.Role,
            User = up.User,
            UserId = up.UserId,
            GroupId = up.GroupId,
            Group = up.Group
        }).ToList();
    }

    public UserDTO ToDTO()
    {
        return new UserDTO
        {
            UserIdentity = this.UserIdentity,
            FirstName = this.FirstName,
            LastName = this.LastName,
            Participations = this.Participations.Select(up => new UserParticipation(up)).ToList()
        };
    }
}
