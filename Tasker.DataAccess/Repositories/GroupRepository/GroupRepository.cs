using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public GroupRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<Group> AddAsync(Group entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<Group?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var groupModel = await _context.Groups.Include(g => g.Participants).FirstOrDefaultAsync(g => g.GroupId == id, cancellationToken);
        if (groupModel == null) return null;
        else return new Group(groupModel);
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Groups.Select(g => new Group(g)).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<Group> UpdateAsync(Group entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Groups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Group>> GetAllAsync(string userId, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.UserParticipations.Include(up => up.Group).ThenInclude(up => up.Participants)
            .Where(up => up.User.UserIdentity == userId)
            .Select(up => new Group(up.Group!))
            .ToListAsync(cancellationToken);
    }
}
