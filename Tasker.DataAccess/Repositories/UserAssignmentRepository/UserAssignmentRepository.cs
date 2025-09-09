using System;
using Microsoft.EntityFrameworkCore;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserAssignmentRepository : IUserAssignmentRepository
{

    private readonly IDbContextFactory<TaskerContext> _contextFactory;

    public UserAssignmentRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<UserAssignment> AddAsync(UserAssignment entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        await _context.UserAssignments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
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
        return await _context.UserAssignments.Select(ua => new UserAssignment(ua)).ToListAsync(cancellationToken);
    }

    public async Task<UserAssignment?> GetAsync((string UserId, long AssignmentId) id, CancellationToken cancellationToken = default)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        var userAssignmentModel = await _context.UserAssignments.FindAsync(id.UserId, id.AssignmentId);
        if(userAssignmentModel == null) return null; 
        else return new UserAssignment(userAssignmentModel);    
    }

    public async Task<UserAssignment> UpdateAsync(UserAssignment entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
         
        _context.UserAssignments.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
