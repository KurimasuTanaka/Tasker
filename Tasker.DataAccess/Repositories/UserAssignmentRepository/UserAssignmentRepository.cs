using System;
using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess.DomainObjects;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class UserAssignmentRepository : IUserAssignmentRepository
{

    private readonly TaskerContext _context;

    public UserAssignmentRepository(TaskerContext context)
    {
        _context = context;
    }

    public async Task<UserAssignment> AddAsync(UserAssignment entity)
    {
        await _context.UserAssignments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync((long UserId, long AssignmentId) id)
    {
        var entity = await _context.UserAssignments.FindAsync(id);
        if (entity == null) return false;

        _context.UserAssignments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UserAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.UserAssignments.Select(ua => new UserAssignment(ua)).ToListAsync(cancellationToken);
    }

    public async Task<UserAssignment?> GetAsync((long UserId, long AssignmentId) id, CancellationToken cancellationToken = default)
    {
        var userAssignmentModel = await _context.UserAssignments.FindAsync(id);
        if(userAssignmentModel == null) return null; 
        else return new UserAssignment(userAssignmentModel);    
    }

    public async Task<UserAssignment> UpdateAsync(UserAssignment entity)
    {
        _context.UserAssignments.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}
