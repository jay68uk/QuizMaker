﻿using Microsoft.EntityFrameworkCore;
using QuizUser.Infrastructure.Users;

namespace QuizUser.Features.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Insert(User user)
    {
        foreach (var role in user.Roles)
        {
            context.Attach(role);
        }

        context.Users.Add(user);
    }
}