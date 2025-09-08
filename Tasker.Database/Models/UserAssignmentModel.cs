using System;

namespace Tasker.Database;

public class UserAssignmentModel
{
    public long UserId { get; set; }
    public long AssignmentId { get; set; }
    // Navigation properties
    public UserModel User { get; set; } = null!;
    public AssignmentModel Assignment { get; set; } = null!;

}
