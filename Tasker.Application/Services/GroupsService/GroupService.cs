using Microsoft.Extensions.Logging;
using Tasker.Domain;
using Tasker.Enums;
namespace Tasker.Application;

public class GroupsService : IGroupsService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserParticipationRepository _userParticipationRepository;
    private readonly ILogger<GroupsService> _logger;

    public GroupsService(IGroupRepository groupRepository, IUserParticipationRepository userParticipationRepository, ILogger<GroupsService> logger)
    {
        _groupRepository = groupRepository;
        _userParticipationRepository = userParticipationRepository;
        _logger = logger;
    }

    public async Task<Result<Group>> AddGroupMember(long groupId, string userId)
    {
        _logger.LogInformation($"Adding user {userId} to group {groupId}");

        try
        {
            await _userParticipationRepository.AddAsync(new UserParticipation
            {
                GroupId = groupId,
                UserId = userId,
                Role = GroupRole.User
            });

            Group? group = await _groupRepository.GetAsync(groupId);

            if (group is null)
            {
                _logger.LogError($"Group {groupId} not found during adding member {userId}");
                return Result.Failure<Group>("Group not found");
            }

            _logger.LogInformation($"Successfully added user {userId} to group {groupId}");
            return Result.Success(group);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error adding member to group: {ex.Message}");
            return Result.Failure<Group>($"Error adding member to group: {ex.Message}");
        }
    }

    public async Task<Result<Group>> CreateGroup(Group group, string userId)
    {
        _logger.LogInformation($"Creating group {group.Name} for user {userId}");
        if (userId == null) return Result.Failure<Group>("Invalid user");

        try
        {
            group.UserParticipations.Add(new UserParticipation() { UserId = userId, Role = GroupRole.Admin });
            var createdGroup = await _groupRepository.AddAsync(group);

            if (createdGroup == null)
            {
                _logger.LogError($"Failed to create group {group.Name} for user {userId}");
                return Result.Failure<Group>("Failed to create group");
            }

            _logger.LogInformation($"Successfully created group {group.Name} for user {userId}");
            return Result.Success(createdGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating group: {ex.Message}");
            return Result.Failure<Group>($"Error creating group: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteGroup(long groupId)
    {
        _logger.LogInformation($"Deleting group {groupId}");

        try
        {
            bool deleted = await _groupRepository.DeleteAsync(groupId);

            if (!deleted)
            {
                _logger.LogError($"Failed to delete group {groupId}");
                return Result.Failure<bool>("Failed to delete group");
            }

            _logger.LogInformation($"Successfully deleted group {groupId}");
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting group {groupId}: {ex.Message}");
            return Result.Failure<bool>($"Error deleting group: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Group>>> GetAllGroups(string userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Retrieving all groups for user {userId}");

        try
        {
            var groups = await _groupRepository.GetAllAsync(userId, cancellationToken);

            _logger.LogInformation($"Successfully retrieved groups for user {userId}");
            return Result.Success(groups);
        }
        catch (Exception ex)
        {

            _logger.LogError($"Error retrieving groups for user {userId}: {ex.Message}");
            return Result.Failure<IEnumerable<Group>>("Error retrieving groups: " + ex.Message);
        }
    }

    public async Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Retrieving group {groupId}");

        try
        {
            var group = await _groupRepository.GetAsync(groupId);
            if (group == null)
            {
                _logger.LogError($"Group {groupId} not found");
                return Result.Failure<Group>("Group not found");
            }

            _logger.LogInformation($"Successfully retrieved group {groupId}");
            return Result.Success(group);

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving group {groupId}: {ex.Message}");
            return Result.Failure<Group>("Error retrieving group: " + ex.Message);
        }

    }

    public async Task<Result<Group>> UpdateGroup(Group group)
    {
        _logger.LogInformation($"Updating group {group.GroupId}");

        try
        {
            var updatedGroup = await _groupRepository.UpdateAsync(group);
            if (updatedGroup is null)
            {
                _logger.LogError($"Group {group.GroupId} was not updated");
                return Result.Failure<Group>("Group was not updated");
            }

            _logger.LogInformation($"Successfully updated group {group.GroupId}");
            return Result.Success(updatedGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating group {group.GroupId}: {ex.Message}");
            return Result.Failure<Group>("Failed updating group: " + ex.Message);
        }
    }
}
