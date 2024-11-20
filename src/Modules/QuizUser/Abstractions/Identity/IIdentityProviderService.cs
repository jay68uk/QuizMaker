using Ardalis.Result;

namespace QuizUser.Abstractions.Identity;

public interface IIdentityProviderService
{
  Task<Result<string>> RegisterUserAsync(UserIdentityProvider user, CancellationToken cancellationToken = default);

  Task<Result> UpdateUserAsync(UserIdentityProvider user,
    CancellationToken cancellationToken = default);
}