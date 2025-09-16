using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Infrastructure;

public class UserAssignmentModel
{
    public string UserId { get; set; } = String.Empty;

    public UserModel? User { get; set; } = null!;
    public long AssignmentId { get; set; }
    public AssignmentModel? Assignment { get; set; } = null!;
}
