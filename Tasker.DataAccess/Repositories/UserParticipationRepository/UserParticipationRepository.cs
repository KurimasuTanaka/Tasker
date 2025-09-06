using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserParticipationRepository : IUserParticipationRepository
{
    private readonly IdentityContext _context;

    public UserParticipationRepository(IdentityContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<UserParticipation> AddAsync(UserParticipation entity)
    {
        await _context.UserParticipations.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<UserParticipation?> GetAsync(object id)
    {
        var userParticipationModel = await _context.UserParticipations.FindAsync(id);
        if(userParticipationModel == null) return null; 
        else return new UserParticipation(userParticipationModel);
    }

    public async Task<IEnumerable<UserParticipation>> GetAllAsync()
    {
        return await _context.UserParticipations.Select(up => new UserParticipation(up)).ToListAsync();
    }

    // Update operations
    public async Task<UserParticipation> UpdateAsync(UserParticipation entity)
    {
        _context.UserParticipations.Update(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(object id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.UserParticipations.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
