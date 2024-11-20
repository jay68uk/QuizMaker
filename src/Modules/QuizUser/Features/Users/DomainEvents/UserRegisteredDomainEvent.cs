using QuizMaker.Common.Domain;

namespace QuizUser.Features.Users.DomainEvents;

public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
  public Guid UserId { get; init; } = userId;
}