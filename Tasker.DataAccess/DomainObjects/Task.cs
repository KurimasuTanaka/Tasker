using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Task : TaskModel
{
	public Task(TaskModel model)
	{
		this.TaskId = model.TaskId;
		this.Title = model.Title;
		this.Description = model.Description;
		this.CreatedDate = model.CreatedDate;
		this.DueDate = model.DueDate;
		this.IsCompleted = model.IsCompleted;
		this.Priority = model.Priority;
	}
}
