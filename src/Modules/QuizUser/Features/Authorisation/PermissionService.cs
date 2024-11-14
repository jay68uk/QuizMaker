using Ardalis.Result;
using MediatR;
using QuizUser.Abstractions.Authorisation;
using QuizUser.Features.GetUserPermissions;

namespace QuizUser.Features.Authorisation;

internal sealed class PermissionService(ISender sender) : IPermissionService
{
  public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId)
  {
    return await sender.Send(new GetUserPermissionsQuery(identityId));
  }
}