using QuizMaker.Common.Domain;

namespace QuizUser.Features.Users.DomainEvents;

public sealed class UserProfileUpdatedDomainEvent(
  Guid userId,
  string firstName,
  string lastName,
  string email)
  : DomainEvent
{
  public Guid UserId { get; init; } = userId;
  public string FirstName { get; init; } = firstName;

  public string LastName { get; init; } = lastName;

  public string Email { get; init; } = email;
}