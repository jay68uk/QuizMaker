using System.Data.Common;
using Microsoft.Extensions.Time.Testing;
using NSubstitute.DbConnection;
using QuizBuilder.Application.Abstractions;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Domain.Quiz;

namespace QuizBuilder.Application.UnitTests.TestData;

public static class SqlResultForDapper
{
    public static void GetQuizByIdDefault(DbConnection mockConnection)
    {
        var fakeTimeProvider = new FakeTimeProvider();
        fakeTimeProvider.SetUtcNow( new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
        mockConnection.SetupQuery(SqlQueries.GetQuizById).Returns(
            Enumerable.Empty<QuizResponseTestData>());

    }
    
    public static void GetQuizByIdSingleQuizNoQuestions(DbConnection mockConnection, QuizResponse quiz)
    {
        var fakeTimeProvider = new FakeTimeProvider();
        fakeTimeProvider.SetUtcNow( new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        mockConnection.SetupQuery(SqlQueries.GetQuizById).Returns(
            new {Id = quiz.Id, Name = quiz.Name, AccessCode = string.Empty, CreatedDate = fakeTimeProvider.GetUtcNow(), RanDate = fakeTimeProvider.GetUtcNow(), CreatedBy = Guid.NewGuid(), Status = QuizStatus.Draft,
                QuestionId = Guid.Empty, QuestionNumber = 0, QuestionDescription = string.Empty });

    }
    
    public static void GetQuizByIdSingleQuizWithQuestion(DbConnection mockConnection, QuizResponse quiz, QuestionResponse question)
    {
        var fakeTimeProvider = new FakeTimeProvider();
        fakeTimeProvider.SetUtcNow( new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
        mockConnection.SetupQuery(SqlQueries.GetQuizById).Returns(
            new {Id = quiz.Id, Name = quiz.Name, AccessCode = string.Empty, CreatedDate = fakeTimeProvider.GetUtcNow(), RanDate = fakeTimeProvider.GetUtcNow(), CreatedBy = Guid.NewGuid(), Status = QuizStatus.Draft,
                QuestionId = question.QuestionId, QuestionNumber = question.QuestionNumber, QuestionDescription = question.QuestionDescription });

    }
}

public record QuizResponseTestData(Guid Id, string Name, string AccessCode, DateTimeOffset CreatedDate, DateTimeOffset RanDate, Guid CreatedBy, QuizStatus Status,
Guid QuestionId, int QuestionNumber,string QuestionDescription);
