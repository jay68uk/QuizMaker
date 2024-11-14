using QuizMaker.Common.Application.Messaging;
using QuizUser.Features.Authorisation;

namespace QuizUser.Features.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string IdentityId) : IQuery<PermissionsResponse>;