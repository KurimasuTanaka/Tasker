using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

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
        UserParticipationModel userParticipationModel = entity.ToModel();
        
        await _context.UserParticipations.AddAsync(userParticipationModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userParticipationModel).ReloadAsync();
        return userParticipationModel.ToDomain();
    }

    // Read operations
    public async Task<UserParticipation?> GetAsync((string userId, long groupId) id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var userParticipationModel = await _context.UserParticipations.FindAsync(id.userId, id.groupId, cancellationToken);
        if(userParticipationModel == null) return null; 
        else return userParticipationModel.ToDomain();
    }

    public async Task<IEnumerable<UserParticipation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.UserParticipations.Select(up => up.ToDomain()).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<UserParticipation> UpdateAsync(UserParticipation entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        UserParticipationModel userParticipationModel = entity.ToModel();
        
        _context.UserParticipations.Update(userParticipationModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userParticipationModel).ReloadAsync();

        return userParticipationModel.ToDomain();
    }

    // Delete operations
    public async Task<bool> DeleteAsync((string userId, long groupId) id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.UserParticipations.Remove(entity.ToModel());
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UserParticipation>> GetUserParticipationsAsyc(string userId, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.UserParticipations.Where(up => up.UserId == userId).Select(up => up.ToDomain()).ToListAsync(cancellationToken);

    }
}
