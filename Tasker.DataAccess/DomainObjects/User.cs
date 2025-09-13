using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class User
{
    public string UserIdentity { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public List<UserParticipation> UserParticipations = new();
    public User() { }

    public User(UserModel model)
    {
        FirstName = model.FirstName;
        LastName = model.LastName;
        UserIdentity = model.UserIdentity;
        UserParticipations = model.Participations.Select(up => new UserParticipation(up)).ToList();
    }
}
