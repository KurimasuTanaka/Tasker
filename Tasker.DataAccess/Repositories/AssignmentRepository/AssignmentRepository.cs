using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly TaskerContext _context;
    public AssignmentRepository(TaskerContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<Assignment> AddAsync(Assignment entity)
    {
        await _context.Assignments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Read operations
    public async Task<Assignment?> GetAsync(long id)
    {
        var assignmentModel = await _context.Assignments.FindAsync(id);
        if (assignmentModel == null) return null; 
        else return new Assignment(assignmentModel);
    }

    public async Task<IEnumerable<Assignment>> GetAllAsync()
    {
        return await _context.Assignments.Select(t => new Assignment(t)).ToListAsync();
    }

    // Update operations
    public async Task<Assignment> UpdateAsync(Assignment entity)
    {
        _context.Assignments.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Assignments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
