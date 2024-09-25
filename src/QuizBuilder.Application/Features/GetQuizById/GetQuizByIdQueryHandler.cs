using Ardalis.Result;
using Dapper;
using QuizBuilder.Application.Abstractions;
using QuizMaker.SharedKernel.Messaging;

namespace QuizBuilder.Application.Features.GetQuizById;

internal sealed class GetQuizByIdQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
  : IQueryHandler<RequestQuizByIdQuery, QuizResponse>
{
  public async Task<Result<QuizResponse>> Handle(RequestQuizByIdQuery request, CancellationToken cancellationToken)
  {
    using var connection = sqlConnectionFactory.CreateConnection();

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