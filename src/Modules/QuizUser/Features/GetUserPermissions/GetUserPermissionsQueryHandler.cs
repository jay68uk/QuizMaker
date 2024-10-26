using System.Data.Common;
using Ardalis.Result;
using Dapper;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Messaging;
using QuizUser.Infrastructure.Authorisation;

namespace QuizUser.Features.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(
        GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                 u.id AS {nameof(UserPermission.UserId)},
                 rp.permission_code AS {nameof(UserPermission.Permission)}
             FROM users.users u
             JOIN users.user_roles ur ON ur.user_id = u.id
             JOIN users.role_permissions rp ON rp.role_name = ur.role_name
             WHERE u.identity_id = @IdentityId
             """;

        var permissions = (await connection!.QueryAsync<UserPermission>(sql, request)).AsList();

        return !permissions.Any() 
            ? Result.Unauthorized() 
            : Result.Success(new PermissionsResponse(permissions[0].UserId,
                permissions.Select(p => p.Permission).ToHashSet()));
    }

    internal sealed class UserPermission
    {
        internal Guid UserId { get; init; }

        internal string Permission { get; init; }
    }
}
