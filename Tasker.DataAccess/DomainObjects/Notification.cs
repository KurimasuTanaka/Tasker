using System;
using Tasker.Database;

namespace Tasker.DataAccess;

public class Notification : NotificationModel
{
    public Notification(NotificationModel model)
    {
        this.NotificationId = model.NotificationId;
        this.Message = model.Message;
        this.IsRead = model.IsRead;
        this.Title = model.Title;
        this.Type = model.Type;
    }
}
