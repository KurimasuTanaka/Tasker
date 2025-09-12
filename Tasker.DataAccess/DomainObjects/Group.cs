using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Group : GroupModel
{

    public Group(GroupModel model)
    {
        this.GroupId = model.GroupId;
        this.Name = model.Name;
        this.Participants = model.Participants;
        this.Assignments = model.Assignments;
    }
    public Group()
    {
    }
}
