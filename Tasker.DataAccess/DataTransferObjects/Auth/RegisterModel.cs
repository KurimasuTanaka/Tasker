using System;

namespace Tasker.DataAccess.Auth;

public class RegisterModel {
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
}
