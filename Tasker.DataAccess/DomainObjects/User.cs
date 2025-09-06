using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class User : UserModel
{
    public User(UserModel model)
    {
        this.FirstName = model.FirstName;
        this.LastName = model.LastName;
        this.UserIdentity = model.UserIdentity;
        this.Participations = model.Participations;
    }
}
