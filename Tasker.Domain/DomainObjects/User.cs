
namespace Tasker.Domain;

public class User
{
    public string UserIdentity = String.Empty;
    public string FirstName = String.Empty;
    public string LastName = String.Empty;
    public List<UserParticipation> UserParticipations = new();
    public User() { }

}
