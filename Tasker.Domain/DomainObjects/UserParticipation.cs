using Tasker.Enums;

namespace Tasker.Domain;

public class UserParticipation
{
    public long UserParticipationId;
    public GroupRole Role;
    public string UserId = String.Empty;
    public User? User;
    public long GroupId;

    public UserParticipation()
    {
    }
}
