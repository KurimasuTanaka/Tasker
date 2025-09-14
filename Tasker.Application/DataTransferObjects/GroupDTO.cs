using System;
using Tasker.Domain;

namespace Tasker.Application;

public class GroupDTO
{
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<UserDTO> Participants { get; set; } = new();
    public List<AssignmentDTO> Assignments { get; set; } = new();

    public GroupDTO()
    {
    }
}

