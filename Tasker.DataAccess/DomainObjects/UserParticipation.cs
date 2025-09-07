using System;
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

    public UserParticipation()
    {
    }
}
