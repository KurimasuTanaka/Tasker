

namespace Tasker.Domain;

public class Assignment
{
	public long AssignmentId { get; set; }
	public string Title { get; set; } = String.Empty;
	public string Description { get; set; } = String.Empty;
	public bool IsCompleted { get; set; } = false;
	public long GroupId { get; set; }
	public List<UserAssignment> UserAssignments = new();

	public Assignment() { }

	public bool HasThisUserAssigned(string userId)
	{
		return UserAssignments.Select(p => p.User.UserIdentity).
				Contains(userId);
	}
}
