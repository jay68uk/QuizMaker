namespace QuizUser.Infrastructure.Authorisation;

public sealed record PermissionsResponse(Guid UserId, HashSet<string> Permissions);
