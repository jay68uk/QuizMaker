using System.Net;
using Ardalis.Result;
using QuizMaker.Common.Infrastructure.Logging;
using QuizUser.Abstractions.Identity;

namespace QuizUser.Infrastructure.Identity;

internal sealed class IdentityProviderService(
  KeyCloakClient keyCloakClient,
  ILoggerAdaptor<IdentityProviderService> logger)
  : IIdentityProviderService
{
  private const string PasswordCredentialType = "password";

  // POST /admin/realms/{realm}/users
  public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
  {
    var userRepresentation = new UserRepresentation(
      user.Email,
      user.Email,
      user.FirstName,
      user.LastName,
      true,
      true,
      [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

    try
    {
      var identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

      return identityId;
    }
    catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
    {
      logger.LogError(exception, "User registration failed");

      return Result.Conflict(IdentityProviderErrors.EmailIsNotUnique);
    }
  }
}