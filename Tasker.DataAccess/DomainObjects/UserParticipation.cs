using Tasker.Database;

namespace Tasker.DataAccess;

public class UserParticipation
{
    public long UserParticipationId { get; set; }
    public GroupRole Role { get; set; }
    public string UserId { get; set; } = String.Empty;
    public User? User { get; set; }
    public long GroupId { get; set; }


    public UserParticipation(UserParticipationModel model)
    {
        UserParticipationId = model.UserParticipationId;
        Role = model.Role;
        UserId = model.UserId;
        GroupId = model.GroupId;
    }
    public UserParticipation()
    {
    }
}
