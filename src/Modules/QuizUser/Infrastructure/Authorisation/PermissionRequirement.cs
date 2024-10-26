﻿using Microsoft.AspNetCore.Authorization;

namespace QuizUser.Infrastructure.Authorisation;

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}