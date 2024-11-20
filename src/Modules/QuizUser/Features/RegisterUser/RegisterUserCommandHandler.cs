using Ardalis.Result;
using QuizMaker.Common.Application.Messaging;
using QuizUser.Abstractions.Identity;
using QuizUser.Abstractions.Users;
using QuizUser.Features.Users;

namespace QuizUser.Features.RegisterUser;

internal sealed class RegisterUserCommandHandler(
  IIdentityProviderService identityProviderService,
  IUserRepository userRepository,
  IUnitOfWork unitOfWork)
  : ICommandHandler<RegisterUserCommand, Guid>
{
  public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
  {
    var result = await identityProviderService.RegisterUserAsync(
      new UserIdentityProvider(string.Empty, request.Email, request.Password, request.FirstName, request.LastName),
      cancellationToken);

    if (result.IsConflict())
    {
      return Result.Conflict((string[])result.Errors);
    }

    var user = User.Create(request.Email, request.FirstName, request.LastName, result.Value);

    userRepository.Insert(user);

    await unitOfWork.SaveChangesAsync(cancellationToken);

    return user.Id;
  }
}