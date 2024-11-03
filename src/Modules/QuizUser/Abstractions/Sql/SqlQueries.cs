using QuizUser.Features.UserProfile;

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
}