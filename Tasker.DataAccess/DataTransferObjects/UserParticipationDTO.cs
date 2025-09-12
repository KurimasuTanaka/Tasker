using System;
using Tasker.Database;

namespace Tasker.DataAccess.DataTransferObjects;

public class UserParticipationDTO
{
    public long UserParticipationId { get; set; }
    public GroupRole Role { get; set; }
    public User User { get; set; } = null!;
    public string UserId { get; set; } = String.Empty;
    public Group Group { get; set; } = null!;
    public long GroupId { get; set; }

}
