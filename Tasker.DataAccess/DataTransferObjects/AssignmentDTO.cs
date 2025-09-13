using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.DataAccess;

public class AssignmentDTO
{
    public long AssignmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public long GroupId { get; set; }
    public List<UserAssignmentDTO> UserAssignments { get; set; } = new();

    public AssignmentDTO(Assignment assignment)
    {
        AssignmentId = assignment.AssignmentId;
        Title = assignment.Title;
        Description = assignment.Description;
        IsCompleted = assignment.IsCompleted;
        GroupId = assignment.GroupId;
        UserAssignments = assignment.UserAssignments.Select(ua => new UserAssignmentDTO(ua)).ToList();
    }

    public Assignment ToDomainObject()
    {
        Assignment assignment = new();
        assignment.AssignmentId = AssignmentId;
        assignment.Title = Title;
        assignment.Description = Description;
        assignment.IsCompleted = IsCompleted;
        assignment.GroupId = GroupId;
        assignment.UserAssignments = UserAssignments.Select(ua => ua.ToDomainObject()).ToList();

        return assignment;   
    }
}
