using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Infrastructure;

public class GroupModel
{
    [Key]
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<UserParticipationModel> UserParticipations { get; set; } = new();
    public List<AssignmentModel> Assignments { get; set; } = new();
}

