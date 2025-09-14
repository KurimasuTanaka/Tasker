using System;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public static partial class NotificationMappingExtensions
{
    public static Notification ToDomain(this NotificationModel entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new Notification
        {
        };
    }

    public static NotificationModel ToModel(this Notification domain)
    {
        if (domain == null) throw new ArgumentNullException(nameof(domain));

        return new NotificationModel
        {
        };
    }
}
