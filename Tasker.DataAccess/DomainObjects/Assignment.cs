using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Assignment
{
	public long AssignmentId { get; set; }
	public string Title { get; set; } = String.Empty;
	public string Description { get; set; } = String.Empty;
	public bool IsCompleted { get; set; } = false;
	public long GroupId { get; set; }
	public List<UserAssignment> UserAssignments = new();

	public Assignment() { }

	//Model
	public Assignment(AssignmentModel model)
	{
		this.AssignmentId = model.AssignmentId;
		this.Title = model.Title;
		this.GroupId = model.GroupId;
		this.Description = model.Description;
		this.IsCompleted = model.IsCompleted;
		this.UserAssignments = model.Participants.Select(p => new UserAssignment(p)).ToList();
	}

	public bool HasThisUserAssigned(string userId)
	{
		return UserAssignments.Select(p => p.User.UserIdentity).
				Contains(userId);
	}
}
