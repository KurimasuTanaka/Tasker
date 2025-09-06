using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly IdentityContext _context;

    public GroupRepository(IdentityContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<Group> AddAsync(Group entity)
    {
        await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<Group?> GetAsync(long id)
    {
        var groupModel = await _context.Groups.FindAsync(id);
        if (groupModel == null) return null; 
        else return new Group(groupModel);
    }

    public async Task<IEnumerable<Group>> GetAllAsync()
    {
        return await _context.Groups.Select(g => new Group(g)).ToListAsync();
    }

    // Update operations
    public async Task<Group> UpdateAsync(Group entity)
    {
        _context.Groups.Update(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Groups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Group>> GetAllAsync(string userId)
    {
        return await _context.UserParticipations.Include(up => up.Group)
            .Where(up => up.User.UserIdentity == userId)
            .Select(up => new Group(up.Group!))
            .ToListAsync();
    }
}
