using Tasker.Domain;
using Tasker.Enums;
namespace Tasker.Application;

public class GroupsService : IGroupsService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserParticipationRepository _userParticipationRepository;

    public GroupsService(IGroupRepository groupRepository, IUserParticipationRepository userParticipationRepository)
    {
        _groupRepository = groupRepository;
        _userParticipationRepository = userParticipationRepository;
    }

    public async Task<Result<Group>> AddGroupMember(long groupId, string userId)
    {
        if (userId == null) return Result.Failure<Group>("Invalid user");
        try
        {
            await _userParticipationRepository.AddAsync(new UserParticipation
            {
                GroupId = groupId,
                UserId = userId,
                Role = GroupRole.User
            });

            Group? group = await _groupRepository.GetAsync(groupId);
            if (group is null) return Result.Failure<Group>("Group not found");
            return Result.Success(group);
        }
        catch (Exception ex)
        {
            return Result.Failure<Group>($"Error adding member to group: {ex.Message}");
        }
    }

    public async Task<Result<Group>> CreateGroup(Group group, string userId)
    {
        if (userId == null) return Result.Failure<Group>("Invalid user");

        try
        {
            group.UserParticipations.Add(new UserParticipation() { UserId = userId, Role = GroupRole.Admin });
            var createdGroup = await _groupRepository.AddAsync(group);
            return Result.Success(createdGroup);
        }
        catch (Exception ex)
        {
            return Result.Failure<Group>($"Error creating group: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteGroup(long groupId)
    {
        try
        {
            await _groupRepository.DeleteAsync(groupId);
            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>($"Error deleting group: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<Group>>> GetAllGroups(string userId, CancellationToken cancellationToken)
    {
        if (userId == null) return Result.Failure<IEnumerable<Group>>("Invalid user");

        try
        {
            var groups = await _groupRepository.GetAllAsync(userId, cancellationToken);
            return Result.Success(groups);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Group>>("Error retrieving groups: " + ex.Message);
        }
    }

    public async Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _groupRepository.GetAsync(groupId);
            if (group == null) return Result.Failure<Group>("Group not found");
            return Result.Success(group);

        }
        catch (Exception ex)
        {
            return Result.Failure<Group>("Error retrieving group: " + ex.Message);
        }

    }

    public async Task<Result<Group>> UpdateGroup(Group group)
    {
        try
        {
            var upatedGroup = await _groupRepository.UpdateAsync(group);
            if (upatedGroup is null) return Result.Failure<Group>("Group was not updated");
            return Result.Success(group);
        }
        catch (Exception ex)
        {
            return Result.Failure<Group>("Failed updating group: " + ex.Message);
        }
    }
}
