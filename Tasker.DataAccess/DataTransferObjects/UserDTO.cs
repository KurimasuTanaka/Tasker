using System;

namespace Tasker.DataAccess.DataTransferObjects;

public class UserDTO
{
    public string UserIdentity { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public List<UserParticipation> Participations { get; set; } = new();
}
