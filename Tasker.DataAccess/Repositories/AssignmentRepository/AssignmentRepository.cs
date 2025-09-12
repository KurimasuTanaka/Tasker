using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;
    public AssignmentRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // Create operations
    public async Task<Assignment> AddAsync(Assignment entity)
    {
        using var _context = await _contextFactory.CreateDbContextAsync();
        await _context.Assignments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Read operations
    public async Task<Assignment?> GetAsync(long id, CancellationToken cancellationToken = default)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();
        var assignmentModel = await _context.Assignments.FindAsync(id, cancellationToken);
        if (assignmentModel == null) return null;
        else return new Assignment(assignmentModel);
    }

    public async Task<IEnumerable<Assignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();
        return await _context.Assignments.Select(t => new Assignment(t)).ToListAsync(cancellationToken);
    }

    // Update operations
    public async Task<Assignment> UpdateAsync(Assignment entity)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();
        _context.Assignments.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();
        var assignmentToDelete = await GetAsync(id);
        if (assignmentToDelete == null) return false;

        List<UserAssignmentModel> userAssignments = await  _context.UserAssignments.Where(ua => ua.AssignmentId == assignmentToDelete.AssignmentId).ToListAsync();

        userAssignments.ForEach(ua =>
        {
            _context.UserAssignments.Remove(ua);
        });

        _context.Assignments.Remove(assignmentToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}
