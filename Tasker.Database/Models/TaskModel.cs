using System;
using System.ComponentModel.DataAnnotations;

namespace Tasker.Database;

public class TaskModel
{
    [Key]
    public long TaskId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public string Priority { get; set; } = String.Empty;
}
