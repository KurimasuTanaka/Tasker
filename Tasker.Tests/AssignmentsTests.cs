using Tasker.API.Services.GroupsService;
using Tasker.DataAccess;
using Tasker.DataAccess.Repositories;
using Tasker.API.Services.AssignmentsService;
using Tasker.DataAccess.DataTransferObjects;
using Tasker.Database;

namespace Tasker.Tests;

public class AssignmentsTests
{
    AssignmentsService _assignmentsService = null!;
    protected TestDbContextFactory _contextFactory;
    protected IAssignmentRepository _assignmentRepository;
    protected IUserAssignmentRepository _userAssignmentRepository;
    protected IUserRepository _userRepository;
    protected IGroupRepository _groupRepository;

    [SetUp]
    public async Task Setup()
    {
        _contextFactory = new TestDbContextFactory();
        _assignmentRepository = new AssignmentRepository(_contextFactory);
        _userAssignmentRepository = new UserAssignmentRepository(_contextFactory);
        _userRepository = new UserRepository(_contextFactory);
        _groupRepository = new GroupRepository(_contextFactory);
        _assignmentsService = new AssignmentsService(_assignmentRepository, _userAssignmentRepository);

        await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName",
            LastName = "TestLastName",
        });
    }

    [Test]
    public async Task CreateAssignment_ValidAssignment_AssignmentIsCreated()
    {
        // Arrange

        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });

        Assignment assignment = new Assignment(new AssignmentModel
        {
            Title = "Test Assignment",
            Description = "Test Desc",
            CreatedDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(1),
            IsCompleted = false,
            Priority = "High",
            GroupId = createdGroup.GroupId
        });
        // Act
        var result = await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment);

        // Assert


        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value.Title, Is.EqualTo("Test Assignment"));
    }

    [Test]
    public async Task GetAllAssignments_TwoAssignmentsAdded_ReturnsTwoAssignments()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });

        var assignment1 = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var assignment2 = new Assignment(new AssignmentModel { Title = "A2", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment1);
        await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment2);

        // Act
        var result = await _assignmentsService.GetAllAssignments(createdGroup.GroupId, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAssignment_ExistingAssignment_ReturnsAssignment()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });

        var assignment = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var created = (await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment)).Value;

        // Act
        var result = await _assignmentsService.GetAssignment(createdGroup.GroupId, created.AssignmentId, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.AssignmentId, Is.EqualTo(created.AssignmentId));
    }

    [Test]
    public async Task UpdateAssignment_ExistingAssignment_UpdatesAssignment()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });

        var assignment = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var created = (await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment)).Value;
        created.Title = "Updated Title";

        // Act
        var result = await _assignmentsService.UpdateAssignment(createdGroup.GroupId, created.AssignmentId, created);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.Title, Is.EqualTo("Updated Title"));
    }

    [Test]
    public async Task DeleteAssignment_ExistingAssignment_DeletesAssignment()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });
        var assignment = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var created = (await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment)).Value;

        // Act
        var result = await _assignmentsService.DeleteAssignment(createdGroup.GroupId, created.AssignmentId);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.True);
    }

    [Test]
    public async Task AssignTaskToUser_ValidUserAssignment_AssignsTask()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });
        User user = await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName",
            LastName = "TestLastName",
        });

        var assignment = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var created = (await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment)).Value;

        var userAssignment = new UserAssignment(new UserAssignmentModel { UserId = user.UserIdentity, AssignmentId = created.AssignmentId });

        // Act
        var result = await _assignmentsService.AssignTaskToUser(userAssignment);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserId, Is.EqualTo(user.UserIdentity));
        Assert.That(result.Value.AssignmentId, Is.EqualTo(created.AssignmentId));
    }

    [Test]
    public async Task UnassignTaskFromUser_ExistingUserAssignment_UnassignsTask()
    {
        // Arrange
        Group createdGroup = await _groupRepository.AddAsync(new() { Name = "Test Group" });
        User user = await _userRepository.AddAsync(new User()
        {
            UserIdentity = Guid.NewGuid().ToString(),
            FirstName = "TestName",
            LastName = "TestLastName",
        });

        var assignment = new Assignment(new AssignmentModel { Title = "A1", CreatedDate = DateTime.UtcNow, GroupId = createdGroup.GroupId });
        var created = (await _assignmentsService.CreateAssignment(createdGroup.GroupId, assignment)).Value;
        var userAssignment = new UserAssignment(new UserAssignmentModel { UserId = user.UserIdentity, AssignmentId = created.AssignmentId });
        await _assignmentsService.AssignTaskToUser(userAssignment);

        // Act
        var result = await _assignmentsService.UnassignTaskFromUser(userAssignment);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.UserId, Is.EqualTo(user.UserIdentity));
        Assert.That(result.Value.AssignmentId, Is.EqualTo(created.AssignmentId));
    }
}
