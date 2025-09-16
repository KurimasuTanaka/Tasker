

namespace Tasker.Domain;

public class Assignment
{
	public long AssignmentId;
	public string Title = String.Empty;
	public string Description = String.Empty;
	public bool IsCompleted = false;
	public long GroupId;
	public List<UserAssignment> UserAssignments = new();

	public Assignment() { }

	public bool HasThisUserAssigned(string userId)
	{
		return UserAssignments.Select(p => p.User.UserIdentity).
				Contains(userId);
	}
}
