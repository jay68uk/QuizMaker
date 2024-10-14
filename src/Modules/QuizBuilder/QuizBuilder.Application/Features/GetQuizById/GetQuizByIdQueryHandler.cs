using System.Data.Common;
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
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT 
                               q.Id, q.Name, qaC.access_code AS AccessCode, q.created_date AS CreatedDate, q.ran_date AS RanDate, q.created_by AS CreatedBy, q.Status,
                               qu.Id AS QuestionId, qu.Number AS QuestionNumber, qu.Description AS QuestionDescription
                           FROM quizzes q
                           LEFT JOIN questions qu ON q.Id = qu.quiz_id
                           LEFT JOIN public."quiz_accessCode" qaC ON q.id = qaC.id
                           WHERE q.Is_Deleted = FALSE AND q.Id = @QuizId
                           """;
        var quizDictionary = new Dictionary<Guid, QuizResponse>();

        _ = await connection!.QueryAsync<QuizResponse, QuestionResponse, QuizResponse>(
            sql,
            (quiz, question) =>
            {
                if (!quizDictionary.TryGetValue(quiz.Id, out var currentQuiz))
                {
                    currentQuiz = quiz with { Questions = [] };
                    quizDictionary.Add(currentQuiz.Id, currentQuiz);
                }

                if (question.QuestionId != Guid.Empty)
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
