using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tasker.Database;

public class IdentityContext : IdentityDbContext<UserModel>
{
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<GroupModel> Groups { get; set; } = null!;
    public DbSet<UserParticipationModel> UserParticipations { get; set; } = null!;

    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }
}
