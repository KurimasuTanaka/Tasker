using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Database;

public class AssignmentModel
{
    [Key]
    public long AssignmentId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsCompleted { get; set; }

    public long GroupId { get; set; }
    public GroupModel Group { get; set; } = null!;

    public List<UserAssignmentModel> Participants { get; set; } = new();
}
