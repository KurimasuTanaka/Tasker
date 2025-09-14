
namespace Tasker.Domain;

public class User
{
    public string UserIdentity { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public List<UserParticipation> UserParticipations = new();
    public User() { }

}
