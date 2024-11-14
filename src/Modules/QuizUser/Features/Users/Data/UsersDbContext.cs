using Microsoft.EntityFrameworkCore;
using QuizUser.Abstractions.Users;

namespace QuizUser.Features.Users.Data;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
  internal DbSet<User> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema(Schemas.Users);

#pragma warning disable S125
    // modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
#pragma warning restore S125
    // modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
    // modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
    // modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    modelBuilder.ApplyConfiguration(new UserConfiguration());
    modelBuilder.ApplyConfiguration(new RoleConfiguration());
    modelBuilder.ApplyConfiguration(new PermissionConfiguration());
  }
}