using Ardalis.Result;
using Dapper;
using QuizBuilder.Application.Abstractions;
using QuizMaker.Common.Application.Data;
using QuizMaker.Common.Application.Messaging;

namespace QuizBuilder.Application.Features.GetQuizById;

internal sealed class GetQuizByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<RequestQuizByIdQuery, QuizResponse>
{
    public async Task<Result<QuizResponse>> Handle(RequestQuizByIdQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = SqlQueries.GetQuizById;
        var quizDictionary = new Dictionary<Guid, QuizResponse>();

        _ = await connection!.QueryAsync<QuizResponse, QuestionResponse?, QuizResponse>(
            sql,
            (quiz, question) =>
            {
                if (!quizDictionary.TryGetValue(quiz.Id, out var currentQuiz))
                {
                    currentQuiz = quiz with { Questions = [] };
                    quizDictionary.Add(currentQuiz.Id, currentQuiz);
                }

                if (question is { QuestionId: var id } && id != Guid.Empty)
                {
                    currentQuiz.Questions.Add(question);
                }

                return currentQuiz;
            },
            new { QuizId = request.Id },
            splitOn: "QuestionId");

        var result = quizDictionary.Values.FirstOrDefault();

        return result is null ? Result<QuizResponse>.NotFound() : Result<QuizResponse>.Success(result);
    }
}
