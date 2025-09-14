using System;
using Microsoft.EntityFrameworkCore;
using Tasker.Infrastructure;

namespace Tasker.Tests;

public class TestDbContextFactory : IDbContextFactory<TaskerContext>
{
    DbContextOptionsBuilder<TaskerContext> builder = new DbContextOptionsBuilder<TaskerContext>();
    public TestDbContextFactory()
    {
        builder.UseSqlite("Data Source=./TaskerTestDb2");

        TaskerContext context = new(builder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    public void DeleteTestDb()
    {
        TaskerContext context = new(builder.Options);
        context.Database.EnsureDeleted();

    }

    public TaskerContext CreateDbContext()
    {
        return new TaskerContext(builder.Options);
    }
}