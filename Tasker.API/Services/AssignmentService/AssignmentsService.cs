using System;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Services.AssignmentsService;

public class AssignmentsService : IAssignmentsService
{
    private readonly IAssignmentRepository _assignmentRepository;

    public AssignmentsService(IAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    public async Task<Result<Assignment>> CreateAssignment(long groupId, Assignment assignment)
    {
        if (assignment == null) return Result.Failure<Assignment>("Assignment cannot be null");

        try
        {
            Assignment createdAssignment = await _assignmentRepository.AddAsync(assignment);
            return Result.Success(createdAssignment);
        }
        catch (Exception ex)
        {
            return Result.Failure<Assignment>($"Error creating assignment: {ex.Message}");
        }

    }

    public async Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId)
    {
        try
        {
            bool deleted = await _assignmentRepository.DeleteAsync(assignmentId);
            return Result.Success(deleted);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error deleting assignment: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Assignment> assignments = await _assignmentRepository.GetAllAsync(cancellationToken);
            return Result.Success(assignments);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Assignment>>($"Error retrieving assignments: {ex.Message}");
        }
    }

    public async Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken)
    {
        try
        {
            Assignment? assignment = await _assignmentRepository.GetAsync(assignmentId, cancellationToken);
            if (assignment == null) return Result.Failure<Assignment>("Assignment not found");
            return Result.Success(assignment);
        }
        catch (Exception ex)
        {
            return Result.Failure<Assignment>($"Error retrieving assignment: {ex.Message}");
        }
    }

    

    public async Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, Assignment updatedAssignment)
    {
        if (updatedAssignment == null) return Result.Failure<Assignment>("Updated assignment cannot be null");

        try
        {
            Assignment updated = await _assignmentRepository.UpdateAsync(updatedAssignment);
            
            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            return Result.Failure<Assignment>($"Error updating assignment: {ex.Message}");
        }
    }
}
