using System;

namespace Tasker.DataAccess.DataTransferObjects;

public class UserDTO
{
    public string UserIdentity { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;

    public UserDTO(User user)
    {
        UserIdentity = user.UserIdentity;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }

    public User ToDomainObject()
    {
        User user = new();
        user.FirstName = FirstName;
        user.LastName = LastName;
        user.UserIdentity = UserIdentity;

        return user;
    }
}
