using Ardalis.Result;
using MediatR;
using QuizUser.Features.GetUserPermissions;

namespace QuizUser.Infrastructure.Authorisation;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
    {
        return await sender.Send(new GetUserPermissionsQuery(identityId));
    }
}
