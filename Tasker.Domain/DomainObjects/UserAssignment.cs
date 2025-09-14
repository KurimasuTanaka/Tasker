namespace Tasker.Domain;

public class UserAssignment
{
    public string UserId { get; set; } = String.Empty;
    public User? User { get; set; }
    public long AssignmentId { get; set; }


    public UserAssignment() { }

}
