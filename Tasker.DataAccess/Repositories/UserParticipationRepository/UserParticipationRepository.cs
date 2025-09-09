using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserParticipationRepository : IUserParticipationRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public UserParticipationRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<UserParticipation> AddAsync(UserParticipation entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        await _context.UserParticipations.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).ReloadAsync();
        return entity;
    }

    // Read operations
    public async Task<UserParticipation?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var userParticipationModel = await _context.UserParticipations.FindAsync(id, cancellationToken);
        if(userParticipationModel == null) return null; 
        else return new UserParticipation(userParticipationModel);
    }

    public async Task<IEnumerable<UserParticipation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.UserParticipations.Select(up => new UserParticipation(up)).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<UserParticipation> UpdateAsync(UserParticipation entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        _context.UserParticipations.Update(entity);
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

        _context.UserParticipations.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
