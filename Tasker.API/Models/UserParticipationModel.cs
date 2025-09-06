using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.API.DB;

public class UserParticipationModel
{
    [Key]
    public long UserParticipationId { get; set; }
    public string Role { get; set; } = String.Empty;
    public UserModel User { get; set; } = null!;
    public GroupModel Group { get; set; } = null!;
}
