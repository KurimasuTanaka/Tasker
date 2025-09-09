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
		this.CreatedDate = model.CreatedDate;
		this.DueDate = model.DueDate;
		this.IsCompleted = model.IsCompleted;
		this.Priority = model.Priority;
	}
}
