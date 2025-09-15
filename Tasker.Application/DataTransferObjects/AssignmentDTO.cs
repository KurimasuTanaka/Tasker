using System;
using Tasker.Domain;

namespace Tasker.Application;

public class AssignmentDTO
{
    public long AssignmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public long GroupId { get; set; }
    public List<UserDTO> Users { get; set; } = new();

    public AssignmentDTO() {}

}
