using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public class NotificationRepository : INotificationRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public NotificationRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<Notification?> AddAsync(Notification entity)
    {
        if (entity == null) return null;

        using var _context = await _contextFactory.CreateDbContextAsync();
        NotificationModel notificationModel = entity.ToModel();

        await _context.Notifications.AddAsync(notificationModel);
        await _context.SaveChangesAsync();
        await _context.Entry(notificationModel).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<Notification?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var notificationModel = await _context.Notifications.FindAsync(id, cancellationToken);
        if (notificationModel == null) return null;
        else return notificationModel.ToDomain();
    }

    public async Task<IEnumerable<Notification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Notifications.Select(n => n.ToDomain()).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<Notification?> UpdateAsync(Notification entity)
    {
        if (entity == null) return null;

        using var _context = await _contextFactory.CreateDbContextAsync();
        NotificationModel notificationModel = entity.ToModel();

        _context.Notifications.Update(notificationModel);
        await _context.SaveChangesAsync();
        await _context.Entry(notificationModel).ReloadAsync();

        return notificationModel.ToDomain();
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Notifications.Remove(entity.ToModel());
        await _context.SaveChangesAsync();
        return true;
    }
}
