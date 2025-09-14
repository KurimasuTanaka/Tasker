using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

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
        GroupModel groupModel = entity.ToModel();

        await _context.Groups.AddAsync(groupModel);
        await _context.SaveChangesAsync();
        await _context.Entry(groupModel).ReloadAsync();
        return groupModel.ToDomain();
    }

    // Read operations
    public async Task<Group?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var groupModel = await _context.Groups.AsNoTracking().AsSplitQuery().Include(g => g.UserParticipations).Include(g => g.Assignments).ThenInclude(a => a.Participants).ThenInclude(ua => ua.User).FirstOrDefaultAsync(g => g.GroupId == id, cancellationToken);
        if (groupModel == null) return null;
        else return groupModel.ToDomain();
    }

    public async Task<IEnumerable<Group>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Groups.Include(g => g.Assignments).Select(g => g.ToDomain()).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<Group> UpdateAsync(Group entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        GroupModel groupModel = entity.ToModel();

        _context.Groups.Update(groupModel);
        await _context.SaveChangesAsync();
        await _context.Entry(groupModel).ReloadAsync();
        return groupModel.ToDomain();
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Groups.Remove(entity.ToModel());
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Group>> GetAllAsync(string userId, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();

        return await _context.Groups.Include(g => g.UserParticipations).Select(g => g).Where(g => g.UserParticipations.Any(up => up.UserId == userId)).Select(g => g.ToDomain()).ToListAsync(cancellationToken);
    }
}
