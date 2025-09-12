using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Database;

public enum GroupRole
{
    User = 1,
    Manager = 2,
    Admin = 3
} 

public class UserParticipationModel
{
    [Key]
    public long UserParticipationId { get; set; }
    public GroupRole Role { get; set; }
    public UserModel User { get; set; } = null!;
    public string UserId { get; set; } = String.Empty;
    public GroupModel Group { get; set; } = null!;
    public long GroupId { get; set; }
}
