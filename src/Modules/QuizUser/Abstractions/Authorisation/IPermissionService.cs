using Ardalis.Result;
using QuizUser.Features.Authorisation;

namespace QuizUser.Abstractions.Authorisation;

public interface IPermissionService
{
  Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}