using Ardalis.Result;
using Dapper;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Messaging;
using QuizUser.Abstractions.Sql;

namespace QuizUser.Features.UserProfile;

internal sealed class GetUserByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
  : IQueryHandler<RequestUserByIdQuery, UserByIdResponse>
{
  public async Task<Result<UserByIdResponse>> Handle(RequestUserByIdQuery request, CancellationToken cancellationToken)
  {
    await using var connection = await dbConnectionFactory.OpenConnection();

    var sql = SqlQueries.GetUserProfileSqlQuery();

    var user = await connection.QueryFirstOrDefaultAsync<UserByIdResponse>(sql,
      new { request.UserId });

    return user is null
      ? Result<UserByIdResponse>.NotFound()
      : Result<UserByIdResponse>.Success(user);
  }
}

internal sealed record RequestUserByIdQuery(Guid UserId) : IQuery<UserByIdResponse>;

internal sealed record UserByIdResponse(Guid UserId, string Email, string FirstName, string LastName);