using Tasker.Enums;

namespace Tasker.Domain;

public class UserParticipation
{
    public long UserParticipationId { get; set; }
    public GroupRole Role { get; set; }
    public string UserId { get; set; } = String.Empty;
    public User? User { get; set; }
    public long GroupId { get; set; }

    public UserParticipation()
    {
    }
}
