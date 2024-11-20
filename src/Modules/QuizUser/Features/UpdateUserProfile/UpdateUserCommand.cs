using QuizMaker.Common.Application.Messaging;

namespace QuizUser.Features.UpdateUserProfile;

internal record UpdateUserCommand(Guid UserId, string FirstName, string LastName, string Email)
  : ICommand;