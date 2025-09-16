using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tasker.Enums;

namespace Tasker.Infrastructure;


public class UserParticipationModel
{
    public GroupRole Role { get; set; }
    public string UserId { get; set; } = String.Empty;
    public UserModel? User { get; set; } 

    public long GroupId { get; set; }
    public GroupModel? Group { get; set; }
}
