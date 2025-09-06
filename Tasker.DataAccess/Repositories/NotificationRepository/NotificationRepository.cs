using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IdentityContext _context;

    public NotificationRepository(IdentityContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<Notification> AddAsync(Notification entity)
    {
        await _context.Notifications.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return new Notification(entity);
    }

    // Read operations
    public async Task<Notification?> GetAsync(object id)
    {
        var notificationModel = await _context.Notifications.FindAsync(id);
        if (notificationModel == null) return null;
        else return new Notification(notificationModel);
    }

    public async Task<IEnumerable<Notification>> GetAllAsync()
    {
        return await _context.Notifications.Select(n => new Notification(n)).ToListAsync();
    }

    // Update operations
    public async Task<Notification> UpdateAsync(Notification entity)
    {
        _context.Notifications.Update(entity);
        await _context.SaveChangesAsync();
        return new Notification(entity);
    }

    // Delete operations
    public async Task<bool> DeleteAsync(object id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Notifications.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
