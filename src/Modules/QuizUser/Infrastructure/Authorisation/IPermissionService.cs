using Ardalis.Result;

namespace QuizUser.Infrastructure.Authorisation;

public interface IPermissionService
{
    Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId);
}
