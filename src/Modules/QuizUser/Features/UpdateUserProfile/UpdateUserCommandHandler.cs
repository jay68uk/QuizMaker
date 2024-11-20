using Ardalis.Result;
using QuizMaker.Common.Application.Messaging;
using QuizMaker.Common.Infrastructure.Logging;
using QuizUser.Abstractions.Errors;
using QuizUser.Abstractions.Identity;
using QuizUser.Abstractions.Users;
using QuizUser.Features.Users;

namespace QuizUser.Features.UpdateUserProfile;

internal sealed class UpdateUserCommandHandler(
  IIdentityProviderService identityProviderService,
  IUserRepository userRepository,
  IUnitOfWork unitOfWork,
  ILoggerAdaptor<UpdateUserCommandHandler> logger)
  : ICommandHandler<UpdateUserCommand>
{
  public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
  {
    var userId = request.UserId;
    var user = await userRepository.GetAsync(userId, cancellationToken);

    if (user is null)
    {
      return Result.NotFound(UserErrors.NotFound(request.UserId).Description);
    }

    var existingUserValues = new Dictionary<string, string>
    {
      { "firstName", user.FirstName },
      { "lastName", user.LastName },
      { "email", user.Email }
    };

    try
    {
      user.Update(request.FirstName, request.LastName, request.Email);

      var identityUpdate = await identityProviderService.UpdateUserAsync(
        new UserIdentityProvider(user.IdentityId, request.Email, string.Empty, request.FirstName, request.LastName),
        cancellationToken);

      if (identityUpdate.IsConflict() || identityUpdate.IsError())
      {
        RollbackUserUpdate(user, existingUserValues);
        return identityUpdate;
      }

      await unitOfWork.SaveChangesAsync(cancellationToken);
      user.RaiseUpdateDomainEvent(user.Id);
      return Result.Success();
    }
    catch (Exception exception)
    {
      logger.LogError(exception, "An error occured while updating the user profile.");
      RollbackUserUpdate(user, existingUserValues);
      _ = await identityProviderService.UpdateUserAsync(
        new UserIdentityProvider(
          user.IdentityId,
          existingUserValues["email"],
          string.Empty,
          existingUserValues["firstName"],
          existingUserValues["lastName"]),
        cancellationToken);
      return Result.Error(exception.Message);
    }
  }

  private static void RollbackUserUpdate(User user, Dictionary<string, string> existingUserValues)
  {
    user.Update(
      existingUserValues["firstName"],
      existingUserValues["lastName"],
      existingUserValues["email"]);
  }
}