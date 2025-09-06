using System;
using Microsoft.Extensions.DependencyInjection;
using Tasker.DataAccess.Repositories;

namespace Tasker.DataAccess;

public static class DataAccessServiceCollectionExtension
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserParticipationRepository, UserParticipationRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}
