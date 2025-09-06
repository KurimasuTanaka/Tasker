using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Database;

public class UserParticipationModel
{
    [Key]
    public long UserParticipationId { get; set; }
    public string Role { get; set; } = String.Empty;
    public UserModel User { get; set; } = null!;
    public string UserId { get; set; } = String.Empty;
    public GroupModel Group { get; set; } = null!;
}
