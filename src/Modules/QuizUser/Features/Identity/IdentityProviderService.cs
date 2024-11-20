using System.Net;
using Ardalis.Result;
using QuizMaker.Common.Infrastructure.Logging;
using QuizUser.Abstractions.Identity;

namespace QuizUser.Features.Identity;

internal sealed class IdentityProviderService : IIdentityProviderService
{
  private const string PasswordCredentialType = "password";
  private readonly KeyCloakClient _keyCloakClient;
  private readonly ILoggerAdaptor<IdentityProviderService> _logger;

  public IdentityProviderService(
    KeyCloakClient keyCloakClient,
    ILoggerAdaptor<IdentityProviderService> logger)
  {
    _keyCloakClient = keyCloakClient;
    _logger = logger;
  }

  // POST /admin/realms/{realm}/users
  public async Task<Result<string>> RegisterUserAsync(UserIdentityProvider user,
    CancellationToken cancellationToken = default)
  {
    var userRepresentation = new UserRepresentation(
      string.Empty,
      user.Email,
      user.Email,
      user.FirstName,
      user.LastName,
      true,
      true,
      [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

    try
    {
      var identityId = await _keyCloakClient.RegisterUser(userRepresentation, cancellationToken);

      return identityId;
    }
    catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
    {
      _logger.LogError(exception, "User registration failed");

      return Result.Conflict(IdentityProviderErrors.EmailIsNotUnique);
    }
  }

  public async Task<Result> UpdateUserAsync(UserIdentityProvider user,
    CancellationToken cancellationToken = default)
  {
    var userRepresentation = new UserRepresentation(
      user.Id,
      user.Email,
      user.Email,
      user.FirstName,
      user.LastName,
      true,
      true,
      []);

    try
    {
      await _keyCloakClient.UpdateUser(userRepresentation, cancellationToken);

      return Result.Success();
    }
    catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
    {
      _logger.LogError(exception, "User update failed due to data conflict");

      return Result.Conflict(IdentityProviderErrors.EmailIsNotUnique);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, "User update failed");

      return Result.Error();
    }
  }
}