namespace QuizUser.Features.Identity;

public sealed record UserRepresentation(
  string? Id,
  string Username,
  string Email,
  string FirstName,
  string LastName,
  bool EmailVerified,
  bool Enabled,
  CredentialRepresentation[] Credentials);