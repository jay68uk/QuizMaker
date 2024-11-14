using Ardalis.Result;
using Dapper;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Messaging;
using QuizUser.Abstractions.Identity;
using QuizUser.Abstractions.Sql;
using QuizUser.Features.Authorisation;

namespace QuizUser.Features.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
  : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
  public async Task<Result<PermissionsResponse>> Handle(
    GetUserPermissionsQuery request,
    CancellationToken cancellationToken)
  {
    await using var connection = await dbConnectionFactory.OpenConnection();

    var sql = SqlQueries.GetUserPermissionsSqlQuery();

    var permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

    return !permissions.Any()
      ? Result.Unauthorized()
      : Result.Success(new PermissionsResponse(permissions[0].UserId,
        permissions.Select(p => p.Permission).ToHashSet()));
  }
}