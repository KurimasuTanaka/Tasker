using System;
using Microsoft.AspNetCore.Identity;

namespace Tasker.API.DB;

public class UserModel : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public List<UserParticipationModel> Participations { get; set; } = new();
}
