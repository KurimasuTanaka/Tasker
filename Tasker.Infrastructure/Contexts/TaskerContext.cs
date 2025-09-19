using System;
using Microsoft.EntityFrameworkCore;

namespace Tasker.Infrastructure;

public class TaskerContext : DbContext
{
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<GroupModel> Groups { get; set; } = null!;
    public DbSet<AssignmentModel> Assignments { get; set; } = null!;
    public DbSet<UserParticipationModel> UserParticipations { get; set; } = null!;
    public DbSet<UserAssignmentModel> UserAssignments { get; set; } = null!;

    public TaskerContext(DbContextOptions<TaskerContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserAssignmentModel>()
            .HasKey(ua => new { ua.UserId, ua.AssignmentId });

        modelBuilder.Entity<UserParticipationModel>()
            .HasKey(up => new { up.UserId, up.GroupId });
    }


}
