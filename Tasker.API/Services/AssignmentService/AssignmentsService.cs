using System;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;

namespace Tasker.API.Services.AssignmentsService;

public class AssignmentsService : IAssignmentsService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUserAssignmentRepository _userAssignmentRepository;

    public AssignmentsService(IAssignmentRepository assignmentRepository, IUserAssignmentRepository userAssignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
        _userAssignmentRepository = userAssignmentRepository;
    }

    public async Task<Result<UserAssignment>> AssignTaskToUser(UserAssignment userAssignment)
    {
        if (userAssignment == null) return Result.Failure<UserAssignment>("UserAssignment cannot be null");
        try
        {
            var createdUserAssignment = await _userAssignmentRepository.AddAsync(userAssignment);
            if (createdUserAssignment == null) return Result.Failure<UserAssignment>("Failed to assign task to user");
            return Result.Success<UserAssignment>(createdUserAssignment);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserAssignment>($"Error assigning task to user: {ex.Message}");
        }
    }

    public async Task<Result<UserAssignment>> UnassignTaskFromUser(UserAssignment userAssignment)
    {
        if (userAssignment == null) return Result.Failure<UserAssignment>("UserAssignment cannot be null");
        try
        {
            bool deletedUserAssignment = await _userAssignmentRepository.DeleteAsync((userAssignment.UserId, userAssignment.AssignmentId));
            if (!deletedUserAssignment) return Result.Failure<UserAssignment>("Failed to unassign task from user");
            return Result.Success<UserAssignment>(userAssignment);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserAssignment>($"Error unassigning task from user: {ex.Message}");
        }

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
