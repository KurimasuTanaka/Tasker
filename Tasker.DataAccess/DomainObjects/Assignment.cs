using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Assignment : AssignmentModel
{
	public Assignment() { }
	public Assignment(AssignmentModel model)
	{
		this.AssignmentId = model.AssignmentId;
		this.Title = model.Title;
		this.GroupId = model.GroupId;
		this.Description = model.Description;
		this.IsCompleted = model.IsCompleted;
	}
}
