
using Tasker.Application;
using Tasker.Domain;
using Tasker.Infrastructure;

namespace Tasker.Tests;

public class GroupTests
{
    GroupsService _groupsService = null!;
    protected TestDbContextFactory _contextFactory;
    protected IGroupRepository _groupRepository;
    protected INotificationRepository _notificationRepository;
    protected IAssignmentRepository _assignmentRepository;
    protected IUserRepository _userRepository;
    protected IUserAssignmentRepository _userAssignmentRepository;
    protected IUserParticipationRepository _userParticipationRepository;

    [SetUp]
    public async Task Setup()
    {
        _contextFactory = new TestDbContextFactory();

        _userRepository = new UserRepository(_contextFactory);
        _groupRepository = new GroupRepository(_contextFactory);
        _userParticipationRepository = new UserParticipationRepository(_contextFactory);
        _assignmentRepository = new AssignmentRepository(_contextFactory);
        _groupsService = new GroupsService(_groupRepository, _userParticipationRepository);


        await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName",
            LastName = "TestLastName",
        });
    }

    [Test]
    public async Task CreateGroup_CreateNewGroup_NewGroupIsCreated()
    {
        //Arrange
        string userId = (await _userRepository.GetAllAsync()).First().UserIdentity;

        var groupName = "Test Group";
        Group group = new() { Name = groupName };


        //Act
        Group createdGroup = (await _groupsService.CreateGroup(group, userId)).Value;

        //Assert

        Assert.That(createdGroup, Is.Not.Null);
        Assert.That(createdGroup.Name, Is.EqualTo(groupName));


    }
    [Test]
    public async Task GetAllGroups_CreateTwoGroupsAndReturnThem_TwoGroupsAreReturned()
    {
        //Arrange
        string userId = (await _userRepository.GetAllAsync()).First().UserIdentity;

        var groupName1 = "Test Group 1";
        var groupName2 = "Test Group 2";
        Group group1 = new() { Name = groupName1 };
        Group group2 = new() { Name = groupName2 };

        await _groupsService.CreateGroup(group1, userId);
        await _groupsService.CreateGroup(group2, userId);
        //Act

        List<Group> createdGroups = (await _groupsService.GetAllGroups(userId, CancellationToken.None)).Value.ToList();

        //Assert

        Assert.That(createdGroups, Is.Not.Null);
        Assert.That(createdGroups.Count, Is.EqualTo(2));

    }
    [Test]
    public async Task GetGroupById_CreateNewGroupAndGetItById_GroupIsReturned()
    {
        //Arrange
        string userId = (await _userRepository.GetAllAsync()).First().UserIdentity;


        Group group1 = new() { Name = "Test Group 1" };

        Group createdGroup = (await _groupsService.CreateGroup(group1, userId)).Value;
        //Act

        Group? retrievedGroup = (await _groupsService.GetGroupById(createdGroup.GroupId, CancellationToken.None)).Value;
        //Assert

        Assert.That(retrievedGroup, Is.Not.Null);
        Assert.That(retrievedGroup?.GroupId, Is.EqualTo(createdGroup.GroupId));
        Assert.That(retrievedGroup?.Name, Is.EqualTo(createdGroup.Name));
    }

    [Test]
    public async Task AddGroupMember_CreateNewGroupAndAddNewUserToIt_UserIsAddedToGroup()
    {
        //Arrange
        string firstUserId = (await _userRepository.GetAllAsync()).First().UserIdentity;
        User addedUser = await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName2",
            LastName = "TestLastName2",
        });

        Group group1 = new() { Name = "Test Group 1" };
        Group createdGroup = (await _groupsService.CreateGroup(group1, firstUserId)).Value;
        //Act

        Group? retrievedGroup = (await _groupsService.AddGroupMember(createdGroup.GroupId, addedUser.UserIdentity)).Value;
        //Assert

        Assert.That(retrievedGroup, Is.Not.Null);
        Assert.That(retrievedGroup?.GroupId, Is.EqualTo(createdGroup.GroupId));
        Assert.That(retrievedGroup?.UserParticipations.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetAllGroups_CreateOneGroupPerUserAndAddOneUserToAnother_ListWithTwoGroupsFromOneUser()
    {
        //Arrange
        string firstUserId = (await _userRepository.GetAllAsync()).First().UserIdentity;
        User secondUser = await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName2",
            LastName = "TestLastName2",
        });

        Group group1 = new() { Name = "Test Group 1" };
        Group group2 = new() { Name = "Test Group 2" };

        Group firstUserGroup = (await _groupsService.CreateGroup(group1, firstUserId)).Value;
        Group secondUserGroup = (await _groupsService.CreateGroup(group2, secondUser.UserIdentity)).Value;

        await _groupsService.AddGroupMember(firstUserGroup.GroupId, secondUser.UserIdentity);

        //Act

        List<Group> retrievedGroups = (await _groupsService.GetAllGroups(secondUser.UserIdentity, CancellationToken.None)).Value.ToList();
        //Assert

        Assert.That(retrievedGroups, Is.Not.Null);
        Assert.That(retrievedGroups.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task DeleteGroup_DeleteGroupWithRelatedData_AllDataSuccessfullyDeleted()
    {
        //Arrange
        string userId = (await _userRepository.GetAllAsync()).First().UserIdentity;

        Group group = new() { Name = "Test Group 1" };
        group.Assignments.Add(new Assignment() { Title = "Test Assignment 1" });
        group.Assignments.Add(new Assignment() { Title = "Test Assignment 2" });
        Group createdGroup = (await _groupsService.CreateGroup(group, userId)).Value;

        //Act

        if ((await _assignmentRepository.GetAllAsync(CancellationToken.None)).Count() != 2)
            Assert.Fail("Assignments were not created properly");
        if ((await _userParticipationRepository.GetAllAsync(CancellationToken.None)).Count() != 1)
            Assert.Fail("User participation was not created properly");

        bool isDeleted = (await _groupsService.DeleteGroup(createdGroup.GroupId)).Value;

        //Assert

        Assert.That(isDeleted, Is.True);
        Assert.That((await _groupsService.GetGroupById(createdGroup.GroupId, CancellationToken.None)).IsSuccess, Is.False);
        Assert.That((await _assignmentRepository.GetAllAsync(CancellationToken.None)).Count(), Is.EqualTo(0));
        Assert.That((await _userParticipationRepository.GetAllAsync(CancellationToken.None)).Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task UpdateGroup_UpdateGroupName_GroupNameSuccessfullyUpdated()
    {
        //Arrange
        string userId = (await _userRepository.GetAllAsync()).First().UserIdentity;

        Group group = new() { Name = "Test Group 1" };
        Group createdGroup = (await _groupsService.CreateGroup(group, userId)).Value;

        //Act

        Group updatedGroup = (await _groupsService.UpdateGroup(new Group() { GroupId = createdGroup.GroupId, Name = "Updated Group Name" })).Value;

        //Assert

        Assert.That(updatedGroup, Is.Not.Null);
        Assert.That(updatedGroup.Name, Is.EqualTo("Updated Group Name"));
    }

}
