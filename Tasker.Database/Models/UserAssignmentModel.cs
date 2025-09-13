using System;

namespace Tasker.Database;

public class UserAssignmentModel
{
    public string UserId { get; set; } = String.Empty;
    public long AssignmentId { get; set; }
    public UserModel User { get; set; } = null!;
    public AssignmentModel Assignment { get; set; } = null!;

}
