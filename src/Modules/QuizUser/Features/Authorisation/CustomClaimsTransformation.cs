﻿using System.Security.Claims;
using Ardalis.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Common.Application.Exceptions;
using QuizUser.Abstractions.Authorisation;
using QuizUser.Features.Authentication;

namespace QuizUser.Features.Authorisation;

internal sealed class CustomClaimsTransformation(IServiceScopeFactory serviceScopeFactory) : IClaimsTransformation
{
  public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
  {
    if (principal.HasClaim(c => c.Type == CustomClaims.Sub))
    {
      return principal;
    }

    using var scope = serviceScopeFactory.CreateScope();

    var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

    var identityId = principal.GetIdentityId();

    var result = await permissionService.GetUserPermissionsAsync(identityId);

    if (result.IsUnauthorized())
    {
      throw new QuizMakerException(nameof(IPermissionService.GetUserPermissionsAsync));
    }

    var claimsIdentity = new ClaimsIdentity();

    claimsIdentity.AddClaim(new Claim(CustomClaims.Sub, result.Value.UserId.ToString()));

    foreach (var permission in result.Value.Permissions)
    {
      claimsIdentity.AddClaim(new Claim(CustomClaims.Permission, permission));
    }

    principal.AddIdentity(claimsIdentity);

    return principal;
  }
}