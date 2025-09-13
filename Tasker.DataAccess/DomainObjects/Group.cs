using System;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Group
{
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;

    public List<UserParticipation> UserParticipations = new();
    public List<Assignment> Assignments = new();

    public Group(GroupModel model)
    {
        this.GroupId = model.GroupId;
        this.Name = model.Name;
        this.UserParticipations = model.Participants.Select(p => new UserParticipation(p)).ToList();
    }

    public Group()
    {
    }

    public int GetNumberOfUncompletedAssignments()
    {
        int retval = Assignments.Where(a => !a.IsCompleted).Count();
        return retval;
    }
}
