using QuizMaker.Common.Application.Messaging;

namespace QuizUser.Features.RegisterUser;

internal sealed record RegisterUserCommand(string Email, string Password, string FirstName, string LastName)
  : ICommand<Guid>;