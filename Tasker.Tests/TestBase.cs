using System;
using Microsoft.Extensions.Logging;
using Moq;
using Tasker.DataAccess.Repositories;

namespace Tasker.Tests;

public class TestBase : IDisposable
{
    protected readonly TestDbContextFactory _contextFactory;

    protected readonly IGroupRepository _groupRepository;
    protected readonly INotificationRepository _notificationRepository;
    protected readonly IUserRepository _userRepository;
    protected readonly IUserAssignmentRepository _userAssignmentRepository;
    protected readonly IUserParticipationRepository _userParticipationRepository;

    public TestBase()
    {

        _contextFactory = new TestDbContextFactory();
        _userRepository = new UserRepository(_contextFactory);
        _groupRepository = new GroupRepository(_contextFactory);
        _notificationRepository = new NotificationRepository(_contextFactory);
        _userAssignmentRepository = new UserAssignmentRepository(_contextFactory);
        _userParticipationRepository = new UserParticipationRepository(_contextFactory);
    }

    public void Dispose()
    {
        _contextFactory.DeleteTestDb();
    }
}
