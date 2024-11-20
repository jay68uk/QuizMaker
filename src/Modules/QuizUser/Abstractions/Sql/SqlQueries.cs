using QuizUser.Abstractions.Identity;
using QuizUser.Features.GetUserProfile;

namespace QuizUser.Abstractions.Sql;

internal static class SqlQueries
{
  public static string GetUserProfileSqlQuery()
  {
    return $"""
            SELECT
                id AS {nameof(UserResponse.UserId)},
                email AS {nameof(UserResponse.Email)},
                first_name AS {nameof(UserResponse.FirstName)},
                last_name AS {nameof(UserResponse.LastName)}
            FROM users.users
            WHERE id = @UserId
            """;
  }

  public static string GetUserPermissionsSqlQuery()
  {
    return $"""
            SELECT DISTINCT
                u.id AS {nameof(UserPermission.UserId)},
                rp.permission_code AS {nameof(UserPermission.Permission)}
            FROM users.users u
            JOIN users.user_roles ur ON ur.user_id = u.id
            JOIN users.role_permissions rp ON rp.role_name = ur.role_name
            WHERE u.identity_id = @IdentityId
            """;
  }
}