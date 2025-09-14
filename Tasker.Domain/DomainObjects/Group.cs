namespace Tasker.Domain;

public class Group
{
    public long GroupId { get; set; }
    public string Name { get; set; } = String.Empty;

    public List<UserParticipation> UserParticipations = new();
    public List<Assignment> Assignments = new();

    public int GetNumberOfUncompletedAssignments()
    {
        int retval = Assignments.Where(a => !a.IsCompleted).Count();
        return retval;
    }
}
