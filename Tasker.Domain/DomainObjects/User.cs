
using System.Diagnostics.CodeAnalysis;

namespace Tasker.Domain;

public class User
{
    public string UserIdentity = String.Empty;
    public string FirstName = String.Empty;
    public string LastName = String.Empty;
    public List<UserParticipation> UserParticipations = new();
    public User() { }

}

public class UserComparison : IEqualityComparer<User>
{
    public bool Equals(User? x, User? y)
    {
        if (x == null || y == null) return false;
        return x.UserIdentity == y.UserIdentity;
    }

    public int GetHashCode([DisallowNull] User obj)
    {
        return obj.UserIdentity.GetHashCode();
    }
}