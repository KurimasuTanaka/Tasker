using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public UserRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<User?> AddAsync(User entity)
    {
        if (entity == null) return null;
        
        using var _context = await _contextFactory.CreateDbContextAsync();
        UserModel userModel = entity.ToModel()!;

        await _context.Users.AddAsync(userModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userModel).ReloadAsync();
        return userModel.ToDomain();
    }

    // Read operations
    public async Task<User?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();

        var userModel = await _context.Users.FirstOrDefaultAsync(u => u.UserIdentity == id, cancellationToken);
        if (userModel == null) return null;
        else return userModel.ToDomain();
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return (await _context.Users.
            Select(u => u.ToDomain()).
            ToListAsync(cancellationToken)).
            Where(u => u != null).Select(u => u!);
            
    }

    // Update operations
    public async Task<User?> UpdateAsync(User entity)
    {
        if (entity == null) return null;

        using var _context = await _contextFactory.CreateDbContextAsync();
        UserModel userModel = entity.ToModel()!;

        _context.Users.Update(userModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userModel).ReloadAsync();
        return userModel.ToDomain();
    }

    // Delete operations
    public async Task<bool> DeleteAsync(string id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Users.Remove(entity.ToModel()!);
        await _context.SaveChangesAsync();
        return true;
    }
}
