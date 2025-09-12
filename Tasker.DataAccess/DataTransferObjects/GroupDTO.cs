using System;

namespace Tasker.DataAccess.DataTransferObjects;

public class GroupDTO
{
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<UserParticipation> Participants { get; set; } = new();
    public List<Assignment> Assignments { get; set; } = new();

}
