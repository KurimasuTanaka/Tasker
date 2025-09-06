using Microsoft.EntityFrameworkCore;
using Tasker.DataAccess;
using Tasker.Database;

namespace Tasker.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IdentityContext _context;
    public TaskRepository(IdentityContext context)
    {
        _context = context;
    }

    // Create operations
    public async Task<Task> AddAsync(Tasker.DataAccess.Task entity)
    {
        await _context.Tasks.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Read operations
    public async Task<Task?> GetAsync(long id)
    {
        var taskModel = await _context.Tasks.FindAsync(id);
        if (taskModel == null) return null; 
        else return new Task(taskModel);
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
    {
        return await _context.Tasks.Select(t => new Task(t)).ToListAsync();
    }

    // Update operations
    public async Task<Task> UpdateAsync(Task entity)
    {
        _context.Tasks.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Delete operations
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await GetAsync(id);
        if (entity == null) return false;

        _context.Tasks.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
