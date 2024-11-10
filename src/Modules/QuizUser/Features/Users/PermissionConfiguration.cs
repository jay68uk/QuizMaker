using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizMaker.Common.Infrastructure.Authorisation;

namespace QuizUser.Features.Users;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
  public void Configure(EntityTypeBuilder<Permission> builder)
  {
    ((EntityTypeBuilder)builder).ToTable("permissions");

    builder.HasKey(p => p.Code);

    builder.Property(p => p.Code).HasMaxLength(100);

    builder
      .HasMany<Role>()
      .WithMany()
      .UsingEntity(joinBuilder =>
      {
        joinBuilder.ToTable("role_permissions");
      });
  }
}