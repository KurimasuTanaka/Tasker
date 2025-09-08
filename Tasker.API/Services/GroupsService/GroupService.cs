using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tasker.DataAccess;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.DataAccess.Repositories;
using System.Security.Claims;

namespace Tasker.API.Services.GroupsService;

public class GroupsService : IGroupsService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserParticipationRepository _userParticipationRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public GroupsService(IGroupRepository groupRepository, IUserParticipationRepository userParticipationRepository, UserManager<IdentityUser> userManager)
    {
        _groupRepository = groupRepository;
        _userParticipationRepository = userParticipationRepository;
        _userManager = userManager;
    }

    public async Task<Result<Group>> AddGroupMember(long groupId, string userId)
    {
        await _userParticipationRepository.AddAsync(new UserParticipation
        {
            GroupId = groupId,
            UserId = userId,
            Role = "Member"
        });

        Group? group = await _groupRepository.GetAsync(groupId);
        if (group is null) return Result.Failure<Group>("Group not found");
        return Result.Success(group);
    }

    public async Task<Result<Group>> CreateGroup(Group group, ClaimsPrincipal user)
    {
        var userId = _userManager.GetUserId(user);
        if (userId == null) return Result.Failure<Group>("Invalid user");

        Group newGroup = new Group();
        newGroup.Name = group.Name;
        newGroup.Participants.Add(new UserParticipation() { UserId = userId, Role = "Admin" });


        var createdGroup = await _groupRepository.AddAsync(newGroup);
        return Result.Success(createdGroup);
    }

    public async Task<Result<IEnumerable<Group>>> GetAllGroups(ClaimsPrincipal user, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetUserId(user);

        if (userId == null) return Result.Failure<IEnumerable<Group>>("Invalid user");

        var groups = await _groupRepository.GetAllAsync(userId, cancellationToken);
        return Result.Success(groups);
    }

    public async Task<Result<Group>> GetGroupById(long groupId, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetAsync(groupId);
        if (group == null) return Result.Failure<Group>("Group not found");

        return Result.Success(group);
    }
}
