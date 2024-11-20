namespace QuizUser.Abstractions.Identity;

public sealed record UserIdentityProvider(
  string? Id,
  string Email,
  string Password,
  string FirstName,
  string LastName);