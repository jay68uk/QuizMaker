using System.Security.Claims;
using QuizMaker.Common.Application.Exceptions;

namespace QuizUser.Features.Authentication;

public static class ClaimsPrincipalExtensions
{
  public static Guid GetUserId(this ClaimsPrincipal? principal)
  {
    var userId = principal?.FindFirst(CustomClaims.Sub)?.Value;

    return Guid.TryParse(userId, out var parsedUserId)
      ? parsedUserId
      : throw new QuizMakerException("User identifier is unavailable");
  }

  public static string GetIdentityId(this ClaimsPrincipal? principal)
  {
    return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
           throw new QuizMakerException("User identity is unavailable");
  }

  public static HashSet<string> GetPermissions(this ClaimsPrincipal? principal)
  {
    var permissionClaims = principal?.FindAll(CustomClaims.Permission) ??
                           throw new QuizMakerException("Permissions are unavailable");

    return permissionClaims.Select(c => c.Value).ToHashSet();
  }
}