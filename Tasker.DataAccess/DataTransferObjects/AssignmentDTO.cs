using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class AssignmentDTO
{
    public long AssignmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public long GroupId { get; set; }
    //public List<UserAssignment> Participants { get; set; } = new List<UserAssignment>();
}
