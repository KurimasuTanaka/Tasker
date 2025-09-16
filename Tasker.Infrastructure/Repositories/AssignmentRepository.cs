using Microsoft.EntityFrameworkCore;
using Tasker.Domain;

namespace Tasker.Infrastructure;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly IDbContextFactory<TaskerContext> _contextFactory;
    public AssignmentRepository(IDbContextFactory<TaskerContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Assignment?> AddAsync(Assignment entity)
    {
        if (entity == null) return null;
        
        using var _context = await _contextFactory.CreateDbContextAsync();
        AssignmentModel assignmentModel = entity.ToModel()!;

        await _context.Assignments.AddAsync(assignmentModel);
        await _context.SaveChangesAsync();
        await _context.Entry(assignmentModel).ReloadAsync();

        return assignmentModel.ToDomain();
    }

    public async Task<Assignment?> GetAsync(long id, CancellationToken cancellationToken = default)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();

        var assignmentModel = await _context.Assignments.FindAsync(id, cancellationToken);

        if (assignmentModel == null) return null;
        else return assignmentModel.ToDomain();
    }

    public async Task<IEnumerable<Assignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();

        return await _context.Assignments.
            Select(t => t.ToDomain()!).
            ToListAsync(cancellationToken);
    }

    public async Task<Assignment?> UpdateAsync(Assignment entity)
    {
        if (entity == null) return null!;

        using var _context = await _contextFactory.CreateDbContextAsync();

        AssignmentModel assignmentModel = entity.ToModel()!;

        _context.Assignments.Update(assignmentModel);
        await _context.SaveChangesAsync();
        await _context.Entry(assignmentModel).ReloadAsync();

        return assignmentModel.ToDomain()!;
    }

    public async Task<bool> DeleteAsync(long id)
    {

        using var _context = await _contextFactory.CreateDbContextAsync();

        var assignmentToDelete = await GetAsync(id);
        
        if (assignmentToDelete == null) return false;

        // Remove related UserAssignments first due to foreign key constraints
        List<UserAssignmentModel> userAssignments = await  _context.UserAssignments.Where(ua => ua.AssignmentId == assignmentToDelete.AssignmentId).ToListAsync();

        userAssignments.ForEach(ua =>
        {
            _context.UserAssignments.Remove(ua);
        });

        _context.Assignments.Remove(assignmentToDelete.ToModel()!);
        await _context.SaveChangesAsync();
        return true;
    }
}
