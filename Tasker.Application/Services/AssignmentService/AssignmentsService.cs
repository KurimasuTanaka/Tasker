using Tasker.Domain;
using Microsoft.Extensions.Logging;

namespace Tasker.Application;

public class AssignmentsService : IAssignmentsService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IUserAssignmentRepository _userAssignmentRepository;
    private readonly ILogger<AssignmentsService> _logger;

    public AssignmentsService(IAssignmentRepository assignmentRepository, IUserAssignmentRepository userAssignmentRepository, ILogger<AssignmentsService> logger)
    {
        _assignmentRepository = assignmentRepository;
        _userAssignmentRepository = userAssignmentRepository;
        _logger = logger;
    }

    public async Task<Result<UserAssignment>> AssignTaskToUser(string userId, long assignmentId)
    {
        _logger.LogInformation($"Assigning task {assignmentId} to user {userId}");

        try
        {
            // Check if the assignment is already assigned to the user
            UserAssignment? existingAssignment = await _userAssignmentRepository.GetAsync((userId, assignmentId));
            if (existingAssignment is null)
            {
                var createdUserAssignment = await _userAssignmentRepository.AddAsync(new UserAssignment
                {
                    UserId = userId,
                    AssignmentId = assignmentId
                });

                if (createdUserAssignment == null)
                {
                    _logger.LogError($"Failed to assign task {assignmentId} to user {userId}");
                    return Result.Failure<UserAssignment>("Failed to assign task to user");
                }

                _logger.LogInformation($"Successfully assigned task {assignmentId} to user {userId}");
                return Result.Success<UserAssignment>(createdUserAssignment);
            }
            else return Result.Success<UserAssignment>(existingAssignment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning task to user: {ex.Message}");
            return Result.Failure<UserAssignment>($"Error assigning task to user: {ex.Message}");
        }

    }

    public async Task<Result<bool>> UnassignTaskFromUser(string userId, long assignmentId)
    {
        _logger.LogInformation($"Unassigning task {assignmentId} from user {userId}");
        try
        {
            bool deletedUserAssignment = await _userAssignmentRepository.DeleteAsync((userId, assignmentId));
            if (!deletedUserAssignment)
            {
                _logger.LogError($"Failed to unassign task {assignmentId} from user {userId}");
                return Result.Failure<bool>("Failed to unassign task from user");
            }


            _logger.LogInformation($"Successfully unassigned task {assignmentId} from user {userId}");
            return Result.Success<bool>(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error unassigning task from user: {ex.Message}");
        }

    }


    public async Task<Result<Assignment>> CreateAssignment(long groupId, Assignment assignment)
    {
        _logger.LogInformation($"Creating assignment in group {groupId} with title {assignment.Title}");

        try
        {
            Assignment? createdAssignment = await _assignmentRepository.AddAsync(new Assignment
            {
                Title = assignment.Title,
                Description = assignment.Description,
                IsCompleted = assignment.IsCompleted,
                GroupId = groupId
            });

            if (createdAssignment == null)
            {
                _logger.LogError($"Failed to create assignment in group {groupId} with title {assignment.Title}");
                return Result.Failure<Assignment>("Failed to create assignment");
            }

            _logger.LogInformation($"Successfully created assignment in group {groupId} with title {assignment.Title}");
            return Result.Success(createdAssignment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating assignment: {ex.Message}");
            return Result.Failure<Assignment>($"Error creating assignment: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAssignment(long groupId, long assignmentId)
    {
        _logger.LogInformation($"Deleting assignment {assignmentId} from group {groupId}");

        try
        {
            bool deleted = await _assignmentRepository.DeleteAsync(assignmentId);
            if (!deleted)
            {
                _logger.LogError($"Failed to delete assignment {assignmentId} from group {groupId}");
                return Result.Failure<bool>("Failed to delete assignment");
            }

            _logger.LogInformation($"Successfully deleted assignment {assignmentId} from group {groupId}");
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting assignment: {ex.Message}");
            return Result.Failure<bool>($"Error deleting assignment: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Assignment>>> GetAllAssignments(long groupId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Retrieving all assignments for group {groupId}");

        try
        {
            IEnumerable<Assignment> assignments = await _assignmentRepository.GetAllAsync(cancellationToken);

            _logger.LogInformation($"Successfully retrieved assignments for group {groupId}");
            return Result.Success(assignments);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving assignments: {ex.Message}");
            return Result.Failure<IEnumerable<Assignment>>($"Error retrieving assignments: {ex.Message}");
        }
    }

    public async Task<Result<Assignment>> GetAssignment(long groupId, long assignmentId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Retrieving assignment {assignmentId} for group {groupId}");

        try
        {
            Assignment? assignment = await _assignmentRepository.GetAsync(assignmentId, cancellationToken);

            if (assignment == null)
            {

                _logger.LogError($"Assignment {assignmentId} not found in group {groupId}");
                return Result.Failure<Assignment>("Assignment not found");
            }

            _logger.LogInformation($"Successfully retrieved assignment {assignmentId} for group {groupId}");
            return Result.Success(assignment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving assignment: {ex.Message}");
            return Result.Failure<Assignment>($"Error retrieving assignment: {ex.Message}");
        }
    }


    public async Task<Result<Assignment>> UpdateAssignment(long groupId, long assignmentId, Assignment updatedAssignment)
    {
        _logger.LogInformation($"Updating assignment {assignmentId} in group {groupId}");

        try
        {
            Assignment? updated = await _assignmentRepository.UpdateAsync(new Assignment
            {
                AssignmentId = assignmentId,
                Title = updatedAssignment.Title,
                Description = updatedAssignment.Description,
                IsCompleted = updatedAssignment.IsCompleted,
                GroupId = groupId
            });
            if (updated == null)
            {
                _logger.LogError($"Failed to update assignment {assignmentId} in group {groupId}");
                return Result.Failure<Assignment>("Failed to update assignment");
            }

            _logger.LogInformation($"Successfully updated assignment {assignmentId} in group {groupId}");
            return Result.Success(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating assignment: {ex.Message}");
            return Result.Failure<Assignment>($"Error updating assignment: {ex.Message}");
        }
    }
}
