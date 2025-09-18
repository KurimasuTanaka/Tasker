namespace Tasker.Domain;

public class Group
{
    public long GroupId;
    public string Name = String.Empty;

    public List<UserParticipation> UserParticipations = new();
    public List<Assignment> Assignments = new();

    public int GetNumberOfUncompletedAssignments()
    {
        int retval = Assignments.Where(a => !a.IsCompleted).Count();
        return retval;
    }

    public List<User> GetGroupMembers()
    {
        try
        {
            return UserParticipations.Select(up => up.User!).ToList();
        }
        catch (Exception)
        {
            return new List<User>();
        }
    }

    public List<Assignment> GetUserAssignments(string userId)
    {
        try
        {
            return Assignments.Where(a => a.UserAssignments.
                    Select(p => p.User!.UserIdentity).
                    Contains(userId)).ToList();
        }
        catch (Exception)
        {
            return new List<Assignment>();
        }
    }

    public Group() {}
}
