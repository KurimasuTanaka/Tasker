using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Database;

public class GroupModel
{
    [Key]
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<UserParticipationModel> Participants { get; set; } = new();
}

