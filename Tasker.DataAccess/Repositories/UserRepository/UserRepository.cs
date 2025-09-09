using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public UserRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<User> AddAsync(User entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<User?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var userModel = await _context.Users.Include(u => u.Participations).FirstOrDefaultAsync(u => u.UserIdentity == id, cancellationToken);
        if (userModel == null) return null; 
        else return new User(userModel);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Users.Select(u => new User(u)).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<User> UpdateAsync(User entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(string id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
