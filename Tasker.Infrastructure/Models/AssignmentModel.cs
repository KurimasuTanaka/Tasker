using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Infrastructure;

public class AssignmentModel
{
    [Key]
    public long AssignmentId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsCompleted { get; set; }
    [ForeignKey(nameof(GroupModel))]
    public long GroupId { get; set; }

    public List<UserAssignmentModel> Participants { get; set; } = new();
}
