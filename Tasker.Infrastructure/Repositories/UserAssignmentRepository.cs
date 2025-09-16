using System;
using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public class UserAssignmentRepository : IUserAssignmentRepository
{

    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public UserAssignmentRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<UserAssignment?> AddAsync(UserAssignment entity)
    {
        if (entity == null) return null;


        using var _context = await _contextFactory.CreateDbContextAsync();
        UserAssignmentModel userAssignmentModel = entity.ToModel()!;

        await _context.UserAssignments.AddAsync(userAssignmentModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userAssignmentModel).ReloadAsync();

        return userAssignmentModel.ToDomain();
    }

    public async Task<bool> DeleteAsync((string UserId, long AssignmentId) id)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var entity = await _context.UserAssignments.FindAsync(id.UserId, id.AssignmentId);
        if (entity == null) return false;

        _context.UserAssignments.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<UserAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();

        return (await _context.UserAssignments.
            Select(ua => ua.ToDomain()).
            ToListAsync(cancellationToken)).
            Where(ua => ua != null).Select(ua => ua!);
    }

    public async Task<UserAssignment?> GetAsync((string UserId, long AssignmentId) id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var userAssignmentModel = await _context.UserAssignments.FindAsync(id.UserId, id.AssignmentId);
        if (userAssignmentModel == null) return null;
        else return userAssignmentModel.ToDomain();
    }

    public async Task<UserAssignment?> UpdateAsync(UserAssignment entity)
    {
        if (entity == null) return null;

        using var _context = await _contextFactory.CreateDbContextAsync();
        UserAssignmentModel userAssignmentModel = entity.ToModel()!;

        _context.UserAssignments.Update(userAssignmentModel);
        await _context.SaveChangesAsync();
        await _context.Entry(userAssignmentModel).ReloadAsync();

        return userAssignmentModel.ToDomain();
    }
}
