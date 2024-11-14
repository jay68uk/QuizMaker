namespace QuizUser.Features.Authorisation;

public sealed record PermissionsResponse(Guid UserId, HashSet<string> Permissions);