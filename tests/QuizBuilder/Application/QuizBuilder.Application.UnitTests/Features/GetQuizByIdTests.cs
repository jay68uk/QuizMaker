using System.Data.Common;
using Ardalis.Result;
using NSubstitute;
using NSubstitute.DbConnection;
using QuizBuilder.Application.Features.GetQuizById;
using QuizBuilder.Application.UnitTests.TestData;
using QuizMaker.Common.Application.Data;

namespace QuizBuilder.Application.UnitTests.Features;

public class GetQuizByIdTests
{
    private readonly IDbConnectionFactory _mockDbConnectionFactory;
    private readonly GetQuizByIdQueryHandler _handler;

    public GetQuizByIdTests()
    {
        _mockDbConnectionFactory = Substitute.For<IDbConnectionFactory>();
        _handler = new GetQuizByIdQueryHandler(_mockDbConnectionFactory);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenQuizDoesNotExist()
    {
        // Arrange
        var query = new RequestQuizByIdQuery(Ulid.NewUlid().ToGuid());
        var mockConnection = Substitute.For<DbConnection>().SetupCommands();
        SqlResultForDapper.GetQuizByIdDefault(mockConnection);
        _mockDbConnectionFactory.OpenConnectionAsync().Returns(mockConnection);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task Handle_ShouldReturnQuiz_WhenQuizExists()
    {
        // Arrange
        var query = new RequestQuizByIdQuery(Ulid.NewUlid().ToGuid());
        var quiz = new QuizResponse { Id = query.Id, Name = "Test Quiz" };
        var mockConnection = Substitute.For<DbConnection>().SetupCommands();
        SqlResultForDapper.GetQuizByIdSingleQuizNoQuestions(mockConnection, quiz);
        _mockDbConnectionFactory.OpenConnectionAsync().Returns(mockConnection);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(query.Id, result.Value.Id);
        Assert.Equal("Test Quiz", result.Value.Name);
    }

    [Fact]
    public async Task Handle_ShouldIncludeQuestions_WhenQuizHasQuestions()
    {
        // Arrange
        var query = new RequestQuizByIdQuery(Ulid.NewUlid().ToGuid());
        var quiz = new QuizResponse { Id = query.Id, Name = "Test Quiz" };
        var question = new QuestionResponse(Ulid.NewUlid().ToGuid(), 1, "Test Question");
        var mockConnection = Substitute.For<DbConnection>().SetupCommands();
        SqlResultForDapper.GetQuizByIdSingleQuizWithQuestion(mockConnection, quiz, question);
        _mockDbConnectionFactory.OpenConnectionAsync().Returns(mockConnection);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.NotNull(result.Value);
        Assert.Equal(query.Id, result.Value.Id);
        Assert.Contains(result.Value.Questions, q => q.QuestionId == question.QuestionId);
    }
}
