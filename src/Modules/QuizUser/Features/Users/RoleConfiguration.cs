﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuizUser.Features.Users;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder)builder, "roles");

        builder.HasKey(r => r.Name);

        builder.Property(r => r.Name).HasMaxLength(50);

        builder
            .HasMany<User>()
            .WithMany(u => u.Roles)
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("user_roles");

                joinBuilder.Property("RolesName").HasColumnName("role_name");
            });

        builder.HasData(
            Role.Member,
            Role.Administrator);
    }
}