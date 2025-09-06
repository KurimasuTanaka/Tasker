using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityContext _context;
    public UserRepository(IdentityContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<User> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<User?> GetAsync(string id)
    {
        var userModel = await _context.Users.FindAsync(id);
        if (userModel == null) return null; 
        else return new User(userModel);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.Select(u => new User(u)).ToListAsync();
    }

    // Update operations
    public async Task<User> UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
