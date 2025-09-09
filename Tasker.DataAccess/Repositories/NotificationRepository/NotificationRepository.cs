using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public NotificationRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<Notification> AddAsync(Notification entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();

        await _context.Notifications.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return new Notification(entity);
    }

    // Read operations
    public async Task<Notification?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var notificationModel = await _context.Notifications.FindAsync(id, cancellationToken);
        if (notificationModel == null) return null;
        else return new Notification(notificationModel);
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Notifications.Select(n => new Notification(n)).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<Notification> UpdateAsync(Notification entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        _context.Notifications.Update(entity);
        await _context.SaveChangesAsync();
        return new Notification(entity);
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Notifications.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
