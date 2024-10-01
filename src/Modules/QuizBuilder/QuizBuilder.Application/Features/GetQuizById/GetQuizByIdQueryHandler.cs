using Ardalis.Result;
using Dapper;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Messaging;

namespace QuizBuilder.Application.Features.GetQuizById;

internal sealed class GetQuizByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
  : IQueryHandler<RequestQuizByIdQuery, QuizResponse>
{
  public async Task<Result<QuizResponse>> Handle(RequestQuizByIdQuery request, CancellationToken cancellationToken)
  {
    using var connection = await dbConnectionFactory.OpenConnectionAsync();

    const string sql = """
                       SELECT
                         id AS Id,
                         name AS Name
                       FROM quizzes
                       WHERE id = @Id
                       """;
    var quiz = await connection.QueryFirstOrDefaultAsync<QuizResponse>(sql, new { request.Id });
    
    return quiz is null? Result<QuizResponse>.NotFound() : Result<QuizResponse>.Success(quiz);
  }
}