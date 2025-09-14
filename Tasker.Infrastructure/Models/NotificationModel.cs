using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Infrastructure;

public class NotificationModel
{
    [Key]
    public long NotificationId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
    public string Type { get; set; } = String.Empty;
}
